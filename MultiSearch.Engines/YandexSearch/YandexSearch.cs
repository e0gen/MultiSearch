using MultiSearch.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SearchEngines
{
    public class YandexSearch : ISearch
    {
        private readonly string _apiUser; // = "AIzaSyAt8AkrmkiLVghrcKA3lFh37R79rSG0NsE";
        private readonly string _apiKey; // = "003470263288780838160:ty47piyybua";

        private string uriBase = "https://yandex.com/search/xml?";
        private string _l10n;
        private string _apiUrl;
        private string _sortby;
        private string _filter;
        private string _groupby;
        //private readonly CustomsearchService _customSearchService;// );

        public YandexSearch(string apiUser, string apiKey)
        {
            //
            //&key=03.27077205:ca73007037f50483b27072e254e3de21
            //&query=flower
            //&l10n=en
            //&sortby=tm.order%3Dascending
            //&filter=strict
            //&groupby=attr%3D%22%22.mode%3Dflat.groups-on-page%3D10.docs-in-group%3D1
            _apiUrl = "https://yandex.com/search/xml?";
            _apiUser = apiUser;
            _apiKey = apiKey;
            _l10n = "en";
            _sortby = Uri.EscapeDataString("tm.order=ascending");
            _filter = "strict";
            _groupby = Uri.EscapeDataString(@"attr="".mode=flat.groups-on-page=10.docs-in-group=1");
            //_customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
        }

        public IEnumerable<Item> Search(string query)
        {
            var items = new List<Item>();

            return items;

            string getRequest = $"{_apiUrl}{_apiUser}&{_apiKey}&{Uri.EscapeDataString(query)}&{_l10n}&{_sortby}&{_filter}&{_groupby}";
            var response = XDocument.Load(getRequest);




            //Лист структур YaSearchResult, который метод в итоге возвращает.
            List<YaSearchResult> ret = new List<YaSearchResult>();

            //из полученного XML'я выдираем все элементы с именем "group" - это результаты поиска
            var groupQuery = from gr in response.Elements().
                          Elements("response").
                          Elements("results").
                          Elements("grouping").
                          Elements("group")
                             select gr;

            ////каждый элемент group преобразовывается в объект SearchResult
            //for (int i = 0; i < groupQuery.Count(); i++)
            //{
            //    string urlQuery = GetValue(groupQuery.ElementAt(i), "url");
            //    string titleQuery = GetValue(groupQuery.ElementAt(i), "title");
            //    string descriptionQuery = GetValue(groupQuery.ElementAt(i), "headline");
            //    string indexedTimeQuery = GetValue(groupQuery.ElementAt(i), "modtime");
            //    string cacheUrlQuery = GetValue(groupQuery.ElementAt(i),
            //                    "saved-copy-url");
            //    ret.Add(new YaSearchResult(urlQuery, cacheUrlQuery, titleQuery, descriptionQuery, indexedTimeQuery));
            //}

            //return ret;

            //var listRequest = _customSearchService.Cse.List(query);
            //listRequest.Cx = _searchEngineId;

            //IList<Result> paging = new List<Result>();
            //var count = 0;
            //while (paging != null)
            //{
            //    listRequest.Start = count * 10 + 1;
            //    paging = listRequest.Execute().Items;
            //    if (paging != null)
            //        foreach (var item in paging)
            //        {
            //            yield return new Item(item.Title, item.Link);
            //        }
            //    count++;
            //}
        }


    }

    public struct YaSearchResult
    {
        //url
        public string DisplayUrl,
        //saved-copy-url
        CacheUrl,
        //title
        Title,
        //headline
        Description,
        //modtime
        IndexedTime;

        public YaSearchResult(string url,
                   string cacheUrl,
                   string title,
                   string description,
                   string indexedTime)
        {
            this.DisplayUrl = url;
            this.CacheUrl = cacheUrl;
            this.Title = title;
            this.Description = description;
            this.IndexedTime = indexedTime;
        }
    }
}
