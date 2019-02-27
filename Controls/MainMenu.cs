using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace WebSite.Core.Controls
{

    public enum MenuTypes { 
        MainOnly, 
        SubInline,
        SubUnder
    }
    [ToolboxData("<{0}:MainMenu runat=server></{0}:MainMenu>")]
    public class MainMenu: CompositeControl
    { 
        protected Literal litHTML;
        public string CssClass { get; set; }
        public bool DisplaySelectedSub{ get; set; }
        public MenuTypes MenuType { get; set; }

        protected StringBuilder objSB = new StringBuilder();

        protected void CreateControlHierarchy()
        {
            if (this.CssClass == null)
                this.CssClass = "mainmenu";

            RenderMainMenu();
            litHTML = new Literal(); 
            Controls.Add(litHTML);
            litHTML.Text = objSB.ToString();
            
        }
        private void RenderMainMenu()
        {
            List<WebPage> WebPages = new WebPage().SelectAllMain();
 
            HTML.Append("<ul class=\"" + this.CssClass + "\">");
            foreach (WebPage link in WebPages)
            {
                HTML.Append("<li class=\"" + this.IsSelected(link.URL) + "\">");
                HTML.Append(FormatLink(link.URL, link.Caption));
                HTML.Append("</li>");
                if (this.Seperator != null)
                    HTML.Append(this.Seperator);
            }
            HTML.Append("</ul>"); 
        }


        private string FormatLink(string URL, string Caption)
        {
            StringBuilder HTML = new StringBuilder();
            HTML.Append("<a href=\"");
            HTML.Append(URL);
            HTML.Append("\"  class=\"" + this.IsSelected(URL) + "\">");
            HTML.Append(Caption);
            HTML.Append("</a>");
            return HTML.ToString();
        }
        private string IsSelected(string arg)
        {
            string ret = "";
            arg = arg.ToLower();
            if (NavUtil.RequestPath.ToLower().Contains(arg))
                ret = "sel";
            return ret;
        }
 
        protected override void CreateChildControls()
        {
            Controls.Clear();
            CreateControlHierarchy();

        }
        protected override void Render(HtmlTextWriter writer)  {
            RenderContents(writer);
        }

    }
}
