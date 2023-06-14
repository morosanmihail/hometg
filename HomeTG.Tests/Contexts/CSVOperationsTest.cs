using HomeTG.Models;
using HomeTG.Models.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTG.Tests.Contexts
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
        public void TestImportShortList()
        {
            var results = CSVOperations.ImportFromCSV(file1);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.That(results[0].Set, Is.EqualTo("RAV"));
            Assert.That(results[0].Quantity, Is.EqualTo(1));
            Assert.That(results[0].FoilQuantity, Is.EqualTo(3));
        }

        [Test]
        public void TestImportShortListWithCustomMapping()
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
        public void TestInvalidCustomMappingDefaultsToNoMapping()
        {
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
