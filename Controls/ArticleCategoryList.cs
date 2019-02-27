using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WebSite.Core 
{
    [ToolboxData("<{0}:ArticleCategoryList runat=server></{0}:ArticleCategoryList>")]
    public class ArticleCategoryList : Control
    {
        public string CssClass { get; set; }
        public string Seperator { get; set; }
        private string ent_type = "articlecategories";
        protected override void Render(HtmlTextWriter writer)
        {
            RenderItems(writer); 
            base.Render(writer);
            
        }
        protected void RenderItems(HtmlTextWriter writer)
        {
            string sel = "";
            WebPage BaseArtPage = new WebPage().SelectBySystemName("Article");
 
            List<ArticleCategory> obj = new ArticleCategory().SelectAllActive().Where(o=>o.Articles.Count() > 0).ToList();
             
            writer.WriteLine("<ul class='" + ent_type + " " + this.CssClass + "'>"); 
            foreach (ArticleCategory a in obj)
            {
                //exp = string.Format("/" + BaseArtPage.Name + "/{0}/{1}.aspx", a.Caption +"-"+ a.ID.ToString(), @"(?<art>[a-zA-Z0-9\-]*\-[0-9]*)");
                sel = "";
                if (this.IsSelected(a.URL))
                    sel = "sel";

                writer.WriteLine("<li><a href='" + a.URL + "' class=\"" + sel + "\">"
                    + a.Caption 
                    + " (" + Convert.ToString(a.Articles.Count()) + ")</a></li>");
            }
            writer.WriteLine("</ul>"); 
        }
        private bool IsSelected(string arg)
        {
            bool ret = false;
            arg = arg.ToLower();
            if (NavUtil.RequestPath.ToLower().Contains(arg))
                ret = true;
            return ret;
        }
    }
}
