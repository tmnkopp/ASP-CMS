using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ASPXUtils;
using WebSite.Core;
using System.Text; 
using System.Collections.Generic;


 namespace WebSite.Core{

     public class ProdCatMenu : Control
    {  
        public bool ShowMain;
        public string LinkFormat = HTTPUtils.SCRIPT_NAME() + "?id={0}";
        public StringBuilder result; 
        public int GetChildrenByID;
        public List<ProdCategory> Items;
        public string CssClass;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            result = new StringBuilder(); 
        } 
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e); 
            //FillList();
            LoadCats();
        }  
        private void LoadCats()
        {

            if (this.GetChildrenByID > 0)
                Items = new ProdCategory().Select(GetChildrenByID).ActiveSubCategories;
            else if (ShowMain)
                Items = new ProdCategory().SelectAllMain().Where(o => o.Active=="True").ToList();

            result.AppendFormat("<div class=' {0} {1} '>", "pcat", CssClass);
            foreach (ProdCategory item in Items)
            {
                result.Append("\n<ul> \n");
                result.AppendFormat(
                    "<li class='pmenu-item-0'><a href='{1}'>{0}</a></li>"
                    , item.Name
                    , String.Format(item.URL, item.ID.ToString())
                );
                LoadChildCats(item, 0);
                result.Append("\n</ul>\n");
            }
            result.AppendFormat("</div >", "");
        }
        private void LoadChildCats(ProdCategory Category, int level)
        {
            level++;
            if (level > 6 || Category.ActiveSubCategories.Count < 1)
                return;
            result.Append("\n<ul>\n");
            foreach (ProdCategory item in Category.ActiveSubCategories)
            {
                result.AppendFormat(
                    "<li class='pmenu-item-{2}'><a href='{1}'>{0}</a></li>"
                    , item.Name
                    , String.Format(item.URL, item.ID.ToString())
                    , level.ToString()
                ); 
                LoadChildCats(item, level);
            }
            result.Append("\n </ul>\n");
        }
        protected override void Render(HtmlTextWriter writer)
        { 
            writer.WriteLine(result.ToString());
            base.Render(writer);
        } 
    }


}
