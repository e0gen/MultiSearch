using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        private readonly HttpClient _client;

        public YandexEngineApi(string apiUser, string apiKey, HttpClient httpClient)
        {
            _apiUser = apiUser;
            _apiKey = apiKey;
            _l10n = "en";
            _sortby = "rlv";
            _filter = "strict";
            _groupby = "attr%3D%22%22.mode%3Dflat.groups-on-page%3D10.docs-in-group%3D1";
            _client = httpClient;
        }

        public string SearchUri(string query, int page)
        {
            var sb = new StringBuilder(uriBase);
            sb.Append($"/search/xml?");
            sb.AppendFormat($"user={_apiUser}");
            sb.AppendFormat($"&key={_apiKey}");
            sb.AppendFormat($"&query={Uri.EscapeDataString(query)}");
            sb.AppendFormat($"&l10n={_l10n}");
            sb.AppendFormat($"&sortby={_sortby}");
            sb.AppendFormat($"&filter={_filter}");
            sb.AppendFormat($"&groupby={_groupby}");
            if (page > 1)
                sb.AppendFormat($"&page={page}");
            return sb.ToString();
        }

        public async Task<IList<WebPage>> SearchAsync(CancellationToken ct, string query, int page)
        {
            HttpResponseMessage response = await _client.GetAsync(SearchUri(query, page), ct);
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var doc = XDocument.Load(stream);

                return doc.Root
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
            }
            return new List<WebPage>();
        }
    }
}
