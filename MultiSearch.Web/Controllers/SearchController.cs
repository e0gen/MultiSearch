using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MultiSearch.Domain;
using MultiSearch.Domain.Models;
using MultiSearch.Web.Models;
using SearchEngines;

namespace MultiSearch.Web.Controllers
{
    [Route("")]
    public class SearchController : Controller
    {
        public IServiceProvider Provider { get; set; }
        public SearchController(IServiceProvider provider)
        {
            Provider = provider;
        }

        
        public IActionResult Index()
        {
            return View();
        }
        [Route("/search")]
        public IActionResult Search([FromQuery]string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return View();

            }


            var results = new List<Item>();
            var searches = Provider.GetServices<ISearch>();
            var bingSearch = searches.First(o => o.GetType() == typeof(BingSearch));
            results.AddRange(bingSearch.Search(q));

            //CancellationTokenSource cts = new CancellationTokenSource();
            //ParallelOptions po = new ParallelOptions
            //{
            //    CancellationToken = cts.Token,
            //    MaxDegreeOfParallelism = System.Environment.ProcessorCount
            //};


            //object lockObj = new object();
            //Parallel.ForEach(searches,
            //    //(q) => new List<Item>(),
            //    po,
            //    (searcher) => // body
            //    {
            //        po.CancellationToken.ThrowIfCancellationRequested();
            //        var tmpResults = searcher.Search(search);

            //        lock (lockObj)
            //        {
            //            results.AddRange(tmpResults);
            //            cts.Cancel();
            //        }
            //    });


            //return View(results);

            return View(new SearchViewModel() { Items = results });
        }
        //var serviceB = services.First(o => o.GetType() == typeof(ServiceB));

    }
}