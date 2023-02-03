using HomeTG.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace HomeTG.Models
{
    [TestFixture]
    public class MTGDBTest
    {
        MTGDB dbContext;
        List<Card> entities = new List<Card>
        {
            new Card("1", "TEST NAME", "SET", "someScryfallId"),
            new Card("2", "TESTS NAME", "SET", "someScryfallId")
        };

        [OneTimeSetUp]
        public void Setup()
        {
            var dbSet = MockDbSetFactory.Create(entities);
            dbContext = new MTGDB(new DbContextOptions<MTGDB>());
            dbContext.Cards = dbSet.Object;
        }

        [Test]
        [Ignore("Seems broken")]
        public void TestSearchCards()
        {
            var results = dbContext.SearchCards(new SearchOptions { SetCode = "SET" });
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("TEST NAME", results.First().Name);
        }

        [Test]
        public void TestBulkSearchCards()
        {
            var results = dbContext.BulkSearchCards(new List<StrictSearchOptions> { new StrictSearchOptions("TEST NAME", "SET") });
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("TEST NAME", results[("TEST NAME", "SET")].Name);
        }

        [Test]
        public void TestGetCards()
        {
            var results = dbContext.GetCards(new List<string> { "1" });
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("TEST NAME", results.First().Name);
        }
    }
}
