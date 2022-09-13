using Microsoft.AspNetCore.Mvc;
using MultiSearch.Domain.Contracts;
using MultiSearch.Web.Models;

namespace MultiSearch.Web.Controllers
{
    [Route("")]
    public class SearchController : Controller
    {
        private readonly IWebPageService _webPageService;
        private readonly ISearchEngine _searchEngine;
        public SearchController(ISearchEngine searchEngine, IWebPageService webPageService)
        {
            _searchEngine = searchEngine;
            _webPageService = webPageService;
        }


        public IActionResult Index()
        {
            return View("Search", new SearchViewModel());
        }

        [Route("/search")]
        public async Task<IActionResult> Search([FromQuery]string q, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(q)) return View(new SearchViewModel());

            var vm = new SearchViewModel() { Query = q };

            var results = await _searchEngine.SearchAsync(ct, q);

            foreach (var wp in results)
            {
                await _webPageService.AddWebPageAsync(wp);
            }
            await _webPageService.SaveChangesAsync();

            vm.WebPages = results;
            return View(vm);
        }
    }
}