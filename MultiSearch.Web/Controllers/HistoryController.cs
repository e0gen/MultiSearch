using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MultiSearch.Domain;
using MultiSearch.Domain.Contracts;
using MultiSearch.Web.Models;

namespace MultiSearch.Web.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IItemService _itemService;

        public HistoryController(IItemService itemService)
        {
            _itemService = itemService;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        // GET: ItemDbs
        [HttpGet("History")]
        [HttpGet("History/Index")]
        public async Task<IActionResult> Index()
        {
            return View(new HistoryViewModel() { Items = await _itemService.ItemsAsync()} );
        }
    }
}