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
    }

    public class StrictSearchOptions
    {
        public string Name { get; set; }
        public string SetCode { get; set; }

        public StrictSearchOptions(string name, string set) 
        {
            this.Name = name;
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

            IQueryable<Card> cards = Cards.AsQueryable();

            if (searchOptions.Name != null && searchOptions.Name.Length > 0)
            {
                cards = cards.Where(c => (c.Name!.ToLower().Contains(searchOptions.Name)));
            }

            if (searchOptions.SetCode != null && searchOptions.SetCode.Length > 0)
            {
                cards = cards.Where(c => c.SetCode!.ToLower().Equals(searchOptions.SetCode));
            }

            return cards;
        }

        public IEnumerable<Card> GetCards(List<string> ids)
        {
            return Cards.Where(c => ids.Contains(c.Id!));
        }
    }
}
