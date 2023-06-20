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
            var from = Collection.Find(collectionName);
            if (from == null)
            {
                return null;
            }
            var to = keepCardsInCollection != "" ? Collection.Find(keepCardsInCollection) : null;
            if (to != null)
            {
                foreach (var c in Cards.Where(c => c.CollectionId == from.Id))
                {
                    c.CollectionId = to.Id;
                }
            }
            Collection.Remove(from);
            SaveChanges();
            return from;
        }

        public List<Collection> ListCollections()
        {
            return Collection.ToList();
        }

        public IEnumerable<CollectionCard> GetCards(List<string> ids)
        {
            return Cards.Where(c => ids.Contains(c.Id!));
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
                    c => newCards.Select(n => n.Id).
                    Contains(c.Id)
                ).ToDictionary(c => c.Id);
            foreach (var newCard in newCards)
            {
                newCard.LastUpdated = DateTime.Now;
                CollectionCard? card = null;
                if (existingCards.ContainsKey(newCard.Id))
                {
                    card = existingCards[newCard.Id];
                    card.Quantity += newCard.Quantity;
                    card.FoilQuantity += newCard.FoilQuantity;
                }
                if (card == null)
                {
                    newCard.CollectionId = collectionName;
                    card = newCard;
                    Cards.Add(newCard);
                    existingCards[newCard.Id] = card;
                }
                SaveChanges();
            }
            return existingCards.Values.ToList();
        }

        public CollectionCard? RemoveCard(CollectionCard card)
        {
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
            return existingCard;
        }
    }
}
