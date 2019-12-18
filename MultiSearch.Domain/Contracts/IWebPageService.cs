using MultiSearch.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiSearch.Domain.Contracts
{
    public interface IWebPageService
    {
        Task AddWebPageAsync(WebPage webPage);
        Task SaveChangesAsync();
        Task<IList<WebPage>> GetWebPagesAsync();
        Task<IList<WebPage>> GetWebPagesAsync(string filter);
    }
}
