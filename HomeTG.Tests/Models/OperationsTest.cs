using HomeTG.Models;
using HomeTG.Models.Contexts;
using HomeTG.Models.Contexts.Options;
using HomeTG.Tests.Helpers;
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
        Dictionary<string, CollectionCard> collectionCards = new Dictionary<string, CollectionCard>
        {
            {"Main1", new CollectionCard("1", 2, 0, "Main", null) },
            {"Main2", new CollectionCard("2", 1, 0, "Main", null) },
            {"Incoming1", new CollectionCard("1", 1, 0, "Incoming", null) },
            {"SpecialList3", new CollectionCard("3", 2, 0, "SpecialList", null) },
        };
        Dictionary<string, Card> cards = new Dictionary<string, Card>
        {
            {"1", new Card("1", "TEST NAME", "SET", "123", "R", "Artist 1", "B,G", "Win the game.") },
            {"2", new Card("2", "TESTS MANE", "SET", "124", "C", "Artist 2", "U", "Lose the game.") },
            {"3", new Card("3", "OH NO", "NOT", "1", "M", "Artist 1", "W,U,B,R,G", "") },
        };
        List<CardIdentifiers> identifiers = new List<CardIdentifiers>
        {
            new CardIdentifiers("1", "Scry1"),
            new CardIdentifiers("2", "Scry2"),
            new CardIdentifiers("3", "Scry3"),
        };

        [SetUp]
        public void Setup()
        {
            MTGDB mtgDBContext = TestHelpers.GetTestEmptyMTGDB();
            mtgDBContext.AddRange(identifiers);
            mtgDBContext.AddRange(cards.Values);
            mtgDBContext.SaveChanges();

            CollectionDB dbContext = TestHelpers.GetTestCollectionDB();
            dbContext.AddRange(collectionCards.Values);
            dbContext.SaveChanges();

            ops = new Operations(dbContext, mtgDBContext);
        }

        [Test]
        public void TestCount()
        {
            var result = ops.Count("Main");
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void TestSearchCollection()
        {
            var results = ops.SearchCollection("Main", new SearchOptions { Name = "TEST NAME" }).ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results[0].MtGCard, Is.EqualTo(cards["1"]));

            results = ops.SearchCollection("Main", new SearchOptions { SetCode = "SET" }).ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.Contains(cards["1"], results.Select(x => x.MtGCard).ToList());
            Assert.Contains(cards["2"], results.Select(x => x.MtGCard).ToList());
        }

        [Test]
        public void TestSearchCollectionWithFailToFind()
        {
            var results = ops.SearchCollection("Main", new SearchOptions { Name = "NO NAME" }).ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(0));

            results = ops.SearchCollection("Main", new SearchOptions { Name = "NO NAME", SetCode = "SET" }).ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestGetCards()
        {
            var results = ops.GetCards("TEST NAME").ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.Contains(collectionCards["Main1"], results);
            Assert.Contains(collectionCards["Incoming1"], results);

            results = ops.GetCards("", "NOT").ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.Contains(collectionCards["SpecialList3"], results);

            results = ops.GetCards("", "BOT").ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestGetCardByID()
        {
            var result = ops.GetCardByID("1");
            Assert.NotNull(result);
            Assert.That(result.MtGCard, Is.EqualTo(cards["1"]));
        }

        [Test]
        public void TestListCards()
        {
            var results = ops.ListCards("Main").ToList();
            var r = results.Select(x => x.Card).ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.Contains(collectionCards["Main1"], r);
            Assert.Contains(collectionCards["Main2"], r);

            results = ops.ListCards("SpecialList").ToList();
            r = results.Select(x => x.Card).ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.Contains(collectionCards["SpecialList3"], r);

            results = ops.ListCards("IDoNotExistList").ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestBulkAddCards()
        {
            var results = ops.ListCards("Main").ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));

            var newCards = ops.BulkAddCards("Main", new List<CSVItem> {
                new CSVItem{ CollectorNumber = "1", Set = "NOT", Quantity = 1 },
                new CSVItem{ CollectorNumber = "123", Set = "SET", Quantity = 2 },
            });
            Assert.NotNull(newCards);
            Assert.That(newCards.Count(), Is.EqualTo(2));

            results = ops.ListCards("Main").ToList();
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(3));

            var updatedCard = results.Where(x => x.MtGCard.Id == "1").First();
            Assert.That(updatedCard.MtGCard, Is.EqualTo(cards["1"]));
            Assert.That(updatedCard.Card?.Quantity, Is.EqualTo(4));

            var newCard = results.Where(x => x.MtGCard.Id == "3").First();
            Assert.That(newCard.MtGCard, Is.EqualTo(cards["3"]));
            Assert.That(newCard.Card?.Quantity, Is.EqualTo(1));
        }
    }
}
