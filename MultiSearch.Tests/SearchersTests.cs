using System;
using System.Linq;
using Moq;
using MultiSearch.Domain;
using NUnit.Framework;
using SearchEngines;

namespace MultiSearch.Tests
{
    public class SearchersTests
    {
        const string _yandexApiUser = "c3p0r2d2c3p0";
        const string _yandexApiKey = "03.997239423:21e25da813a07a4212d0768ef29f0918";
        
        const string _bingApiKey = "4202bcd3d7c546debedbc8f308def029";

        const string _googleApiKey = "AIzaSyAt8AkrmkiLVghrcKA3lFh37R79rSG0NsE";
        const string _googleSearchEngineId = "003470263288780838160:ty47piyybua";

        const string _queue = "C#";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void YandexSearch()
        {
            //YandexSearch sut = Mock.Of<YandexSearch>(d => d.SearchUri(It.IsAny<string>()) == @"D:\Projects\MultiSearchEngine\MultiSearch.Tests\YandexResponse.xml");
            var sut = new YandexSearch(_yandexApiUser, _yandexApiKey);

            var res = sut.Search(_queue).ToList(); ;

            Assert.IsNotNull(res);
            Assert.AreEqual(10, res.Count());
        }


        [Test]
        public void BingSearch()
        {
            var sut = new BingSearch(_bingApiKey);

            var res = sut.Search(_queue).ToList(); ;

            Assert.IsNotNull(res);
            Assert.AreEqual(10, res.Count());
        }

        [Test]
        public void BingSearchAsync()
        {
            var sut = new BingSearch(_bingApiKey);

            var res = sut.SearchAsync(_queue).Result.ToList();

            Assert.IsNotNull(res);
            Assert.AreEqual(10, res.Count());
        }

        [Test]
        public void GoogleSearch()
        {
            var sut = new GoogleSearch(_googleApiKey, _googleSearchEngineId);

            var res = sut.Search(_queue).Take(10).ToList();

            Assert.IsNotNull(res);
            Assert.AreEqual(10, res.Count());
        }
    }
}