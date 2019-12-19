using HtmlAgilityPack;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace MultiSearch.Engines
{
    public class YandexEngineHtml : ISearchEngine
    {
        private const string searchTag = "Yandex Html";
        private const string uriBase = "http://yandex.ru";

        private readonly HttpClient _client;

        public YandexEngineHtml()
        {
            _client = new HttpClient();
        }

        public string SearchUri(string query, int page)
        {
            return $"{uriBase}/yandsearch?text={Uri.EscapeDataString(query)}&amp;p={page}&amp;rnd=28759";
        }

        public IEnumerable<WebPage> Search(string query, int page)
        {
            HttpResponseMessage response = _client.GetAsync(SearchUri(query, page)).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                var doc = new HtmlDocument();
                doc.LoadHtml(data);

                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//li[@class='serp-item']");

                if (nodes != null)
                    foreach (HtmlNode node in nodes)
                    {
                        var titleNode = node.Descendants("div").Where(x => x.Attributes.Contains("class"))
                            .FirstOrDefault(x => x.Attributes["class"].Value.Contains("organic__url-text"));
                        var linkNode = node.Descendants("a").Where(x => x.Attributes.Contains("class"))
                            .FirstOrDefault(x => x.Attributes["class"].Value.Contains("link_theme_normal"));
                        var snippetNode = node.Descendants("span")
                            .FirstOrDefault(x => x.Attributes["class"].Value.Contains("extended-text__short"));

                        var tiltle = HttpUtility.HtmlDecode(titleNode?.InnerText);
                        var link = HttpUtility.HtmlDecode(linkNode?.Attributes["href"].Value);
                        var snippet = HttpUtility.HtmlDecode(snippetNode?.InnerText);

                        yield return new WebPage(query, tiltle, link, snippet, searchTag);
                    }
            }
        }
    }
}
