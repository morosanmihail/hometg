using HomeTG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeTG.Controllers.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    public class WebController : Controller
    {
        private CollectionDB _db;
        private MTGDB _mtgdb;
        public WebController(CollectionDB db, MTGDB mtgdb)
        {
            _db = db;
            _mtgdb = mtgdb;
        }

        [Route("/")]
        public IActionResult ListItems(int offset = 0)
        {
            var collectionCards = _db.ListCards(offset, 400);
            var cards = _mtgdb.GetCards(collectionCards.Select(c => c.Id).ToList()).Join(collectionCards, c => c.Id, c => c.Id, (a, b) => new ListViewItem(a, b));
            return View("View", cards);
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
            return View("ListView", cards.Select(c => new ListViewItem(c, null)));
        }
    }
}
