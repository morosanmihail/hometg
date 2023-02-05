using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTG.Models
{
    [Table("cards")]
    [PrimaryKey("Id")]
    public class Card
    {
        public Card(string id, string name, string setCode, string collectorNumber, string? scryfallId)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SetCode = setCode ?? throw new ArgumentNullException(nameof(setCode));
            CollectorNumber = collectorNumber ?? throw new ArgumentNullException(nameof(collectorNumber));
            ScryfallId = scryfallId;
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
    }
}
