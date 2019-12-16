using MultiSearch.Domain;
using MultiSearch.Domain.Models;

namespace MultiSearch.DataAccess
{
    public class ItemDb : Item
    {
        public ItemDb(string queue, string title, string link, string snippet, string engine) : base(queue, title, link, snippet, engine)
        {
        }

        public long ItemDbId { get; set; }
    }
}
