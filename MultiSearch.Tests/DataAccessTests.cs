using Microsoft.EntityFrameworkCore;
using Moq;
using MultiSearch.DataAccess;
using MultiSearch.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiSearch.Tests
{
    class DataAccessTests
    {
        [Test]
        public void AddItemToDb()
        {
            var options = new DbContextOptionsBuilder<WorkDBContext>()
                .UseInMemoryDatabase(databaseName: "AddItemsToDb")
                .Options;
            
            using (var context = new WorkDBContext(options))
            {
                var service = new ItemService(context);
                service.AddItem(new Item("Sample", "Sample", "Sample", "Sample", "Sample"));
                context.SaveChanges();
            }
            
            using (var context = new WorkDBContext(options))
            {
                Assert.AreEqual(1, context.Items.Count());
                Assert.AreEqual("Sample", context.Items.Single().Title);
            }
        }

        [Test]
        public void FindSearches()
        {
            var options = new DbContextOptionsBuilder<WorkDBContext>()
                .UseInMemoryDatabase(databaseName: "FindSearches")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new WorkDBContext(options))
            {
                context.Items.Add(new ItemDb("My cat", "AAA", "AAA", "AAA", "Google"));
                context.Items.Add(new ItemDb("Her cats", "AAA", "AAA", "AAA", "Yandex"));
                context.Items.Add(new ItemDb("His dogs", "AAA", "AAA", "AAA", "Bing"));
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new WorkDBContext(options))
            {
                var service = new ItemService(context);
                var result = service.Find("cat");
                Assert.AreEqual(2, result.Count());
            }
        }

        //[Test]
        //public void GetItems_orders_by_name()
        //{
        //    var data = new List<ItemDb>
        //    {
        //        new ItemDb("BBB", "BBB", "BBB", "BBB", "Google"),
        //        new ItemDb("ZZZ", "ZZZ", "ZZZ", "ZZZ", "Google"),
        //        new ItemDb("AAA","AAA", "AAA", "AAA", "Google"),
        //    }.AsQueryable();

        //    var mockSet = new Mock<DbSet<ItemDb>>();
        //    mockSet.As<IQueryable<ItemDb>>().Setup(m => m.Provider).Returns(data.Provider);
        //    mockSet.As<IQueryable<ItemDb>>().Setup(m => m.Expression).Returns(data.Expression);
        //    mockSet.As<IQueryable<ItemDb>>().Setup(m => m.ElementType).Returns(data.ElementType);
        //    mockSet.As<IQueryable<ItemDb>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        //    var mockContext = new Mock<WorkDBContext>();
        //    mockContext.Setup(c => c.Items).Returns(mockSet.Object);

        //    var service = new ItemService(mockContext.Object);
        //    var items = service.GetItems();

        //    Assert.AreEqual(3, items.Count);
        //    Assert.AreEqual("AAA", blogs[0].Name);
        //    Assert.AreEqual("BBB", blogs[1].Name);
        //    Assert.AreEqual("ZZZ", blogs[2].Name);
        //}
    }
}
