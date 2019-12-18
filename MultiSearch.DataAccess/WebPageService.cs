using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;

namespace MultiSearch.DataAccess
{
    public class WebPageService : IWebPageService
    {
        private readonly WorkDbContext _context;

        public WebPageService(WorkDbContext context)
        {
            _context = context;
        }
        public async Task AddWebPageAsync(WebPage webPage)
        {
            var webPageEntity = new WebPageEntity(webPage.Queue, webPage.Title, webPage.Link, webPage.Snippet, webPage.Engine);
            await _context.WebPages.AddAsync(webPageEntity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IList<WebPage>> GetWebPagesAsync()
        {
            return await GetWebPages()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IList<WebPage>> GetWebPagesAsync(string filter)
        {
            return await GetWebPages()
                .Where(x => x.Queue.Contains(filter))
                .AsNoTracking()
                .ToListAsync();
        }

        private IQueryable<WebPage> GetWebPages()
        {
            return _context.WebPages
                .Select(x => new WebPage(x.Queue, x.Title, x.Link, x.Snippet, x.Engine));
        }

        //private IQueryable<Item> FindItems(this IQueryable<Item> source, string filter)
        //{
        //    return source.Where(x => x.Queue.Contains(filter));
        //}

        //public IEnumerable<WebPage> Find(string term)
        //{
        //    return _context.WebPages
        //        .Where(b => b.Queue.Contains(term))
        //        .OrderBy(b => b.WebPageEntityId)
        //        .ToList();
        //}
    }
}
