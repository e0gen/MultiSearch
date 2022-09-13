using MultiSearch.Domain.Models;
using System.Collections.Generic;

namespace MultiSearch.Web.Models
{
    public class HistoryViewModel : BaseViewModel
    {
        public IList<WebPage> WebPages { get; set; }
        public string CurrentFilter { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
