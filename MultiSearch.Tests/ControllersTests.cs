using Moq;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using MultiSearch.Web.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace MultiSearch.Tests
{
    [TestFixture]
    class ControllersTests
    {
        Mock<ISearchEngine> _searcher;
        Mock<IWebPageService> _dataServive;
        const string _query = "c#";

        List<WebPage> emptyResult = new () { };
        List<WebPage> searchResult = new () {
                new WebPage(_query, "BBB", "BBB", "BBB", "Searcher1"),
                new WebPage(_query, "ZZZ", "ZZZ", "ZZZ", "Searcher1"),
                new WebPage(_query, "AAA", "AAA", "AAA", "Searcher1"),
        };
        List<WebPage> dbResult = new () {
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
            CancellationTokenSource cts = new();
            _searcher.Setup(x => x.SearchAsync(cts.Token, It.IsAny<string>(), 1))
                .ReturnsAsync(searchResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(_query, cts.Token).Result as ViewResult;

            // Assert
            _searcher.Verify(m => m.SearchAsync(cts.Token, It.Is<string>(c => c == _query), 1), Times.Once);
        }

        [Test]
        public void SearchControllerNotCallSearchOnceIfNothingQuered()
        {
            // Arrange
            CancellationTokenSource cts = new();
            _searcher.Setup(x => x.SearchAsync(cts.Token, null, 1))
                .ReturnsAsync(emptyResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(null, cts.Token).Result as ViewResult;

            // Assert
            _searcher.Verify(m => m.SearchAsync(cts.Token, null, 1), Times.Never);
        }

        [Test]
        public void SearchControllerCallSaveResult()
        {
            // Arrange
            CancellationTokenSource cts = new();
            _searcher.Setup(x => x.SearchAsync(cts.Token, It.IsAny<string>(), 1))
                .ReturnsAsync(searchResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(_query, cts.Token).Result as ViewResult;

            // Assert
            _dataServive.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void SearchControllerNotSaveResultIfNothingQuered()
        {
            // Arrange
            CancellationTokenSource cts = new();
            _searcher.Setup(x => x.SearchAsync(cts.Token, null, 1))
                .ReturnsAsync(emptyResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(null, cts.Token).Result as ViewResult;

            // Assert
            _dataServive.Verify(m => m.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void SearchControllerCallAddResult()
        {
            // Arrange
            CancellationTokenSource cts = new();
            _searcher.Setup(x => x.SearchAsync(cts.Token, It.IsAny<string>(), 1))
                .ReturnsAsync(searchResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(_query, cts.Token).Result as ViewResult;

            // Assert
            foreach (var wp in searchResult)
                _dataServive.Verify(m => m.AddWebPageAsync(wp), Times.Once);
        }

        [Test]
        public void SearchControllerNotCallAddResultIfNothingQuered()
        {
            // Arrange
            CancellationTokenSource cts = new();
            _searcher.Setup(x => x.SearchAsync(cts.Token, null, 1))
                .ReturnsAsync(emptyResult);
            var controller = new SearchController(_searcher.Object, _dataServive.Object);

            // Act
            var result = controller.Search(null, cts.Token).Result as ViewResult;

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
