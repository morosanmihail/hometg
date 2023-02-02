using CsvHelper.Configuration;
using CsvHelper;
using HomeTG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HomeTGCollection.Controllers.Collection
{
    [Route("collection")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private CollectionDB _db;
        private MTGDB _mtgdb;
        public CollectionController(CollectionDB db, MTGDB mtgdb)
        {
            _db = db;
            _mtgdb = mtgdb;
        }

        [HttpGet("cards/get/{name}")]
        public IEnumerable<CollectionCard> GetCards(string name, string? set = null)
        {
            var cards = _mtgdb.SearchCards(new SearchOptions { Name = name, SetCode = set });
            var cardsInCollection = _db.GetCards(cards.Select(c => c.Id!).ToList());
            // cardsInCollection.Join(cards, c => c.Id, cdb => cdb.Id, (c, cdb) => new { c.Id, Scryfall = cdb.ScryfallId }).ToList();
            return cardsInCollection;
        }

        [HttpPut("cards/{collection}/add")]
        public IEnumerable<CollectionCard> AddCards(string collection, [FromQuery] CollectionCard card)
        {
            return _db.AddCards(collection, new List<CollectionCard> { card });
        }

        [HttpPost("cards/{collection}/delete")]
        public CollectionCard? RemoveCards(string collection, [FromQuery] CollectionCard card)
        {
            if (collection.ToLower() != card.Collection.ToLower())
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

            var matchingCards = _mtgdb.BulkSearchCards(items.Select(c => new StrictSearchOptions(c.Name, c.Set)).ToList());
            var cardsToAdd = items.Where(c => matchingCards.ContainsKey((c.Name, c.Set))).Select(
                c => new CollectionCard(
                    matchingCards[(c.Name, c.Set)].Id, c.Quantity, c.FoilQuantity, null, DateTime.UtcNow
                )
            ).GroupBy(c => c.Id).
            Select(l => new CollectionCard(
                        l.First().Id,
                        l.Sum(c => c.Quantity),
                        l.Sum(c => c.FoilQuantity),
                        collection,
                        l.First().LastUpdated
                    )).ToList();

            _db.AddCards(collection, cardsToAdd);

            return cardsToAdd;
        }

        [HttpGet("cards/{collection}/count")]
        public int IncomingCount(string collection)
        {
            return _db.Cards.Where(c => c.Collection.ToLower() == collection.ToLower()).Count();
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

    public struct CSVItem
    {
        public string Name { get; set; }
        public string Set { get; set; }
        public int Quantity { get; set; }
        public int FoilQuantity { get; set; }
        public string Acquired { get; set; }
    }
}
