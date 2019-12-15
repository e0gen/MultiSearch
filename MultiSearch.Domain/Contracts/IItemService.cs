using System.Collections.Generic;
using System.Threading.Tasks;
using MultiSearch.Domain.Models;

namespace MultiSearch.Domain.Contracts
{
    public interface IItemService
    {
        Task<IList<Item>> ItemsAsync();
    }
}
