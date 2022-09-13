using MultiSearch.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiSearch.Domain.Contracts
{
    public interface ISearchEngine
    {
        Task<IList<WebPage>> SearchAsync(CancellationToken token, string query, int page = 1);
    }
}
