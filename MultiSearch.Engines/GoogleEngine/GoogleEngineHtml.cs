using HtmlAgilityPack;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace MultiSearch.Engines
{
    public class GoogleEngineHtml : ISearchEngine
    {
        private const string searchTag = "Google Html";
        private const string uriBase = "http://www.google.com";

        private readonly HttpClient _client;

        public GoogleEngineHtml()
        {
            _client = new HttpClient();
            //Google provide obfuscated less parsable response if userAgent was undefined.
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
        }

        public string SearchUri(string query, int page)
        {
            return $"{uriBase}/search?q={Uri.EscapeDataString(query)}&start={page}";
        }

        public IEnumerable<WebPage> Search(string query, int page)
        {
            HttpResponseMessage response = _client.GetAsync(SearchUri(query, page)).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                var doc = new HtmlDocument();
                doc.LoadHtml(data);
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='g']");

                foreach (HtmlNode node in nodes)
                {
                    var titleNode = node.Descendants("h3").First(x => x.Attributes["class"].Value.Contains("LC20lb"));
                    var linkNode = node.Descendants("a").First();
                    var snippetNode = node.Descendants("span").Where(x => x.Attributes.Count > 0).First(x => x.Attributes["class"].Value.Contains("st"));

                    yield return new WebPage(query, titleNode?.InnerText, linkNode?.Attributes["href"].Value, snippetNode?.InnerText, searchTag);
                }
            }
        }
    }
}
