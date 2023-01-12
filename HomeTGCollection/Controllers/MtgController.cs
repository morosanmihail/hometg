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

        [HttpGet("cards/{name}")]
        public IEnumerable<Card> GetCards(string name, string? set = null)
        {
            return _db.GetCards(name, set);
        }
    }
}
