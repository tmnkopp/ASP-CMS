using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace WebSite.Core 
{
    public class DDGalleryCats : DropDownList
    {

        protected override void OnInit(System.EventArgs e)
        {
            Items.Clear();
            List<GalleryCategory> oList = new GalleryCategory().SelectAll();
            Items.Add(new ListItem("", ""));
            foreach (GalleryCategory item in oList)
                Items.Add(new ListItem(item.Caption, item.Caption));

            base.OnInit(e);
        }
    }


}

