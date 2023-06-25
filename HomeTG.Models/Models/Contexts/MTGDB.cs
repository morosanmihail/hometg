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

        public IEnumerable<Card> SearchCards(SearchOptions searchOptions, int pageSize = 24, int offset = 0)
        {
            return Search.SearchCards(
                Cards.Include(x => x.CardIdentifiers).Select(x => x),
                searchOptions,
                pageSize, offset
            );
        }

        public Dictionary<(string, string), Card> BulkSearchCards(List<StrictSearchOptions> searchOptions, int pageSize = 24, int offset = 0)
        {
            return Search.BulkSearchCards(
                Cards.Include(x => x.CardIdentifiers).Select(x => x).AsEnumerable(),
                searchOptions,
                pageSize, offset
            );
        }

        public Dictionary<string, Card> GetCards(List<string> ids)
        {
            return Cards.Include(x => x.CardIdentifiers).Where(c => ids.Contains(c.Id!)).ToDictionary(c => c.Id, c => c);
        }
    }
}
