using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WebSite.Core 
{

    public enum LinkableListType { 
        TitleOnly,
        Brief,
        Detailed,
        StaticList
    }
    [ToolboxData("<{0}:LinkableList runat=Server></{0}:LinkableList>")]
    public class LinkableList : Control
    { 
        public ILinkable Item { get; set; }
        public List<ILinkable> Items { get; set; }
        public string CssClass { get; set; }
        public string Seperator { get; set; }
        public int Max { get; set; }
        public LinkableListType ListType { get; set; }
        private StringBuilder HTML = new StringBuilder();

        public string GetHTML() {

            //if (Items == null)  {
            //    if (this.Max != 0)
            //        Items = Item.SelectLinkable().Take(this.Max).ToList();
            //    else
            //        Items = Item.SelectLinkable().ToList();
            //}

            //RenderItems();
            return HTML.ToString();
        }

        protected override void Render(HtmlTextWriter writer)
        { 
            base.Render(writer); 
            writer.Write(GetHTML()); 
        }
        protected void RenderItems( )
        {
            string tagmain = "ul";
            string tagitem = "li";

            if (this.ListType == LinkableListType.Detailed) {
                tagmain = "div";
                tagitem = "div";
            } 

            HTML.Append("<" + tagmain + " class=\"" + this.CssClass + "-list\">");
            foreach (ILinkable item in Items)
            {
                HTML.Append("<" + tagitem + " class=\"" + this.CssClass + "-list-item\">");
                RenderMenuItem( item);
                HTML.Append("</" + tagitem + ">");
                if (this.Seperator != null)
                    HTML.Append(this.Seperator);
            }
            HTML.Append("</" + tagmain + ">"); 
        }
        protected void RenderMenuItem( ILinkable item)
        {
            string caption = "<a class=\"" + this.CssClass + "-ahref\" href=\"" + item.URL + "\">" + item.Caption + "</a>";
            string more = "<a class=\"" + this.CssClass + "-ahref-more\" href=\"" + item.URL + "\">more</a>";
           
            if (this.ListType == LinkableListType.Detailed)
            {
                HTML.Append("<h2 class=\"" + this.CssClass + "-caption\">" + caption + "</h2>");
                HTML.Append("<p class=\"" + this.CssClass + "-shortdesc\">" + item.ShortDesc + more + "</p>");
            }
            else if (this.ListType == LinkableListType.Brief)
            {
                HTML.Append("<h6 class=\"" + this.CssClass + "-caption\">" + caption + "</h6>");
                HTML.Append("<p class=\"" + this.CssClass + "-shortdesc\">" + item.ShortDesc + "</p>");
            }
            else if (this.ListType == LinkableListType.StaticList)  
            {
                HTML.Append("" + item.Caption + "");
            }
            else 
            {
                HTML.Append("" + caption + "");
            }

            
        }
    }
}
