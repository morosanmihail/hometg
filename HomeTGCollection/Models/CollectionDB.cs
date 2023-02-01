using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

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
            return Cards.Where(c => ids.Contains(c.Id!));
        }

        public IEnumerable<CollectionCard> ListCards(int offset, int pagesize = 50)
        {
            return Cards.OrderBy(c => c.LastUpdated).Skip(offset).Take(pagesize);
        }

        public IEnumerable<CollectionCard> ListIncoming(int offset)
        {
            return IncomingCards.Skip(offset).Take(50);
        }

        public List<CollectionCard> AddCardsToCollection(List<CollectionCard> newCards)
        {
            return AddCards(Cards, newCards);
        }

        public List<CollectionCard> AddCardsToIncoming(List<CollectionCard> newCards)
        {
            return AddCards(IncomingCards, newCards);
        }

        private List<CollectionCard> AddCards(DbSet<CollectionCard> db, List<CollectionCard> newCards)
        {
            var cards = new List<CollectionCard>();
            var existingCards = db.Where(c => newCards.Select(n => n.Id).Contains(c.Id)).ToDictionary(c => c.Id);
            foreach (var newCard in newCards) {
                CollectionCard? card = null;
                if (existingCards.ContainsKey(newCard.Id))
                {
                    card = existingCards[newCard.Id];
                    card.Quantity += newCard.Quantity;
                    card.FoilQuantity += newCard.FoilQuantity;
                }
                if (card == null)
                {
                    card = newCard;
                    db.Add(newCard);
                }
                cards.Add(card);
                this.SaveChanges();
            }
            return cards;
        }

        public CollectionCard? RemoveCardFromCollection(CollectionCard card)
        {
            return RemoveCard(Cards, card);
        }

        public CollectionCard? RemoveCardFromIncoming(CollectionCard card)
        {
            return RemoveCard(IncomingCards, card);
        }

        private CollectionCard? RemoveCard(DbSet<CollectionCard> db, CollectionCard card)
        {
            var existingCard = db.Find(card.Id);
            if (existingCard != null)
            {
                existingCard.Quantity = Math.Max(existingCard.Quantity - card.Quantity, 0);
                existingCard.FoilQuantity = Math.Max(existingCard.FoilQuantity - card.FoilQuantity, 0);

                if (existingCard.Quantity + existingCard.FoilQuantity == 0)
                {
                    db.Remove(existingCard);
                }
                this.SaveChanges();
            }
            return existingCard;
        }
    }
}
