using HtmlAgilityPack;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MultiSearch.Engines
{
    public class YandexEngineHtml : ISearchEngine
    {
        private const string searchTag = "Yandex Html";
        private const string uriBase = "http://yandex.ru";

        private readonly HttpClient _client;

        public YandexEngineHtml(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public string SearchUri(string query, int page)
        {
            var sb = new StringBuilder(uriBase);
            sb.Append($"/yandsearch?");
            sb.AppendFormat($"text={Uri.EscapeDataString(query)}&amp;");
            if (page > 1)
                sb.AppendFormat($"&page={page}&amp;");
            sb.Append("rnd=28759");
            return sb.ToString();
        }

        public async Task<IList<WebPage>> SearchAsync(CancellationToken ct, string query, int page)
        {
            HttpResponseMessage response = await _client.GetAsync(SearchUri(query, page), ct);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(data);

                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//li[@class='serp-item']");

                if (nodes != null)
                    return nodes.Select(node => {

                        var titleNode = node.Descendants("div").Where(x => x.Attributes.Contains("class"))
                            .FirstOrDefault(x => x.Attributes["class"].Value.Contains("organic__url-text"));
                        var linkNode = node.Descendants("a").Where(x => x.Attributes.Contains("class"))
                            .FirstOrDefault(x => x.Attributes["class"].Value.Contains("link_theme_normal"));
                        var snippetNode = node.Descendants("span")
                            .FirstOrDefault(x => x.Attributes["class"].Value.Contains("extended-text__short"));

                        var title = HttpUtility.HtmlDecode(titleNode?.InnerText);
                        var link = HttpUtility.HtmlDecode(linkNode?.Attributes["href"].Value);
                        var snippet = HttpUtility.HtmlDecode(snippetNode?.InnerText);

                        return new WebPage(query, title, link, snippet, searchTag);
                    })
                    .ToList();
            }
            return new List<WebPage>();
        }
    }
}
