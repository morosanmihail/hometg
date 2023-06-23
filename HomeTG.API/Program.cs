using HomeTG.API.Models.Contexts;
using HomeTG.API.Utils;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json").
    AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MtGDBPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "hometg",
        "DB/AllPrintings.db"
    );

await DBFiles.DownloadPrintingsDBIfNotExists(
    @"https://mtgjson.com/api/v5/AllPrintings.sqlite",
    MtGDBPath
);

builder.Services.AddDbContext<MTGDB>(
    options => options.UseSqlite(
        "Data Source=" + MtGDBPath + ";Mode=ReadOnly"
    )
);

builder.Services.AddDbContext<CollectionDB>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CollectionDB>();
    context.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
