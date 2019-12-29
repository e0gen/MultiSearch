using MultiSearch.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiSearch.Domain.Contracts
{
    public interface ISearchEngine
    {
        IEnumerable<WebPage> Search(string query, int page = 1);
        Task<IList<WebPage>> SearchAsync(string query, int page = 1);
    }
}
