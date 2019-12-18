using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiSearch.Domain.Models;

namespace MultiSearch.Domain.Contracts
{
    public interface IWebPageService
    {
        Task AddWebPageAsync(WebPage webPage);
        Task SaveChangesAsync();
        Task<IList<WebPage>> GetWebPagesAsync();
        Task<IList<WebPage>> GetWebPagesAsync(string filter);
        //IQueryable<Item> GetItems();
    }
}
