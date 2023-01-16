using Microsoft.EntityFrameworkCore;
using HomeTG.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Net;
using System;
using System.IO;
using HomeTGCollection.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var collectionSQL = "create table if not exists cards(uuid string, quantity int32, foilquantity int32, collection string); create table collection(id string, description string);";
var incomingSQL = "create table if not exists incoming(uuid string, quantity int32, foilquantity int32, collection string);";
DBFiles.CreateDBIfNotExists(@"DB\", "Collection.db", collectionSQL + incomingSQL);
await DBFiles.DownloadPrintingsDBIfNotExists(@"https://mtgjson.com/api/v5/AllPrintings.sqlite", @"DB\", "AllPrintings.db");

builder.Services.AddDbContext<DB>(options => options.UseSqlite(builder.Configuration.GetConnectionString("MtgJson")));
builder.Services.AddDbContext<CollectionDB>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Collection")));

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
