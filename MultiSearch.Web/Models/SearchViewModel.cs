using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiSearch.Web.Models
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel() : base()
        {
            Items = new List<WebPage>();
        }

        public IList<WebPage> Items { get; set; }
        public string Queue { get; set; }

        public override string Title
        {
            get
            {
                return string.IsNullOrEmpty(Queue) ? "Search" : $"{Queue}";
            }
        }
    }
}
