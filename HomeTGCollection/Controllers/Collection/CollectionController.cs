using CsvHelper.Configuration;
using CsvHelper;
using HomeTG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using HomeTG.Models.Contexts;
using HomeTG.Models.Contexts.Options;

namespace HomeTGCollection.Controllers.Collection
{
    [Route("collection")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private CollectionDB _db;
        private MTGDB _mtgdb;

        private Operations _ops;

        public CollectionController(CollectionDB db, MTGDB mtgdb)
        {
            _db = db;
            _mtgdb = mtgdb;
            _ops = new Operations(_db, _mtgdb);
        }

        [HttpGet("cards/get/{name}")]
        public IEnumerable<CollectionCard> GetCards(string name, string? set = null)
        {
            return _ops.GetCards(name, set);
        }

        [HttpPut("cards/{collection}/add")]
        public IEnumerable<CollectionCard> AddCards(string collection, [FromQuery] CollectionCard card)
        {
            return _db.AddCards(collection, new List<CollectionCard> { card });
        }

        [HttpPost("cards/{collection}/delete")]
        public CollectionCard? RemoveCards(string collection, [FromQuery] CollectionCard card)
        {
            if (collection.ToLower() != card.CollectionId.ToLower())
            {
                return null;
            }
            return _db.RemoveCard(card);
        }

        [HttpGet("cards/{collection}/list")]
        public IEnumerable<CollectionCard> ListCards(string collection, int offset = 0)
        {
            return _db.ListCards(collection, offset).ToList();
        }

        [HttpPost("cards/{collection}/import")]
        public async Task<IEnumerable<CollectionCard>> UploadCardsList(string collection, IFormFile file)
        {
            var filePath = Path.GetTempFileName();
            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            var items = ImportFromCSV(filePath);
            System.IO.File.Delete(filePath);

            return _ops.BulkAddCards(collection, items);
        }

        [HttpGet("cards/{collection}/count")]
        public int Count(string collection)
        {
            return _ops.Count(collection);
        }

        [HttpGet("cards/{collection}/search")]
        public IEnumerable<CollectionCardWithDetails> Search(string collection, SearchOptions searchOptions)
        {
            return _ops.SearchCollection(collection, searchOptions);
        }

        List<CSVItem> ImportFromCSV(string filename)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true
            };

            var items = new List<CSVItem>();
            using (var reader = System.IO.File.OpenText(filename))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                items = csv.GetRecords<CSVItem>().ToList();
            }
            return items;
        }
    }
}
