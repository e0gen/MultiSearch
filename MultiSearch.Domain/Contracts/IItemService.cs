using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiSearch.Domain.Models;

namespace MultiSearch.Domain.Contracts
{
    public interface IItemService
    {
        void AddItem(Item item);
        void SaveChanges();
        Task<IList<Item>> GetItemsAsync();
        Task<IList<Item>> GetItemsAsync(string filter);
        //IQueryable<Item> GetItems();
    }
}
