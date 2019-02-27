using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using ASPXUtils;
using System.Reflection;
using ASPXUtils.Controls;
namespace WebSite.Core
{ 
    public class SiteSettingsForm : EntityForm
    {
        
        protected override void DeleteForm(){}
        protected override void SaveForm()
        { 
            // ControlHolder.Controls.Add(new Literal() { Text = "Hello World" });
            SiteSettings.Instance.DefaultTitle = (string)GetValueFromControl("DefaultTitle");
            SiteSettings.Instance.DefaultDescription = (string)GetValueFromControl("DefaultDescription");
            SiteSettings.Instance.SubDomain = (string)GetValueFromControl("SubDomain");
            SiteSettings.Instance.SiteTitlePrefix = (string)GetValueFromControl("SiteTitlePrefix");
            SiteSettings.Instance.SiteTitlePostfix = (string)GetValueFromControl("SiteTitlePostfix");
            SiteSettings.Instance.NavRootDocumentName = (string)GetValueFromControl("NavRootDocumentName");
            SiteSettings.Instance.PageExtention = (string)GetValueFromControl("PageExtention");
            SiteSettings.Instance.TemplatePath = (string)GetValueFromControl("TemplatePath");
            SiteSettings.Instance.GoogleAnalytic = (string)GetValueFromControl("GoogleAnalytic");
            SiteSettings.Instance.ContactTo = (string)GetValueFromControl("ContactTo");
            SiteSettings.Instance.ContactCC = (string)GetValueFromControl("ContactCC");
            SiteSettings.Instance.UseCaptcha = Convert.ToBoolean((string)GetValueFromControl("UseCaptcha"));
            SiteSettings.Instance.SiteOffline = Convert.ToBoolean((string)GetValueFromControl("SiteOffline")); 
            SiteSettings.Instance.EditWebPagePath = (string)GetValueFromControl("EditWebPagePath");

            SiteSettings.Instance.ContentSlider = (string)GetValueFromControl("ContentSlider");
            SiteSettings.Instance.MaxImageSize = (string)GetValueFromControl("MaxImageSize");
            SiteSettings.Instance.CKEditWidth = (string)GetValueFromControl("CKEditWidth");
            SiteSettings.Instance.IncludeMenuHome = Convert.ToBoolean((string)GetValueFromControl("IncludeMenuHome"));
            SiteSettings.Instance.EditorFontNames =  (string)GetValueFromControl("EditorFontNames") ;
            SiteSettings.Instance.Favicon = (string)GetValueFromControl("Favicon");
            
            SiteSettings.Instance.HeaderFormat = (string)GetValueFromControl("HeaderFormat");
            SiteSettings.Instance.GalleryCategoryCaption = (string)GetValueFromControl("GalleryCategoryCaption");
            SiteSettings.Instance.DefaultArticleCount = (string)GetValueFromControl("DefaultArticleCount");
            SiteSettings.Instance.ImgUploadCnt = (string)GetValueFromControl("ImgUploadCnt");

            SiteSettings.Instance.FacebookLink = (string)GetValueFromControl("FacebookLink");
            SiteSettings.Instance.TwitterLink = (string)GetValueFromControl("TwitterLink");
            SiteSettings.Instance.LinkedInLink = (string)GetValueFromControl("LinkedInLink");
            SiteSettings.Instance.RssLink = (string)GetValueFromControl("RssLink");
            

            SiteSettings.Instance.Save();
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl, true);
        }
        protected override void LoadForm()
        {
            SetControlValue("DefaultTitle", SiteSettings.Instance.DefaultTitle, null);
            SetControlValue("DefaultDescription", SiteSettings.Instance.DefaultDescription, null); 
            SetControlValue("SubDomain", SiteSettings.Instance.SubDomain, null);
            SetControlValue("SiteTitlePrefix", SiteSettings.Instance.SiteTitlePrefix, null);
            SetControlValue("SiteTitlePostfix", SiteSettings.Instance.SiteTitlePostfix, null);
            SetControlValue("NavRootDocumentName", SiteSettings.Instance.NavRootDocumentName, null);
            SetControlValue("PageExtention", SiteSettings.Instance.PageExtention, null);
            SetControlValue("TemplatePath", SiteSettings.Instance.TemplatePath, null);
            SetControlValue("GoogleAnalytic", SiteSettings.Instance.GoogleAnalytic, null);

            SetControlValue("ContactTo", SiteSettings.Instance.ContactTo, null);
            SetControlValue("ContactCC", SiteSettings.Instance.ContactCC, null);
            SetControlValue("UseCaptcha", SiteSettings.Instance.UseCaptcha, null);

            SetControlValue("SiteOffline", SiteSettings.Instance.SiteOffline, null);
            SetControlValue("EditWebPagePath", SiteSettings.Instance.EditWebPagePath, null); 


            SetControlValue("ContentSlider", SiteSettings.Instance.ContentSlider, null);
            SetControlValue("MaxImageSize", SiteSettings.Instance.MaxImageSize, null);
            SetControlValue("CKEditWidth", SiteSettings.Instance.CKEditWidth, null);
            SetControlValue("EditorFontNames", SiteSettings.Instance.EditorFontNames, null);
            SetControlValue("IncludeMenuHome", Convert.ToString(SiteSettings.Instance.IncludeMenuHome), null);
            SetControlValue("HeaderFormat", SiteSettings.Instance.HeaderFormat, null);
            SetControlValue("Favicon", SiteSettings.Instance.Favicon, null);
            SetControlValue("GalleryCategoryCaption", SiteSettings.Instance.GalleryCategoryCaption, null);
            SetControlValue("DefaultArticleCount", SiteSettings.Instance.DefaultArticleCount, null);

            SetControlValue("ImgUploadCnt", SiteSettings.Instance.ImgUploadCnt, null);
            

            SetControlValue("FacebookLink", SiteSettings.Instance.FacebookLink, null);
            SetControlValue("TwitterLink", SiteSettings.Instance.TwitterLink, null);
            SetControlValue("LinkedInLink", SiteSettings.Instance.LinkedInLink, null);
            SetControlValue("RssLink", SiteSettings.Instance.RssLink, null);
             
        }  
    }
}
