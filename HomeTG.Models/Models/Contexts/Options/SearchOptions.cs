namespace HomeTG.API.Models.Contexts.Options
{
    public class SearchOptions
    {
        public string? Name { get; set; }
        public string? SetCode { get; set; }
        public string? CollectorNumber { get; set; }
        public string? Artist { get; set; }
        public List<string>? ColorIdentities { get; set; }
        public string? Text { get; set; }
    }
}
