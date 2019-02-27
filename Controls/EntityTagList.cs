using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ASPXUtils;

namespace WebSite.Core 
{
    [ToolboxData("<{0}:EntityTagList runat=server></{0}:EntityTagList>")]
    public class EntityTagList : Control
    {
        public string CssClass { get; set; }
        public string Seperator { get; set; }
        public string URLFormat { get; set; } 
        public List<EntityTag> EntityTags = new List<EntityTag>();
        protected override void Render(HtmlTextWriter writer)
        {
            if (URLFormat == null)
                URLFormat = NavUtil.URLRoot() + "/tags/{0}";
     
            RenderItems(writer); 
            base.Render(writer); 
        }
        protected void RenderItems(HtmlTextWriter writer)
        { 
            writer.WriteLine("<ul class='entiry-tag-list " + this.CssClass + "'>");
            foreach (EntityTag item in EntityTags)
            { 
                writer.WriteLine(
                    String.Format(@"<li><a href='{0}' class='entiry-tag-link'>{1}</a></li>"
                        , String.Format(URLFormat, HTTPUtils.ToDirFriendly(item.Name+ "-" + item.ID.ToString() )), item.Name)
                    );
            }
            writer.WriteLine("</ul>"); 
        } 
    }
}
