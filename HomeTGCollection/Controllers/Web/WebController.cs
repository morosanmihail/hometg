using HomeTG.Models;
using HomeTG.Models.Contexts;
using HomeTG.Models.Contexts.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace HomeTG.Controllers.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    public class WebController : Controller
    {
        private CollectionDB _db;
        private MTGDB _mtgdb;

        private Operations _ops;

        private static Dictionary<string, ImportTask> _tasks = new Dictionary<string, ImportTask>();

        public WebController(CollectionDB db, MTGDB mtgdb)
        {
            _db = db;
            _mtgdb = mtgdb;
            _ops = new Operations(_db, _mtgdb);
        }

        [Route("/ListCollections")]
        public IActionResult ListCollections(string collection)
        {
            return View(
                "ListCollections", 
                new ListCollectionsModel(
                    _db.ListCollections(),
                    collection
                )
            );
        }

        [Route("/{collection?}")]
        public IActionResult Index(string collection = "Main", int offset = 0)
        {
            var collectionCards = _db.ListCards(collection, offset, 12);
            var cards = _mtgdb.GetCards(collectionCards.Select(c => c.Id).ToList()).Join(collectionCards, c => c.Id, c => c.Id, (a, b) => new ListViewItem(a, b));
            var collections = _db.ListCollections();
            return View("View", new MainPageData(
                cards, 
                new ListCollectionsModel(collections, collection)
            ));
        }

        [Route("{collection}/ListItems")]
        public IActionResult ListItems(string collection, int offset = 0)
        {
            var collectionCards = _db.ListCards(collection, offset, 12);
            var cards = _mtgdb.GetCards(collectionCards.Select(c => c.Id).ToList()).Join(collectionCards, c => c.Id, c => c.Id, (a, b) => new ListViewItem(a, b));
            return View("ListView", cards);
        }

        [Route("{collection}/GetItem")]
        public IActionResult GetItem(string Id)
        {
            var card = _db.GetCards(new List<string> { Id }).First();
            var cardDetails = _mtgdb.GetCards(new List<string> { Id }).First();
            return View("CardWithDetails", new ListViewItem(cardDetails, card));
        }

        [Route("{collection}/UpdateQuantity")]
        public IActionResult? UpdateQuantity(string Id, string collection, int deltaQuantity = 0, int deltaFoilQuantity = 0)
        {
            CollectionCard? card = null;
            if (deltaQuantity <= 0 && deltaFoilQuantity <= 0)
            {
                card = _db.RemoveCard(new CollectionCard(Id, -deltaQuantity, -deltaFoilQuantity, collection, null));
            } else
            {
                card = _db.AddCards(collection, new List<CollectionCard> {
                    new CollectionCard(Id, deltaQuantity, deltaFoilQuantity, collection, null)
                }).FirstOrDefault();
            }
            return View("CardDetails", card);
        }

        [Route("Search")]
        public IActionResult Search(SearchOptions searchOptions)
        {
            var cards = _mtgdb.SearchCards(searchOptions).ToList();
            var cardsInCollection = _ops.SearchCollection("", searchOptions).
            GroupBy(c => c.Id).
            ToDictionary(c => c.Key, c => c.ToList());

            // TODO: could group by collection?
            return View("ListView", cards.Select(
                c => new ListViewItem(
                    c, 
                    cardsInCollection.ContainsKey(c.Id) ? cardsInCollection[c.Id].First() : new CollectionCard(c.Id, 0, 0, "", null)
                )
            ));
        }

        [Route("{collection}/Search")]
        public IActionResult SearchCollection(string collection, SearchOptions searchOptions)
        {
            var cards = _mtgdb.SearchCards(searchOptions).ToList();
            var cardsInCollection = _ops.SearchCollection(collection, searchOptions).
            GroupBy(c => c.Id).
            ToDictionary(c => c.Key, c => c.ToList());

            return View("ListView", cards.Where(c => cardsInCollection.ContainsKey(c.Id)).Select(
                c => new ListViewItem(
                    c, cardsInCollection[c.Id].First()
                )
            ));
        }

        [Route("ImportProgress")]
        public ImportTask ImportProgress(string Filename)
        {
            return GetTask(Filename);
        }

        [Route("{collection}/ImportCSV")]
        public ImportTask ImportCSV()
        {
            var task = CreateTask("test", 10);
            Task.Run(() => LongRunningImport("test"));
            return task;
        }

        private void LongRunningImport(string Filename)
        {
            var task = GetTask(Filename);
            for (int i = 1; i <= task.Total; i++)
            {
                Thread.Sleep(1000);
                task.Current = i;
            }
            RemoveTask(Filename);
        }

        private ImportTask GetTask(string Filename)
        {
            return _tasks.ContainsKey(Filename) ? _tasks[Filename] : new ImportTask(Filename, 1, 1);
        }

        private ImportTask CreateTask(string Filename, int Total)
        {
            var task = new ImportTask("test", Total, 0);

            _tasks.Add(Filename, task);

            return task;
        }

        private void RemoveTask(string Filename)
        {
            _tasks.Remove(Filename);
        }
    }
}
