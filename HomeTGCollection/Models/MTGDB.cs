using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace HomeTG.Models
{
    public class SearchOptions
    {
        public string? Name { get; set; }
        public string? SetCode { get; set; }
        public string? CollectorNumber { get; set; }
        public string? Artist { get; set; }
        public List<string>? ColorIdentities { get; set; }
        public string? Text { get; set; }
    }

    public class StrictSearchOptions
    {
        public string CollectorNumber { get; set; }
        public string SetCode { get; set; }

        public StrictSearchOptions(string collectornumber, string set) 
        {
            this.CollectorNumber = collectornumber;
            this.SetCode = set;
        }
    }

    public class MTGDB : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public MTGDB(DbContextOptions<MTGDB> options) : base(options)
        {}

        public IEnumerable<Card> SearchCards(SearchOptions searchOptions)
        {
            searchOptions.Name = searchOptions.Name?.ToLower();
            searchOptions.SetCode = searchOptions.SetCode?.ToLower();
            searchOptions.Artist = searchOptions.Artist?.ToLower();
            searchOptions.Text = searchOptions.Text?.ToLower();

            IQueryable<Card> cards = Cards.Select(x => x);

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

            if(searchOptions.ColorIdentities != null && searchOptions.ColorIdentities.Count > 0)
            {
                cards = cards.Where(c => searchOptions.ColorIdentities.All(y => c.ColorIdentity!.Contains(y)));
            }

            if(searchOptions.Text != null && searchOptions.Text.Length > 0)
            {
                cards = cards.Where(c => c.Text!.ToLower().Contains(searchOptions.Text));
            }

            return cards;
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
