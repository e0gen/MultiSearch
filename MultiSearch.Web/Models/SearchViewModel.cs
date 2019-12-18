using MultiSearch.Domain.Models;
using System.Collections.Generic;

namespace MultiSearch.Web.Models
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel()
        {
            Items = new List<WebPage>();
        }

        public IList<WebPage> Items { get; set; }
        public string Queue { get; set; }

        public override string Title => string.IsNullOrEmpty(Queue) ? "Search" : $"{Queue}";
    }
}
