using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using MultiSearch.Domain.Models;
using MultiSearch.Domain.Contracts;

namespace MultiSearch.Engines
{
    public class YandexEngineApi : ISearchEngine
    {
        private const string searchTag = "Yandex Api";

        private readonly string _apiUser;
        private readonly string _apiKey;
        private readonly HttpClient client;

        private readonly string uriBase = "https://yandex.com";
        private readonly string l10n;
        private readonly string sortby;
        private readonly string filter;
        private readonly string groupby;

        public YandexEngineApi(string apiUser, string apiKey)
        {
            client = new HttpClient();
            _apiUser = apiUser;
            _apiKey = apiKey;
            l10n = "en";
            sortby = "rlv";
            filter = "strict";
            groupby = "attr%3D%22%22.mode%3Dflat.groups-on-page%3D10.docs-in-group%3D1";
        }

        public string SearchUri(string query, int page)
        {
            return $"{uriBase}/search/xml?user={_apiUser}&key={_apiKey}&query={Uri.EscapeDataString(query)}&l10n={l10n}&sortby={sortby}&filter={filter}&groupby={groupby}&page={page}";
        }

        public IEnumerable<WebPage> Search(string query, int page)
        {
            var url = SearchUri(query, page);
            var response = XDocument.Load(url);

            var result = response.Root
                .Elements("response")
                .Elements("results")
                .Elements("grouping")
                .Elements("group")
                .Elements("doc")
                .Select(x => new WebPage(
                    query, 
                    x.Element("title")?.Value, 
                    HttpUtility.UrlDecode(x.Element("url")?.Value),
                    x.Element("headline")?.Value, 
                    searchTag))
                .ToList();

            return result;
        }
    }
}
