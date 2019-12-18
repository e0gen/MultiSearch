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
using MultiSearch.Engines;

namespace MultiSearch.Web.Controllers
{
    [Route("")]
    public class SearchController : Controller
    {
        private readonly IWebPageService _itemService;
        private readonly ISearchEngine _searchEngine;
        public SearchController(ISearchEngine searchEngine, IWebPageService webPageService)
        {
            _searchEngine = searchEngine;
            _itemService = webPageService;
        }

        
        public IActionResult Index()
        {
            return View("Search", new SearchViewModel());
        }

        [Route("/search")]
        public async Task<IActionResult> Search([FromQuery]string q)
        {
            if (string.IsNullOrEmpty(q)) return View(new SearchViewModel());

            var vm = new SearchViewModel() { Queue = q };

            var results = new List<WebPage>();

            results.AddRange(_searchEngine.Search(q));

            foreach(var item in results)
            {
                await _itemService.AddWebPageAsync(item);
            }
            await _itemService.SaveChangesAsync();

            vm.Items = results;
            return View(vm);
        }
    }
}