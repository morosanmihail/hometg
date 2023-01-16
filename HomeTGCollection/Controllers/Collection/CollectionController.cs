using HomeTG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeTGCollection.Controllers.Collection
{
    [Route("collection")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private CollectionDB _db;
        private DB _mtgdb;
        public CollectionController(CollectionDB db, DB mtgdb)
        {
            _db = db;
            _mtgdb = mtgdb;
        }

        [HttpGet("cards/{name}")]
        public IEnumerable<CollectionCard> GetCards(string name, string? set = null)
        {
            var cards = _mtgdb.SearchCards(new SearchOptions { Name = name, Set = set });
            var cardsInCollection = _db.GetCards(cards.Select(c => c.Id).ToList());
            return cardsInCollection;
        }

        [HttpPut("cards/add")]
        public CollectionCard AddCards(string id, int quantity = 0, int foilquantity = 0)
        {
            return _db.AddCard(id, quantity, foilquantity);
        }

        [HttpPost("cards/delete")]
        public CollectionCard RemoveCards(string id, int quantity = 0, int foilquantity = 0)
        {
            return _db.RemoveCard(id, quantity, foilquantity);
        }

        [HttpGet("cards/list")]
        public IEnumerable<CollectionCard> ListCards(int offset = 0)
        {
            return _db.ListCards(offset).ToList();
        }
    }
}
