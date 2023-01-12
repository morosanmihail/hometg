using Microsoft.EntityFrameworkCore;
using HomeTG.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var collectionSQL = "create table if not exists cards(uuid string, quantity int32, foilquantity int32, collection string); create table collection(id string, description string);";
var incomingSQL = "create table if not exists incoming(uuid string, quantity int32, foilquantity int32, collection string);";
CreateDBIfNotExists(@"DB\Collection.db", collectionSQL);
CreateDBIfNotExists(@"DB\Incoming.db", incomingSQL);
// Add download of newer AllPrintings.db here

var connectionString = builder.Configuration.GetConnectionString("MtgJson");
builder.Services.AddDbContext<DB>(options => options.UseSqlite(connectionString));
var connectionString2 = builder.Configuration.GetConnectionString("Collection");
builder.Services.AddDbContext<CollectionDB>(options => options.UseSqlite(connectionString2));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

static void CreateDBIfNotExists(string DB, string sql)
{
    if (!System.IO.File.Exists(DB))
    {
        using (var sqlite = new SqliteConnection(@"Data Source=" + DB))
        {
            sqlite.Open();
            SqliteCommand command = new SqliteCommand(sql, sqlite);
            command.ExecuteNonQuery();
        }
    }
}
