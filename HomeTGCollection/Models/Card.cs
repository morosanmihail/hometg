using HomeTG.Models.Contexts.Options;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTG.Models
{
    [Table("cards")]
    [PrimaryKey("Id")]
    public class Card
    {
        public Card(
            string id, string name, string setCode, string collectorNumber, 
            string? rarity, string? artist, string? colorIdentity, string? text
        )
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SetCode = setCode ?? throw new ArgumentNullException(nameof(setCode));
            CollectorNumber = collectorNumber ?? throw new ArgumentNullException(nameof(collectorNumber));
            Rarity = rarity;
            Artist = artist;
            ColorIdentity = colorIdentity;
            Text = text;
        }

        [Key]
        [Column("uuid")]
        [ForeignKey("CardIdentifiers")]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("setcode")]
        public string SetCode { get; set; }

        [Column("number")]
        public string CollectorNumber { get; set; }

        [Column("rarity")]
        public string? Rarity { get; set; }

        [Column("artist")]
        public string? Artist { get; set; }

        [Column("colorIdentity")]
        public string? ColorIdentity { get; set; }

        [Column("text")]
        public string? Text { get; set; }

        public CardIdentifiers? CardIdentifiers { get; set; }
    }

    [Table("cardIdentifiers")]
    [PrimaryKey("Id")]
    public class CardIdentifiers
    {
        public CardIdentifiers(string id, string? scryfallId)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            ScryfallId = scryfallId;
        }

        [Column("uuid")]
        public string Id { get; set; }

        [Column("scryfallId")]
        public string? ScryfallId { get; set; }
    }
}
