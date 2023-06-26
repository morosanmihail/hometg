using HomeTG.API.Models;
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
        public IEnumerable<Models.Collection> ListCollections()
        {
            return _db.ListCollections();
        }

        [HttpPost("add")]
        public Models.Collection AddCollection(Models.Collection collection)
        {
            return _db.GetOrCreateCollection(collection.Id);
        }

        [HttpPost("remove/{collection}")]
        public Models.Collection? RemoveCollection(string collection, string keepCardsInCollection = "")
        {
            return _db.RemoveCollection(collection, keepCardsInCollection);
        }

        [HttpPost("move/{to}")]
        public IEnumerable<CollectionCard> MoveCardsToCollection(string to, List<CollectionCard> cards)
        {
            return _db.MoveCardsToCollection(to, cards);
        }

        [HttpGet("cards/{collection}/get")]
        public Dictionary<string, CollectionCard> GetCards(string collection, [FromQuery] List<string> ids)
        {
            return _db.GetCardsFromCollection(collection, ids);
        }

        [HttpPost("cards/{collection}/add")]
        public IEnumerable<CollectionCard> AddCard(string collection, CollectionCard card)
        {
            return _db.AddCards(collection, new List<CollectionCard> { card });
        }

        [HttpPost("cards/{collection}/delete")]
        public CollectionCard? RemoveCard(string collection, CollectionCard card)
        {
            if (collection.ToLower() != card.CollectionId.ToLower())
            {
                return null;
            }
            return _db.RemoveCard(card);
        }

        [HttpPost("cards/{collection}/remove")]
        public IEnumerable<CollectionCard> FullyRemoveCards(List<CollectionCard> cards)
        {
            return _db.RemoveCardsEntirely(cards);
        }

        [HttpGet("cards/{collection}/list")]
        public IEnumerable<CollectionCard> ListCards(string collection, int offset = 0, int pageSize = 12)
        {
            return _db.ListCards(collection, offset, pageSize).ToList();
        }

        [HttpPost("import")]
        public async Task<int> UploadCardsList([FromForm] ImportModel model)
        {
            // TODO: use long running tasks
            //var task = CreateTask("test", 10);
            //Task.Run(() => LongRunningImport("test"));
            //return task;

            var filePath = Path.GetTempFileName();
            if (model.file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.file.CopyToAsync(stream);
                }
            }

            var items = CSVOperations.ImportFromCSV(filePath, null);
            System.IO.File.Delete(filePath);

            _ops.BulkAddCards(model.collection, items);
            return items.Count;
        }

        [HttpGet("export/{collection}")]
        public FileContentResult ExportCollection(string collection) {
            var filename = CSVOperations.ExportToCSV(_ops.ExportCollection(collection).ToList());
            byte[] fileData = System.IO.File.ReadAllBytes(filename);
            return new FileContentResult(fileData, "text/csv")
            {
                FileDownloadName = filename
            };
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
