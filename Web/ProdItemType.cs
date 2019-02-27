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

    public class ProdItemType
    {


        private int _id;
        public int ID
        {
            get { return  _id; }
            set { _id = value; }
        }

        private string _typename;
        public string TypeName
        {
            get { return _typename == null ? "" : _typename; }
            set { _typename = value; }
        }

        private string _ImageUrl;
        public string ImageUrl
        {
            get { return _ImageUrl == null ? "" : _ImageUrl; }
            set { _ImageUrl = value; }
        }



        private string _loc;
        public string LOC
        {
            get { return _loc == null ? "" : _loc; }
            set { _loc = value; }
        }


        private List<ProdItemType> _MenuItemTypeCollection;
        public List<ProdItemType> MenuItemTypeCollection
        {
            get { return _MenuItemTypeCollection; }
            set { _MenuItemTypeCollection = value; }
        }
        public List<ProdItemType> SelectAll()
        {
            string _iLoc = "/uploads/ui/pic50/";
            List<ProdItemType> MenuItemTypes = new List<ProdItemType>();
            //string[] _types = new string[] { "", "Food: Brunch", "Food: Lunch/Dinner", "Food: Dessert", "Food: Other", "Drinks: Beer", "Drinks: Wine", "Drinks: Cocktails", "Drinks: Other" };
            MenuItemTypes.Add(new ProdItemType() { TypeName = "", ImageUrl = _iLoc + "no-pic.png" });
            MenuItemTypes.Add(new ProdItemType() { TypeName = "Food: Brunch", ImageUrl = _iLoc + "brunch.png" });
            MenuItemTypes.Add(new ProdItemType() { TypeName = "Food: Lunch/Dinner", ImageUrl = _iLoc + "dinner.png" });
            MenuItemTypes.Add(new ProdItemType() { TypeName = "Food: Dessert", ImageUrl = _iLoc + "dessert.png" });
            MenuItemTypes.Add(new ProdItemType() { TypeName = "Food: Other", ImageUrl = _iLoc + "app.png" });
            MenuItemTypes.Add(new ProdItemType() { TypeName = "Drinks: Beer", ImageUrl = _iLoc + "beer.png" });
            MenuItemTypes.Add(new ProdItemType() { TypeName = "Drinks: Wine", ImageUrl = _iLoc + "redwine.png" });
            MenuItemTypes.Add(new ProdItemType() { TypeName = "Drinks: Cocktails", ImageUrl = _iLoc + "rocks.png" });
            MenuItemTypes.Add(new ProdItemType() { TypeName = "Drinks: Other", ImageUrl = _iLoc + "cat_beverages.png" });


            this.MenuItemTypeCollection = MenuItemTypes;
            return this.MenuItemTypeCollection;
        }

    }

}