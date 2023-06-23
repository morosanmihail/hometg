using CsvHelper.Configuration;

public static class CsvHelperExtensions
{
    public static void Map<T>(this ClassMap<T> classMap, IDictionary<string, string> csvMappings)
    {
        foreach (var mapping in csvMappings)
        {
            var property = typeof(T).GetProperty(mapping.Key);

            if (property == null)
            {
                throw new ArgumentException($"Class {typeof(T).Name} does not have a property named {mapping.Key}");
            }

            classMap.Map(typeof(T), property).Name(mapping.Value);
        }
    }
}
