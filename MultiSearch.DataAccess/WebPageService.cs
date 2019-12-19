using Microsoft.EntityFrameworkCore;
using MultiSearch.DataAccess.Entities;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var webPageEntity = new WebPageEntity(webPage.Query, webPage.Title, webPage.Link, webPage.Snippet, webPage.Engine);
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
                .Where(x => x.Query.Contains(filter))
                .AsNoTracking()
                .ToListAsync();
        }

        private IQueryable<WebPage> GetWebPages()
        {
            return _context.WebPages
                .Select(x => new WebPage(x.Query, x.Title, x.Link, x.Snippet, x.Engine));
        }
    }
}
