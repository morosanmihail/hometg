using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

        public List<CollectionCard> AddCards(List<CollectionCard> newCards)
        {
            var cards = new List<CollectionCard>();
            foreach (var newCard in newCards) {
                var card = Cards.Find(newCard.Id);
                if (card != null)
                {
                    card.Quantity += newCard.Quantity;
                    card.FoilQuantity += newCard.FoilQuantity;
                } else
                {
                    card = newCard;
                    Cards.Add(newCard);
                }
                cards.Add(card);
            }
            this.SaveChanges();
            return cards;
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
