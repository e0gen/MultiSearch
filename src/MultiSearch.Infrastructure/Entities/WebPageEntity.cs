using MultiSearch.Domain.Models;

namespace MultiSearch.Infrastructure.Entities
{
    public class WebPageEntity : WebPage
    {
        public WebPageEntity(string query, string title, string link, string snippet, string engine) : base(query, title, link, snippet, engine)
        {
        }

        public long WebPageEntityId { get; set; }
    }
}
