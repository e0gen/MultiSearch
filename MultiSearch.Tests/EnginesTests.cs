using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using MultiSearch.Engines;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace MultiSearch.Tests
{
    [TestFixture]
    public class EnginesTests
    {
        string _yandexApiUser;
        string _yandexApiKey;

        string _bingApiKey;

        string _googleApiKey;
        string _googleSearchEngineId;

        const string _queue = "C#";

        [OneTimeSetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
             .AddJsonFile("testsettings.json")
             .Build();

            _yandexApiUser = config["YandexUser"];
            _yandexApiKey = config["YandexKey"];

            _bingApiKey = config["BingKey"];

            _googleApiKey = config["GoogleKey"];
            _googleSearchEngineId = config["GoogleSearchEngineId"];
        }

        [Test]
        public void YandexEngineApi()
        {
            ISearchEngine sut = new YandexEngineHtml();

            var res = sut.Search(_queue).ToList(); ;

            Assert.AreEqual(10, res.Count);
        }

        [Test]
        public void YandexEngineHtml()
        {
            ISearchEngine sut = new YandexEngineHtml();

            var res = sut.Search(_queue).ToList(); ;
            
            Assert.AreEqual(10, res.Count);
        }


        [Test]
        public void BingEngineHtml()
        {
            ISearchEngine sut = new BingEngineApi(_bingApiKey);

            var res = sut.Search(_queue).ToList(); ;

            Assert.AreEqual(10, res.Count);
        }

        [Test]
        public void BingEngineApi()
        {
            ISearchEngine sut = new BingEngineApi(_bingApiKey);

            var res = sut.Search(_queue).ToList(); ;
            
            Assert.AreEqual(10, res.Count);
        }

        [Test]
        public void GoogleSearchApi()
        {
            ISearchEngine sut = new GoogleEngineApi(_googleApiKey, _googleSearchEngineId);

            var res = sut.Search(_queue).ToList();
            
            Assert.AreEqual(10, res.Count);
        }

        [Test]
        public void GoogleSearchHtml()
        {
            ISearchEngine sut = new GoogleEngineHtml();

            var res = sut.Search(_queue).ToList();
            
            Assert.AreEqual(10, res.Count);
        }

        [TestCase(50, 150)]
        [TestCase(150, 50)]
        public void MultiEngineDecoratorReturnsFastestSubEngine(int elapsed1, int elapsed2)
        {
            List<WebPage> result1 = new List<WebPage>() {
                new WebPage("q", "BBB", "BBB", "BBB", "Searcher1"),
                new WebPage("q", "ZZZ", "ZZZ", "ZZZ", "Searcher1"),
                new WebPage("q", "AAA", "AAA", "AAA", "Searcher1"),
            };

            List<WebPage> result2 = new List<WebPage>() {
                new WebPage("q", "BBBB", "BBBB", "BBBB", "Searcher2"),
                new WebPage("q", "ZZZZ", "ZZZZ", "ZZZZ", "Searcher2"),
                new WebPage("q", "AAAA", "AAAA", "AAAA", "Searcher2"),
            };

            var searcher1 = new Mock<ISearchEngine>();
            var searcher2 = new Mock<ISearchEngine>();

            searcher1.Setup(x => x.Search(It.IsAny<string>(), 1))
                .Callback(() => Thread.Sleep(elapsed1))
                .Returns(result1);

            searcher2.Setup(x => x.Search(It.IsAny<string>(), 1))
                .Callback(() => Thread.Sleep(elapsed2))
                .Returns(result2);

            ISearchEngine sut = new MultiEngine(new ISearchEngine[] { searcher1.Object, searcher2.Object });

            var res = sut.Search(_queue).ToList();
            
            if (elapsed1 < elapsed2)
                Assert.AreEqual(result1, res);
            else
                Assert.AreEqual(result2, res);
            
        }
    }
}