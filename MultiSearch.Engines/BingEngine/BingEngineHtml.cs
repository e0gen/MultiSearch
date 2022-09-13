using HtmlAgilityPack;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Text;
using System.Threading;

namespace MultiSearch.Engines
{
    public class BingEngineHtml : ISearchEngine
    {
        private const string searchTag = "Bing Html";
        private const string uriBase = "http://bing.com";

        private readonly HttpClient _client;

        public BingEngineHtml(HttpClient httpClient)
        {
            _client = httpClient;
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
                var doc = new HtmlDocument();
                doc.LoadHtml(data);

                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//li[@class='b_algo']");

                if (nodes != null)
                    return nodes.Select(node => {

                        var titleNode = node.Descendants("h2").FirstOrDefault();
                        var linkNode = node.Descendants("a").FirstOrDefault();
                        var snippetNode = node.Descendants("p").FirstOrDefault();

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
