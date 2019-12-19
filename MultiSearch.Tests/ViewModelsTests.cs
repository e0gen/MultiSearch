using MultiSearch.Web.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiSearch.Tests
{
    [TestFixture]
    public class ViewModelsTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("Search request")]
        public void SearchViewModelAddQueryInTitle(string q)
        {
            var sut = new SearchViewModel() { Query = q };

            if(string.IsNullOrEmpty(q))
                Assert.AreEqual("Search", sut.Title);
            else
                Assert.AreEqual(q, sut.Title);
        }
    }
}
