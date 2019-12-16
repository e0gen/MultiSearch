using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using MultiSearch.Web.Models;

namespace MultiSearch.Web.Controllers
{
    [Route("History")]
    public class HistoryController : Controller
    {
        private readonly IItemService _itemService;

        public HistoryController(IItemService itemService)
        {
            _itemService = itemService;
        }
        
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var vm = new HistoryViewModel() { PageNumber = pageNumber ?? 1, PageSize = 10 };

            vm.Items = (!string.IsNullOrEmpty(searchString)) ?
                await _itemService.GetItemsAsync(searchString) :
                await _itemService.GetItemsAsync();

            return View(vm);
        }
    }
}