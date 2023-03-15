namespace HomeTG.Models
{
    public class StrictSearchOptions
    {
        public string CollectorNumber { get; set; }
        public string SetCode { get; set; }

        public StrictSearchOptions(string collectornumber, string set) 
        {
            this.CollectorNumber = collectornumber;
            this.SetCode = set;
        }
    }
}
