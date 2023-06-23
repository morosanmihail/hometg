namespace HomeTG.API.Models
{
    public class CollectionCardWithDetails
    {
        public Card MtGCard { get; set; }
        public CollectionCard? Card { get; set; }

        public CollectionCardWithDetails(Card MtGCard, CollectionCard? Card)
        {
            this.MtGCard = MtGCard;
            this.Card = Card;
        }
    }
}
