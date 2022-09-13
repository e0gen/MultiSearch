using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiSearch.Engines
{
    public class MultiEngine : ISearchEngine
    {
        private readonly ISearchEngine[] _searchers;

        public MultiEngine(ISearchEngine[] searchers)
        {
            _searchers = searchers;
        }


        public async Task<IList<WebPage>> SearchAsync(CancellationToken ct, string query, int page)
        {
            var cts = new CancellationTokenSource();

            var completedTask = await Task.WhenAny(_searchers.Select(x => x.SearchAsync(cts.Token, query, page)));

            cts.Cancel();

            return completedTask.Result;
        }
    }
}
