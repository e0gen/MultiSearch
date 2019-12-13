using MultiSearch.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiSearch.DataAccess
{
    public class ItemService
    {
        private readonly WorkDBContext context;

        public ItemService(WorkDBContext context)
        {
            this.context = context;
        }
        public void AddItem(Item item)
        {
            var itemDb = new ItemDb(item.Queue, item.Title, item.Link, item.Snippet, item.Engine);
            context.Items.Add(itemDb);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public IList<Item> GetItems(int start, int count)
        {
            return context.Items
                .Select(x => new Item(x.Queue, x.Title, x.Link, x.Snippet, x.Engine))
                .Skip(start)
                .Take(count)
                .ToList();
        }

        public IEnumerable<Item> Find(string term)
        {
            return context.Items
                .Where(b => b.Queue.Contains(term))
                .OrderBy(b => b.ItemDbId)
                .ToList();
        }
    }
}
