using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace WebSite.Core 
{ 
    public class DDParentPages : DropDownList{

        protected override void OnInit(System.EventArgs e)
        {
            Items.Clear();
            List<WebPage> oParentPages = new WebPage().SelectAllMain();
            Items.Add(new ListItem("", "0"));
            foreach (WebPage WP in oParentPages) 
                Items.Add(new ListItem(WP.Name, Convert.ToString(WP.ID)));

            base.OnInit(e);
        }
    }


}

