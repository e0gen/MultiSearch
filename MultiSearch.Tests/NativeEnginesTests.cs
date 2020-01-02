using Microsoft.Extensions.Configuration;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using MultiSearch.Engines;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

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
        public async Task SearchAsyncByYandexEngineApi()
        {
            ISearchEngine sut = new YandexEngineApi(_yandexApiUser, _yandexApiKey, new HttpClient());

            var res = await sut.SearchAsync(_query);

            Assert.Greater(res.Count, 0);
        }

        [Test]
        public async Task SearchAsyncByYandexEngineHtml()
        {
            ISearchEngine sut = new YandexEngineHtml(new HttpClient());

            var res = await sut.SearchAsync(_query);

            Assert.Greater(res.Count, 0);
        }

        [Test]
        public async Task SearchAsyncByBingEngineApi()
        {
            ISearchEngine sut = new BingEngineApi(_bingApiKey, new HttpClient());

            var res = await sut.SearchAsync(_query);

            Assert.Greater(res.Count, 0);
        }

        [Test]
        public async Task SearchAsyncByBingEngineHtml()
        {
            ISearchEngine sut = new BingEngineHtml(new HttpClient());

            var res = await sut.SearchAsync(_query);

            Assert.Greater(res.Count, 0);
        }

        [Test]
        public async Task SearchAsyncByGoogleEngineApi()
        {
            ISearchEngine sut = new GoogleEngineApi(_googleApiKey, _googleSearchEngineId);

            var res = await sut.SearchAsync(_query);

            Assert.Greater(res.Count, 0);
        }

        [Test]
        public async Task SearchAsyncByGoogleSearchHtml()
        {
            ISearchEngine sut = new GoogleEngineHtml(new HttpClient());

            var res = await sut.SearchAsync(_query);

            Assert.Greater(res.Count, 0);
        }
    }
}