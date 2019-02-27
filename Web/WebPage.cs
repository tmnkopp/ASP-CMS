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

namespace WebSite.Core
{

[EntityAttribute("Pages")]
[FormAttribute("WebPage", Tabs = new string[] { "MainContent", "MainContentAlt", "SystemNavigation", "MetaInfo" })]

    public class WebPage : ILinkable, IBusinessEntity 
{

    private static WebPage instance;
    public static WebPage Instance
    {
        get
        {
            if (instance == null || instance.ID.ToString() != HTTPUtils.QString("wpid")   )
            { 
                if(  StringUtils.IsInteger(HTTPUtils.QString("wpid"))   ){
                    instance = new WebPage().Load(Convert.ToInt32(HTTPUtils.QString("wpid")));
                }  
            }
            return instance;
        }
        set {
            instance = value;
        }
    }

    #region Properties

    private int _id;
     [EntityGridAttribute("ID", IsID=true)]
    public int ID
    {
        get { return  _id; }
        set { _id = value; }
    }

    private string _name;
    [EntityGridAttribute("PageURL")]
    [Formfield("Page Name (URL)", Required = true, MaxLength = 100, Width = 350, Tab = "MainContent",
        ValidateRegEx = @"^[a-zA-Z0-9\-]*$", ToolTip = @"<span style=""color:#e55749;"">IMPORTANT: </span>This page name is a permanent part of the URL. Changing this name will break all links to this page.<br> Choose Page Names carefully.
      <br><br> TIPS: <li>Use words relevant to page content</li>
      <li>Use keywords relevant to page content</li>
      <br><br> RESTRICTIONS:
      <li>Must begin with a letter / 75 Character limit / Use only Letters, Numbers and Hyphens ( - )<br>For spaces use Hyphens( - )<br><br> ex. California-Wedding-Photographer-Price-List</li>"
        )]
    public string Name
    {
        get { return _name == null ? "" : _name; }
        set { _name = value; }
    }

 
    private int _parent;
    [Formfield("Parent Page", ToolTip = "", Tab = "SystemNavigation", ControlType = typeof(DDParentPages))]
    public int Parent
    {
        get { return  _parent; }
        set { _parent = value; }
    }
 
    private string _menucaption;
    [EntityGridAttribute("Menu Caption")]
    [Formfield("Menu Caption", Required = true, Width=200, MaxLength = 100, Tab = "MainContent",
         ValidateRegEx = @"^[a-zA-Z0-9\s\&\.\-\,\'\?]*$", ErrorMessage = "Can only contain Letters, Numbers and Spaces", ToolTip = "Can only contain Letters, Numbers and Spaces")]
    public string MenuCaption
    {
        get { return _menucaption == null || _menucaption == "" ? this.Name.Replace("-", " ") : _menucaption; }
        set { _menucaption = value; }
    }

    private string _SubCaption;
    [Formfield("Sub Caption", Required = false, Width = 200, MaxLength = 250, Tab = "SystemNavigation",
         ValidateRegEx = @"^[a-zA-Z0-9\s\&\.\-\,\']*$", ErrorMessage = "Can only contain Letters, Numbers and Spaces", ToolTip = "Can only contain Letters, Numbers and Spaces")]
    public string SubCaption
    {
        get { return _SubCaption == null ? ""  : _SubCaption; }
        set { _SubCaption = value; }
    }
    


    private string _title;
    [Formfield("Page Title", Required = true, MaxLength = 100, Tab = "MainContent"
        , ToolTip="<li>Use fewer than 7 words (100 Character limit).</li><li>Use Letters, Numbers and Characters ( - | )</li><li>Use meaningful, relevant words describing the page content.</li> <br> ex. California Wedding Photographer Price List ")]
    public string Title
    {
        get { return _title == null ? "" : _title; }
        set { _title = value; }
    }

    private string _header;
    [Formfield("Page Header", MaxLength = 150, Tab = "SystemNavigation")]
    public string Header
    {
        get   {   return _header;  }
        set { _header = value; }
    } 
      
    private string _ShortDesc;
    [Formfield("Page Description", Required = false, ControlType = typeof(ASPXUtils.Controls.TextArea), MaxLength = 250, Tab = "MetaInfo",
       ToolTip = "Describe the content of this page. This description appears in search results.  Write meaningful, relevant descriptions in about 50 words.<br><br>Example<br><br> Price List for Professional wedding photography in California. I specialize in color, black and white, digital, and film wedding photography. ")]
    public string ShortDesc
    {
        get { return _ShortDesc == null ? "" : _ShortDesc; }
        set { _ShortDesc = value; }
    }

