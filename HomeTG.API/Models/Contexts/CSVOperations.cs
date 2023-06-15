using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace HomeTG.Models.Contexts
{
    public static class CSVOperations
    {
        public static List<CSVItem> ImportFromCSV(string filename, Dictionary<string, string>? customMapping = null)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true
            };

            var items = new List<CSVItem>();
            using (var reader = System.IO.File.OpenText(filename))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                if (ValidateMapping(customMapping))
                {
                    var mapping = new DefaultClassMap<CSVItem>();
                    mapping.Map(customMapping!);
                    csv.Context.RegisterClassMap(mapping);
                }

                items = csv.GetRecords<CSVItem>().ToList();
            }
            return items;
        }

        static bool ValidateMapping(Dictionary<string, string>? mapping)
        {
            if (mapping == null)
            {
                return false;
            }

            bool allKeys = (
                mapping.ContainsKey("CollectorNumber") && mapping.ContainsKey("Set") &&
                (mapping.ContainsKey("Quantity") || mapping.ContainsKey("FoilQuantity"))
            );

            return allKeys;
        }
    }

    public struct CSVItem
    {
        public string CollectorNumber { get; set; }
        public string Set { get; set; }
        public int Quantity { get; set; }
        public int FoilQuantity { get; set; }
        public string Acquired { get; set; }
    }
}
