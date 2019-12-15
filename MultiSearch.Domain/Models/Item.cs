namespace MultiSearch.Domain.Models
{
    public class Item
    {
        public string Queue { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Snippet { get; set; }
        public string Engine { get; set; }

        public Item(string queue, string title, string link, string snippet, string engine)
        {
            Queue = queue;
            Title = title;
            Link = link;
            Snippet = snippet;
            Engine = engine;
        }
    }
}
