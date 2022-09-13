using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MultiSearch.Engines
{
    public class BingEngineApi : ISearchEngine
    {
        private const string searchTag = "Bing Api";
        private const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0";

        private readonly string _apiKey;
        private readonly HttpClient _client;
        public BingEngineApi(string apiKey, HttpClient httpClient)
        {
            _apiKey = apiKey;
            _client = httpClient;
            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public string SearchUri(string query, int page)
        {
            var sb = new StringBuilder(uriBase);
            sb.AppendFormat($"/search?q={Uri.EscapeDataString(query)}");
            if (page > 1)
                sb.AppendFormat($"&first={(page - 1) * 10 + 1}");
            return sb.ToString();
        }

        public async Task<IList<WebPage>> SearchAsync(CancellationToken ct, string query, int page)
        {
            HttpResponseMessage response = await _client.GetAsync(SearchUri(query, page), ct);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                BingCustomSearchResponse bingResponse = JsonConvert.DeserializeObject<BingCustomSearchResponse>(data);

                return bingResponse.WebPages.Value
                    .Select(webPage =>
                        new WebPage(query, webPage.Name, HttpUtility.UrlDecode(webPage.Url), webPage.Snippet, searchTag))
                    .ToList();
            }
            return new List<WebPage>();
        }
    }
}
