using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI; 
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using ASPXUtils;
 
namespace WebSite.Core
{ 
    public abstract class BasePage : Page
    { 
        public string _rp = HTTPUtils.QString("rp");
        public string _wpid = HTTPUtils.QString("wpid");
        public string _cid = HTTPUtils.QString("cid");
        public string _id = HTTPUtils.QString("id");
        public string _mt = HTTPUtils.QString("mt");
        public string _yr = HTTPUtils.QString("yr"); 

        //private WebPage _WP;
        public WebPage oWebPage
        {
            get { 
                return WebPage.Instance;
            }  
        }
        public BasePage()    {      }
        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
        }
        protected override void OnLoad(  EventArgs e)
        {
             
            if (StringUtils.IsInteger(_wpid))
            {

                if (oWebPage != null)
                {
                    MetaDescription = oWebPage.ShortDesc;
                    MetaKeyWords = oWebPage.Keywords;
                    string _title = oWebPage.Title;

                    if (oWebPage.UseExactTitle != "True")
                    {
                        if (SiteSettings.Instance.SiteTitlePrefix != "")
                            _title = SiteSettings.Instance.SiteTitlePrefix + " " + _title;
                        if (SiteSettings.Instance.SiteTitlePostfix != "")
                            _title = _title + " " + SiteSettings.Instance.SiteTitlePostfix;
                    }

                    PageTitle = _title;
                } 
            }
            base.OnLoad(e);
        }
        protected string RenderPageContent() {
            if (oWebPage==null)
                return "";
            if (oWebPage.PageContent==null)
                return "";

            return Utils.RenderContent(oWebPage);//  DynamicContentHelper.InjectControls(oWebPage.PageContent); 
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Title == "Untitled Page" || this.Title == "")
                this.Title = StringUtils.Pcase(HTTPUtils.GetPageName()) + " " +  HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();
            base.OnPreRender(e);
        }
       
        protected void AddMetaTag(string name, string value )
        {
            AddMetaTag(name, value, true, true);
        } 
        protected void AddMetaTag(string name, string value, bool overwrite, bool ForceUnique)
        {
            bool CanAdd = true;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
                return;

            if (ForceUnique) {
                foreach (Control c in Page.Header.Controls)
                {
                    if (c is HtmlMeta) 
                    {
                        if (((HtmlMeta)c).Name == name)
                        {
                            if (overwrite) 
                            {
                                ((HtmlMeta)c).Content = value;
                            }
                            CanAdd = false;
                            break;
                        }
                    }
                }
            }
            if (CanAdd)
            {
                HtmlMeta meta = new HtmlMeta();
                meta.Name = name;
                meta.Content = value;
                Page.Header.Controls.Add(meta);
                Page.Header.Controls.Add(new Literal() { Text = "\n" });
            }
        }

     
        public void AddJavaScriptInclude(string url, bool ForceUnique)
        {
            url = NavUtil.URLRoot() + url;
            bool CanAdd = true; 
            if (ForceUnique)
            {
                foreach (Control c in Page.Header.Controls)
                {
                    if (c is HtmlGenericControl)
                    {
                        if (((HtmlGenericControl)c).Attributes["src"] == url)
                        {
                            CanAdd = false;
                            break;
                        }
                    }
                }
            }

            if(CanAdd){
                HtmlGenericControl script = new HtmlGenericControl("script");
                script.Attributes["type"] = "text/javascript";
                script.Attributes["src"] = url;
                Page.Header.Controls.Add(script);
                Page.Header.Controls.Add(new Literal() { Text="\n" });
            }
        }

        public void AddStylesheetInclude(string url)
        {
            url = NavUtil.URLRoot() + url;
            HtmlLink link = new HtmlLink();
            link.Attributes["type"] = "text/css";
            link.Attributes["href"] = url;
            link.Attributes["rel"] = "stylesheet";
            Page.Header.Controls.Add(link);
            Page.Header.Controls.Add(new Literal() { Text = "\n" });
        }
       public string GetMetaValue(string sTagName)
       {
            HtmlMeta meta = (HtmlMeta)Page.Header.FindControl(sTagName);
            if (null != meta)
            {
                return meta.Content;
            }
            return string.Empty;
        }

       public void SetPageTitle(string arg)
       {
 
           Page.Title = arg;
       }
       protected string MetaDescription
       {
            get {  return GetMetaValue("description");  }
            set {  AddMetaTag("description", value);  }
       }
       protected string PageTitle
       { 
            set { SetPageTitle(value); }
       } 
       protected string MetaKeyWords
       {
            get { 