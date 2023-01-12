using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace HomeTG.Models
{
    public class SearchOptions
    {
        public string? Name { get; set; }
        public string? Set { get; set; }
    }

    public class DB : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public DB(DbContextOptions<DB> options) : base(options)
        {}

        public IEnumerable<Card> SearchCards(SearchOptions searchOptions)
        {
            searchOptions.Name = searchOptions.Name?.ToLower();
            searchOptions.Set = searchOptions.Set?.ToLower();

            IQueryable<Card> cards = Cards.AsQueryable();

            if (searchOptions.Name != null && searchOptions.Name.Length > 0)
            {
                cards = cards.Where(c => (c.Name.ToLower().Contains(searchOptions.Name)));
            }

            if (searchOptions.Set != null && searchOptions.Set.Length > 0)
            {
                cards = cards.Where(c => c.SetCode.ToLower().Equals(searchOptions.Set));
            }

            return cards.ToList();
        }

        public IEnumerable<Card> GetCards(List<string> ids)
        {
            return Cards.Where(c=> ids.Contains(c.Id)).ToList();
        }
    }

    public class CollectionDB : DbContext
    {
        public DbSet<CollectionCard> Cards { get; set; }
        public DbSet<Collection> Collection { get; set; }

        // public DbSet<CollectionCard> IncomingCards { get; set; }

        public CollectionDB(DbContextOptions<CollectionDB> options) : base(options)
        { }

        public IEnumerable<CollectionCard> GetCards(List<string> ids)
        {
            return Cards.Where(c => ids.Contains(c.Id)).ToList();
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
