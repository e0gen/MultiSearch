using System.Collections.Generic;
using MultiSearch.Domain.Models;

namespace MultiSearch.Domain
{
    public interface ISearch
    {
        IEnumerable<Item> Search(string query);
    }
}
