using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using System.Web;
using System.Collections.Generic;
using MultiSearch.Domain.Models;
using MultiSearch.Domain.Contracts;


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
            
            IList<Result> paging = new List<Result>();
            page--;
            listRequest.Start = page * 10 + 1;
            paging = listRequest.Execute().Items;
            if (paging != null)
                foreach (var item in paging)
                {
                    yield return new WebPage(query, item.Title, HttpUtility.UrlDecode(item.Link), item.Snippet, searchTag);
                }
        }
    }
}
