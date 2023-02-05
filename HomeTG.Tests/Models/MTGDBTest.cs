using HomeTG.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HomeTG.Tests.Models
{
    [TestFixture]
    public class MTGDBTest
    {
        MTGDB dbContext;
        List<Card> entities = new List<Card>
        {
            new Card("1", "TEST NAME", "SET", "123", "someScryfallId"),
            new Card("2", "TESTS MANE", "SET", "124", "someScryfallId")
        };

        [OneTimeSetUp]
        public void Setup()
        {
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<MTGDB>()
                                .UseSqlite(_connection)
                                .Options;
            dbContext = new MTGDB(options);
            dbContext.Database.EnsureCreated();
            dbContext.AddRange(entities);
            dbContext.SaveChanges();
        }

        [Test]
        public void TestSearchCards()
        {
            var results = dbContext.SearchCards(new SearchOptions { Name = "NAME" });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Name, Is.EqualTo("TEST NAME"));

            results = dbContext.SearchCards(new SearchOptions { SetCode = "SET" });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.That(results.First().SetCode, Is.EqualTo("SET"));

            results = dbContext.SearchCards(new SearchOptions { CollectorNumber = "124" });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().SetCode, Is.EqualTo("SET"));
            Assert.That(results.First().CollectorNumber, Is.EqualTo("124"));
            Assert.That(results.First().Name, Is.EqualTo("TESTS MANE"));
        }

        [Test]
        public void TestBulkSearchCards()
        {
            var results = dbContext.BulkSearchCards(new List<StrictSearchOptions> { new StrictSearchOptions("123", "SET") });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results[("123", "SET")].Name, Is.EqualTo("TEST NAME"));
        }

        [Test]
        public void TestGetCards()
        {
            var results = dbContext.GetCards(new List<string> { "1" });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Name, Is.EqualTo("TEST NAME"));
        }
    }
}
