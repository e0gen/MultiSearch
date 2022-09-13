using MultiSearch.Infrastructure;
using MultiSearch.Infrastructure.Entities;
using MultiSearch.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MultiSearch.UnitTests
{
    [TestFixture]
    class DataAccessTests
    {
        [Test]
        public async Task AddWebPageToDbAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WorkDbContext>()
                .UseInMemoryDatabase(databaseName: "AddItemsToDb")
                .Options;
            // Act
            using (var context = new WorkDbContext(options))
            {
                var sut = new WebPageService(context);
                await sut.AddWebPageAsync(new WebPage("Sample", "Sample", "Sample", "Sample", "Sample"));
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
            // Arrange
            var options = new DbContextOptionsBuilder<WorkDbContext>()
                .UseInMemoryDatabase(databaseName: "FindSearches")
                .Options;

            // Act
            using (var context = new WorkDbContext(options))
            {
                context.WebPages.Add(new WebPageEntity("My cat", "AAA", "AAA", "AAA", "Google"));
                context.WebPages.Add(new WebPageEntity("Her cats", "AAA", "AAA", "AAA", "Yandex"));
                context.WebPages.Add(new WebPageEntity("His dogs", "AAA", "AAA", "AAA", "Bing"));
                context.SaveChanges();
            }

            // Assert
            using (var context = new WorkDbContext(options))
            {
                var sut = new WebPageService(context);
                var result = await sut.GetWebPagesAsync("cat");
                Assert.AreEqual(2, result.Count);
            }
        }
    }
}
