using HomeTG.API.Models.Contexts.Options;
using HomeTG.Tests.Helpers;

namespace HomeTG.API.Models.Contexts.Tests
{
    [TestFixture]
    public class MTGDBTest
    {
        MTGDB dbContext;
        List<Card> entities = new List<Card>
        {
            new Card("1", "TEST NAME", "SET", "123", "R", "Artist 1", "B,G", "Win the game."),
            new Card("2", "TESTS MANE", "SET", "124", "C", "Artist 2", "U", "Lose the game."),
            new Card("3", "TESTS MANE", "SET2", "124", "C", "Artist 2", "U", "Lose the game."),
            new Card("4", "Some Card", "BET", "12", "C", "Artist 3", "U", "2/2."),
            new Card("5", "Storm Crow", "ALL", "36a", "C", "Sandra Everingham", "U", "Lose the game.")
        };

        List<CardIdentifiers> identifiers = new List<CardIdentifiers>
        {
            new CardIdentifiers("1", "Scry1"),
            new CardIdentifiers("2", "Scry2"),
            new CardIdentifiers("3", "Scry3"),
            new CardIdentifiers("4", "Scry4"),
            new CardIdentifiers("5", "Scry5")
        };

        [OneTimeSetUp]
        public void Setup()
        {
            dbContext = TestHelpers.GetTestEmptyMTGDB();
            dbContext.AddRange(identifiers);
            dbContext.AddRange(entities);
            dbContext.SaveChanges();
        }

        [Test]
        public void SearchCardsTest()
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
        public void SearchCardsPageSizeTest()
        {
            var results = dbContext.SearchCards(new SearchOptions{ Name = "TESTS MANE"}, 1, 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Name, Is.EqualTo("TESTS MANE"));
            Assert.That(results.First().SetCode, Is.EqualTo("SET"));

            results = dbContext.SearchCards(new SearchOptions{ Name = "TESTS MANE"}, 1, 1);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Name, Is.EqualTo("TESTS MANE"));
            Assert.That(results.First().SetCode, Is.EqualTo("SET2"));

            results = dbContext.SearchCards(new SearchOptions{ Name = "TESTS MANE"}, 2, 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.That(results.First().Name, Is.EqualTo("TESTS MANE"));
            Assert.That(results.Last().Name, Is.EqualTo("TESTS MANE"));
        }

        [Test]
        public void BulkSearchCardsTest()
        {
            var results = dbContext.BulkSearchCards(new List<StrictSearchOptions> { new StrictSearchOptions("123", "SET") });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results[("123", "SET")].Name, Is.EqualTo("TEST NAME"));

            results = dbContext.BulkSearchCards(new List<StrictSearchOptions> {
                new StrictSearchOptions("123", "SET"),
                new StrictSearchOptions("124", "SET")
            });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.That(results[("123", "SET")].Name, Is.EqualTo("TEST NAME"));
            Assert.That(results[("124", "SET")].Name, Is.EqualTo("TESTS MANE"));
        }

        [Test]
        public void BulkSearchCardsWithFailToFindTest()
        {
            var results = dbContext.BulkSearchCards(new List<StrictSearchOptions> { new StrictSearchOptions("126", "SET") });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(0));

            results = dbContext.BulkSearchCards(new List<StrictSearchOptions> {
                new StrictSearchOptions("123", "SET"),
                new StrictSearchOptions("126", "SET")
            });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results[("123", "SET")].Name, Is.EqualTo("TEST NAME"));
        }

        [Test]
        public void GetCardsTest()
        {
            var results = dbContext.GetCards(new List<string> { "1" });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results["1"].Name, Is.EqualTo("TEST NAME"));
        }

        [Test]
        public void GetCardsWithFailToFindTest()
        {
            var results = dbContext.GetCards(new List<string> { "6doesnotexist" });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(0));

            results = dbContext.GetCards(new List<string> { "1", "7alsoismissing" });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results["1"].Name, Is.EqualTo("TEST NAME"));
        }
    }
}
