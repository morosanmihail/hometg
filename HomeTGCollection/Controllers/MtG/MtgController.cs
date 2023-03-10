using HomeTG.Models;
using HomeTGCollection.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeTG.Controllers.MtG
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
        public IEnumerable<Card> SearchCards(SearchOptions options)
        {
            return _db.SearchCards(options);
        }

        [HttpGet("cards")]
        public IEnumerable<Card> GetCards([FromQuery] string[] ids)
        {
            return _db.GetCards(ids.ToList());
        }

        [HttpGet("update")]
        public async Task<string> UpdateDatabase()
        {
            var downloadNeeded = await DBFiles.DownloadPrintingsDBIfNotExists(@"https://mtgjson.com/api/v5/AllPrintings.sqlite", @"DB/AllPrintings.db");
            return downloadNeeded ? "New version downloaded." : "No new version needed.";
        }
    }
}
