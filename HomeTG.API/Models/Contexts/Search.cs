using HomeTG.API.Models.Contexts.Options;

namespace HomeTG.API.Models.Contexts
{
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
