using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<IList<WebPage>> SearchAsync(string query, int page)
        {
            var completedTask = await Task.WhenAny(_searchers.Select(x => x.SearchAsync(query, page)));

            return await completedTask;
        }
    }
}
