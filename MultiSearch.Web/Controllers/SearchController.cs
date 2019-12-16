using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using MultiSearch.Web.Models;
using SearchEngines;

namespace MultiSearch.Web.Controllers
{
    [Route("")]
    public class SearchController : Controller
    {
        public IServiceProvider _provider { get; set; }
        public IItemService _itemService { get; set; }
        public SearchController(IServiceProvider provider, IItemService itemService)
        {
            _provider = provider;
            _itemService = itemService;
        }

        
        public IActionResult Index()
        {
            return View("Search", new SearchViewModel());
        }

        [Route("/search")]
        public IActionResult Search([FromQuery]string q)
        {
            if (string.IsNullOrEmpty(q)) return View();

            var vm = new SearchViewModel() { Queue = q };

            var results = new List<Item>();
            var searches = _provider.GetServices<ISearch>();
            var bingSearch = searches.First(o => o.GetType() == typeof(BingSearch));
            results.AddRange(bingSearch.Search(q));

            foreach(var item in results)
            {
                _itemService.AddItem(item);
            }
            _itemService.SaveChanges();

            vm.Items = results;
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

            return View(vm);
        }
        //var serviceB = services.First(o => o.GetType() == typeof(ServiceB));

    }
}