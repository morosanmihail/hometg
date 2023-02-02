namespace HomeTG.Controllers.Web
{
    public class MainPageData
    {
        public MainPageData(IEnumerable<ListViewItem> listViewItems, string collection)
        {
            this.ListViewItems = listViewItems;
            Collection = collection;
        }

        public IEnumerable<ListViewItem> ListViewItems { get; set; }
        public string Collection { get; set; }
    }
}
