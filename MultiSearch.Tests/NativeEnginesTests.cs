using Microsoft.Extensions.Configuration;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using MultiSearch.Engines;
using NUnit.Framework;
using System.Linq;

namespace MultiSearch.Tests
{
    [TestFixture]
    public class NativeEnginesTests
    {
        string _yandexApiUser;
        string _yandexApiKey;

        string _bingApiKey;

        string _googleApiKey;
        string _googleSearchEngineId;

        const string _query = "silicone";

        [OneTimeSetUp]
        public void OneTimeSetUp()
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
            ISearchEngine sut = new YandexEngineApi(_yandexApiUser, _yandexApiKey);

            var res = sut.Search(_query).ToList();

            Assert.Greater(res.Count, 0);
        }

        [Test]
        public void YandexEngineHtml()
        {
            ISearchEngine sut = new YandexEngineHtml();

            var res = sut.Search(_query).ToList();

            Assert.Greater(res.Count, 0);
        }

        [Test]
        public void BingEngineApi()
        {
            ISearchEngine sut = new BingEngineApi(_bingApiKey);

            var res = sut.Search(_query).ToList();

            Assert.Greater(res.Count, 0);
        }

        [Test]
        public void BingEngineHtml()
        {
            ISearchEngine sut = new BingEngineHtml();

            var res = sut.Search(_query).ToList();

            Assert.Greater(res.Count, 0);
        }

        [Test]
        public void GoogleSearchApi()
        {
            ISearchEngine sut = new GoogleEngineApi(_googleApiKey, _googleSearchEngineId);

            var res = sut.Search(_query).ToList();

            Assert.Greater(res.Count, 0);
        }

        [Test]
        public void GoogleSearchHtml()
        {
            ISearchEngine sut = new GoogleEngineHtml();

            var res = sut.Search(_query).ToList();

            Assert.Greater(res.Count, 0);
        }
    }
}