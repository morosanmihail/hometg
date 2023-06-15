namespace HomeTG.Models.Contexts.Options
{
    public class StrictSearchOptions
    {
        public string CollectorNumber { get; set; }
        public string SetCode { get; set; }

        public StrictSearchOptions(string collectornumber, string set)
        {
            CollectorNumber = collectornumber;
            SetCode = set;
        }
    }
}
