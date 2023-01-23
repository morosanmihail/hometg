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
            return Cards.Where(c => ids.Contains(c.Id!)).ToList();
        }

        public IEnumerable<CollectionCard> ListCards(int offset)
        {
            return Cards.Skip(offset).Take(50).ToList();
        }

        public IEnumerable<CollectionCard> ListIncoming(int offset)
        {
            return IncomingCards.Skip(offset).Take(50).ToList();
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
                // var card = db.Find(newCard.Id);
                var card = existingCards[newCard.Id];
                if (card != null)
                {
                    card.Quantity += newCard.Quantity;
                    card.FoilQuantity += newCard.FoilQuantity;
                } else
                {
                    card = newCard;
                    db.Add(newCard);
                }
                cards.Add(card);
            }
            this.SaveChanges();
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
