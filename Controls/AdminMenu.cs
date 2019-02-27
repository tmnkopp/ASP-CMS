using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPXUtils;
using System.IO;
using System.Diagnostics;
namespace WebSite.Core
{
    [ToolboxData("<{0}:AdminMenu id=\"oAdminMenu\" runat=\"server\"></{0}:AdminMenu>")]
    public class AdminMenu : Control
    {
        #region Properties
        public string CssClass { get; set; }
        public string BasePage { get; set; }
        #endregion
        protected override void Render(HtmlTextWriter writer)
        {
            if (Security.IsAdmin())
            {
            writer.WriteLine("<ul class='adminmenu " + this.CssClass + "'>"); 
                var folders = from folder in new DirectoryInfo(HttpContext.Current.Server.MapPath("~/admin")).GetDirectories()
                              where !folder.Name.StartsWith("_")
                              select folder;  

                writer.WriteLine("<li><a href='/admin/'>Admin Home</a></li> ");
                writer.WriteLine("<li><a href='/admin/SiteSettings.aspx?etype=SiteSettings'>Site Settings</a></li> ");
                if (folders != null)
                { 
                    foreach (DirectoryInfo d in folders)
                    {
                        writer.WriteLine("<li><a href='/admin/EntityGridForm.aspx?etype=" + d.Name + "'>" + StringUtils.ParseCamel(d.Name) + "</a></li> ");
                        //writer.WriteLine("<li><a href='/admin/" + d.Name + "/" + Convert.ToString(this.BasePage) + "'>" + StringUtils.ParseCamel(d.Name) + "</a></li> ");
                    }
                } 
            writer.WriteLine("</ul>");
            }
            base.Render(writer);
        }
    }
}
