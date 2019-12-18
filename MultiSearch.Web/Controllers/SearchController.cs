using Microsoft.AspNetCore.Mvc;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using MultiSearch.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            List<WebPage> results = _searchEngine.Search(q).ToList();

            foreach (var wp in results)
            {
                await _itemService.AddWebPageAsync(wp);
            }
            await _itemService.SaveChangesAsync();

            vm.Items = results;
            return View(vm);
        }
    }
}