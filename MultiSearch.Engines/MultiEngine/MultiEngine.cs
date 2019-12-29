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
            //var tasks = new List<Task<IList<WebPage>>>();
            //foreach (var searcher in _searchers)
            //    tasks.Add(searcher.SearchAsync(query, page));



            var completedTask = await Task.WhenAny(_searchers.Select(x => x.SearchAsync(query, page)));

            return await completedTask;
            //var data = await completedTask;

            ////Task.Run(tasks);

            ////while (tasks.Any())
            ////{
            ////    var completedTask = await Task.WhenAny(tasks);
            ////    if (await completedTask.Result.Count > 0)
            ////        return true;

            ////    tasks.Remove(completedTask);
            ////}



            //foreach (var searcher in _searchers)
            //{
            //    var watch = System.Diagnostics.Stopwatch.StartNew();
            //    var tmpResult = await searcher.SearchAsync(query, page);
            //    if (tmpResult.Count < 10)
            //    {
            //        var nxtTmpResult = await searcher.SearchAsync(query, page++);
            //        for (int i = 0; i < 10 - tmpResult.Count; i++)
            //            tmpResult.Add(nxtTmpResult[i]);
            //    }

            //    watch.Stop();
            //    var elapsed = watch.ElapsedMilliseconds;
            //}
            //Task.WhenAny()


            //var results = new List<WebPage>();
            //long minElapsed = long.MaxValue;

            //object lockObj = new object();
            //Parallel.ForEach(_searchers,
            //    (searcher) =>
            //    {
            //        var watch = System.Diagnostics.Stopwatch.StartNew();
            //        var tmpResult = searcher.Search(query, page).Take(10).ToList();
            //        if (tmpResult.Count < 10)
            //            tmpResult.AddRange(searcher.Search(query, page++).Take(10 - tmpResult.Count));
            //        watch.Stop();
            //        var elapsed = watch.ElapsedMilliseconds;

            //        lock (lockObj)
            //        {
            //            if (elapsed < minElapsed && tmpResult.Count > 0)
            //            {
            //                minElapsed = elapsed;
            //                results = tmpResult;
            //            }
            //        }
            //    });
            //return results;
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
                    var tmpResult = searcher.Search(query, page).Take(10).ToList();
                    if (tmpResult.Count < 10)
                        tmpResult.AddRange(searcher.Search(query, page++).Take(10 - tmpResult.Count));
                    watch.Stop();
                    var elapsed = watch.ElapsedMilliseconds;

                    lock (lockObj)
                    {
                        if (elapsed < minElapsed && tmpResult.Count > 0)
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
