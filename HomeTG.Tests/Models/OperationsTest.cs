using HomeTG.Models;
using HomeTG.Models.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTG.Tests.Models
{
    [TestFixture]
    public class OperationsTest
    {
        Operations ops;
        List<CollectionCard> collectionCards = new List<CollectionCard>
        {
            new CollectionCard("1", 2, 0, "Main", null),
            new CollectionCard("2", 1, 0, "Main", null),
            new CollectionCard("1", 1, 0, "Incoming", null),
            new CollectionCard("3", 2, 0, "SpecialList", null),
        };
        List<Card> cards = new List<Card>
        {
            new Card("1", "TEST NAME", "SET", "123", "someScryfallId", "R", "Artist 1", "B,G", "Win the game."),
            new Card("2", "TESTS MANE", "SET", "124", "someScryfallId", "C", "Artist 2", "U", "Lose the game.")
        };

        [OneTimeSetUp]
        public void Setup()
        {
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<MTGDB>()
                                .UseSqlite(_connection)
                                .Options;
            MTGDB mtgDBContext = new MTGDB(options);
            mtgDBContext.Database.EnsureCreated();
            mtgDBContext.AddRange(cards);
            mtgDBContext.SaveChanges();

            var _connection2 = new SqliteConnection("Filename=:memory:");
            _connection2.Open();
            var options2 = new DbContextOptionsBuilder<CollectionDB>()
                                .UseSqlite(_connection2)
                                .Options;
            CollectionDB dbContext = new CollectionDB(options2);
            dbContext.Database.EnsureCreated();
            dbContext.AddRange(collectionCards);
            dbContext.SaveChanges();

            ops = new Operations(dbContext, mtgDBContext);
        }

        [Test]
        public void TestCount()
        {
            var result = ops.Count("Main");
            Assert.That(result, Is.EqualTo(2));
        }
    }
}
