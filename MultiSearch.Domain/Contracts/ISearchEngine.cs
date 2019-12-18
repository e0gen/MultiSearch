﻿using MultiSearch.Domain.Models;
using System.Collections.Generic;

namespace MultiSearch.Domain.Contracts
{
    public interface ISearchEngine
    {
        IEnumerable<WebPage> Search(string query, int page = 1);
    }
}