using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Net;
using System;
using System.IO;
using HomeTGCollection.Utils;
using HomeTG.Models.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var DBPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "hometg",
        "DB/Collection.db"
    );
var MtGDBPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "hometg",
        "DB/AllPrintings.db"
    );

DBFiles.CreateDBIfNotExists(
    DBPath, "DB/CollectionDB.sql"
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
        "Data Source=" + DBPath
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseAuthorization();

app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
