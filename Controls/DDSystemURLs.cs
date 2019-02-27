using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;

namespace WebSite.Core 
{
    public class DDSystemURLs : DropDownList
    {
        protected override void OnInit(System.EventArgs e)
        {
            Items.Clear();
            Items.Add("");  
            string path = SiteSettings.Instance.TemplatePath;
            if (!path.StartsWith("~")) 
                path = "~" + path;

            System.IO.DirectoryInfo dir;
            if (Directory.Exists(HttpContext.Current.Server.MapPath(path)))
            {
                dir = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(path));
                foreach (System.IO.FileInfo f in dir.GetFiles("*.aspx"))
                {
                    Items.Add(new ListItem()
                    {
                        Text = path + f.Name
                        ,  Value = path + f.Name 
                    });

                }
            }
            dir = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath("~/"));
            foreach (System.IO.FileInfo f in dir.GetFiles("*.aspx"))
            {
                if (SiteSettings.SYSFILE_TEMPLATE_PREFIX != null)
                {
                    if (f.Name.StartsWith(SiteSettings.SYSFILE_TEMPLATE_PREFIX))
                    {
                        Items.Add(new ListItem()
                        {
                            Text = "~/" + f.Name
                            ,
                            Value = "~/" + f.Name
                        });
                    } 
                } 
            }
            base.OnInit(e);
        }
    }

}
