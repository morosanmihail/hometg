using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTG.Models
{
    [Table("cards")]
    [PrimaryKey("Id")]
    public class Card
    {
        public Card(string id, string name, string setCode, string collectorNumber, string? scryfallId, string? rarity, string? artist, string? colorIdentity, string? text)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SetCode = setCode ?? throw new ArgumentNullException(nameof(setCode));
            CollectorNumber = collectorNumber ?? throw new ArgumentNullException(nameof(collectorNumber));
            ScryfallId = scryfallId;
            Rarity = rarity;
            Artist = artist;
            ColorIdentity = colorIdentity;
            Text = text;
        }

        [Column("uuid")]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("setcode")]
        public string SetCode { get; set; }

        [Column("number")]
        public string CollectorNumber { get; set; }

        [Column("scryfallId")]
        public string? ScryfallId { get; set; }

        [Column("rarity")]
        public string? Rarity { get; set; }

        [Column("artist")]
        public string? Artist { get; set; }

        [Column("colorIdentity")]
        public string? ColorIdentity { get; set; }

        [Column("text")]
        public string? Text { get; set; }
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
