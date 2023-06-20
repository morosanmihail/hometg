using HomeTG.API.Models.Contexts;
using HomeTG.API.Utils;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HomeTG.Tests.Helpers
{
    public static class TestHelpers
    {
        public static CollectionDB GetTestCollectionDB()
        {
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<CollectionDB>()
                                .UseSqlite(_connection)
                                .Options;
            var dbContext = new CollectionDB(options);
            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
            return dbContext;
        }

        public static MTGDB GetTestEmptyMTGDB()
        {
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<MTGDB>()
                                .UseSqlite(_connection)
                                .Options;
            var dbContext = new MTGDB(options);
            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
            return dbContext;
        }

        public static MTGDB GetActualMTGDB()
        {
            var MtGDBPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "hometg",
                "DB/AllPrintings.db"
            );

            _ = DBFiles.DownloadPrintingsDBIfNotExists(
                @"https://mtgjson.com/api/v5/AllPrintings.sqlite",
                MtGDBPath
            );

            var _connection = new SqliteConnection("Data Source=" + MtGDBPath + ";Mode=ReadOnly");
            _connection.Open();
            var options = new DbContextOptionsBuilder<MTGDB>()
                                .UseSqlite(_connection)
                                .Options;
            var mtgContext = new MTGDB(options);
            return mtgContext;
        }

        public static string GetTestFile(string filename)
        {
            return Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "TestData",
                filename
            );
        }
    }
}
