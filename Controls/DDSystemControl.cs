using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace WebSite.Core 
{ 
    public class DDSystemControl : DropDownList{

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            Items.Clear();
            Items.Add("");
            Dictionary<string, string> dict = SystemControl.GetSystemControlList();
            foreach (KeyValuePair<string, string> pair in dict)
            {
                Items.Add(new ListItem(pair.Value, pair.Key));
            }
            
        } 
    }
}
