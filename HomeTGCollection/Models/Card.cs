using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTG.Models
{
    [Table("cards")]
    [PrimaryKey("Id")]
    public class Card
    {
        [Column("uuid")]
        public string Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("setcode")]
        public string? SetCode { get; set; }

        [Column("scryfallId")]
        public string? ScryfallId { get; set; }
    }
}
