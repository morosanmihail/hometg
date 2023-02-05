﻿using HomeTG.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTG.Tests.Models
{
    [TestFixture]
    public class CollectionDBTest
    {
        CollectionDB dbContext;
        List<CollectionCard> cards = new List<CollectionCard>
        {
            new CollectionCard("1", 1, 0, "Main", null),
            new CollectionCard("2", 1, 0, "Main", null),
            new CollectionCard("1", 1, 0, "Incoming", null)
        };

        [SetUp]
        public void Setup()
        {
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<CollectionDB>()
                                .UseSqlite(_connection)
                                .Options;
            dbContext = new CollectionDB(options);

            dbContext.Database.EnsureCreated();

            dbContext.AddRange(cards);
            
            dbContext.SaveChanges();
        }

        [Test]
        public void TestListCards()
        {
            var results = dbContext.ListCards("Main", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(2));

            results = dbContext.ListCards("Incoming", 0);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Id, Is.EqualTo("1"));

            results = dbContext.ListCards("Main", 1);
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
        }

        [Test]
        public void TestAddCards()
        {
            var results = dbContext.AddCards("Incoming", new List<CollectionCard>()
            {
                new CollectionCard("3", 0, 4, "Incoming", null)
            });
            Assert.NotNull(results);
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Id, Is.EqualTo("3"));

            var listResults = dbContext.ListCards("Incoming", 0);
            Assert.NotNull(listResults);
            Assert.That(listResults.Count(), Is.EqualTo(2));
        }
    }
}
