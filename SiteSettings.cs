using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Web.Configuration;
using System.Web;
using ASPXUtils;
using ASPXUtils.Controls;
namespace WebSite.Core
{
    public class SiteSettings
    {
        public static string THUMBS_PATH = "th/";
        public static string SITE_ID = WebConfigurationManager.AppSettings["SITE_ID"];
        public static string VIR_GALLERY_PATH = WebConfigurationManager.AppSettings["GalleryPath"];
        public static string PHY_GALLERY_PATH =  HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["GalleryPath"]);
        public static string TABLE_PREFIX = WebConfigurationManager.AppSettings["TABLE_PREFIX"];
        public static string SYSFILE_TEMPLATE_PREFIX = WebConfigurationManager.AppSettings["SYSFILE_TEMPLATE_PREFIX"];
        public static string UPLOAD_PATH = WebConfigurationManager.AppSettings["UploadPath"];
        public static string SITE_U = WebConfigurationManager.AppSettings["SITEU"];
        public static string SITE_P = WebConfigurationManager.AppSettings["SITEP"];

  
        private static SiteSettings siteSettingsSingleton;
        public static SiteSettings Instance
        {
            get   {
                if (siteSettingsSingleton == null)  {
                    siteSettingsSingleton = new SiteSettings();
                } 
                return siteSettingsSingleton;
            }
        } 
        [Formfield("", Caption = "Default Title" )]
        public string DefaultTitle { get; set; }
        [Formfield("", Caption = "Default Description", ControlType = typeof(ASPXUtils.Controls.TextArea))]
        public string DefaultDescription { get; set; }

        [Formfield("", FieldAccess = FormFieldAccess.fa_SiteConfig, Caption = "Sub Domain")]
        public string SubDomain { get; set; } 
        [Formfield("", Caption = "Title Prefix" )]
        public string SiteTitlePrefix { get; set; } 
        [Formfield("", Caption = "Title Postfix" )]
        public string SiteTitlePostfix { get; set; }

        private string _HeaderFormat;
        [Formfield("", FieldAccess = FormFieldAccess.fa_SiteConfig, Caption = "Header Format")]
        public string HeaderFormat
        {
            get
            {
                if (_HeaderFormat == null)
                    _HeaderFormat = "{0}";
                return _HeaderFormat;
            }
            set { _HeaderFormat = value; }
        }


        public bool UseExtentionLessParentPages { get; set; }

        private string _NavRootDoc; 
        [Formfield("", Caption = "Root Document Name" )]
        public string NavRootDocumentName
        {
            get
            {
                if (_NavRootDoc == null)
                    _NavRootDoc = "/";
                return _NavRootDoc;
            }
            set { _NavRootDoc = value; }
        }
        [Formfield("", Caption = "Site Offline", ControlType = typeof(DDTrueFalse))]
        public bool SiteOffline { get; set; }

        private bool _IncludeMenuHome = true;
        [Formfield("", Caption = "Include 'Home' in Menu", ControlType = typeof(DDTrueFalse))]
        public bool IncludeMenuHome
        {
            get   {   return _IncludeMenuHome;    }
            set { _IncludeMenuHome = value; }
        }


        private string _PageExtention;
        [Formfield("", Caption = "Page Extention", FieldAccess = FormFieldAccess.fa_SiteConfig, Required = true)]
        public string PageExtention  
        {
            get
            {
                if (_PageExtention == null)
                    _PageExtention = ".aspx";
                return _PageExtention;
            }
            set { _PageExtention = value; }
        }


        private string _EditWebPagePath;
        [Formfield("", Caption = "Edit Web Page Path", FieldAccess = FormFieldAccess.fa_SiteConfig, Required = true)]
        public string EditWebPagePath  
        {
            get
            {
                if (_EditWebPagePath == null)
                    _EditWebPagePath = "/Admin/EntityGridForm.aspx?etype=WebPage&id={0}";
                return _EditWebPagePath;
            }
            set { _EditWebPagePath = value; }
        }

       

        private string _EditorFontNames = "Arial;Verdana;Trebuchet MS;Tahoma;Lucida Sans Unicode;Georgia;Times New Roman;Helvetica;sans-serif";
        [Formfield("", Caption = "Editor Font Names", FieldAccess = FormFieldAccess.fa_SiteConfig, Required = true)]
        public string EditorFontNames  
        {
            get { return _EditorFontNames; }
            set { _EditorFontNames = value; }
        }


        private string _GalleryCategoryCaption = "GalleryCategory";
        [Formfield("Gallery Category Caption", FieldAccess=FormFieldAccess.fa_SiteConfig , Caption = "Gallery Category Caption", Width = 100 , Required=true)]
        public string GalleryCategoryCaption
        {
            get { return _GalleryCategoryCaption; }
            set { _GalleryCategoryCaption = value; } 
        }

        private string _MaxImageSize = "800";
        [Formfield("Max Upload Image Size", FieldAccess = FormFieldAccess.fa_SiteConfig, Caption = "Max Image Size", Width = 50, ValidateInteger = true)]
        public string MaxImageSize
        {
            get { return _MaxImageSize; }
            set { _MaxImageSize = value; } 
        }

        private string _CKEditWidth = "900";
        [Formfield("Ck Edit Width", FieldAccess = FormFieldAccess.fa_SiteConfig, Caption = "Ck Edit Width", Width = 50, ValidateInteger = true)]
        public string CKEditWidth
        {
            get { return _CKEditWidth; }
            set { _CKEditWidth = value; }
        }

