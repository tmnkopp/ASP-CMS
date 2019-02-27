using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using ASPXUtils;
using System.Web.UI.WebControls;

namespace WebSite.Core
{
    [ToolboxData("<{0}:BreadCrumbs runat=server></{0}:BreadCrumbs>")]
    public class BreadCrumbs : Control
    {
        #region Properties
        
        public string CssClass { get; set; }
        public string RootURL { get; set; }
        public string RootCaption { get; set; }
        public string Seperator { get; set; }
        public bool LinkLast { get; set; }
        private List<ILinkable> _Links = new List<ILinkable>();
        public List<ILinkable> Links {
            get {  return _Links; }
            set {  _Links = value;  
            }
        }

        #endregion

        public void AddLink(ILinkable link) {
            if (_Links==null)
                _Links = new List<ILinkable>();
            _Links.Add(link);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if(this.Links.Count < 1){
                //string rp = URLTranslator.Instance.RewritePath;
                this._Links.AddRange(URLTranslator.Instance.BreadCrumbs);
                //URLTranslator oURLTranslator = new URLTranslator(NavUtil.RequestPath);
                //this._Links.AddRange(oURLTranslator.BreadCrumbs);
            } 
 
            writer.WriteLine("<ul id=\"\" class=\"breadcrumbs " + this.CssClass + "\">");

            if (this.RootCaption != null)
            {
                    if (this.RootURL == null)
                        this.RootURL = NavUtil.URLRoot() + SiteSettings.Instance.NavRootDocumentName; 
                writer.WriteLine("<li id=\"\" >");

                RenderMenuItem(writer, this.RootURL, this.RootCaption);

                if (this.Seperator != null)
                    writer.Write(this.Seperator);

                writer.WriteLine("</li>");
            }

            int itemcnt = 0;
           // string url = "";
            foreach (ILinkable link in Links)
            {
                if (link!=null)
                {
                    writer.WriteLine("<li id=\"\" >");

                    if ((itemcnt == Links.Count() - 1) && !this.LinkLast)
                    {
                        writer.WriteLine(link.Caption);
                    }
                    else
                    {
                        RenderMenuItem(writer, link.URL, link.Caption);
                    }

                    if (this.Seperator != null && itemcnt != Links.Count() - 1)
                        writer.Write(this.Seperator);

                    writer.WriteLine("</li>");
                    itemcnt++;
                } 
            }

            writer.WriteLine("</ul>");
            
        }
        protected virtual void RenderMenuItem(HtmlTextWriter writer, string URL, string Caption )
        {
            writer.Write("<a href=\"");
            writer.Write(URL);
            writer.Write("\">");
            writer.Write(Caption);
            writer.WriteLine("</a>");
        }
    }
}
