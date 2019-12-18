using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiSearch.Web.Models
{
    public class HistoryViewModel : BaseViewModel
    {
        public IList<WebPage> Items { get; set; }
        public string CurrentFilter { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
