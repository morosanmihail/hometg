using HomeTG.Models;

namespace HomeTG.Controllers.Web.Models
{
    public class MainPageData
    {
        public MainPageData(IEnumerable<CollectionCardWithDetails> listViewItems, ListCollectionsModel collections)
        {
            ListViewItems = listViewItems;
            Collections = collections;
        }

        public IEnumerable<CollectionCardWithDetails> ListViewItems { get; set; }
        public ListCollectionsModel Collections { get; set; }
    }
}
