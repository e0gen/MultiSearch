using System;

namespace MultiSearch.Engines
{
    public class BingCustomSearchResponse
    {
        public string _type { get; set; }
        public BingWebPages webPages { get; set; }
    }

    public class BingWebPages
    {
        public string webSearchUrl { get; set; }
        public int totalEstimatedMatches { get; set; }
        public BingWebPage[] value { get; set; }
    }

    public class BingWebPage
    {
        public string name { get; set; }
        public string url { get; set; }
        public string displayUrl { get; set; }
        public string snippet { get; set; }
        public DateTime dateLastCrawled { get; set; }
        public string cachedPageUrl { get; set; }
        public BingOpenGraphImage openGraphImage { get; set; }
    }

    public class BingOpenGraphImage
    {
        public string contentUrl { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
