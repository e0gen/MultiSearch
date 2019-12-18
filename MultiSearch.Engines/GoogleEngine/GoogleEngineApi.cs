using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System.Collections.Generic;
using System.Web;


namespace MultiSearch.Engines
{
    public class GoogleEngineApi : ISearchEngine
    {
        private const string searchTag = "Google Api";

        private readonly string _apiKey;
        private readonly string _searchEngineId;
        private readonly CustomsearchService _customSearchService;

        public GoogleEngineApi(string apiKey, string searchEngineId)
        {
            _apiKey = apiKey;
            _searchEngineId = searchEngineId;
            _customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
        }

        public IEnumerable<WebPage> Search(string query, int page)
        {
            var listRequest = _customSearchService.Cse.List(query);
            listRequest.Cx = _searchEngineId;

            page--;
            listRequest.Start = page * 10 + 1;
            IList<Result> paging = listRequest.Execute().Items;
            if (paging != null)
                foreach (var item in paging)
                {
                    yield return new WebPage(query, item.Title, HttpUtility.UrlDecode(item.Link), item.Snippet, searchTag);
                }
        }
    }
}
