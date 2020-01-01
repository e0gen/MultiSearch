using System;

namespace MultiSearch.Engines
{
    public class BingCustomSearchResponse
    {
        public string _type { get; set; }
        public BingWebPages WebPages { get; set; }
    }

    public class BingWebPages
    {
        public string WebSearchUrl { get; set; }
        public int TotalEstimatedMatches { get; set; }
        public BingWebPage[] Value { get; set; }
    }

    public class BingWebPage
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string DisplayUrl { get; set; }
        public string Snippet { get; set; }
        public DateTime DateLastCrawled { get; set; }
        public string CachedPageUrl { get; set; }
        public BingOpenGraphImage OpenGraphImage { get; set; }
    }

    public class BingOpenGraphImage
    {
        public string ContentUrl { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
