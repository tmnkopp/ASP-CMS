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

    public class ProdMenu : Control
    {
        public int Selected { get; set; }
        public StringBuilder Result;
        public string CATID = "";
        public string CssClass { get; set; }

        private bool _ShowByCategory = false;
        public bool ShowByCategory 
        {
            get { return _ShowByCategory; }
            set { _ShowByCategory = value; }
        }

        private bool _ShowCategoryOnly = false;
        public bool ShowCategoryOnly
        {
            get { return _ShowCategoryOnly; }
            set { _ShowCategoryOnly = value; }
        }

        private bool _Modal = false;
        public bool Modal
        {
            get { return _Modal; }
            set { _Modal = value; }
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CATID = HTTPUtils.QString("cid");
            Result = new StringBuilder();

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!ShowByCategory || CATID != "")
                LoadCats();
        }

        private string FormatDesc(string desc) {
            return desc.Replace("\n", "<br>");
        }
        private void LoadCats()
        {

            List<ProdCategory> MainCats;
            ProdCategory MainCat;
            
            Result.AppendFormat("\n<div class='{0}'>\n", this.CssClass);
            if (CATID != "")
            {
                MainCat = new ProdCategory().Select(Convert.ToInt32(CATID));
                Result.AppendFormat("\n<h1>{0}</h1>\n", MainCat.Name);
                Result.AppendFormat("\n<span class='p-cat-desc'>{0}</span>\n", FormatDesc(MainCat.Description));
                GetItems(MainCat, 0);
                MainCats = MainCat.SubCategories;
            }
            else
                MainCats = new ProdCategory().SelectAllMain();

            
            foreach (ProdCategory item in MainCats)
            {
                Result.Append("\n<ul>\n<li>\n");
                GetMainHeader(item);
                GetItems(item, 0);
                LoadChildCats(item, 0);
                Result.Append("\n</li>\n</ul>\n");
            }
            Result.AppendFormat("\n</div >\n", this.CssClass);
        }
        private void LoadChildCats(ProdCategory Category, int level)
        {
            level++;
            if (level > 6 || Category.SubCategories.Count < 1)
                return;
            Result.Append("\n<ul>\n");
            foreach (ProdCategory item in Category.SubCategories)
            {
                GetSubHeader(item, level);
                GetItems(item, level);
                LoadChildCats(item, level);
            }
            Result.Append("\n </li>\n</ul>\n");
        }

        private void GetMainHeader(ProdCategory item)
        {
            Result.AppendFormat(
            "\n<h2 class='p-cat' {1}>{0}</h2> \n"
            , item.Name
            , GetEdit("menu_categories", item.ID)
        );
        }
        private void GetSubHeader(ProdCategory item, int level)
        {
            Result.AppendFormat("\n<li class='p-item'>\n", "");
            Result.AppendFormat("\n<h{2} class='p-cat' {1}>{0}</h{2}>\n"
                , item.Name
                , GetEdit("menu_categories", item.ID)
                , (level + 2).ToString()
            );
        }
        private void GetItems(ProdCategory item, int level)
        {
            if (item.MenuItems.Count > 0 && !ShowCategoryOnly)
            {
                Result.Append("\n<ul>\n");
                foreach (ProdItem proditem in item.MenuItems)
                {
                    Result.AppendFormat(
                        "\n<li class='p-item' {3}><span class='p-itemname'>{0}</span><span  class='p-itemdesc'>{1}</span><span  class='p-itemprice'>{2}</span></li>\n"
                        , proditem.ItemName
                        , proditem.Description
                        , GetPrice(proditem)
                        , GetEdit( "menu_items", proditem.ID)
                     );
                } 
                Result.Append("\n</li></ul>\n");
            }

        }
        private string GetEdit(string type, int id)
        {

            return DynamicContentHelper.RenderEditableRegion(String.Format("/admin/{1}/edit.aspx?id={0}", Convert.ToString(id), type), this.Modal);
        }

        private string GetPrice(ProdItem item)
        {
            string sPrice = Convert.ToString(item.Price);
            sPrice = sPrice.Replace(".00", "");
            if (sPrice == "0")
                sPrice = item.PriceInfo;
            else
                sPrice = "$" + sPrice;
            return sPrice;
        }
        private void LoadRows()
        {
            DataView oDataView = ProdCategoryDAL.GetOrdered();
            if (CATID != "")
                oDataView.RowFilter = " MASTERID = " + Convert.ToString(CATID) + "";

            string MASTERID = "";
            string SubCat = "";
            string DisplayType = "";
            int cnt = 0;
            Result.AppendFormat("\n<div class='{0}'>\n", this.CssClass);
            Result.AppendFormat("\n<ul class='{0}'>\n", "");
        
            foreach (DataRowView oDataRow in oDataView)
            {
                cnt++;
                DisplayType = Convert.ToString(oDataRow["DisplayType"]);
                 
                if (MASTERID != Convert.ToString(oDataRow["MASTERID"]))
                {
                    if (MASTERID != "")
                        Result.Append("\n</ul>\n</li>\n");

          
                    MASTERID = Convert.ToString(oDataRow["MASTERID"]);
                    SubCat = Convert.ToString(oDataRow["ID"]);

                    Result.AppendFormat(
                        "\n<li>\n<h2 class='p-cat' {2}>{0}</h2><span class='p-cat-desc'>{1}</span>\n<ul class='p-menu'>\n"
                        , Convert.ToString(oDataRow["Name"])
                         , Convert.ToString(oDataRow["CatDesc"])
                        , DynamicContentHelper.RenderEditableRegion(
                            String.Format
                                ("/admin/menu_categories/edit.aspx?id={0}", Convert.ToString(oDataRow["ID"])) ,this.Modal
                            )
                    );
 
                }

                if (SubCat != Convert.ToString(oDataRow["ID"])  )
                {
                    SubCat = Convert.ToString(oDataRow["ID"]);
                    Result.AppendFormat("\n<h3 class='p-cat' {2}>{0}</h3><span class='p-cat-desc'>{1}</span>\n"
                        , Convert.ToString(oDataRow["Name"])
                        , Convert.ToString(oDataRow["CatDesc"])
                        , DynamicContentHelper.RenderEditableRegion(
                            String.Format
                                ("/admin/menu_categories/edit.aspx?id={0}", Convert.ToString(oDataRow["ID"])), this.Modal
                            )
                    );
                }

                if (Convert.ToString(oDataRow["ItemName"]) != "" && !ShowCategoryOnly)
                {
                    Result.AppendFormat(
                        "\n<li class='p-item' {3}><span class='p-itemname'>{0}</span><span  class='p-itemdesc'>{1}</span><span  class='p-itemprice'>{2}</span></li>\n"
                        , Convert.ToString(oDataRow["ItemName"])
                        , Convert.ToString(oDataRow["ItemDesc"])
                        , GetPrice(oDataRow)
                        , DynamicContentHelper.RenderEditableRegion(
                            String.Format
                                ("/admin/menu_items/edit.aspx?id={0}", Convert.ToString(oDataRow["ItemID"])), this.Modal
                            )
                     );
                }

            }
            Result.Append(" <li>\n</ul>\n</ul>\n");
            Result.Append("\n</div>\n");

        }


        private string GetPrice(DataRowView oDataRow)
        {
            string sPrice = Convert.ToString(oDataRow["Price"]);
            sPrice = sPrice.Replace(".00", "");
            if (sPrice == "0")
                sPrice = Convert.ToString(oDataRow["PriceInfo"]);
            else
                sPrice = "$" + sPrice;
            return sPrice;
        }
        protected override void Render(HtmlTextWriter writer)
        { 
            writer.WriteLine(Result.ToString());
            base.Render(writer);
        } 
    }


}
