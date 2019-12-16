using MultiSearch.Domain;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MultiSearch.Domain.Contracts;
using MultiSearch.Domain.Models;

namespace MultiSearch.DataAccess
{
    public class ItemService : IItemService
    {
        private readonly WorkDbContext _context;

        public ItemService(WorkDbContext context)
        {
            _context = context;
        }
        public void AddItem(Item item)
        {
            var itemDb = new ItemDb(item.Queue, item.Title, item.Link, item.Snippet, item.Engine);
            _context.Items.Add(itemDb);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<IList<Item>> GetItemsAsync()
        {
            return await GetItems()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IList<Item>> GetItemsAsync(string filter)
        {
            return await GetItems()
                .Where(x => x.Queue.Contains(filter))
                .AsNoTracking()
                .ToListAsync();
        }

        private IQueryable<Item> GetItems()
        {
            return _context.Items
                .Select(x => new Item(x.Queue, x.Title, x.Link, x.Snippet, x.Engine));
        }

        //private IQueryable<Item> FindItems(this IQueryable<Item> source, string filter)
        //{
        //    return source.Where(x => x.Queue.Contains(filter));
        //}

        public IEnumerable<Item> Find(string term)
        {
            return _context.Items
                .Where(b => b.Queue.Contains(term))
                .OrderBy(b => b.ItemDbId)
                .ToList();
        }
    }
}
