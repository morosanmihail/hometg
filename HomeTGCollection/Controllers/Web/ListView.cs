using HomeTG.Models;

namespace HomeTG.Controllers.Web
{
    public class ListView
    {
        public Card MtGCard { get; set; }
        public CollectionCard Card { get; set; }

        public ListView(Card MtGCard, CollectionCard Card)
        {
            this.MtGCard = MtGCard;
            this.Card = Card;
        }
    }
}
