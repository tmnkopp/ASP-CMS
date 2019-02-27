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
     public class ProdCatsCheckList : CheckBoxList
    {
        public List<ProdCategory> SelectedCats;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e); 
        }
        protected override void OnPreRender(EventArgs e)
        { 
            base.OnPreRender(e); 
        } 
  

        public void LoadData1()
        {
            Items.Clear(); 
            List<ProdCategory> mMainCatList = new ProdCategory().SelectAllMain();
            string _cLink = "/Admin/Menu_Categories/Edit.aspx?id="; 
            ListItem li;
            int _MainCatCount = 0;
            this.Items.Clear();
            foreach (ProdCategory m in mMainCatList)
            { 
                li = new ListItem() { Text = "<a href='" + _cLink + Convert.ToString(m.ID) + "'  >" + m.Name + "</a>"  , Value = Convert.ToString(m.ID) };
                li.Attributes.Add("class", "mc-item");
                this.Items.Add( li ); 
                foreach (ProdCategory s in m.SubCategories  )
                {
                    li = new ListItem() { Text = "<a href='" + _cLink + Convert.ToString(s.ID) + "&parent=" + m.ID + "'  >" + s.Name + "</a>", Value = Convert.ToString(s.ID) };
                    li.Attributes.Add("class", "mc-sub-item"); 
                    this.Items.Add(li);
                } 
                this.RepeatColumns = 1; 
                _MainCatCount++;
            }

            if (SelectedCats != null)
            {
                List<ProdCategory> oSelectedCats = SelectedCats; 
                foreach (ProdCategory m in oSelectedCats)
                {
                    if (m != null)
                    {
                        li = this.Items.FindByValue(   Convert.ToString(m.ID)    );
                        if (li != null)
                        {
                            li.Selected = true;
                        }          
                    } 
                }
            } 
            //this.DataBind(); 
        }

        public void LoadData()
        { 
            List<ProdCategory> Items = new ProdCategory().SelectAllMain();
            this.Items.Clear();
 
            foreach (ProdCategory item in Items)
            {
                ListItem li = new ListItem() { Text = item.Name, Value = Convert.ToString(item.ID) };
                li.Attributes.Add("class", "mc-item");
                this.Items.Add(li);
                LoadChildCats(item, 0); 
            } 
        }
        private void LoadChildCats(ProdCategory Category, int level)
        {
            level++;
            if (level > 6 || Category.SubCategories.Count < 1)
                return; 
            foreach (ProdCategory item in Category.SubCategories)
            {
                ListItem li = new ListItem() { Text = item.Name, Value = Convert.ToString(item.ID) };
                li.Attributes.Add("class", "mc-sub-item sub-item" + level.ToString());
                this.Items.Add(li); 
                LoadChildCats(item, level);
            }
            
        }

    } 
}
