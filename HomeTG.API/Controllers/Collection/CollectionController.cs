﻿using HomeTG.API.Models;
using HomeTG.API.Models.Contexts;
using HomeTG.API.Models.Contexts.Options;
using Microsoft.AspNetCore.Mvc;

namespace HomeTG.API.Controllers.Collection
{
    [Route("collection")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private CollectionDB _db;
        private MTGDB _mtgdb;

        private Operations _ops;

        private static Dictionary<string, ImportTask> _tasks = new Dictionary<string, ImportTask>();

        public CollectionController(CollectionDB db, MTGDB mtgdb)
        {
            _db = db;
            _mtgdb = mtgdb;
            _ops = new Operations(_db, _mtgdb);
        }

        [HttpGet("list")]
        public IEnumerable<HomeTG.API.Models.Collection> ListCollections()
        {
            return _db.ListCollections();
        }

        [HttpPost("add")]
        public HomeTG.API.Models.Collection AddCollection(HomeTG.API.Models.Collection collection)
        {
            return _db.GetOrCreateCollection(collection.Id);
        }

        [HttpPost("remove/{collection}")]
        public HomeTG.API.Models.Collection? RemoveCollection(string collection, string keepCardsInCollection = "")
        {
            return _db.RemoveCollection(collection, keepCardsInCollection);
        }

        [HttpPost("move/{from}/{to}")]
        public IEnumerable<Models.CollectionCard> MoveCardsToCollection(string from, string to, [FromQuery] List<CollectionCard> cards)
        {
            // TODO
            return cards;
        }

        [HttpGet("cards/{collection}/{id}")]
        public CollectionCard? GetCard(string collection, string id)
        {
            return _ops.GetCard(collection, id);
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
        public async Task<IEnumerable<CollectionCard>> UploadCardsList(string collection, IFormFile file, Dictionary<string, string>? customMapping = null)
        {
            // TODO: use long running tasks
            //var task = CreateTask("test", 10);
            //Task.Run(() => LongRunningImport("test"));
            //return task;

            var filePath = Path.GetTempFileName();
            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            var items = CSVOperations.ImportFromCSV(filePath, customMapping);
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

        [HttpGet("progress/{filename}")]
        public ImportTask ImportProgress(string Filename)
        {
            return GetTask(Filename);
        }

        private void LongRunningImport(string Filename)
        {
            var task = GetTask(Filename);
            for (int i = 1; i <= task.Total; i++)
            {
                Thread.Sleep(1000);
                task.Current = i;
            }
            RemoveTask(Filename);
        }

        private ImportTask GetTask(string Filename)
        {
            return _tasks.ContainsKey(Filename) ? _tasks[Filename] : new ImportTask(Filename, 1, 1);
        }

        private ImportTask CreateTask(string Filename, int Total)
        {
            var task = new ImportTask("test", Total, 0);

            _tasks.Add(Filename, task);

            return task;
        }

        private void RemoveTask(string Filename)
        {
            _tasks.Remove(Filename);
        }
    }
}
