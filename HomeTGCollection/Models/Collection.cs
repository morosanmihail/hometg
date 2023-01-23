using HomeTG.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTG.Models
{
    [Table("cards")]
    public class CollectionCard
    {
        [Column("uuid")]
        public string Id { get; set; }

        [Column("quantity")]
        public Int32 Quantity { get; set; }

        [Column("foilquantity")]
        public Int32 FoilQuantity { get; set; }

        [Column("collection")]
        public string? Collection { get; set; }

        [Column("lastupdated")]
        public DateTime? LastUpdated { get; set; }
    }

    [Table("collection")]
    public class Collection
    {
        [Column("id")]
        public string? Id { get; set; }
    }
}
