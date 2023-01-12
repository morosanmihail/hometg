using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTG.Models
{
    [Table("cards")]
    public class Card
    {
        [Column("uuid")]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("setcode")]
        public string SetCode { get; set; }
    }
}
