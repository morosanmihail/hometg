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

        public IEnumerable<CollectionCard> SearchCollection(string collection, SearchOptions searchOptions)
        {
            var cards = _mtgdb.SearchCards(searchOptions).ToList();
            var cardsInCollection = _db.GetCards(
                cards.Select(c => c.Id).ToList()
            );

            if (collection.Length > 0)
            {
                cardsInCollection = cardsInCollection.Where(c => c.CollectionId == collection);
            }

            return cardsInCollection;
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
}
