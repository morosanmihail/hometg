namespace HomeTG.API.Models.Contexts.Tests
{
    [TestFixture]
    public class CSVOperationsTest
    {
        string file1;

        [SetUp]
        public void Setup()
        {
            file1 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "TestData", "short_list.csv");
        }

        [Test]
        public void ImportFromCSVTest()
        {
            var results = CSVOperations.ImportFromCSV(file1);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.That(results[0].Set, Is.EqualTo("RAV"));
            Assert.That(results[0].Quantity, Is.EqualTo(1));
            Assert.That(results[0].FoilQuantity, Is.EqualTo(3));
        }

        [Test]
        public void ImportFromCSVShortListWithCustomMappingTest()
        {
            var mapping = new Dictionary<string, string> {
                {"CollectorNumber", "CollectorNumber"},
                {"Set", "Set"},
                {"Quantity", "Quantity"},
            };
            var results = CSVOperations.ImportFromCSV(file1, mapping);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.That(results[0].Set, Is.EqualTo("RAV"));
            Assert.That(results[0].Quantity, Is.EqualTo(1));
            // Note that the foil quantity is now 0.
            // This is because, with the custom mapping, that field is ignored.
            Assert.That(results[0].FoilQuantity, Is.EqualTo(0));
        }

        [Test]
        public void ImportFromCSVInvalidCustomMappingDefaultsToNoMappingTest()
        {
            // Custom Mapping requires CollectorNumber, Set and one of Quantity or FoilQuantity
            var mapping = new Dictionary<string, string> {
                {"Set", "Set"},
            };
            var results = CSVOperations.ImportFromCSV(file1, mapping);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.That(results[0].Set, Is.EqualTo("RAV"));
            Assert.That(results[0].Quantity, Is.EqualTo(1));
            Assert.That(results[0].FoilQuantity, Is.EqualTo(3));
        }
    }
}
