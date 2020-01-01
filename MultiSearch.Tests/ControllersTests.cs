using Moq;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using MultiSearch.Web.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MultiSearch.Tests
{
    [TestFixture]
    class ControllersTests
    {
        Mock<ISearchEngine> _searcher;
        Mock<IWebPageService> _dataServive;
        const string _query = "c#";

        List<WebPage> emptyResult = new List<WebPage>() { };
        List<WebPage> searchResult = new List<WebPage>() {
                new WebPage(_query, "BBB", "BBB", "BBB", "Searcher1"),
                new WebPage(_query, "ZZZ", "ZZZ", "ZZZ", "Searcher1"),
                new WebPage(_query, "AAA", "AAA", "AAA", "Searcher1"),
        };
        List<WebPage> dbResult = new List<WebPage>() {
                new WebPage(_query, "BBB", "BBB", "BBB", "Searcher2"),
                new WebPage(_query, "AAA", "AAA", "AAA", "Searcher2"),
                new WebPage("other query", "BBB", "BBB", "BBB", "Searcher2"),
                new WebPage("other query", "AAA", "AAA", "AAA", "Searcher2"),
                new WebPage("other query with filter", "BBB", "BBB", "BBB", "Searcher2"),
                new WebPage("other query with filter", "AAA", "AAA", "AAA", "Searcher2"),
            };
        

        [SetUp]
        public void SetUp()
        {
            _searcher = new Mock<ISearchEngine>();

            _dataServive = new Mock<IWebPageService>();

            _dataServive.Setup(x => x.GetWebPagesAsync(_query))
                .ReturnsAsync(dbResult);
            _dataServive.Setup(x => x.GetWebPagesAsync())
                .ReturnsAsync(emptyResult);
        }

        [Test]
        public void SearchControllerCallSearchOnce()
        {
            // Arrange
            _searcher.Setup(x => x.SearchAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(searchResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(_query).Result as ViewResult;

            // Assert
            _searcher.Verify(m => m.SearchAsync(It.Is<string>(c => c == _query), 1), Times.Once);
        }

        [Test]
        public void SearchControllerNotCallSearchOnceIfNothingQuered()
        {
            // Arrange
            _searcher.Setup(x => x.SearchAsync(null, 1))
                .ReturnsAsync(emptyResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(null).Result as ViewResult;

            // Assert
            _searcher.Verify(m => m.SearchAsync(null, 1), Times.Never);
        }

        [Test]
        public void SearchControllerCallSaveResult()
        {
            // Arrange
            _searcher.Setup(x => x.SearchAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(searchResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(_query).Result as ViewResult;

            // Assert
            _dataServive.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void SearchControllerNotSaveResultIfNothingQuered()
        {
            // Arrange
            _searcher.Setup(x => x.SearchAsync(null, 1))
                .ReturnsAsync(emptyResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(null).Result as ViewResult;

            // Assert
            _dataServive.Verify(m => m.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void SearchControllerCallAddResult()
        {
            // Arrange
            _searcher.Setup(x => x.SearchAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(searchResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(_query).Result as ViewResult;

            // Assert
            foreach (var wp in searchResult)
                _dataServive.Verify(m => m.AddWebPageAsync(wp), Times.Once);
        }

        [Test]
        public void SearchControllerNotCallAddResultIfNothingQuered()
        {
            // Arrange
            _searcher.Setup(x => x.SearchAsync(null, 1))
                .ReturnsAsync(emptyResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(null).Result as ViewResult;

            // Assert
            _dataServive.Verify(m => m.AddWebPageAsync(searchResult[0]), Times.Never);
        }


        [Test]
        public void HistoryControllerCallGetWebPagesAsyncWithFilter()
        {
            var controller = new HistoryController(_dataServive.Object);

            // Act
            var result = controller.Index("filter", 1).Result as ViewResult;

            // Assert
            _dataServive.Verify(m => m.GetWebPagesAsync("filter"), Times.Once);
        }

        [Test]
        public void HistoryControllerCallGetWebPagesAsync()
        {
            // Arrange
            var controller = new HistoryController(_dataServive.Object);

            // Act
            var result = controller.Index(It.Is<string>(s => s == null), 1).Result as ViewResult;

            // Assert
            _dataServive.Verify(m => m.GetWebPagesAsync(), Times.Once);
        }
    }
}
