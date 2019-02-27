using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WebSite.Core 
{
    public enum ArticleListType { 
        Full, 
        TitleOnly,
        LinkedList,
        HeadingBriefDescription,
        SingleInstance
    }

    [ToolboxData("<{0}:ArticleList runat=server></{0}:ArticleList>")]
    public class ArticleList: BaseControl
    {
        public List<Article> Items { get; set; }
        public Article Item { get; set; }
        public bool AutoDetectBehavior{ get; set; }
        public string CssClass { get; set; }
        public string Seperator { get; set; }
        public ArticleListType ListType { get; set; }
        public int ItemCount { get; set; }
        private string ent_type = "article";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(this.ItemCount < 1){
                this.ItemCount = ASPXUtils.CommonUtils.GetDataInt(SiteSettings.Instance.DefaultArticleCount);
            }
            if (AutoDetectBehavior)
            {
                if (base._id != "")
                {
                    this.ListType = ArticleListType.SingleInstance;
                    Item = new Article().Select(ASPXUtils.CommonUtils.GetDataInt(base._id));
                }
            }
        }   

        protected override void Render(HtmlTextWriter writer)
        { 
             
            if (this.ListType == ArticleListType.SingleInstance)
            {
                if (Item == null)
                    return;
                this.ListType = ArticleListType.Full;
                RenderItem(writer, Item);
            }
            else
            {
                if (Items == null)
                { 
                    Items = new Article().SelectAllActive()
                        .OrderByDescending(o=>o.ReleaseDate)
                        .Take(Convert.ToInt32(this.ItemCount) )
                        .ToList();
                }
                
                RenderItems(writer);
            }
            base.Render(writer);
            
        }
        protected void RenderItems(HtmlTextWriter writer)
        { 
            writer.WriteLine("<ul class=\"" + ent_type + "-list " + this.CssClass + "\">");
            foreach (Article item in Items)
            {
                writer.Write("<li class=\"" + ent_type + "-list-item\">");
                RenderItem(writer, item);
                writer.Write("</li>");
                if (this.Seperator != null)
                    writer.Write(this.Seperator);
            }
            writer.WriteLine("</ul>"); 
        }
        protected void RenderItem(HtmlTextWriter writer, Article item)
        { 
            string link = "<a href=\"" + item.URL + "\">" + item.Title + "</a>";
            if (this.ListType == ArticleListType.HeadingBriefDescription)
            {
                writer.WriteLine("<h2 class=\"" + ent_type + "-caption\">" + link + "</h2>");
                writer.WriteLine("<span class=\"" + ent_type + "-date\">" + item.ReleaseDate.ToLongDateString() + "</span>");
                writer.WriteLine("<p class=\"" + ent_type + "-shortdesc\">" + item.ContentBrief + "</p>");
            }
            else if (this.ListType == ArticleListType.TitleOnly)
            {
                writer.WriteLine("<span class=\"" + ent_type + "-date\">" + item.ReleaseDate.ToShortDateString() + " - " + item.Title + "</span>");
            }
            else if (this.ListType == ArticleListType.LinkedList) 
            {
                writer.WriteLine(link);
            }
            else
            {
                writer.WriteLine("<div class=\"" + ent_type + "-item " + this.CssClass + "\">");
                writer.WriteLine("  <h1 class=\"" + ent_type + "-title\">" + item.Title 