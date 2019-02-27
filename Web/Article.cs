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
  using ASPXUtils;
  using WebSite.Core;
using System.Diagnostics;
using ASPXUtils.Controls;

namespace WebSite.Core{

    [FormAttribute("Caption", Tabs = new string[] { "Main", "ArticleContent" })]
    public class Article : ILinkable, IBusinessEntity 
    { 


    public static string ArchiveFmt = NavUtil.URLRoot()
        + "/" + WebPage.GetNameBySysControl("Article") + "/{0}/{1}/Archive" + SiteSettings.Instance.PageExtention;

    public Article( )
    {

    }
    public Article(ILinkable instance) {
        this.ID = instance.ID;
        this.Title = instance.Caption; 
        this.SystemURL = instance.SystemURL;
        this.ShortDesc = instance.ShortDesc; 
    }

  
    #region Properties
     
    private int _id;
   [EntityGridAttribute("", IsID = true)]
    public int ID
    {
        get { return  _id; }
        set { _id = value; }
    }

    private string _title;

    [EntityGridAttribute("Title")]
    [Formfield("Title", Required = true, ToolTip = "", Tab = "Main" )]
    public string Title
    {
        get { return _title == null ? "" : _title; }
        set { _title = value; }
    }
    private string _ShortDesc;
    [Formfield("Short Description", ToolTip = "", Tab = "Main",  ControlType = typeof(ASPXUtils.Controls.TextArea)   )]
    public string ShortDesc
    {
        get { return _ShortDesc == null ? "" : _ShortDesc; }
        set { _ShortDesc = value; }
    }
     
    private string _content;
    [Formfield("Content", Tab = "ArticleContent", ToolTip = "", ControlType = typeof(CKEditor))]
    public string Content
    {
        get { return _content; }
        set { _content = value; }
    } 

    public string ContentBrief
    {
        get
        {
            string _content = StringUtils.StripHTML(this.Content);
            int len = _content.Length;
            int iStopPos = len;
            if (len > 1)
            {
                iStopPos = _content.IndexOf(".", 2);
            }
            if (iStopPos < 1)
                iStopPos = len;

            if (iStopPos > len)
                iStopPos = len - 1;

            string _temp = _content;
            if (iStopPos > 0)
                _temp = _content.Substring(0, iStopPos);

            return _temp;
        }
    }
    private string _active;
    [Formfield("Active", Tab = "Main", Width = 5, ControlType = typeof(DDTrueFalse) )]
    public string Active
    {
        get { return _active == null ? "1" : _active; }
        set { _active = value; }
    }
 
   
 
    private DateTime _ReleaseDate;
    [Formfield("Release Date", Tab = "Main", ToolTip = "", Width = 150, ControlType = typeof(ExtendedTextBox) , ValidateDate = true, ShowCalendar = true)]
    public DateTime ReleaseDate
    {
        get {
            if (_ReleaseDate < Convert.ToDateTime("1/2/1900"))
                _ReleaseDate = this.DateCreated;
            if (_ReleaseDate < Convert.ToDateTime("1/2/1900"))
                _ReleaseDate = DateTime.Now;
            return _ReleaseDate; }
        set { _ReleaseDate = value; }
    }  


    private DateTime _datecreated;
    public DateTime DateCreated
    {
        get { return _datecreated; }
        set { _datecreated = value; }
    }  
    private DateTime _dateupdated;
    public DateTime DateUpdated
    {
        get { return _dateupdated; }
        set { _dateupdated = value; }
    }


    private List<Article> _ArticleCollection;
    public List<Article> ArticleCollection
    {
        get { return _ArticleCollection; }
        set { _ArticleCollection = value; }
    }

    
    private List<ArticleCategory> _ArticleCategories = new List<ArticleCategory>();
    [Formfield("Article Categories", Tab = "Main", ControlType = typeof(CBLArticleCats))]
    public List<ArticleCategory> ArticleCategories
    {
        get { return _ArticleCategories; }
        set { _ArticleCategories = value; }
    }
    #endregion

     #region Methods
     
     public int SaveChanges() {
         Update(this);
         return 0;
     }  
     public int Insert()
     {
         return Insert(this);
     }
     public int Insert(Article obj)
     {
         return ArticleDAL.Add(obj);
     }

     public void Update()
     {
         Update(this);
     }
     public void Update(Article obj)
     {
         ArticleDAL.Update(obj);
     } 
     public void Delete()
     {
         Delete(this);
     }
     public void Delete(Article obj)
     {
         ArticleDAL.Delete(obj.ID);
     }
 
     public Article Select(int ID)
     {
         var oResult = ((from o in SelectAll().AsQueryable() where o.ID == ID select o).Take(1)).ToList();
         return (oResult.Count > 0) ? oResult[0] : null;
     }
     public int SelectCount()
     {
         return SelectAll().Count();
     }
     public List<Article> SelectAllActive()
     {
         var oResult = (from a in SelectAll() 
                        where   a.Active == "True"
                        orderby a.ReleaseDate descending
                        select a).ToList(); 
         return oResult;
     }
     public List<Article> SelectByCat(int _CID)
     {
         var oResult = (from a in SelectAll()
                        from c in a.ArticleCategories
                        where c.ID == _CID
                        && a.Active == "True"
                        orderby a.ReleaseDate descending
                        select a).ToList();
         return oResult;
     }

     public List<Article> SelectAll()
     {
         if (this.ArticleCollection == null)
             this.ArticleCollection = new ArticleDAL().GetAll();
         return this.ArticleCollection;
     }
     
     #endregion

         #region ILinkable Members


      public string Caption
      {
          get { return this.Title; }
      }
       

      public string URL
      {
          get
          { 
              string _URL = NavUtil.URLRoot() + "/" + WebPage.GetNameBySysControl("Article") + SiteSettings.Instance.SubDirSep + HTTPUtils.ToDirFriendly(this.Title) ;

              if (SiteSettings.Instance.UseIDInUrl)
                  _URL += SiteSettings.Instance.IDSep + this.ID.ToString();

              if (!SiteSettings.Instance.UseExtentionLessParentPages)
                  _URL = _URL + SiteSettings.Instance.PageExtention;
              return _URL;
          }
      }
      private string _systemURL = "";
      public string SystemURL { get { return _systemURL; } set { this._systemURL = value; } }

      #endregion
    } 
 
} 
