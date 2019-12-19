using MultiSearch.Domain.Models;
using System.Collections.Generic;

namespace MultiSearch.Web.Models
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel()
        {
            WebPages = new List<WebPage>();
        }

        public IList<WebPage> WebPages { get; set; }
        public string Query { get; set; }

        public override string Title => string.IsNullOrEmpty(Query) ? "Search" : $"{Query}";
    }
}
