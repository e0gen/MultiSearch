using Microsoft.EntityFrameworkCore;
using MultiSearch.DataAccess;
using MultiSearch.DataAccess.Entities;
using MultiSearch.Domain.Models;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace MultiSearch.Tests
{
    [TestFixture]
    class DataAccessTests
    {
        [Test]
        public async Task AddWebPageToDbAsync()
        {
            var options = new DbContextOptionsBuilder<WorkDbContext>()
                .UseInMemoryDatabase(databaseName: "AddItemsToDb")
                .Options;

            using (var context = new WorkDbContext(options))
            {
                var service = new WebPageService(context);
                await service.AddWebPageAsync(new WebPage("Sample", "Sample", "Sample", "Sample", "Sample"));
                await context.SaveChangesAsync();
            }

            using (var context = new WorkDbContext(options))
            {
                Assert.AreEqual(1, context.WebPages.Count());
                Assert.AreEqual("Sample", context.WebPages.Single().Title);
            }
        }

        [Test]
        public async Task FilterWebPagesAsync()
        {
            var options = new DbContextOptionsBuilder<WorkDbContext>()
                .UseInMemoryDatabase(databaseName: "FindSearches")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new WorkDbContext(options))
            {
                context.WebPages.Add(new WebPageEntity("My cat", "AAA", "AAA", "AAA", "Google"));
                context.WebPages.Add(new WebPageEntity("Her cats", "AAA", "AAA", "AAA", "Yandex"));
                context.WebPages.Add(new WebPageEntity("His dogs", "AAA", "AAA", "AAA", "Bing"));
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new WorkDbContext(options))
            {
                var service = new WebPageService(context);
                var result = await service.GetWebPagesAsync("cat");
                Assert.AreEqual(2, result.Count);
            }
        }
    }
}
