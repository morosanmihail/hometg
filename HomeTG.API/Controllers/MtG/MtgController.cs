using HomeTG.API.Models;
using HomeTG.API.Models.Contexts;
using HomeTG.API.Models.Contexts.Options;
using HomeTG.API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HomeTG.API.Controllers.MtG
{
    [Route("mtg")]
    [ApiController]
    public class MtgController : ControllerBase
    {
        private MTGDB _db;
        public MtgController(MTGDB db)
        {
            _db = db;
        }

        [HttpPost("cards/search")]
        public IEnumerable<Card> SearchCards(SearchOptions options, int pageSize = 24, int offset = 0)
        {
            return _db.SearchCards(options, pageSize, offset);
        }

        [HttpGet("cards")]
        public Dictionary<string, Card> GetCards([FromQuery] string[] ids)
        {
            return _db.GetCards(ids.ToList());
        }

        [HttpGet("update")]
        public async Task<string> UpdateDatabase()
        {
            var downloadNeeded = await DBFiles.DownloadPrintingsDBIfNotExists(@"https://mtgjson.com/api/v5/AllPrintings.sqlite", @"DB/AllPrintings.db");
            return downloadNeeded ? "New version downloaded." : "No new version needed.";
        }

        [HttpGet("sets")]
        public IEnumerable<string> GetSets()
        {
            return _db.Cards.Select(c => c.SetCode).Distinct();
        }
    }
}