    private string _keywords;
    [Formfield("Page Keywords", MaxLength = 500, Tab = "MetaInfo")]
    public string Keywords
    {
        get { return _keywords == null ? "" : _keywords; }
        set { _keywords = value; }
    }



    private string _pagecontent;
    [Formfield("Page Content", Tab = "MainContent", ControlType = typeof(CKEditor))]
    public string PageContent
    {
        get { return _pagecontent; }
        set { _pagecontent = value; }
    }
 


    private string _mainmenu;
    [Formfield("Show On Main Menu",   Tab = "SystemNavigation", ControlType = typeof(DDTrueFalse))]
    public string MainMenu
    {
        get { return _mainmenu == null || _mainmenu == "" ? "1" : _mainmenu; }
        set { _mainmenu = value; }
    }
    private string _active;
    [Formfield("Active", Tab = "SystemNavigation", Width = 5, ControlType = typeof(DDTrueFalse))]
    public string Active
    {
        get { return _active == null ? "1" : _active; }
        set { _active = value; }
    }

    private int _sortorder;
    [EntityGridAttribute("Sort Order")]
    [Formfield("Sort Order", Width = 50, ValidateInteger = true, Tab = "SystemNavigation")]
    public int SortOrder
    {
        get { return _sortorder == 0 ? 1 : _sortorder; }
        set { _sortorder = value; }
    }

    private string _Notes;
    [Formfield("Notes", MaxLength = 500, Tab = "MetaInfo", ControlType = typeof(ASPXUtils.Controls.TextArea))]
    public string Notes
    {
        get { return _Notes; }
        set { _Notes = value; }
    } 


    private string _externalurl;
    [Formfield("External URL", Tab = "SystemNavigation", FieldAccess = FormFieldAccess.fa_SiteConfig)]
    public string ExternalURL
    {
        get { return _externalurl == null ? "" : _externalurl; }
        set { _externalurl = value; }
    }


    private string _SystemParam;
    [Formfield("System Param", Tab = "SystemNavigation" , FieldAccess = FormFieldAccess.fa_SiteConfig)]
    public string SystemParam
    {
        get { return _SystemParam == null ? "" : _SystemParam; }
        set { _SystemParam = value; }

    }

    private string _systemurl;
    [Formfield("System URL", Tab = "SystemNavigation", ControlType=typeof(DDSystemURLs)
        , FieldAccess = FormFieldAccess.fa_SiteConfig)]
    public string SystemURL
    {
        get { return _systemurl == null ? "" : _systemurl; }
        set { _systemurl = value; }
    }

    private string _SystemLayout;
    [Formfield("System Layout", Tab = "SystemNavigation", ControlType = typeof(DDSystemLayouts)
        , FieldAccess = FormFieldAccess.fa_SiteConfig)]
    public string SystemLayout
    {
        get { return _SystemLayout == null ? "" : _SystemLayout; }
        set { _SystemLayout = value; }
    }
 

    private string _SystemControl;
    [Formfield("System Control", Tab = "SystemNavigation", ControlType = typeof(DDSystemControl)
        , FieldAccess = FormFieldAccess.fa_SiteConfig)]
    public string SystemControl
    {
        get { return _SystemControl == null ? "" : _SystemControl; }
        set { _SystemControl = value; }
    }


    private string _menucaptionalt;
    [EntityGridAttribute("Alt Menu Caption ")]
    [Formfield("Alt Menu Caption", Required = false, Width = 200, MaxLength = 100, Tab = "MainContentAlt",
         ValidateRegEx = @"^[a-zA-Z0-9\s\&\.\-\,]*$", ErrorMessage = "Can only contain Letters, Numbers and Spaces", ToolTip = "Can only contain Letters, Numbers and Spaces")]
    public string MenuCaptionAlt
    {
        get { return _menucaptionalt == null || _menucaptionalt == "" ? this.Name.Replace("-", " ") : _menucaptionalt; }
        set { _menucaptionalt = value; }
    }

    private string _pagecontentalt;
    [Formfield("Alt Page Content", Tab = "MainContentAlt", ControlType = typeof(CKEditor))]
    public string PageContentAlt
    {
        get { return _pagecontentalt; }
        set { _pagecontentalt = value; }
    }





