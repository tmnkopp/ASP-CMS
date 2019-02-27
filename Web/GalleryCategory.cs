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
using ASPXUtils.Controls;

namespace WebSite.Core {

    [FormAttribute("Caption", Tabs = new string[] { "MainInfo", "Images"  })]
    public class GalleryCategory : ILinkable, IBusinessEntity, INameValue
    {


        public static string TagFmt = NavUtil.URLRoot()
        + "/" + HTTPUtils.ToDirFriendly(SiteSettings.Instance.GalleryCategoryCaption)
        + "/tags/{0}"+ SiteSettings.Instance.PageExtention;


    #region Properties
  
    private int _catid;
    [EntityGridAttribute("", IsID = true)]
    public int CATID
    {
        get { return  _catid; }
        set { _catid = value; }
    }

    private string _name;
    [EntityGridAttribute("Name")]
    [Formfield("Name", Tab = "MainInfo", Required = true, ToolTip = "")]
    public string Name
    {
        get { return _name == null ? "" : _name; }
        set { _name = value; }
    }
    private string _ShortDesc;
    
    public string ShortDesc
    {
        get { return _ShortDesc == null ? "" : _ShortDesc; }
        set { _ShortDesc = value; }
    }
    private string _description;
    [Formfield("Description", Tab = "MainInfo", ControlType = typeof(ASPXUtils.Controls.TextArea))]
    public string Description
    {
        get { return _description == null ? "" : _description; }
        set { _description = value; }
    }

    
    private string _GalleryType;
    [Formfield("Gallery Type", FieldAccess= FormFieldAccess.fa_SiteConfig, Tab = "MainInfo")]
    public string GalleryType
    {
        get { return _GalleryType == null ? "" : _GalleryType; }
        set { _GalleryType = value; }
    }
 
    private string _DisplayType;
    [Formfield("Display Type", FieldAccess = FormFieldAccess.fa_SiteConfig, Tab = "MainInfo")]
    public string DisplayType
    {
        get { return _DisplayType == null ? "" : _DisplayType; }
        set { _DisplayType = value; }
    }

    private string _ImgMaxHeight;
    [Formfield("Max Image Height", FieldAccess = FormFieldAccess.fa_SiteConfig, Tab = "MainInfo")]
    public string ImgMaxHeight {
        get {   return _ImgMaxHeight == null ? "" : _ImgMaxHeight;  }
        set { _ImgMaxHeight = value; }
    }

    private string _ImgMaxWidth;
    [Formfield("Max Image Width", FieldAccess = FormFieldAccess.fa_SiteConfig, Tab = "MainInfo")]
    public string ImgMaxWidth
    {
        get { return _ImgMaxWidth == null ? "" : _ImgMaxWidth; }
        set { _ImgMaxWidth = value; }
    }
 
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }


    private string _active;
    [Formfield("Active", Tab = "MainInfo", Width = 5, ControlType = typeof(DDTrueFalse))]
    public string Active
    {
        get { return _active == null ? "1" : _active; }
        set { _active = value; }
    }

    private int _sortorder;
    [EntityGridAttribute("SortOrder")]
    [Formfield("SortOrder", Width = 50, ValidateInteger = true, Required = true, Tab = "MainInfo")]
    public int SortOrder
    {
        get { return  _sortorder; }
        set { _sortorder = value; }
    }

    private List<GalleryImage> _GalleryImages;
    [Formfield("Gallery Images", CssClass = "GalleryImages", Tab = "Images", ControlType = typeof(GalleryImagesList))]
    public List<GalleryImage> GalleryImages
    {
        get { return _GalleryImages; }
        set { _GalleryImages = value; }
    }
 
    private List<GalleryCategory> _GalleryCategoryCollection;
    public List<GalleryCategory> GalleryCategoryCollection
    {
        get { return _GalleryCategoryCollection; }
        set { _GalleryCategoryCollection = value; }
    }

    private List<EntityTag> _EntityTagCollection;
    public List<EntityTag> EntityTagCollection
    {
        get { return _EntityTagCollection; }
        set { _EntityTagCollection = value; }
    }  

    #endregion

    #region Methods
    public int Insert()
    {
        return Insert(this);
    }
    public int Insert(GalleryCategory obj)
    {
        return GalleryCategoryDAL.Add(obj);
    }

    public void Update()
    {
        Update(this);
    }
    public void Update(GalleryCategory obj)
    {
        GalleryCategoryDAL.Update(obj);
    }

    public void Delete()
    {
        Delete(this);
    }
    public void Delete(GalleryCategory obj)
    {
        GalleryCategoryDAL.Delete(obj.CATID);
    }

    public GalleryCategory Select(int ID)
    {
        var oResult = ((from o in SelectAll().AsQueryable() where o.CATID == ID select o).Take(1)).ToList();
        return (oResult.Count > 0) ? oResult[0] : null;
    }
    public GalleryCategory SelectByName(string name)
    {
        var oResult = ((from o in SelectAll().AsQueryable() where o.Name == name select o).Take(1)).ToList();
        return (oResult.Count > 0) ? oResult[0] : null;
    }

    public int SelectCount()
    {
        return SelectAll().Count();
    }
    public List<GalleryCategory> SelectAllActive()
    {
        var oResult = ((from o in SelectAll().AsQueryable() where o.Active == "True" select o)).ToList();
        return oResult;
    }
 
    public List<GalleryCategory> SelectAll()
    {
        if (this.GalleryCategoryCollection == null)
            this.GalleryCategoryCollection = new GalleryCategoryDAL().GetAll();
        return this.GalleryCategoryCollection;
    }

    
    #endregion

    #region ILinkable Members
 

      public string Caption
      {
          get { return this.Name; }
      }
       

      public int ID
      {
          get { return this.CATID; }
          set { CATID = value; }
      }

      public string SystemURL { get { return ""; } set { this.SystemURL = value; } }

      public string URL
      {
          get
          {
              string _URL = NavUtil.URLRoot() + "/" + WebPage.GetNameBySysControl("GalleryCategory") + SiteSettings.Instance.SubDirSep + HTTPUtils.ToDirFriendly(this.Name);

              if (SiteSettings.Instance.UseIDInUrl)
                  _URL += SiteSettings.Instance.IDSep + this.ID.ToString();

              if (!SiteSettings.Instance.UseExtentionLessParentPages)
                  _URL = _URL + SiteSettings.Instance.PageExtention;
              return _URL;
          }
      }

      #endregion
}
}