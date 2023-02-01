using HomeTG.Models;
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

        private static Dictionary<string, ImportTask> _tasks = new Dictionary<string, ImportTask>();

        public WebController(CollectionDB db, MTGDB mtgdb)
        {
            _db = db;
            _mtgdb = mtgdb;
        }

        [Route("/")]
        public IActionResult Index()
        {
            var collectionCards = _db.ListCards(0, 24);
            var cards = _mtgdb.GetCards(collectionCards.Select(c => c.Id).ToList()).Join(collectionCards, c => c.Id, c => c.Id, (a, b) => new ListViewItem(a, b));
            return View("View", cards);
        }

        [Route("/ListItems")]
        public IActionResult ListItems(int offset = 0)
        {
            var collectionCards = _db.ListCards(offset, 24);
            var cards = _mtgdb.GetCards(collectionCards.Select(c => c.Id).ToList()).Join(collectionCards, c => c.Id, c => c.Id, (a, b) => new ListViewItem(a, b));
            return View("ListView", cards);
        }

        [Route("GetItem")]
        public IActionResult GetItem(string Id)
        {
            var card = _db.GetCards(new List<string> { Id }).First();
            var cardDetails = _mtgdb.GetCards(new List<string> { Id }).First();
            return View("CardWithDetails", new ListViewItem(cardDetails, card));
        }

        [Route("UpdateQuantity")]
        public IActionResult? UpdateQuantity(string Id, int deltaQuantity = 0, int deltaFoilQuantity = 0)
        {
            CollectionCard? card = null;
            if (deltaQuantity <= 0 && deltaFoilQuantity <= 0)
            {
                card = _db.RemoveCardFromCollection(new CollectionCard(Id, -deltaQuantity, -deltaFoilQuantity, null, null));
            } else
            {
                card = _db.AddCardsToCollection(new List<CollectionCard> {
                    new CollectionCard(Id, deltaQuantity, deltaFoilQuantity, null, null)
                }).FirstOrDefault();
            }
            return View("CardDetails", card);
        }

        [Route("Search")]
        public IActionResult Search(string? Name, string? SetCode)
        {
            var cards = _mtgdb.SearchCards(new SearchOptions { Name = Name, SetCode = SetCode }).ToList();
            var cardsInCollection = _db.GetCards(cards.Select(c => c.Id).ToList()).ToDictionary(c => c.Id, c => c);
            return View("ListView", cards.Select(
                c => new ListViewItem(
                    c, 
                    cardsInCollection.ContainsKey(c.Id) ? cardsInCollection[c.Id] : new CollectionCard(c.Id, 0, 0, null, null)
                )
            ));
        }

        [Route("ImportProgress")]
        public ImportTask ImportProgress(string Filename)
        {
            return GetTask(Filename);
        }

        [Route("ImportCSV")]
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
