using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeTG.API.Models.Contexts
{
    public class CollectionDB : DbContext
    {
        public DbSet<CollectionCard> Cards { get; set; }
        public DbSet<Collection> Collection { get; set; }

        public CollectionDB(DbContextOptions<CollectionDB> options) : base(options)
        { }

        public Collection? GetCollection(string collectionName)
        {
            return Collection.Find(collectionName);
        }

        public Collection GetOrCreateCollection(string collectionName)
        {
            var collection = Collection.Find(collectionName);
            if (collection == null)
            {
                collection = Collection.Add(new Collection(collectionName)).Entity;
                SaveChanges();
            }
            return collection;
        }

        public Collection? RemoveCollection(string collectionName, string keepCardsInCollection = "")
        {
            var from = GetCollection(collectionName);
            if (from == null)
            {
                return null;
            }
            var to = keepCardsInCollection != "" ? GetCollection(keepCardsInCollection) : null;
            if (to != null)
            {
                MoveCardsToCollection(keepCardsInCollection, Cards.Where(c => c.CollectionId.ToLower() == from.Id.ToLower()).ToList());
            }
            Collection.Remove(from);
            SaveChanges();
            return from;
        }

        public List<Collection> ListCollections()
        {
            return Collection.ToList();
        }

        public Dictionary<string, List<CollectionCard>> GetCards(List<string> ids)
        {
            return Cards.Where(c => ids.Contains(c.Id!)).GroupBy(c => c.Id).ToDictionary(c => c.Key, c => c.ToList());
        }

        public Dictionary<string, CollectionCard> GetCardsFromCollection(string collectionName, List<string> ids)
        {
            return Cards.Where(c => ids.Contains(c.Id!) && c.CollectionId.ToLower() == collectionName.ToLower()).
                ToDictionary(c => c.Id, c => c);
        }

        public IEnumerable<CollectionCard> ListCards(string collection, int offset, int pagesize = 12)
        {
            return Cards.Where(
                c => c.CollectionId.ToLower() == collection.ToLower()
            ).OrderByDescending(c => c.LastUpdated).Skip(offset).Take(pagesize);
        }

        // TODO: mix Add/Remove cards together maybe? One clean function?
        public List<CollectionCard> AddCards(string collectionName, List<CollectionCard> newCards)
        {
            var collection = GetOrCreateCollection(collectionName);

            var existingCards = Cards.Where(c => c.CollectionId.ToLower() == collectionName.ToLower()).
                Where(
                    c => newCards.Select(n => n.Id).Contains(c.Id)
                ).ToDictionary(c => c.Id);

            foreach (var newCard in newCards)
            {
                if (existingCards.ContainsKey(newCard.Id))
                {
                    existingCards[newCard.Id].Quantity += newCard.Quantity;
                    existingCards[newCard.Id].FoilQuantity += newCard.FoilQuantity;
                } else {
                    var card = new CollectionCard {
                        Id = newCard.Id,
                        CollectionId = collectionName,
                        Quantity = newCard.Quantity,
                        FoilQuantity = newCard.FoilQuantity,
                        LastUpdated = DateTime.Now,
                    };
                    Cards.Add(card);
                    existingCards[newCard.Id] = card;
                }
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            SaveChanges();
            watch.Stop();
            Console.WriteLine("--- DIAGNOSTIC: SaveChanges: " + watch.ElapsedMilliseconds + " ms");
            return existingCards.Values.ToList();
        }

        public CollectionCard? RemoveCard(CollectionCard card)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var existingCard = Cards.Find(card.Id, card.CollectionId);
            if (existingCard != null)
            {
                existingCard.Quantity = Math.Max(existingCard.Quantity - card.Quantity, 0);
                existingCard.FoilQuantity = Math.Max(existingCard.FoilQuantity - card.FoilQuantity, 0);

                if (existingCard.Quantity + existingCard.FoilQuantity == 0)
                {
                    Cards.Remove(existingCard);
                }
                SaveChanges();
            }
            watch.Stop();
            Console.WriteLine("--- DIAGNOSTIC: RemoveCard: " + watch.ElapsedMilliseconds + " ms");
            return existingCard;
        }

        public IEnumerable<CollectionCard> RemoveCardsEntirely(IEnumerable<CollectionCard> cards)
        {
            if (cards == null || cards.Count() == 0)
            {
                return new List<CollectionCard>();
            }
            var toRemove = GetCardsFromCollection(cards.First().CollectionId, cards.Select(c => c.Id).ToList()).Values;
            Cards.RemoveRange(toRemove);
            SaveChanges();
            return toRemove;
        }

        public IEnumerable<CollectionCard> MoveCardsToCollection(string to, List<CollectionCard> cards)
        {
            var added = AddCards(to, cards);
            foreach (var card in cards)
            {
                if (card != null)
                {
                    RemoveCard(card);
                }
            }
            return added;
        }
    }
}
