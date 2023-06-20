using NUnit.Framework;
using HomeTG.API.Models.Contexts;
using HomeTG.Tests.Helpers;

namespace HomeTG.API.Models.Contexts.Tests
{
    [TestFixture]
    public class CollectionDBTest
    {
        CollectionDB dbContext;
        Dictionary<string, CollectionCard> cards = new Dictionary<string, CollectionCard>
        {
            {"Main1", new CollectionCard("1", 2, 0, "Main", null) },
            {"Main2", new CollectionCard("2", 1, 0, "Main", null) },
            {"Incoming1", new CollectionCard("1", 1, 0, "Incoming", null) },
            {"SpecialList3", new CollectionCard("3", 2, 0, "SpecialList", null) },
        };

        List<Collection> collections = new List<Collection>
        {
            new Collection("Main")
        };

        [SetUp]
        public void Setup()
        {
            dbContext = TestHelpers.GetTestCollectionDB();
            dbContext.AddRange(cards.Values);
            dbContext.AddRange(collections);
            dbContext.SaveChanges();
        }

        [Test]
        public void ListCardsTest()
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
        public void AddCardsTest()
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
        public void AddCardsWrongCollectionTest()
        {
            var results = dbContext.AddCards("Incoming", new List<CollectionCard>()
            {
                new CollectionCard("3", 0, 4, "Outgoing", null)
            });
            Assert.NotNull(results);

            var resultsAgain = dbContext.AddCards("Incoming", new List<CollectionCard>()
            {
                new CollectionCard("3", 0, 4, "Outgoing", null)
            });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().FoilQuantity, Is.EqualTo(8));
            Assert.That(results.First().CollectionId, Is.EqualTo("Incoming"));
        }

        [Test]
        public void GetCardsTest()
        {
            var results = dbContext.GetCards(new List<string> { "1" });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results["1"].Count(), Is.EqualTo(2));
        }

        [Test]
        public void RemoveCardTest()
        {
            // Remove one quantity
            var result = dbContext.RemoveCard(
                new CollectionCard("3", 1, 0, "SpecialList", null)
            );
            Assert.NotNull(result);

            var results = dbContext.ListCards("SpecialList", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));

            var getResults = dbContext.GetCards(new List<string> { "3" });
            Assert.NotNull(getResults);
            Assert.That(getResults["3"].Count(), Is.EqualTo(1));
            Assert.That(getResults["3"].First().Quantity, Is.EqualTo(1));

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

        [Test]
        public void GetCollectionTest()
        {
            var result = dbContext.GetCollection("Main");
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo("Main"));

            result = dbContext.GetCollection("NoExisty");
            Assert.Null(result);
        }

        [Test]
        public void GetOrCreateCollectionTest()
        {
            var result = dbContext.GetOrCreateCollection("Main");
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo("Main"));

            result = dbContext.GetOrCreateCollection("NoExisty");
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo("NoExisty"));
        }

        [Test]
        public void ListCollectionsTest()
        {
            var results = dbContext.ListCollections();
            Assert.NotNull(results);
            Assert.That(results.Count, Is.EqualTo(1));

            _ = dbContext.GetOrCreateCollection("NoExisty");

            results = dbContext.ListCollections();
            Assert.NotNull(results);
            Assert.That(results.Count, Is.EqualTo(2));
        }

        [Test]
        public void MoveEntireCardToCollectionTest()
        {
            var results = dbContext.ListCards("Main", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));

            dbContext.GetOrCreateCollection("New");

            results = dbContext.ListCards("New", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(0));

            var moveResults = dbContext.MoveCardsToCollection("New", new List<CollectionCard>
            {
                new CollectionCard("2", 1, 0, "Main", null),
            });
            Assert.NotNull(moveResults);
            Assert.That(moveResults.Count(), Is.EqualTo(1));
            Assert.That(moveResults.First().CollectionId, Is.EqualTo("New"));
            Assert.That(moveResults.First().Id, Is.EqualTo("2"));
            Assert.That(moveResults.First().Quantity, Is.EqualTo(1));
            Assert.That(moveResults.First().FoilQuantity, Is.EqualTo(0));

            results = dbContext.ListCards("Main", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().CollectionId, Is.EqualTo("Main"));
            Assert.That(results.First().Id, Is.EqualTo("1"));
            Assert.That(results.First().Quantity, Is.EqualTo(2));
            Assert.That(results.First().FoilQuantity, Is.EqualTo(0));

            results = dbContext.ListCards("New", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().CollectionId, Is.EqualTo("New"));
            Assert.That(results.First().Id, Is.EqualTo("2"));
            Assert.That(results.First().Quantity, Is.EqualTo(1));
            Assert.That(results.First().FoilQuantity, Is.EqualTo(0));
        }

        [Test]
        public void MoveFragmentCardToCollectionTest()
        {
            var results = dbContext.ListCards("Main", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));

            dbContext.GetOrCreateCollection("New");

            results = dbContext.ListCards("New", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(0));

            var moveResults = dbContext.MoveCardsToCollection("New", new List<CollectionCard>
            {
                new CollectionCard("1", 1, 0, "Main", null),
            });
            Assert.NotNull(moveResults);
            Assert.That(moveResults.Count(), Is.EqualTo(1));
            Assert.That(moveResults.First().CollectionId, Is.EqualTo("New"));
            Assert.That(moveResults.First().Id, Is.EqualTo("1"));
            Assert.That(moveResults.First().Quantity, Is.EqualTo(1));
            Assert.That(moveResults.First().FoilQuantity, Is.EqualTo(0));

            results = dbContext.ListCards("Main", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));

            results = dbContext.ListCards("New", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().CollectionId, Is.EqualTo("New"));
            Assert.That(results.First().Id, Is.EqualTo("1"));
            Assert.That(results.First().Quantity, Is.EqualTo(1));
            Assert.That(results.First().FoilQuantity, Is.EqualTo(0));
        }

        [Test()]
        public void RemoveCardsEntirelyTest()
        {
            var removeResult = dbContext.RemoveCardsEntirely(new List<CollectionCard>
            {
                new CollectionCard("1", 0, 0, "Main", null)
            });
            Assert.NotNull(removeResult);
            Assert.That(removeResult.Count(), Is.EqualTo(1));

            var results = dbContext.ListCards("Main", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
        }
    }
}
