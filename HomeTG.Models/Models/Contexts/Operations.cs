using HomeTG.API.Models.Contexts.Options;

namespace HomeTG.API.Models.Contexts
{
    public class Operations
    {
        private CollectionDB _db;
        private MTGDB _mtgdb;

        public Operations(CollectionDB db, MTGDB mtgdb)
        {
            _db = db;
            _mtgdb = mtgdb;
        }

        public IEnumerable<CollectionCardWithDetails> SearchCollection(string collection, SearchOptions searchOptions, int offset = 0, int pageSize = 12)
        {
            var cards = _mtgdb.SearchCards(searchOptions, pageSize, offset).ToList();
            var cardsInCollection = _db.GetCardsFromCollection(collection, cards.Select(c => c.Id).ToList());

            return cards.Select(
                c => new CollectionCardWithDetails(
                    c,
                    cardsInCollection[c.Id]
                )
            );
        }

        public IEnumerable<CollectionCardWithDetails> SearchAllCollections(SearchOptions searchOptions, int offset = 0, int pageSize = 12, bool skipNotOwned = false)
        {
            var cards = _mtgdb.SearchCards(searchOptions, pageSize, offset).ToList();
            var cardsInCollection = _db.GetCards(cards.Select(c => c.Id).ToList());

            var finalResult = new List<CollectionCardWithDetails>();
            foreach (var card in cards) {
                if (cardsInCollection.ContainsKey(card.Id)) {
                    foreach (var col in cardsInCollection[card.Id]) {
                        finalResult.Add(new CollectionCardWithDetails(card, col));
                    }
                } else {
                    if (!skipNotOwned)
                        finalResult.Add(new CollectionCardWithDetails(card, null));
                }
            }

            return finalResult;
        }

        public int Count(string collection)
        {
            return _db.Cards.Where(c => c.CollectionId.ToLower() == collection.ToLower()).Count();
        }

        public CollectionCard? GetCard(string collectionName, string id)
        {
            var cardInCollections = _db.GetCardsFromCollection(collectionName, new List<string> { id });
            if (cardInCollections.ContainsKey(id))
            {
                return cardInCollections[id];
            }
            return null;
        }

        public IEnumerable<CollectionCardWithDetails> GetCardsByID(List<string> ids)
        {
            var cards = _db.GetCards(ids);
            var cardDetails = _mtgdb.GetCards(ids);
            return cards.SelectMany(p => p.Value).Select(
                c => new CollectionCardWithDetails(cardDetails[c.Id], c)
            );
        }

        public IEnumerable<CollectionCard> BulkAddCards(string collection, List<CSVItem> items)
        {
            if (collection.Length == 0 || items.Count == 0)
            {
                return Enumerable.Empty<CollectionCard>();
            }

            var matchingCards = _mtgdb.BulkSearchCards(items.Select(c => new StrictSearchOptions(c.CollectorNumber, c.Set)).ToList());
            var cardsToAdd = items.Where(c => matchingCards.ContainsKey((c.CollectorNumber, c.Set))).Select(
                c => new CollectionCard(
                    matchingCards[(c.CollectorNumber, c.Set)].Id, c.Quantity, c.FoilQuantity, collection, DateTime.UtcNow
                )
            ).GroupBy(c => c.Id).
            Select(l => new CollectionCard(
                        l.First().Id,
                        l.Sum(c => c.Quantity),
                        l.Sum(c => c.FoilQuantity),
                        collection,
                        l.First().TimeAdded
                    )).ToList();

            _db.AddCards(collection, cardsToAdd);

            return cardsToAdd;
        }

        public IEnumerable<CollectionCardWithDetails> ListCards(string collection, int pageSize = 12, int offset = 0)
        {
            var collectionCards = _db.ListCards(collection, offset, pageSize);
            var cards = _mtgdb.GetCards(collectionCards.Select(c => c.Id).ToList()).
                Join(collectionCards, c => c.Value.Id, c => c.Id, (a, b) => new CollectionCardWithDetails(a.Value, b));
            return cards;
        }

        public IEnumerable<CSVItem> ExportCollection(string collection)
        {
            var collectionCards = _db.Cards.Where(
                c => c.CollectionId.ToLower() == collection.ToLower()
            ).OrderByDescending(c => c.TimeAdded);
            var cards = _mtgdb.GetCards(collectionCards.Select(c => c.Id).ToList()).
                Join(collectionCards, c => c.Value.Id, c => c.Id, (a, b) => new CSVItem{
                    CollectorNumber = a.Value.CollectorNumber,
                    Set = a.Value.SetCode,
                    Quantity = b.Quantity,
                    FoilQuantity = b.FoilQuantity,
                    Acquired = (b.TimeAdded != null) ? b.TimeAdded.ToString() : "",
                });
            return cards;
        }
    }
}
