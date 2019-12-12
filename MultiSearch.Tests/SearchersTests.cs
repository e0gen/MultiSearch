using System;
using System.Linq;
using NUnit.Framework;
using SearchEngines;

namespace Tests
{
    public class SearchersTests
    {
        const string _yandexApiUser = "enevzorov";
        const string _yandexApiKey = "03.27077205:ca73007037f50483b27072e254e3de21";
        
        const string _bingApiKey = "4202bcd3d7c546debedbc8f308def029";

        const string _googleApiKey = "AIzaSyAt8AkrmkiLVghrcKA3lFh37R79rSG0NsE";
        const string _googleSearchEngineId = "003470263288780838160:ty47piyybua";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void YandexSearch()
        {
            var sut = new YandexSearch(_yandexApiUser, _yandexApiKey);

            var res = sut.Search("flower").ToList(); ;

            Assert.IsNotNull(res);
            Assert.AreEqual(10, res.Count());
        }


        [Test]
        public void BingSearch()
        {
            var sut = new BingSearch(_bingApiKey);

            var res = sut.Search("flower").ToList(); ;

            Assert.IsNotNull(res);
            Assert.AreEqual(10, res.Count());
        }

        [Test]
        public void BingSearchAsync()
        {
            var sut = new BingSearch(_bingApiKey);

            var res = sut.SearchAsync("flower").Result.ToList();

            Assert.IsNotNull(res);
            Assert.AreEqual(10, res.Count());
        }

        [Test]
        public void GoogleSearchGot10()
        {
            var sut = new GoogleSearch(_googleApiKey, _googleSearchEngineId);

            var res = sut.Search("flower").Take(10).ToList();

            Assert.IsNotNull(res);
            Assert.AreEqual(10, res.Count());
        }
    }
}