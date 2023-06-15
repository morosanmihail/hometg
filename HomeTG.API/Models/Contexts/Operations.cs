using HomeTG.Models.Contexts.Options;

namespace HomeTG.Models.Contexts
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

        public IEnumerable<CollectionCardWithDetails> SearchCollection(string collection, SearchOptions searchOptions)
        {
            var cards = _mtgdb.SearchCards(searchOptions).ToList();
            var cardsInCollection = _db.GetCards(
                cards.Select(c => c.Id).ToList()
            );

            if (collection.Length > 0)
            {
                cardsInCollection = cardsInCollection.Where(c => c.CollectionId == collection);
            }

            var groupedCards = cardsInCollection.GroupBy(c => c.Id).
                ToDictionary(c => c.Key, c => c.ToList());

            return cards.Select(
                c => new CollectionCardWithDetails(
                    c,
                    groupedCards.ContainsKey(c.Id) ? groupedCards[c.Id].First() : new CollectionCard(c.Id, 0, 0, "", null)
                )
            );
        }

        public int Count(string collection)
        {
            return _db.Cards.Where(c => c.CollectionId.ToLower() == collection.ToLower()).Count();
        }

        public IEnumerable<CollectionCard> GetCards(string name, string? set = null)
        {
            var cards = _mtgdb.SearchCards(new SearchOptions { Name = name, SetCode = set });
            var cardsInCollection = _db.GetCards(cards.Select(c => c.Id!).ToList());
            // cardsInCollection.Join(cards, c => c.Id, cdb => cdb.Id, (c, cdb) => new { c.Id, Scryfall = cdb.ScryfallId }).ToList();
            return cardsInCollection;
        }

        public CollectionCardWithDetails GetCardByID(string id)
        {
            // TODO: this is potentially undesirable behaviour. And unclear.
            // This should return all items from the collections with that ID.
            var card = _db.GetCards(new List<string> { id }).First();
            var cardDetails = _mtgdb.GetCards(new List<string> { id }).First();
            return new CollectionCardWithDetails(cardDetails, card);
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
                        l.First().LastUpdated
                    )).ToList();

            _db.AddCards(collection, cardsToAdd);

            return cardsToAdd;
        }

        public IEnumerable<CollectionCardWithDetails> ListCards(string collection, int offset = 0)
        {
            var collectionCards = _db.ListCards(collection, offset, 12);
            var cards = _mtgdb.GetCards(collectionCards.Select(c => c.Id).ToList()).
                Join(collectionCards, c => c.Id, c => c.Id, (a, b) => new CollectionCardWithDetails(a, b));
            return cards;
        }
    }
}
