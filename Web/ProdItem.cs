using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using WebSite.Core;
using ASPXUtils;

namespace WebSite.Core
{
    public class ProdItem : IBusinessEntity
    {


        private int _id;
        public int ID
        {
            get { return   _id; }
            set { _id = value; }
        }

        private string _itemname;
        public string ItemName
        {
            get { return _itemname == null ? "" : _itemname; }
            set { _itemname = value; }
        }
        private string _ItemType;
        public string ItemType
        {
            get { return _ItemType == null ? "" : _ItemType; }
            set { _ItemType = value; }
        }
        private string _DisplayType;
        public string DisplayType
        {
            get { return _DisplayType == null ? "" : _DisplayType; }
            set { _DisplayType = value; }
        }

        private string _LOC;
        public string LOC
        {
            get { return _LOC == null ? "" : _LOC; }
            set { _LOC = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description == null ? "" : _description; }
            set { _description = value; }
        }
        private string _Comments;
        public string Comments
        {
            get { return _Comments == null ? "" : _Comments; }
            set { _Comments = value; }
        }
        private string _imageurl;
        public string ImageURL
        {
            get { return _imageurl == null ? "" : _imageurl; }
            set { _imageurl = value; }
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private string _priceinfo;
        public string PriceInfo
        {
            get { return _priceinfo == null ? "" : _priceinfo; }
            set { _priceinfo = value; }
        }

        private string _active;
        public string Active
        {
            get { return _active == null ? "1" : _active; }
            set { _active = value; }
        }
        private string _IsVeg;
        public string IsVeg
        {
            get { return _IsVeg == null ? "0" : _IsVeg; }
            set { _IsVeg = value; }
        }
        private string _IsFeatured;
        public string IsFeatured
        {
            get { return _IsFeatured == null ? "0" : _IsFeatured; }
            set { _IsFeatured = value; }
        }


        private int _sortorder;
        public int SortOrder
        {
            get { return   _sortorder; }
            set { _sortorder = value; }
        }
        private int _CatSort;
        public int CatSort
        {
            get { return  _CatSort; }
            set { _CatSort = value; }
        }

        private List<ProdItem> _MenuItemCollection;
        public List<ProdItem> MenuItemCollection
        {
            get { return _MenuItemCollection; }
            set { _MenuItemCollection = value; }
        }
        private List<ProdCategory> _MenuCategories;
        public List<ProdCategory> MenuCategories
        {
            get { return _MenuCategories; }
            set { _MenuCategories = value; }
        }
        public int Insert()
        {
            return Insert(this);
        }
        public int Insert(ProdItem obj)
        {
            return ProdItemDAL.Add(obj);
        }
        public void Update()
        {
            Update(this);
        }
        public void Update(ProdItem obj)
        {
            ProdItemDAL.Update(obj);
        }
        public void Delete()
        {
            Delete(this);
        }
        public void Delete(ProdItem obj)
        {
            ProdItemDAL.Delete(obj.ID);
        }

        public ProdItem Load(int ID)
        {
            return Select(ID);
        }
        public ProdItem Select(int ID)
        {
            List<ProdItem> oResult = SelectAll().Where(o => o.ID == ID).Take(1).ToList();
            return (oResult.Count > 0) ? oResult[0] : null;
        }
        public int SelectCount()
        {
            return SelectAll().Count();
        }
        public List<ProdItem> SelectAllActive()
        {
            return SelectAll().Where(o => o.Active == "True").ToList();
        }

        public List<ProdItem> SelectAllByLoc(string _loc)
        {
            return SelectAll().Where(o => o.LOC == _loc).Distinct().ToList();
        }

        public List<ProdItem> SelectByCat(int _catid)
        {
            var oResult = (from item in SelectAll().AsQueryable()
                           from mc in item.MenuCategories
                           where mc.ID == _catid
                           select item).ToList();

            foreach (ProdItem item in oResult)
            {
                item.MenuCategories.RemoveAll(s => !s.ID.Equals(_catid));
                item.CatSort = item.MenuCategories[0].ItemCatSort;
            }
            return oResult;
        }

        public List<ProdItem> SelectAll()
        {
            if (this.MenuItemCollection == null)
                this.MenuItemCollection = new ProdItemDAL().GetAll();
            return this.MenuItemCollection;
        }
        private string caption;
        public string Caption
        {
            get
            {
                if (this.caption == null)
                    this.caption = this.ItemName + " - " + this.Description;

                return this.caption;
            }
            set { caption = value; }
        }
        public static string URL_FMT = "";

        private string link;
        public string Link
        {
            set { link = value; }
            get { return ""; }
        }

    }

}