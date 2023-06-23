using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTG.API.Models
{
    [Table("cards")]
    [PrimaryKey("Id", "CollectionId")]
    [Index(nameof(Id), nameof(CollectionId))]
    public class CollectionCard
    {
        [Key]
        [Column("uuid")]
        public string Id { get; set; }

        [Column("quantity")]
        public Int32 Quantity { get; set; }

        [Column("foilquantity")]
        public Int32 FoilQuantity { get; set; }

        [Key]
        [Column("collection")]
        public string CollectionId { get; set; }

        [Column("timeadded")]
        public DateTime? TimeAdded { get; set; }

        public CollectionCard(string id, int quantity, int foilQuantity, string collection, DateTime? timeAdded)
        {
            Id = id;
            Quantity = quantity;
            FoilQuantity = foilQuantity;
            CollectionId = collection;
            TimeAdded = timeAdded;
        }

        public CollectionCard()
        {
            Id = "";
            CollectionId = "";
        }
    }

    [Table("collection")]
    public class Collection
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        public Collection(string id)
        {
            Id = id;
        }
    }
}