        private string _DefaultArticleCount = "5";
        [Formfield("Default Article Count", FieldAccess = FormFieldAccess.fa_SiteConfig, Caption = "Default Article Count", Width = 50, ValidateInteger = true)]
        public string DefaultArticleCount
        {
            get { return _DefaultArticleCount; }
            set { _DefaultArticleCount = value; }
        }


        private string _ImgUploadCnt = "5";
        [Formfield("Image Upload Count", FieldAccess = FormFieldAccess.fa_SiteConfig, Caption = "Image Upload Count", Width = 50, ValidateInteger = true)]
        public string ImgUploadCnt
        {
            get { return _ImgUploadCnt; }
            set { _ImgUploadCnt = value; }
        }


        private string _DefaultContent = "Lorem ipsum dolor sit amet. ";
        [Formfield("Default Content Placeholder", FieldAccess = FormFieldAccess.fa_SiteConfig )]
        public string DefaultContent
        {
            get { return _DefaultContent; }
            set { _DefaultContent = value; }
        }

        [Formfield("", Caption = "Content Slider", FieldAccess = FormFieldAccess.fa_SiteConfig, ControlType = typeof(DDGalleryCats))]
        public string ContentSlider { get; set; }

        private string _TemplatePath;
        [Formfield("", FieldAccess = FormFieldAccess.fa_SiteConfig, Caption = "Template Path")]
        public string TemplatePath
        {
            get
            {
                if (_TemplatePath == null)
                    _TemplatePath = "~/layouts/";
                return _TemplatePath;
            }
            set { _TemplatePath = value; }
        }

        private string _IDSep;
        public string IDSep
        {
            get  {
                if (_IDSep == null)
                    _IDSep = "-";
                return _IDSep;
            }
            set { _IDSep = value; }
        }

        private bool _useID = true;
        public bool UseIDInUrl
        {
            get {  return _useID; }
            set { _useID = value; }
        }

        private string _SubDirSep;
        public string SubDirSep
        {
            get
            {
                if (_SubDirSep == null)
                    _SubDirSep = "/";
                return _SubDirSep;
            }
            set { _SubDirSep = value; }
        }



        private string _GoogleAnalytic = "";
        [Formfield("", FieldAccess = FormFieldAccess.fa_SiteConfig, Caption = "Google Analytic", ControlType = typeof(ASPXUtils.Controls.TextArea))]
        public string GoogleAnalytic
        {
            get { return _GoogleAnalytic; }
            set { _GoogleAnalytic = value; }
        }

        private string _ContactTo = "";
        [Formfield("", Caption = "Contact To", Required = true)]
        public string ContactTo
        {
            get { return _ContactTo; }
            set { _ContactTo = value; }
        }
 
        private string _ContactCC = "";
        [Formfield("", Caption = "Contact CC" )]
        public string ContactCC
        {
            get { return _ContactCC; }
            set { _ContactCC = value; }
        }


        [Formfield("", Caption = "Use Captcha", ControlType = typeof(DDTrueFalse))]
        public bool UseCaptcha { get; set; }





        [Formfield("Favicon")]
        public string Favicon { get; set; }

        [Formfield("Facebook Link")]
        public string FacebookLink { get; set; }
        [Formfield("Twitter Link")]
        public string TwitterLink { get; set; }
        [Formfield("LinkedIn Link")]
        public string LinkedInLink { get; set; }
        [Formfield("Rss Link")]
        public string RssLink { get; set; }


        public SiteSettings() { 
            Load(); 
        }
  

        public void Save()
        {
            System.Collections.Specialized.StringDictionary dic = new System.Collections.Specialized.StringDictionary();
            Type settingsType = this.GetType();

            //------------------------------------------------------------
            //	Enumerate through settings properties
            //------------------------------------------------------------
            foreach (PropertyInfo propertyInformation in settingsType.GetProperties())
            {

                if (propertyInformation.Name != "Instance" && propertyInformation.CanWrite)
                { 
                    object propertyValue = propertyInformation.GetValue(this, null); 
                    string valueAsString; 
                    if (propertyValue == null || propertyValue.Equals(Int32.MinValue) || propertyValue.Equals(Single.MinValue))
                    {
                        valueAsString = String.Empty;
                    }  else  {
                        valueAsString = propertyValue.ToString();
                    } 
                    dic.Add(propertyInformation.Name, valueAsString);
                }
            }
            SiteSettingsDAL.SaveSettings(dic);
            
        }
 

        private void Load()
        {
            Type settingsType = this.GetType();  
            System.Collections.Specialized.StringDictionary dic = SiteSettingsDAL.LoadSettings();

            foreach (string key in dic.Keys)
            { 
                string name = key;
                string value = dic[key]; 
                foreach (PropertyInfo propertyInformation in settingsType.GetProperties())
                { 
                    if (propertyInformation.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    { 
                        try  {
                            if (propertyInformation.CanWrite)
                            {
                                propertyInformation.SetValue(this, Convert.ChangeType(value, propertyInformation.PropertyType, CultureInfo.CurrentCulture), null);
                            }
                        }
                        catch
                        {
                            // TODO: Log exception to a common logging framework?
                        }
                        break;
                    }
                }
            }
             
        } 
    } 
}
