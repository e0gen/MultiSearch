using HtmlAgilityPack;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            var sb = new StringBuilder(uriBase);
            sb.AppendFormat($"/search?q={Uri.EscapeDataString(query)}");
            if (page > 1)
                sb.AppendFormat($"&start={page}");
            return sb.ToString();
        }
        
        public async Task<IList<WebPage>> SearchAsync(string query, int page)
        {
            HttpResponseMessage response = await _client.GetAsync(SearchUri(query, page));
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(data);

                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='g']");

                if (nodes != null)
                    return nodes.Select(node => {

                        var titleNode = node.Descendants("h3").Where(x => x.Attributes.Contains("class"))
                            .FirstOrDefault(x => x.Attributes["class"].Value.Contains("LC20lb"));
                        var linkNode = node.Descendants("a")
                            .FirstOrDefault();
                        var snippetNode = node.Descendants("span").Where(x => x.Attributes.Contains("class"))
                            .FirstOrDefault(x => x.Attributes["class"].Value.Contains("st"));

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
