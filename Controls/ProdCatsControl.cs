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
    public class ProdCatsControl: DropDownList
    { 
        public bool ShowMain = true;
        public bool EmptyOption = true;
        public List<ProdCategory> CatItems;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Items.Clear();
            if (CatItems == null)
            {
                if (ShowMain)
                    CatItems = new ProdCategory().SelectAllMain();
                else
                    CatItems = new ProdCategory().SelectAll();
            }


            LoadData();

        }

        public void LoadData()
        {
            List<ProdCategory> CatItems = new ProdCategory().SelectAllMain();
            this.Items.Clear();

            if (EmptyOption)
                this.Items.Add(new ListItem() { Text = "", Value = "" });

            foreach (ProdCategory item in CatItems)
            {
                this.Items.Add(new ListItem() { Text = item.Name, Value = item.ID.ToString() });
                LoadChildCats(item, 0);
            }
        }
        private void LoadChildCats(ProdCategory Category, int level)
        {
            level++;
            string slevel = "";
            if (level > 6 || Category.SubCategories.Count < 1)
                return;

            for (int i = 0; i < level; i++)
                slevel += "   ...   ";


            foreach (ProdCategory item in Category.SubCategories)
            {
                string caption = slevel + item.Name; 
                this.Items.Add(new ListItem() { Text = caption, Value = item.ID.ToString() });
                LoadChildCats(item, level);
            }

        }






    }


}
