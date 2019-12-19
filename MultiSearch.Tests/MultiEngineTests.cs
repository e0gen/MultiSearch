using Moq;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using MultiSearch.Engines;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MultiSearch.Tests
{
    [TestFixture]
    class MultiEngineTests
    {
        const string _query = "c#";

        [TestCase(50, 150)]
        [TestCase(150, 50)]
        public void MultiEngineDecoratorReturnsFastestSubEngine(int elapsed1, int elapsed2)
        {
            List<WebPage> result1 = new List<WebPage>() {
                new WebPage(_query, "BBB", "BBB", "BBB", "Searcher1"),
                new WebPage(_query, "ZZZ", "ZZZ", "ZZZ", "Searcher1"),
                new WebPage(_query, "AAA", "AAA", "AAA", "Searcher1"),
            };

            List<WebPage> result2 = new List<WebPage>() {
                new WebPage(_query, "BBBB", "BBBB", "BBBB", "Searcher2"),
                new WebPage(_query, "ZZZZ", "ZZZZ", "ZZZZ", "Searcher2"),
                new WebPage(_query, "AAAA", "AAAA", "AAAA", "Searcher2"),
            };

            var searcher1 = new Mock<ISearchEngine>();
            var searcher2 = new Mock<ISearchEngine>();

            searcher1.Setup(x => x.Search(It.IsAny<string>(), 1))
                .Callback(() => Thread.Sleep(elapsed1))
                .Returns(result1);

            searcher2.Setup(x => x.Search(It.IsAny<string>(), 1))
                .Callback(() => Thread.Sleep(elapsed2))
                .Returns(result2);

            ISearchEngine sut = new MultiEngine(new[] { searcher1.Object, searcher2.Object });

            var res = sut.Search(_query).ToList();

            var expResult = elapsed1 < elapsed2 ? result1 : result2;
            Assert.AreEqual(expResult, res);
        }
    }
}
