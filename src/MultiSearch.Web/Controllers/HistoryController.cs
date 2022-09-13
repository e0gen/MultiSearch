using Microsoft.AspNetCore.Mvc;
using MultiSearch.Domain.Contracts;
using MultiSearch.Web.Models;
using System.Threading.Tasks;

namespace MultiSearch.Web.Controllers
{
    [Route("History")]
    public class HistoryController : Controller
    {
        private readonly IWebPageService _webPageService;

        public HistoryController(IWebPageService webPageService)
        {
            _webPageService = webPageService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index(string currentFilter, int? pageNumber)
        {
            var vm = new HistoryViewModel
            {
                CurrentFilter = currentFilter,
                PageNumber = pageNumber ?? 1,
                PageSize = 10,
                WebPages = (!string.IsNullOrEmpty(currentFilter))
                    ? await _webPageService.GetWebPagesAsync(currentFilter)
                    : await _webPageService.GetWebPagesAsync()
            };


            return View(vm);
        }
    }
}