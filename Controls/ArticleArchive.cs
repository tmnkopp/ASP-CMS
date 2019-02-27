using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WebSite.Core 
{
   

    [ToolboxData("<{0}:ArticleArchive runat=server></{0}:ArticleArchive>")]
    public class ArticleArchive : Control
    {
        public string CssClass { get; set; }
        public string Seperator { get; set; }
        //private string ent_type = "articlearchive";
        protected override void Render(HtmlTextWriter writer)
        {
            RenderItems(writer); 
            base.Render(writer);
            
        }
        protected void RenderItems(HtmlTextWriter writer)
        {
            string sel = "";
            List<Article> oArticles = new Article().SelectAllActive();
            var q = (from a in oArticles orderby a.ReleaseDate descending
                     group a by new {a.ReleaseDate.Year , a.ReleaseDate.Month }  into g
                     select new { g.Key, cnt = g.Count(), D = g.Min(c => c.ReleaseDate) });

            writer.WriteLine("<ul class='archive " + this.CssClass + "'>");
            string link = "";
            foreach (var a in q)
            { 
                link = string.Format(Article.ArchiveFmt, Convert.ToString(a.D.Month), Convert.ToString(a.D.Year));

                sel = "";
                if (this.IsSelected(link))
                    sel = " sel ";

                writer.WriteLine("<li><a href='" + link + "'  class=\"" + sel + "\">"
                    + Convert.ToString(a.D.ToString("MMMM"))
                    + " " + Convert.ToString(a.D.Year) + ""
                    + " (" + Convert.ToString(a.cnt) + ")</a></li>");
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
