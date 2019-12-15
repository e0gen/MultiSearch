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

        public async Task<IList<Item>> ItemsAsync()
        {
            return await _context.Items
                .Select(x => new Item(x.Queue, x.Title, x.Link, x.Snippet, x.Engine)).ToListAsync();
        }

        public IList<Item> GetItems(int start, int count)
        {
            return _context.Items
                .Select(x => new Item(x.Queue, x.Title, x.Link, x.Snippet, x.Engine))
                .Skip(start)
                .Take(count)
                .ToList();
        }

        public IEnumerable<Item> Find(string term)
        {
            return _context.Items
                .Where(b => b.Queue.Contains(term))
                .OrderBy(b => b.ItemDbId)
                .ToList();
        }
    }
}
