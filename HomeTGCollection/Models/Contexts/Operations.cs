using HomeTG.Models.Contexts.Options;
using Moq;

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
            var card = _db.GetCards(new List<string> { id }).First();
            var cardDetails = _mtgdb.GetCards(new List<string> { id }).First();
            return new CollectionCardWithDetails(cardDetails, card);
        }

        public IEnumerable<CollectionCard> BulkAddCards(string collection, List<CSVItem> items)
        {
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

    public static class Search
    {
        public static IEnumerable<Card> SearchCards(IQueryable<Card> cards, SearchOptions searchOptions)
        {
            searchOptions.Name = searchOptions.Name?.ToLower();
            searchOptions.SetCode = searchOptions.SetCode?.ToLower();
            searchOptions.Artist = searchOptions.Artist?.ToLower();
            searchOptions.Text = searchOptions.Text?.ToLower();

            if (searchOptions.Name != null && searchOptions.Name.Length > 0)
            {
                cards = cards.Where(c => c.Name.ToLower().Contains(searchOptions.Name));
            }

            if (searchOptions.SetCode != null && searchOptions.SetCode.Length > 0)
            {
                cards = cards.Where(c => c.SetCode.ToLower().Equals(searchOptions.SetCode));
            }

            if (searchOptions.CollectorNumber != null && searchOptions.CollectorNumber.Length > 0)
            {
                cards = cards.Where(c => c.CollectorNumber.ToLower().Equals(searchOptions.CollectorNumber));
            }

            if (searchOptions.Artist != null && searchOptions.Artist.Length > 0)
            {
                cards = cards.Where(c => c.Artist!.ToLower().Contains(searchOptions.Artist));
            }

            if (searchOptions.ColorIdentities != null && searchOptions.ColorIdentities.Count > 0)
            {
                cards = cards.Where(c => searchOptions.ColorIdentities.All(y => c.ColorIdentity!.Contains(y)));
            }

            if (searchOptions.Text != null && searchOptions.Text.Length > 0)
            {
                cards = cards.Where(c => c.Text!.ToLower().Contains(searchOptions.Text));
            }

            return cards;
        }
    }

    public struct CSVItem
    {
        public string CollectorNumber { get; set; }
        public string Set { get; set; }
        public int Quantity { get; set; }
        public int FoilQuantity { get; set; }
        public string Acquired { get; set; }
    }
}
