using HomeTG.API.Models.Contexts.Options;
using Microsoft.EntityFrameworkCore;

namespace HomeTG.API.Models.Contexts
{
    public class MTGDB : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardIdentifiers> CardIdentifiers { get; set; }

        public MTGDB(DbContextOptions<MTGDB> options) : base(options)
        { }

        public IEnumerable<Card> SearchCards(SearchOptions searchOptions)
        {
            return Search.SearchCards(Cards.Include(x => x.CardIdentifiers).Select(x => x), searchOptions);
        }

        public Dictionary<(string, string), Card> BulkSearchCards(List<StrictSearchOptions> searchOptions)
        {
            var itemsList = new List<(string, string)> { };
            for (int i = 0; i < searchOptions.Count; i++)
            {
                itemsList.Add((searchOptions[i].CollectorNumber, searchOptions[i].SetCode));
            }

            var matchingCardsTest = Cards.Include(x => x.CardIdentifiers).AsEnumerable().
                Where(c => itemsList.Any(t => c.CollectorNumber == t.Item1 && c.SetCode == t.Item2)).
                GroupBy(c => (c.CollectorNumber, c.SetCode)).
                ToDictionary(c => c.Key, c => c.First());

            return matchingCardsTest;
        }

        public Dictionary<string, Card> GetCards(List<string> ids)
        {
            return Cards.Include(x => x.CardIdentifiers).Where(c => ids.Contains(c.Id!)).ToDictionary(c => c.Id, c => c);
        }
    }
}
