using HomeTG.Models;
using HomeTG.Models.Contexts;
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
        List<CollectionCard> collectionCards = new List<CollectionCard>
        {
            new CollectionCard("1", 2, 0, "Main", null),
            new CollectionCard("2", 1, 0, "Main", null),
            new CollectionCard("1", 1, 0, "Incoming", null),
            new CollectionCard("3", 2, 0, "SpecialList", null),
        };
        List<Card> cards = new List<Card>
        {
            new Card("1", "TEST NAME", "SET", "123", "R", "Artist 1", "B,G", "Win the game."),
            new Card("2", "TESTS MANE", "SET", "124", "C", "Artist 2", "U", "Lose the game.")
        };
        List<CardIdentifiers> identifiers = new List<CardIdentifiers>
        {
            new CardIdentifiers("1", "Scry1"),
            new CardIdentifiers("2", "Scry2")
        };

        [OneTimeSetUp]
        public void Setup()
        {
            MTGDB mtgDBContext = TestHelpers.GetTestEmptyMTGDB();
            mtgDBContext.AddRange(identifiers);
            mtgDBContext.AddRange(cards);
            mtgDBContext.SaveChanges();

            CollectionDB dbContext = TestHelpers.GetTestCollectionDB();
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
