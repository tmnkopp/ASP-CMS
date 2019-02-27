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
using ASPXUtils.Controls;


namespace WebSite.Core{

    [FormAttribute("Caption", Tabs=new string[]{"MainInfo"})]
    public class ArticleCategory : ILinkable, IBusinessEntity , INameValue
    {

        public static string ArticleCatURLFmt = NavUtil.URLRoot()
        + "/" + WebPage.GetNameBySysControl("Article") + "/Category/{0}" + SiteSettings.Instance.PageExtention;
 

        #region Properties

        private int _id;
        [EntityGridAttribute("", IsID = true)]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        } 
        private string _title;
        [EntityGridAttribute("Title" )]
        [Formfield("Title", Required = true, ToolTip = "")]
        public string Title
        {
            get { return _title == null ? "" : _title; }
            set { _title = value; }
        }
        public string Name
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _ShortDesc;
        [Formfield("ShortDesc", Tab = "MainInfo", ControlType = typeof(ASPXUtils.Controls.TextArea))]
        public string ShortDesc
        {
            get { return _ShortDesc == null ? "" : _ShortDesc; }
            set { _ShortDesc = value; }
        }
        private string _active;
        [Formfield("Active", Tab = "MainInfo", Width = 5, ControlType = typeof(DDTrueFalse))]
        public string Active
        {
            get { return _active == null ? "1" : _active; }
            set { _active = value; }
        }

        private int _sortorder;
        [Formfield("SortOrder", Width = 50, ValidateInteger = true, Required = true, Tab = "MainInfo")]
        public int SortOrder
        {
            get { return _sortorder; }
            set { _sortorder = value; }
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


        private List<ArticleCategory> _ArticleCategoryCollection;
        public List<ArticleCategory> ArticleCategoryCollection
        {
            get { return _ArticleCategoryCollection; }
            set { _ArticleCategoryCollection = value; }
        }
        private List<Article> _Articles;
        public List<Article> Articles
        {
            get
            {
                if (this._Articles == null)
                    this._Articles = new Article().SelectByCat(this.ID);
                return this._Articles;
            }
        }

        #endregion

        #region Methods
        public int Insert()
        {
            return Insert(this);
        }
        public int Insert(ArticleCategory obj)
        {
            return ArticleCategoryDAL.Add(obj);
        }

        public void Update()
        {
            Update(this);
        }
        public void Update(ArticleCategory obj)
        {
            ArticleCategoryDAL.Update(obj);
        }

        public void Delete()
        {
            Delete(this);
        }
        public void Delete(ArticleCategory obj)
        {
            ArticleCategoryDAL.Delete(obj.ID);
        }

        public ArticleCategory Load(int ID)
        {
            return Select(ID);
        }
        public ArticleCategory Select(int ID)
        {
            var oResult = ((from o in SelectAll().AsQueryable() where o.ID == ID select o).Take(1)).ToList();
            return (oResult.Count > 0) ? oResult[0] : null;
        }
        public int SelectCount()
        {
            return SelectAll().Count();
        }
        public List<ArticleCategory> SelectAllActive()
        {
            var oResult = ((from o in SelectAll().AsQueryable() where o.Active == "True" select o)).ToList();
            return oResult;
        }
        public List<ArticleCategory> SelectAll()
        {
            if (this.ArticleCategoryCollection == null)
                this.ArticleCategoryCollection = new ArticleCategoryDAL().GetAll();
            return this.ArticleCategoryCollection;
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
                string _URL = NavUtil.URLRoot() + "/" + WebPage.GetNameBySysControl("Article") + "/Category"
                    + SiteSettings.Instance.SubDirSep + HTTPUtils.ToDirFriendly(this.Title);

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
   
