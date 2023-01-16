using Microsoft.EntityFrameworkCore;
using HomeTG.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Net;
using System;
using System.IO;

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
DownloadPrintingsDBIfNotExists(@"https://mtgjson.com/api/v5/AllPrintings.sqlite", @"DB\AllPrintings.db");

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

static void DownloadPrintingsDBIfNotExists(string DBURL, string LocalFile)
{
    bool download = false;

    string remoteHash = "";
    using (WebClient client = new WebClient())
    {
        client.DownloadFile(DBURL + ".sha256", LocalFile + ".sha256");
        remoteHash = File.ReadAllText(LocalFile + ".sha256");
    }

    string localHash = "";
    if (!File.Exists(LocalFile))
    {
        download = true;
    }
    else
    {
        localHash = GetSHA256HashFromFile(LocalFile);
        if(remoteHash != localHash)
        {
            download = true;
        }
    }

    if (download) {
        Console.WriteLine("File is NOT up to date. Downloading...");
        using (WebClient client = new WebClient())
        {
            client.DownloadFile(DBURL, LocalFile);
        }
        Console.WriteLine("Downloaded.");
    }
}

static string GetSHA256HashFromFile(string filePath)
{
    using (FileStream stream = File.OpenRead(filePath))
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] hash = sha.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}