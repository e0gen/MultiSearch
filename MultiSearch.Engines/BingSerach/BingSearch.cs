using MultiSearch.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SearchEngines
{
    public class BingSearch : ISearch
    {
        private readonly string _apiKey;

        private const string searchTag = "Bing";
        private const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0";
        private HttpClient client;
        public BingSearch(string apiKey)
        {
            _apiKey = apiKey;
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
        }

        public async Task<IEnumerable<Item>> SearchAsync(string query)
        {
            var url = $"{uriBase}/search?q={Uri.EscapeDataString(query)}";
            HttpResponseMessage response = await client.GetAsync(url);

            var items = new List<Item>();
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                BingCustomSearchResponse bingResponse = JsonConvert.DeserializeObject<BingCustomSearchResponse>(data);
                
                for (int i = 0; i < bingResponse.webPages.value.Length; i++)
                {
                    var webPage = bingResponse.webPages.value[i];

                    items.Add(new Item(query, webPage.name, webPage.url, webPage.snippet, searchTag));
                }
            }
            return items;
        }

        public IEnumerable<Item> Search(string query)
        {
            var url = $"{uriBase}/search?q={Uri.EscapeDataString(query)}";
            HttpResponseMessage response = client.GetAsync(url).Result;

            var items = new List<Item>();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                BingCustomSearchResponse bingResponse = JsonConvert.DeserializeObject<BingCustomSearchResponse>(data);
                
                for (int i = 0; i < bingResponse.webPages.value.Length; i++)
                {
                    var webPage = bingResponse.webPages.value[i];
                    
                    yield return new Item(query, webPage.name, webPage.url, webPage.snippet, searchTag);
                }
            }
        }
    }
}
