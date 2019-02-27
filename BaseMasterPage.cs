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
    
    public abstract class BaseMasterPage : MasterPage
    {
        public string BodyClass = "";
        public string PageName = "";
        public string SystemPageName = "";
        public BaseMasterPage() { }
        public bool IncludeJquery{get; set;} 
        protected override void OnInit(EventArgs e)
        {
            if(HTTPUtils.QString("tim")=="1")
                Settings.SaveSetting("tim", "1"); 

            PageName = String.Format(" wp{0}", HTTPUtils.QString("rp").Replace("/", "-").Replace(SiteSettings.Instance.PageExtention, ""));
            SystemPageName = String.Format(" sn{0}", Utils.HTMLFriendlyScriptName());
            string ParentPage = "";

            if(WebPage.Instance  != null){
                if (WebPage.Instance.ParentPage != null)
                {
                    ParentPage = String.Format(" pp-{0}", WebPage.Instance.ParentPage.Name);
                } 
            }

            StringBuilder SB = new StringBuilder();

            SB.AppendFormat("master wpid-{0}", HTTPUtils.QString("wpid"));
            SB.AppendFormat(PageName);
            SB.AppendFormat(SystemPageName);
            SB.AppendFormat(ParentPage);
            
            BodyClass = SB.ToString();

            Page.Header.Controls.Add(new Literal() { Text=SiteSettings.Instance.GoogleAnalytic});

            //<link rel='icon' type='image/x-ico' href='{1}' /><link rel='shortcut icon' type='image/x-icon' href='{1}' />

            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (SiteSettings.Instance.Favicon != null) {
                string favicon = string.Format("\n<link rel=\"icon\" type=\"image/x-ico\" href=\"{0}\" />\n<link rel=\"shortcut icon\"  type=\"image/x-icon\"  href=\"{0}\"  />", SiteSettings.Instance.Favicon);
                Page.Header.Controls.Add(new Literal()
                {
                    Text = favicon
                });  
            }


            if(IncludeJquery){
                Page.ClientScript.RegisterClientScriptInclude("jquery",
                     Page.ClientScript.GetWebResourceUrl(typeof(BaseMasterPage), "WebSite.Core.Resources.js.jquery-latest.min.js"));
            }
            if (SiteSettings.Instance.SiteOffline)
            { 
                string sReadOnly = HTTPUtils.QString( Security.ReadOnlyQName()   );  
                if (Server.UrlEncode(sReadOnly) == Security.ReadOnlyQValue())
                { 
                    Settings.SaveSetting("ReadOnly", "1");
                }
                if (!Security.IsAdmin() && HTTPUtils.GetPageName() != "l0g1n" && Settings.GetSetting("ReadOnly") != "1")
                {

                    //Response.Write(HTTPUtils.GetPageName());
                    Response.Redirect("/welcome.html");
                }
            }

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
            Utils.AddJavaScriptInclude(this.Page, url, ForceUnique);
        }

        public void AddStylesheetInclude(string url)
        {
            Utils.AddStylesheetInclude(this.Page, url);
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
            get {  return GetMetaValue("keywords");  }
            set {  AddMetaTag("keywords", value);   }
       } 
    }
}

