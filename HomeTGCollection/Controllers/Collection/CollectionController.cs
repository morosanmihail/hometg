﻿using CsvHelper.Configuration;
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

        [HttpPut("cards/add")]
        public IEnumerable<CollectionCard> AddCards([FromQuery] CollectionCard card)
        {
            return _db.AddCardsToCollection(new List<CollectionCard> { card });
        }

        [HttpPost("cards/delete")]
        public CollectionCard? RemoveCards([FromQuery] CollectionCard card)
        {
            return _db.RemoveCardFromCollection(card);
        }

        [HttpGet("cards/list")]
        public IEnumerable<CollectionCard> ListCards(int offset = 0)
        {
            return _db.ListCards(offset).ToList();
        }

        [HttpPost("cards/import")]
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
                var matchingCards = _mtgdb.SearchCards(new SearchOptions { Name = items[i].Name, SetCode = items[i].Set }).ToList();
                if (matchingCards.Count() > 0)
                {
                    cardsAdded.Add(new CollectionCard(matchingCards[0].Id, items[i].Quantity, items[i].FoilQuantity, null, DateTime.UtcNow));
                } else
                {
                    cardsFailedToFind.Add(items[i]);
                }
            }
            _db.AddCardsToCollection(cardsAdded);

            return cardsAdded;
        }

        [HttpPut("incoming/add")]
        public IEnumerable<CollectionCard> AddIncomingCard([FromQuery] CollectionCard card)
        {
            return _db.AddCardsToIncoming(new List<CollectionCard> { card });
        }

        [HttpGet("incoming/list")]
        public IEnumerable<CollectionCard> ListIncoming(int offset = 0)
        {
            return _db.ListIncoming(offset).ToList();
        }

        [HttpPost("incoming/delete")]
        public CollectionCard? RemoveIncomingCards([FromQuery] CollectionCard card)
        {
            return _db.RemoveCardFromIncoming(card);
        }

        [HttpGet("incoming/count")]
        public int IncomingCount()
        {
            return _db.IncomingCards.Count();
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
