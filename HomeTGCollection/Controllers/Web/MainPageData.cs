using HomeTG.Models;

namespace HomeTG.Controllers.Web
{
    public class MainPageData
    {
        public MainPageData(IEnumerable<ListViewItem> listViewItems, ListCollectionsModel collections)
        {
            this.ListViewItems = listViewItems;
            Collections = collections;
        }

        public IEnumerable<ListViewItem> ListViewItems { get; set; }
        public ListCollectionsModel Collections { get; set; }
    }
}
