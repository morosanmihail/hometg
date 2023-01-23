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

DBFiles.CreateDBIfNotExists(@"DB/Collection.db", @"DB/CollectionDB.sql");
await DBFiles.DownloadPrintingsDBIfNotExists(@"https://mtgjson.com/api/v5/AllPrintings.sqlite", @"DB/AllPrintings.db");

builder.Services.AddDbContext<MTGDB>(options => options.UseSqlite(builder.Configuration.GetConnectionString("MtgJson")));
builder.Services.AddDbContext<CollectionDB>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Collection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseAuthorization();

app.MapControllers();

app.Run();
