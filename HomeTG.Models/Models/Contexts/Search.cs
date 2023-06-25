using HomeTG.API.Models.Contexts.Options;
using Microsoft.EntityFrameworkCore;

namespace HomeTG.API.Models.Contexts
{
    public static class Search
    {
        public static IEnumerable<Card> SearchCards(IQueryable<Card> cards, SearchOptions searchOptions, int pageSize, int offset)
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

            return cards.Skip(offset).Take(pageSize);
        }

        public static Dictionary<(string, string), Card> BulkSearchCards(IEnumerable<Card> cards, List<StrictSearchOptions> searchOptions, int pageSize, int offset) {
            var itemsList = new List<(string, string)> { };
            for (int i = 0; i < searchOptions.Count; i++)
            {
                itemsList.Add((searchOptions[i].CollectorNumber, searchOptions[i].SetCode));
            }

            var matchingCardsTest = cards.
                Where(c => itemsList.Any(t => c.CollectorNumber == t.Item1 && c.SetCode == t.Item2)).
                Skip(offset).Take(pageSize).
                GroupBy(c => (c.CollectorNumber, c.SetCode)).
                ToDictionary(c => c.Key, c => c.First());

            return matchingCardsTest;
        }
    }
}
