using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MultiSearch.Engines
{
    public class YandexEngineApi : ISearchEngine
    {
        private const string searchTag = "Yandex Api";

        private readonly string _apiUser;
        private readonly string _apiKey;

        private readonly string uriBase = "https://yandex.com";
        private readonly string _l10n;
        private readonly string _sortby;
        private readonly string _filter;
        private readonly string _groupby;

        public YandexEngineApi(string apiUser, string apiKey)
        {
            _apiUser = apiUser;
            _apiKey = apiKey;
            _l10n = "en";
            _sortby = "rlv";
            _filter = "strict";
            _groupby = "attr%3D%22%22.mode%3Dflat.groups-on-page%3D10.docs-in-group%3D1";
        }

        public string SearchUri(string query, int page)
        {
            return $"{uriBase}/search/xml?user={_apiUser}&key={_apiKey}&query={Uri.EscapeDataString(query)}&l10n={_l10n}&sortby={_sortby}&filter={_filter}&groupby={_groupby}&page={page}";
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
