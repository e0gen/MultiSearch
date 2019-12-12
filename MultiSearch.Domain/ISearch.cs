using System;
using System.Collections.Generic;
using System.Text;

namespace MultiSearch.Domain
{
    public interface ISearch
    {
        IEnumerable<Item> Search(string query);
    }
}
