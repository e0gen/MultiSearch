namespace MultiSearch.Domain.Models
{
    public class WebPage
    {
        public string Query { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Snippet { get; set; }
        public string Engine { get; set; }

        public WebPage(string query, string title, string link, string snippet, string engine)
        {
            Query = query;
            Title = title;
            Link = link;
            Snippet = snippet;
            Engine = engine;
        }
    }
}
