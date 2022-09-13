using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using MultiSearch.Engines;


namespace MultiSearch.UnitTests
{
    [TestFixture]
    class MultiEngineTests
    {
        const string _query = "c#";

        [TestCase(50, 150)]
        [TestCase(150, 50)]
        public async Task MultiEngineDecoratorReturnsFastestSubEngine(int elapsed1, int elapsed2)
        {
            List<WebPage> result1 = new () {
                new WebPage(_query, "BBB", "BBB", "BBB", "Searcher1"),
                new WebPage(_query, "ZZZ", "ZZZ", "ZZZ", "Searcher1"),
                new WebPage(_query, "AAA", "AAA", "AAA", "Searcher1"),
            };

            List<WebPage> result2 = new () {
                new WebPage(_query, "BBBB", "BBBB", "BBBB", "Searcher2"),
                new WebPage(_query, "ZZZZ", "ZZZZ", "ZZZZ", "Searcher2"),
                new WebPage(_query, "AAAA", "AAAA", "AAAA", "Searcher2"),
            };

            var searcher1 = new Mock<ISearchEngine>();
            var searcher2 = new Mock<ISearchEngine>();

            searcher1.Setup(x => x.SearchAsync(It.IsAny<CancellationToken>(), It.IsAny<string>(), 1))
                .ReturnsAsync(result1, TimeSpan.FromMilliseconds(elapsed1));

            searcher2.Setup(x => x.SearchAsync(It.IsAny<CancellationToken>(), It.IsAny<string>(), 1))
                .ReturnsAsync(result2, TimeSpan.FromMilliseconds(elapsed2));

            ISearchEngine sut = new MultiEngine(new[] { searcher1.Object, searcher2.Object });

            var res = await sut.SearchAsync(new CancellationTokenSource().Token, _query);

            var expResult = elapsed1 < elapsed2 ? result1 : result2;
            Assert.AreEqual(expResult, res);
        }
    }
}
