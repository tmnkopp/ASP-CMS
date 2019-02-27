using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WebSite.Core 
{

    public enum EventListType { 
        TitleOnly, 
        Full,
        SingleInstance
    }
    [ToolboxData("<{0}:EventList runat=Server></{0}:EventList>")]
    public class EventList : Control
    {
        public List<Event> Items { get; set; }
        public EventListType ListType { get; set; }
        public int Key { get; set; }
        public string CssClass { get; set; }
        public string Seperator { get; set; }
        private string ent_type = "service";
        protected override void Render(HtmlTextWriter writer)
        {
            if (Items == null)
                return;

            if (this.ListType == EventListType.SingleInstance)
            {
                this.ListType = EventListType.Full;
                RenderItem(writer, Items[0]);
            }
            else
            {
                RenderItems(writer);
            }
            base.Render(writer);

        }
        protected void RenderItems(HtmlTextWriter writer)
        {
            writer.WriteLine("<ul class=\"" + ent_type + "-list " + this.CssClass + "\">");
            foreach (Event item in Items)
            {
                writer.Write("<li class=\"" + ent_type + "-list-item\">");
                RenderItem(writer, item);
                writer.Write("</li>");
                if (this.Seperator != null)
                    writer.Write(this.Seperator);
            }
            writer.WriteLine("</ul>");
        }
        protected void RenderItem(HtmlTextWriter writer, Event item)
        {
            string caption = "<a href=\""+ item.URL +"\">" + item.Caption + "</a>";
            if (this.ListType == EventListType.TitleOnly)
            { 
                writer.Write(" " + caption + " ");
            }
            else {
                writer.WriteLine("<div class=\"" + ent_type + "-item\">");
                writer.WriteLine("<h2 class=\"" + ent_type + "-caption\">" + item.Name + "</h2>");
                writer.WriteLine("<div class=\"" + ent_type + "-body\">" + item.Description + "</div>");
                writer.WriteLine("</div>");
            }

            
        }
    }
}
