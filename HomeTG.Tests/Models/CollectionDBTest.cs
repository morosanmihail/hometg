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
    public class CollectionDBTest
    {
        CollectionDB dbContext;
        List<CollectionCard> cards = new List<CollectionCard>
        {
            new CollectionCard("1", 2, 0, "Main", null),
            new CollectionCard("2", 1, 0, "Main", null),
            new CollectionCard("1", 1, 0, "Incoming", null),
            new CollectionCard("3", 2, 0, "SpecialList", null),
        };

        [SetUp]
        public void Setup()
        {
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<CollectionDB>()
                                .UseSqlite(_connection)
                                .Options;
            dbContext = new CollectionDB(options);
            dbContext.Database.EnsureCreated();
            dbContext.AddRange(cards); 
            dbContext.SaveChanges();
        }

        [Test]
        public void TestListCards()
        {
            var results = dbContext.ListCards("Main", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));

            results = dbContext.ListCards("Incoming", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Id, Is.EqualTo("1"));

            results = dbContext.ListCards("Main", 1);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
        }

        [Test]
        public void TestAddCards()
        {
            var results = dbContext.AddCards("Incoming", new List<CollectionCard>()
            {
                new CollectionCard("3", 0, 4, "Incoming", null)
            });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Id, Is.EqualTo("3"));

            var listResults = dbContext.ListCards("Incoming", 0);
            Assert.NotNull(listResults);
            Assert.That(listResults.Count(), Is.EqualTo(2));

            listResults = dbContext.ListCards("NewList", 0);
            Assert.NotNull(listResults);
            Assert.That(listResults.Count(), Is.EqualTo(0));

            results = dbContext.AddCards("NewList", new List<CollectionCard>()
            {
                new CollectionCard("4", 0, 2, "NewList", null),
                new CollectionCard("4", 0, 4, "NewList", null),
            });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().FoilQuantity, Is.EqualTo(6));

            listResults = dbContext.ListCards("NewList", 0);
            Assert.NotNull(listResults);
            Assert.That(listResults.Count(), Is.EqualTo(1));
            Assert.That(results.First().FoilQuantity, Is.EqualTo(6));
        }

        [Test]
        public void TestAddCardsWrongCollection()
        {
            Assert.Ignore();

            var results = dbContext.AddCards("Incoming", new List<CollectionCard>()
            {
                new CollectionCard("3", 0, 4, "Outgoing", null)
            });
            Assert.NotNull(results);

            // Fix this bug, then re-enable test
            var resultsAgain = dbContext.AddCards("Incoming", new List<CollectionCard>()
            {
                new CollectionCard("3", 0, 4, "Outgoing", null)
            });
            Assert.NotNull(results);
        }

        [Test]
        public void TestGetCards()
        {
            var results = dbContext.GetCards(new List<string> { "1" });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
        }

        [Test]
        public void TestRemoveCard()
        {
            // Remove one quantity
            var result = dbContext.RemoveCard(
                new CollectionCard("3", 1, 0, "SpecialList", null)
            );
            Assert.NotNull(result);

            var results = dbContext.ListCards("SpecialList", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));

            results = dbContext.GetCards(new List<string> { "3" });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Quantity, Is.EqualTo(1));

            // Remove final quantity
            result = dbContext.RemoveCard(
                new CollectionCard("3", 1, 0, "SpecialList", null)
            );
            Assert.NotNull(result);

            results = dbContext.ListCards("SpecialList", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(0));

            // Also remove final quantity from different collection
            result = dbContext.RemoveCard(
                new CollectionCard("1", 1, 0, "Incoming", null)
            );
            Assert.NotNull(result);

            results = dbContext.ListCards("Incoming", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(0));
        }
    }
}
