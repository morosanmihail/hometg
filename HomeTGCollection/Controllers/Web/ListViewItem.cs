using HomeTG.Models;

namespace HomeTG.Controllers.Web
{
    public class ListViewItem
    {
        public Card MtGCard { get; set; }
        public CollectionCard Card { get; set; }

        public ListViewItem(Card MtGCard, CollectionCard Card)
        {
            this.MtGCard = MtGCard;
            this.Card = Card;
        }
    }
}
