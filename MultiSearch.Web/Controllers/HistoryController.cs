using Microsoft.AspNetCore.Mvc;
using MultiSearch.Domain.Contracts;
using MultiSearch.Web.Models;
using System.Threading.Tasks;

namespace MultiSearch.Web.Controllers
{
    [Route("History")]
    public class HistoryController : Controller
    {
        private readonly IWebPageService _itemService;

        public HistoryController(IWebPageService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var vm = new HistoryViewModel
            {
                PageNumber = pageNumber ?? 1,
                PageSize = 10,
                Items = (!string.IsNullOrEmpty(searchString))
                    ? await _itemService.GetWebPagesAsync(searchString)
                    : await _itemService.GetWebPagesAsync()
            };


            return View(vm);
        }
    }
}