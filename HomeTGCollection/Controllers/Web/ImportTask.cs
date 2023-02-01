namespace HomeTG.Controllers.Web
{
    public class ImportTask
    {
        public string Filename { get; set; }
        public int Current { get; set; }
        public int Total { get; set; }

        public ImportTask(string filename, int total = 1, int current = 0) {
            Filename = filename;
            Current = current;
            Total = total;
        }
    }
}
