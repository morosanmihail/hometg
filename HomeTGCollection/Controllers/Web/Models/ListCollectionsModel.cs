using HomeTG.Models;

namespace HomeTG.Controllers.Web.Models
{
    public class ListCollectionsModel
    {
        public ListCollectionsModel(List<Collection> collections, string currentCollection)
        {
            Collections = collections;
            CurrentCollection = currentCollection;
        }

        public List<Collection> Collections { get; set; }

        public string CurrentCollection { get; set; }
    }
}
