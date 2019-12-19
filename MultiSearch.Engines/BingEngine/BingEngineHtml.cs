﻿using HtmlAgilityPack;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace MultiSearch.Engines
{
    public class BingEngineHtml : ISearchEngine
    {
        private const string searchTag = "Bing Html";
        private const string uriBase = "http://bing.com";

        private readonly HttpClient _client;

        public BingEngineHtml()
        {
            _client = new HttpClient();
        }

        public string SearchUri(string query, int page)
        {
            page--;
            return $"{uriBase}/search?q={Uri.EscapeDataString(query)}&first={page * 10 + 1}";
        }

        public IEnumerable<WebPage> Search(string query, int page)
        {
            HttpResponseMessage response = _client.GetAsync(SearchUri(query, page)).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                var doc = new HtmlDocument();
                doc.LoadHtml(data);

                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//li[@class='b_algo']");

                if (nodes != null)
                    foreach (HtmlNode node in nodes)
                    {
                        var titleNode = node.Descendants("h2").FirstOrDefault();
                        var linkNode = node.Descendants("a").FirstOrDefault();
                        var snippetNode = node.Descendants("p").FirstOrDefault();

                        var tiltle = HttpUtility.HtmlDecode(titleNode?.InnerText);
                        var link = HttpUtility.HtmlDecode(linkNode?.Attributes["href"].Value);
                        var snippet = HttpUtility.HtmlDecode(snippetNode?.InnerText);

                        yield return new WebPage(query, tiltle, link, snippet, searchTag);
                    }
            }
        }
    }
}
