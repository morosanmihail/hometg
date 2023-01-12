using HomeTG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeTG.Controllers
{
    [Route("api/mtg")]
    [ApiController]
    public class MtgController : ControllerBase
    {
        private DB _db;
        public MtgController(DB db)
        {
            _db = db;
        }

        [HttpPost("cards/search")]
        public IEnumerable<Card> SearchCards(SearchOptions options)
        {
            return _db.SearchCards(options);
        }

        [HttpGet("cards")]
        public IEnumerable<Card> GetCards([FromQuery] string[] ids)
        {
            return _db.GetCards(ids.ToList());
        }
    }
}
