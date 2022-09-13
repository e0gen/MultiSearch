using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<IList<WebPage>> SearchAsync(CancellationToken ct, string query, int page)
        {
            var listRequest = _customSearchService.Cse.List();
            listRequest.Q = query;
            listRequest.Cx = _searchEngineId;
            listRequest.Start = (page - 1) * 10 + 1;

            var data = await listRequest.ExecuteAsync(ct);
            IList<Result> paging = data.Items;
            if (paging != null)
                return paging
                    .Select(item =>
                        new WebPage(query, item.Title, HttpUtility.UrlDecode(item.Link), item.Snippet, searchTag))
                    .ToList();
            return new List<WebPage>();
        }
    }
}
