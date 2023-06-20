using BenchmarkDotNet.Attributes;
using HomeTG.API.Models;
using HomeTG.API.Models.Contexts;
using HomeTG.Tests.Helpers;

namespace HomeTG.Benchmarks
{
    public class Benchmarks
    {
        CollectionDB dbContext;
        MTGDB mtgContext;
        private Operations _ops;

        List<CSVItem> itemsToAdd;

        List<CollectionCard> cards = new List<CollectionCard>
        {
            new CollectionCard("1", 2, 0, "Somecollection", null),
            new CollectionCard("2", 1, 0, "a", null),
            new CollectionCard("1", 1, 0, "b", null),
            new CollectionCard("3", 2, 0, "c", null),
            new CollectionCard("5", 3, 0, "d", null),
        };

        [GlobalSetup]
        public void GlobalSetup()
        {
            dbContext = TestHelpers.GetTestCollectionDB();
            mtgContext = TestHelpers.GetActualMTGDB();

            _ops = new Operations(dbContext, mtgContext);

            var file1 = TestHelpers.GetTestFile("short_list.csv");
            itemsToAdd = CSVOperations.ImportFromCSV(file1);
        }

        [Benchmark]
        [Arguments(1)]
        [Arguments(3)]
        [Arguments(5)]
        public List<CollectionCard> TestCollectionAdd(int count)
        {
            return dbContext.AddCards("Somecollection", cards.Take(count).ToList());
        }

        [Benchmark]
        public List<CollectionCard> TestBulkCollectionAdd()
        {
            return _ops.BulkAddCards("collection", itemsToAdd).ToList();
        }
    }
}
