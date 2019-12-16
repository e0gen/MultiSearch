using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using System.Collections.Generic;
using MultiSearch.Domain.Models;
using MultiSearch.Domain.Contracts;

namespace SearchEngines
{
    public class GoogleSearch : ISearch
    {
        private const string searchTag = "Google";

        private readonly string _apiKey; // = "AIzaSyAt8AkrmkiLVghrcKA3lFh37R79rSG0NsE";
        private readonly string _searchEngineId; // = "003470263288780838160:ty47piyybua";
        private readonly CustomsearchService _customSearchService;// );

        public GoogleSearch(string apiKey, string searchEngineId)
        {
            _apiKey = apiKey;
            _searchEngineId = searchEngineId;
            _customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
        }

        public IEnumerable<Item> Search(string query)
        {
            var listRequest = _customSearchService.Cse.List(query);
            listRequest.Cx = _searchEngineId;
            
            IList<Result> paging = new List<Result>();
            var count = 0;
            while (paging != null)
            {
                listRequest.Start = count * 10 + 1;
                paging = listRequest.Execute().Items;
                if (paging != null)
                    foreach (var item in paging)
                    {
                        yield return new Item(query, item.Title, item.Link, item.Snippet, searchTag);
                    }
                count++;
            }
        }
    }
}
