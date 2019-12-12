using System;

namespace MultiSearch.Domain
{
    public class Item
    {

        long Id;
        public string Title;
        public string Link;
        public string Snippet;
        public string Engine;

        public Item(string title, string link, string snippet, string engine)
        {
            Title = title;
            Link = link;
            Snippet = snippet;
            Engine = engine;
        }
    }
}
