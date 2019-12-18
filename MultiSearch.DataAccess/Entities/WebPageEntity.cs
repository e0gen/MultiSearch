using MultiSearch.Domain.Models;

namespace MultiSearch.DataAccess.Entities
{
    public class WebPageEntity : WebPage
    {
        public WebPageEntity(string queue, string title, string link, string snippet, string engine) : base(queue, title, link, snippet, engine)
        {
        }

        public long WebPageEntityId { get; set; }
    }
}
