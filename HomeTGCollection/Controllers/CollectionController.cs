using HomeTG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeTGCollection.Controllers
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
            var cards = _mtgdb.SearchCards(new SearchOptions { Name = name, Set = set});
            var cardsInCollection = _db.GetCards(cards.Select(c => c.Id).ToList());
            return cardsInCollection;
        }

        [HttpPut("cards/add")]
        public CollectionCard AddCards(string id, Int32 quantity = 0, Int32 foilquantity = 0)
        {
            return _db.AddCard(id, quantity, foilquantity);
        }

        [HttpPost("cards/delete")]
        public CollectionCard RemoveCards(string id, Int32 quantity = 0, Int32 foilquantity = 0)
        {
            return _db.RemoveCard(id, quantity, foilquantity);
        }
    }
}
