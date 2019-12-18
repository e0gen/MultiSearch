using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiSearch.Web.Models
{
    public abstract class BaseViewModel
    {
        public virtual string Title
        {
            get
            {
                return "";
            }
        }
    }
}
