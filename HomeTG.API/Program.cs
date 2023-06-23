using HomeTG.API.Models.Contexts;
using HomeTG.API.Utils;
using Microsoft.EntityFrameworkCore;
using static HomeTG.API.Provider;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json").
    AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
builder.Configuration.AddEnvironmentVariables();

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

builder.Services.AddDbContext<CollectionDB>(options =>
{
    var provider = builder.Configuration.GetValue("provider", Sqlite.Name);

    if (provider == Postgres.Name)
    {
        options.UseNpgsql(
               builder.Configuration.GetConnectionString("Postgres")!,
               x => x.MigrationsAssembly("HomeTG.Postgres")
        );
    }

    if (provider == Sqlite.Name)
    {
        options.UseSqlite(
                builder.Configuration.GetConnectionString("Sqlite")!,
               x => x.MigrationsAssembly("HomeTG.Sqlite")
        );
    }
});

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
