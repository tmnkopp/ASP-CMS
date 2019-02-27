using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Core
{
    public interface ILinkable  
    {
        int ID { get; }
        string Caption { get;   } 
        string URL { get;   }
        string SystemURL { get; set; }
        string ShortDesc { get;  } 
    } 
}
