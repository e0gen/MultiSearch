using MultiSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiSearch.Web.Models
{
    public class SearchViewModel
    {
        //private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public SearchViewModel()//RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            //_context = context;
        }

        public IList<Item> Items { get; set; }

        public async Task OnGetAsync()
        {
            //Movie = await _context.Movie.ToListAsync();
        }
    }
}
