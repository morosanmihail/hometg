using HomeTG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeTG.Controllers.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("Default")]
    public class WebController : Controller
    {
        private CollectionDB _db;
        private MTGDB _mtgdb;
        public WebController(CollectionDB db, MTGDB mtgdb)
        {
            _db = db;
            _mtgdb = mtgdb;
        }

        [Route("ListItems")]
        public IActionResult ListItems(int offset = 0)
        {
            var collectionCards = _db.ListCards(offset, 400);
            var cards = _mtgdb.GetCards(collectionCards.Select(c => c.Id).ToList()).Join(collectionCards, c => c.Id, c => c.Id, (a, b) => new ListViewItem(a, b));
            return View("View", cards);
        }

        [Route("GetItems")]
        public IActionResult GetItems(string Id)
        {
            var card = _db.GetCards(new List<string> { Id }).First();
            var cardDetails = _mtgdb.GetCards(new List<string> { Id }).First();
            return View("CardWithDetails", new ListViewItem(cardDetails, card));
        }
    }
}
