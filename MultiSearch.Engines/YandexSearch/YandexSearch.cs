using MultiSearch.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SearchEngines
{
    public class YandexSearch : ISearch
    {
        private const string searchTag = "Yandex";

        private readonly string _apiUser; // = "AIzaSyAt8AkrmkiLVghrcKA3lFh37R79rSG0NsE";
        private readonly string _apiKey; // = "003470263288780838160:ty47piyybua";

        private string uriBase = "https://yandex.com/search/xml?";
        private string l10n = "en";
        private string sortby;
        private string filter;
        private string groupby;

        public YandexSearch(string apiUser, string apiKey)
        {
            _apiUser = apiUser;
            _apiKey = apiKey;
            l10n = "en";
            sortby = "rlv";
            filter = "strict";
            groupby = "attr%3D%22%22.mode%3Dflat.groups-on-page%3D10.docs-in-group%3D1";
        }

        public string SearchUri(string query)
        {
            return $"{uriBase}user={_apiUser}&key={_apiKey}&query={Uri.EscapeDataString(query)}&l10n={l10n}&sortby={sortby}&filter={filter}&groupby={groupby}";
        }

        public IEnumerable<Item> Search(string query)
        {
            var url = SearchUri(query);
            var response = XDocument.Load(SearchUri(query));
            
            var items = response.Root.Elements("response").Elements("results").Elements("grouping").Elements("group").Elements("doc")
                .Select(x => new Item(query, x.Element("title")?.Value, x.Element("url")?.Value, x.Element("headline")?.Value, searchTag))
                .ToList();

            return items;
        }
    }
}
