using HomeTG.Models;
using HomeTG.Models.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTG.Benchmarks
{
    public class CollectionAdd
    {
        CollectionDB dbContext;
        List<CollectionCard> cards = new List<CollectionCard>
        {
            new CollectionCard("1", 2, 0, "Somecollection", null),
            new CollectionCard("2", 1, 0, "Somecollection", null),
            new CollectionCard("1", 1, 0, "Somecollection", null),
            new CollectionCard("3", 2, 0, "Somecollection", null),
            new CollectionCard("5", 3, 0, "Somecollection", null),
        };

        [GlobalSetup]
        public void GlobalSetup()
        {
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<CollectionDB>()
                                .UseSqlite(_connection)
                                .Options;
            dbContext = new CollectionDB(options);
            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
        }

        [Benchmark]
        [Arguments(1)]
        [Arguments(3)]
        [Arguments(5)]
        public List<CollectionCard> TestCollectionAdd(int count)
        {
            return dbContext.AddCards("Somecollection", cards.Take(count).ToList());
        }
    }
}
