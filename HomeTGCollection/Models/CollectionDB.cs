using Microsoft.EntityFrameworkCore;

namespace HomeTG.Models
{
    public class CollectionDB : DbContext
    {
        public DbSet<CollectionCard> Cards { get; set; }
        public DbSet<Collection> Collection { get; set; }

        public DbSet<CollectionCard> IncomingCards { get; set; }

        public CollectionDB(DbContextOptions<CollectionDB> options) : base(options)
        { }

        public IEnumerable<CollectionCard> GetCards(List<string> ids)
        {
            return Cards.Where(c => ids.Contains(c.Id)).ToList();
        }

        public IEnumerable<CollectionCard> ListCards(int offset)
        {
            return Cards.Skip(offset).Take(50).ToList();
        }

        public CollectionCard AddCard(string id, Int32 quantity = 0, Int32 foilquantity = 0)
        {
            var card = Cards.Find(id);
            if (card != null)
            {
                card.Quantity += quantity;
                card.FoilQuantity += foilquantity;
            } else
            {
                card = new CollectionCard { Id = id, Quantity = quantity, FoilQuantity = foilquantity };
                Cards.Add(card);
            }
            this.SaveChanges();
            return card;
        }

        public CollectionCard RemoveCard(string id, Int32 quantity = 0, Int32 foilquantity = 0)
        {
            var existingCard = Cards.Find(id);
            if (existingCard != null)
            {
                existingCard.Quantity = Math.Max(existingCard.Quantity - quantity, 0);
                existingCard.FoilQuantity = Math.Max(existingCard.FoilQuantity - foilquantity, 0);

                if (existingCard.Quantity + existingCard.FoilQuantity == 0)
                {
                    Cards.Remove(existingCard);
                }
                this.SaveChanges();
            }
            return existingCard;
        }
    }
}
