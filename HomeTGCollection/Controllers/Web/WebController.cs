using HomeTG.Controllers.Web.Models;
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

        [Route("/{collection?}")]
        public IActionResult Index(string collection = "Main", int offset = 0)
        {
            var cards = _ops.ListCards(collection, offset);
            var collections = _db.ListCollections();
            return View("View", new MainPageData(
                cards, 
                new ListCollectionsModel(collections, collection)
            ));
        }

        [Route("{collection}/ListItems")]
        public IActionResult ListItems(string collection, int offset = 0)
        {
            var cards = _ops.ListCards(collection, offset);
            return View("ListView", cards);
        }

        [Route("{collection}/GetItem/{id}")]
        public IActionResult GetItem(string id)
        {
            return View("CardWithDetails", _ops.GetCardByID(id));
        }

        // TODO: this should probably look like CollectionDB.cs/AddCards (or RemoveCard) more
        [Route("{collection}/UpdateQuantity")]
        public IActionResult? UpdateQuantity(string id, string collection, int deltaQuantity = 0, int deltaFoilQuantity = 0)
        {
            CollectionCard? card = null;
            if (deltaQuantity <= 0 && deltaFoilQuantity <= 0)
            {
                card = _db.RemoveCard(new CollectionCard(id, -deltaQuantity, -deltaFoilQuantity, collection, null));
            } else
            {
                card = _db.AddCards(collection, new List<CollectionCard> {
                    new CollectionCard(id, deltaQuantity, deltaFoilQuantity, collection, null)
                }).FirstOrDefault();
            }
            return View("CardDetails", card);
        }

        [Route("Search")]
        public IActionResult Search(SearchOptions searchOptions)
        {
            var cardsInCollection = _ops.SearchCollection("", searchOptions);

            // TODO: could group by collection?
            return View("ListView", cardsInCollection);
        }

        [Route("{collection}/Search")]
        public IActionResult SearchCollection(string collection, SearchOptions searchOptions)
        {
            var cardsInCollection = _ops.SearchCollection(collection, searchOptions);

            return View("ListView", cardsInCollection);
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
