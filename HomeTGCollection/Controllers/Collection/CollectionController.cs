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
            var cards = _mtgdb.SearchCards(new SearchOptions { Name = name, Set = set });
            var cardsInCollection = _db.GetCards(cards.Select(c => c.Id).ToList());
            // cardsInCollection.Join(cards, c => c.Id, cdb => cdb.Id, (c, cdb) => new { c.Id, Scryfall = cdb.ScryfallId }).ToList();
            return cardsInCollection;
        }

        [HttpPut("cards/add")]
        public IEnumerable<CollectionCard> AddCards(string id, int quantity = 0, int foilquantity = 0)
        {
            var card = new CollectionCard { Id = id, Quantity = quantity, FoilQuantity = foilquantity };
            return _db.AddCards(new List<CollectionCard> { card });
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

        [HttpPost("cards/add/bulk")]
        public async Task<IEnumerable<CollectionCard>> UploadCardsList(IFormFile file)
        {
            var filePath = Path.GetTempFileName();
            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            var cardsAdded = new List<CollectionCard>();
            var cardsFailedToFind = new List<CSVItem>();

            var items = ImportFromCSV(filePath);
            System.IO.File.Delete(filePath);

            for (int i=0; i<items.Count; i++)
            {
                var matchingCards = _mtgdb.SearchCards(new SearchOptions { Name = items[i].Name, Set = items[i].Set }).ToList();
                if (matchingCards.Count() > 0)
                {
                    cardsAdded.Add(new CollectionCard { Id = matchingCards[0].Id, Quantity = items[i].Quantity, FoilQuantity = items[i].FoilQuantity, LastUpdated = DateTime.UtcNow });
                } else
                {
                    cardsFailedToFind.Add(items[i]);
                }
            }
            _db.AddCards(cardsAdded);

            return cardsAdded;
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
