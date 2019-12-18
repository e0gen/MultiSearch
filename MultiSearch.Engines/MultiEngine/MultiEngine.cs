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

        public IEnumerable<WebPage> Search(string query, int page)
        {
            var results = new List<WebPage>();
            long minElapsed = long.MaxValue;

            object lockObj = new object();
            Parallel.ForEach(_searchers,
                (searcher) =>
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    var tmpResult = searcher.Search(query, page).ToList();
                    watch.Stop();
                    var elapsed = watch.ElapsedMilliseconds;

                    lock (lockObj)
                    {
                        if (elapsed < minElapsed)
                        {
                            minElapsed = elapsed;
                            results = tmpResult;
                        }
                    }
                });
            return results;
        }
    }
}
