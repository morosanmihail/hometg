using HomeTG.API.Models.Contexts.Options;

namespace HomeTG.API.Models.Contexts
{
    public class Operations
    {
        private CollectionDB _db;
        private MTGDB _mtgdb;

        const int PAGE_SIZE = 12;
        public Operations(CollectionDB db, MTGDB mtgdb)
        {
            _db = db;
            _mtgdb = mtgdb;
        }

        public IEnumerable<CollectionCardWithDetails> SearchCollection(string collection, SearchOptions searchOptions)
        {
            var cards = _mtgdb.SearchCards(searchOptions).ToList();
            var cardsInCollection = _db.GetCardsFromCollection(collection, cards.Select(c => c.Id).ToList());

            return cards.Select(
                c => new CollectionCardWithDetails(
                    c,
                    cardsInCollection[c.Id]
                )
            );
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

        public IEnumerable<CollectionCardWithDetails> GetCardsByID(string id)
        {
            var cards = _db.GetCards(new List<string> { id })[id];
            var cardDetails = _mtgdb.GetCards(new List<string> { id })[id];
            return cards.Select(
                c => new CollectionCardWithDetails(cardDetails, c)
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
                    matchingCards[(c.CollectorNumber, c.Set)].Id, c.Quantity, c.FoilQuantity, collection, DateTime.UtcNow.Ticks
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

        public IEnumerable<CollectionCardWithDetails> ListCards(string collection, int offset = 0)
        {
            var collectionCards = _db.ListCards(collection, offset, PAGE_SIZE);
            var cards = _mtgdb.GetCards(collectionCards.Select(c => c.Id).ToList()).
                Join(collectionCards, c => c.Value.Id, c => c.Id, (a, b) => new CollectionCardWithDetails(a.Value, b));
            return cards;
        }
    }
}
