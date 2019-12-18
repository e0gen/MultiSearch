using System.Collections.Generic;
using MultiSearch.Domain.Models;
using MultiSearch.Domain.Contracts;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System;
using HtmlAgilityPack;
using System.Web;

namespace MultiSearch.Engines
{
    public class BingEngineHtml : ISearchEngine
    {
        private const string searchTag = "Bing Html";
        private const string uriBase = "http://www.bing.com";

        private readonly HttpClient client;

        public BingEngineHtml()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
        }

        public string SearchUri(string query, int page)
        {
            page--;
            return $"{uriBase}/search?q={Uri.EscapeDataString(query)}&first={page * 10 + 1}";
        }

        public IEnumerable<WebPage> Search(string query, int page)
        {
            HttpResponseMessage response = client.GetAsync(SearchUri(query, page)).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(System.IO.Path.Combine("D://", "WriteLines2.txt")))
                {
                     outputFile.WriteLine(data);
                }

                var doc = new HtmlDocument();
                HtmlNodeCollection nodes;
                doc.LoadHtml(data);
                //nodes = doc.DocumentNode.SelectNodes("//li[@class='serp-item']");
                nodes = doc.DocumentNode.SelectNodes("//div[@class='g']");

                foreach (HtmlNode node in nodes)
                {
                    var titleNode = node.Descendants("h3").First(x => x.Attributes["class"].Value.Contains("LC20lb"));
                    var linkNode = node.Descendants("a").First();

                    //var snips = node.Descendants("span");
                    var snippetNode = node.Descendants("span").Where(x => x.Attributes.Count > 0).First(x => x.Attributes["class"].Value.Contains("st"));


                    //var nodes1 = node.Descendants("div");
                    //var nodes2 = node.Descendants("a");
                    ////var titleNode = node.Descendants("div").First();
                    ////var titleNode = node.SelectSingleNode("//div/div[1]/a/div[1]");
                    //var linkNode = node.Descendants("a").First();
                    //var titleNode = linkNode.Descendants("div").First();
                    ////var linkNode = node.SelectNodes("//div[1]/div[1]/a[1]").First();
                    //var snippetNode = node.SelectSingleNode("//div/div[3]/div/div/div/div/div");

                    //var val1 = HttpUtility.HtmlDecode(titleNode.InnerText);
                    //var val2 = linkNode.Attributes["href"].Value;
                    //var val3 = HttpUtility.HtmlDecode(snippetNode.InnerText);


                    yield return new WebPage(query, titleNode?.InnerText, linkNode?.Attributes["href"].Value, snippetNode?.InnerText, searchTag);
                }
            }
        }
    }
}
