using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using ASPXUtils;
namespace WebSite.Core
{ 
    public abstract class BaseNavMenu : Control
    {
        #region Properties
 
        public string CssClass { get; set; }
        public string Seperator { get; set; } 
        #endregion
        private string sel = "";
        protected virtual void RenderList<T>(HtmlTextWriter writer, List<T> Links) where T : ILinkable  
        {
            int itemcnt = 0;
            sel = "";
            string url = "";
            writer.WriteLine("<ul id=\"\" class=\"" + this.CssClass + "\">");
            foreach (ILinkable link in Links)
            {
                if (link != null)
                {
                    sel = "";
                    if (IsSelected(link.URL))
                        sel = "sel";

                    writer.WriteLine("<li id=\"\" class=\"" + sel + "\">");

                    url = link.URL;
                    //if (link.SystemURL != "")
                    //    url = link.SystemURL;
                

                    RenderMenuItem(writer, url, link.Caption, sel);
                    writer.WriteLine("</li>");

                    if (this.Seperator != null)
                        writer.Write(this.Seperator);
                    itemcnt++; 
                }
            }

            writer.WriteLine("</ul>");

        }
        protected virtual void RenderMenuItem(HtmlTextWriter writer, string URL, string Caption, string sel)
        {
            writer.Write("<a href=\"");
            writer.Write(URL);
            writer.Write("\"  class=\"" + sel + "\">");
            writer.Write(Caption);
            writer.WriteLine("</a>"); 
        }
        protected virtual bool IsSelected(string arg)
        {
            bool ret = false;
            arg = arg.ToLower();
            if (NavUtil.RequestPath.ToLower().Contains(arg))
                ret = true;
            return ret;
        }
    }
}
