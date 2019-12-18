using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace MultiSearch.Engines
{
    public class BingEngineApi : ISearchEngine
    {
        private const string searchTag = "Bing Api";
        private const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0";

        private readonly string _apiKey;
        private readonly HttpClient _client;
        public BingEngineApi(string apiKey)
        {
            _apiKey = apiKey;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public string SearchUri(string query, int page)
        {
            return $"{uriBase}/search?q={Uri.EscapeDataString(query)}";
        }

        public IEnumerable<WebPage> Search(string query, int page)
        {
            HttpResponseMessage response = _client.GetAsync(SearchUri(query, page)).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                BingCustomSearchResponse bingResponse = JsonConvert.DeserializeObject<BingCustomSearchResponse>(data);

                foreach (var webPage in bingResponse.webPages.value)
                {
                    yield return new WebPage(query, webPage.name, HttpUtility.UrlDecode(webPage.url), webPage.snippet, searchTag);
                }
            }
        }
    }
}
