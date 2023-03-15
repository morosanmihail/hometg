using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace HomeTG.Models
{
    public class MTGDB : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public MTGDB(DbContextOptions<MTGDB> options) : base(options)
        {}

        public IEnumerable<Card> SearchCards(SearchOptions searchOptions)
        {
            return Search.SearchCards(Cards.Select(x => x), searchOptions);
        }

        public Dictionary<(string, string), Card> BulkSearchCards(List<StrictSearchOptions> searchOptions)
        {
            var itemsList = new List<(string, string)> { };
            for (int i = 0; i < searchOptions.Count; i++)
            {
                itemsList.Add((searchOptions[i].CollectorNumber, searchOptions[i].SetCode));
            }

            var matchingCardsTest = Cards.AsEnumerable().
                Where(c => itemsList.Any(t => c.CollectorNumber == t.Item1 && c.SetCode == t.Item2)).
                GroupBy(c => (c.CollectorNumber, c.SetCode)).
                ToDictionary(c => (c.Key), c => c.First());

            return matchingCardsTest;
        }

        public IEnumerable<Card> GetCards(List<string> ids)
        {
            return Cards.Where(c => ids.Contains(c.Id!));
        }
    }
}