    public int PageContentLength
    {
        get {
            if (PageContent != null)
                return PageContent.Length; 
            else
                return 0; 
        }
    }
    private string _datecreated;
    public string DateCreated
    {
        get { return _datecreated == null ? System.DateTime.Now.ToString() : _datecreated; }
        set { _datecreated = value; }
    }

    private string _dateupdated;
    public string DateUpdated
    {
        get { return _dateupdated == null ? System.DateTime.Now.ToString() : _dateupdated; }
        set { _dateupdated = value; }
    }
 
    public DateTime LastUpdate
    {
        get { return Convert.ToDateTime(DateUpdated); }
    }




    [Formfield("Use Exact Title", FieldAccess = FormFieldAccess.fa_SiteConfig, Tab = "MetaInfo", Width = 5, ControlType = typeof(DDTrueFalse))]
    public string UseExactTitle { get; set; }
      



    private List<WebPage> _WebPageCollection;
    public List<WebPage> WebPageCollection
    {
        get { return _WebPageCollection; }
        set { _WebPageCollection = value; }
    }

    private List<WebPage> _SubPages;
    public List<WebPage> SubPages
    {
        get
        {
            if (_SubPages == null)
                _SubPages = this.SelectSubByParent(this.ID);

            return _SubPages;
        }
        set { _SubPages = value; }
    }

    private WebPage _ParentPage;
    public WebPage ParentPage
    {
        get
        {
            if (_ParentPage == null)
                _ParentPage = new WebPage().Select(this.Parent);

            return _ParentPage;
        }
        set { _ParentPage = value; }
    }
    #endregion

     #region Methods


      public int Insert()
      {
          return Insert(this);
      }
      public int Insert(WebPage obj)
      {
          return WebPageDAL.Add(obj);
      }
  
      public void Update()
      {
          Update(this);
      }
      public void Update(WebPage obj)
      {
          WebPageDAL.Update(obj);
      }
  
      public void Delete()
      {
          Delete(this);
      }
      public void Delete(WebPage obj)
      {
          WebPageDAL.Delete(obj.ID);
      }

      public WebPage Load(int ID)
      {
          return Select(ID);
      }
      public WebPage Select(int ID)
      {
          List<WebPage> oResult = SelectAll().Where(o => o.ID == ID).Take(1).ToList();
          return (oResult.Count > 0) ? oResult[0] : null;
      }
      public int SelectCount()
      {
          return SelectAll().Count();
     }
      public List<WebPage> SelectAllActive()
      {
          return (List<WebPage>)SelectAll().Where(o => o.Active == "True").ToList();
      }
      public  WebPage SelectBySystemName(string SysName)
      { 
          List<WebPage> oResult = SelectAll().Where(o => o.SystemControl == SysName).ToList();
          return (oResult.Count > 0) ? oResult[0] : null;
      }

      public static string GetNameBySysControl(string SysName)
      {
          string ret = "";
          List<WebPage> oResult = new WebPage().SelectAllMain().Where(o => o.SystemControl == SysName).ToList();
          if (oResult.Count > 0) 
          { 
            ret = oResult[0].Name;
          }
          if(ret=="")
              ret=SysName;
          return ret;
      }


      public List<WebPage> SelectAll()
      {
          if (this.WebPageCollection == null)
            this.WebPageCollection = new WebPageDAL().GetAll();
          return this.WebPageCollection;
      }
      public List<WebPage> SelectAllMain()
      {
          return (List<WebPage>)SelectAll().Where(o => o.Parent == 0).ToList();
      }
      public List<WebPage> SelectSubByParent(int _parent)
      {
          return (List<WebPage>)SelectAll().Where(o => o.Parent == _parent && o.Parent != 0).ToList();
      }
      
      #endregion

     #region ILinkable Members 
       
      private string _Caption = "";
      public string Caption  {
          get   {
              if (_Caption=="")
              {
                  _Caption = this.MenuCaption;
                  if (Globals.GetLang() != "" && this.MenuCaptionAlt != "") {
                      _Caption = this.MenuCaptionAlt;
                  } 
              }
              return _Caption;  
          }
          set { _Caption = value; }

      }

      private string _URL = null;
      public string URL  {
          get {
              if (this.ExternalURL != "" ) 
                  _