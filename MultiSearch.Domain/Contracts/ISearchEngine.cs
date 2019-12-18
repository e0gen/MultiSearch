using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSearch.Domain.Models;

namespace MultiSearch.Domain.Contracts
{
    public interface ISearchEngine
    {
        IEnumerable<WebPage> Search(string query, int page = 1);
    }
}
