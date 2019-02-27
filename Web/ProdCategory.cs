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



namespace WebSite.Core {



    public class ProdCategory : ILinkable, IBusinessEntity, INameValue
{

    #region Properties

    private int _id;
    public int ID
    {
        get { return  _id; }
        set { _id = value; }
    }

    private int _Parent;
    public int Parent
    {
        get { return  _Parent; }
        set { _Parent = value; }
    }

    private string _loc;
    public string LOC
    {
        get { return _loc == null ? "" : _loc; }
        set { _loc = value; }
    }

    private string _name;
    public string Name
    {
        get { return _name == null ? "" : _name; }
        set { _name = value; }
    }

    private string _description;
    public string Description
    {
        get { return _description == null ? "" : _description; }
        set { _description = value; }
    }

    private int _sortorder;
    public int SortOrder
    {
        get { return  _sortorder; }
        set { _sortorder = value; }
    }
    private string _active;
    public string Active
    {
        get { return _active == null ? "1" : _active; }
        set { _active = value; }
    }
    private int _itemCatSort;
    public int ItemCatSort
    {
        get { return   _itemCatSort; }
        set { _itemCatSort = value; }
    }


    private string _ImageURL;
    public string ImageURL
    {
        get { return _ImageURL == null ? "" : _ImageURL; }
        set { _ImageURL = value; }
    }


    private string _fullname;
    public string FullName
    {
        get
        {
            if (this.ParentCategory != null)
            {
                _fullname = this.ParentCategory.Name + " : " + this.Name;
            }
            else
            {
                _fullname = this.Name;
            }

            return _fullname;
        }
        set { _fullname = value; }

    }
    
    #endregion
     private List<ProdCategory> _MenuCategoryCollection;
     public List<ProdCategory> MenuCategoryCollection
     {
         get { return _MenuCategoryCollection; }
         set { _MenuCategoryCollection = value; }
     }

     private List<ProdCategory> _SubCategories;
     public List<ProdCategory> SubCategories
     {
         get {
             if (_SubCategories == null) 
                 _SubCategories = GetSubCategories(this.ID);
        
             return _SubCategories; 
         }
         set { _SubCategories = value; }
     }

     private List<ProdCategory> _ActiveSubCategories;
     public List<ProdCategory> ActiveSubCategories
     {
         get  {
             if (_ActiveSubCategories == null) 
                 _ActiveSubCategories = this.SubCategories.Where( o => o.Active == "True").ToList();
       
             return _ActiveSubCategories;
         }
         set { _ActiveSubCategories = value; }
     }

     private List<ProdItem> _MenuItems;
     public List<ProdItem> MenuItems
     {
         get  {
             if (_MenuItems == null) 
                 _MenuItems = new ProdItem().SelectByCat(this.ID); 
             return _MenuItems;
         }
         set { _MenuItems = value; }
     }

     private List<ProdItem> _ActiveMenuItems;
     public List<ProdItem> ActiveMenuItems
     {
         get   {
             if (_ActiveMenuItems == null)
                 _ActiveMenuItems = this.MenuItems.Where(o => o.Active == "True").ToList();
             return _ActiveMenuItems;
         }
         set { _ActiveMenuItems = value; }
     }


     private ProdCategory _ParentCategory;
     public ProdCategory ParentCategory
     {
         get {
             if (_ParentCategory == null)
                 _ParentCategory = new ProdCategory().Select(this.Parent);
             return _ParentCategory;   }
         set { _ParentCategory = value; }
     }
     
      public int Insert() {
          return Insert(this);
      }
      public int Insert(ProdCategory obj)  {
          return ProdCategoryDAL.Add(obj);
      }
      public void AddMenuItem(int ItemID, int CatID)  {
          ProdCategoryDAL.AddMenuItem(ItemID, CatID);
      }  
      public void Update()   {
          Update(this);
      }
      public void Update(ProdCategory obj)   {
          ProdCategoryDAL.Update(obj);
      } 
      public void Delete()   {
          Delete(this);
      }
      public void Delete(ProdCategory obj)  {
          ProdCategoryDAL.Delete(obj.ID);
      } 
      public ProdCategory Load(int ID)  {
          ProdCategory obj = Select(ID);
          if (obj == null)
              obj = new ProdCategory();
          return obj;
      }
      public ProdCategory Select(int ID)  { 
          List<ProdCategory> oResult = SelectAll().Where(o => o.ID == ID).Take(1).ToList();
          return (oResult.Count > 0) ? oResult[0] : null;
      }
      public int SelectCount()  {
          return SelectAll().Count();
      } 
      public List<ProdCategory> SelectAll()  {
          if (this.MenuCategoryCollection==null)
            this.MenuCategoryCollection = new ProdCategoryDAL().GetAll();
          return this.MenuCategoryCollection;
      }
      public List<ProdCategory> SelectAllActive()
      {
          return SelectAll().Where(o => o.Active == "True").ToList();
      }
      public List<ProdCategory> SelectAllByLoc(string _loc)  {
          return SelectAll().Where(o => o.LOC == _loc).Distinct().ToList();
      } 
      public List<ProdCategory> SelectAllMain()  { 
          return SelectAll().Where(o => o.Parent == 0).Distinct().ToList();
      }
      public  List<ProdCategory> GetSubCategories(int _parent)  { 
          return SelectAll().Where(o => o.Parent == _parent).Distinct().ToList();
      }



      #region ILinkable Members


      public string Caption
      {
          get { return this.Name; }
      }        
      public string ShortDesc
      {
          get { return this.Name; }
      }
      public string URL
      {
          get
          {
              string _URL = NavUtil.URLRoot() + "/" + WebPage.GetNameBySysControl("ProdCategory") + "/" + HTTPUtils.ToDirFriendly(this.Name);

              if (SiteSettings.Instance.UseIDInUrl)
                  _URL += SiteSettings.Instance.IDSep + this.ID.ToString();

              if (!SiteSettings.Instance.UseExtentionLessParentPages)
                  _URL = _URL + SiteSettings.Instance.PageExtention;
              return _URL;
          }
      }

      public string SystemURL
      {
          get
          {
              return "";
          }
          set
          {
              this.SystemURL = value;
          }
      } 

      #endregion
} 

 }