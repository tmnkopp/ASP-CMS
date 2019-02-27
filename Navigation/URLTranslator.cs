using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Diagnostics;
using ASPXUtils; 
namespace WebSite.Core
{
    public class URLTranslator
    {

    
        private static URLTranslator instance;
        public static URLTranslator Instance
        {
            get
            {
                if (instance == null || NavUtil.RequestPath=="")
                { 
                    instance = new URLTranslator(NavUtil.RequestPath); 
                }
                else if (instance.RequestPath != NavUtil.RequestPath)
                {
                    instance = new URLTranslator(NavUtil.RequestPath); 
                } 
                return instance;
            }
            set {
                instance = value;
            }
        }


        public URLTranslator (string RequestPath)
        { 

            this.RequestPath = RequestPath;
            Load(this.RequestPath);
        }


        private SystemControlType _ControlType;
        public SystemControlType ControlType
        {
            get { return _ControlType; }
            set { _ControlType = value; }
        }
 
        private string sRewritePath = "";
        public string RewritePath {
            get { return sRewritePath; }
            set { sRewritePath = value; }  
        }

        private List<ILinkable> _BreadCrumbs;
        public List<ILinkable> BreadCrumbs {
            get { return _BreadCrumbs; }
            set { _BreadCrumbs = value; }
        }

        private List<WebPage> oAllWebPages;
        private string RequestPath { get; set; }


        public string GetSubDomain()
        {
            string ret = "";
            if (SiteSettings.Instance.SubDomain!= null)
            {
                ret = SiteSettings.Instance.SubDomain;
            }
            return ret;
        }


        public void Load(string RequestPath)
        {

            if (RequestPath.ToLower().Contains("/admin/"))
            {
                return; 
            }

            oAllWebPages = new WebPage().SelectAllActive(); 
            BreadCrumbs = new List<ILinkable>();
            sRewritePath = "";
            Regex regex;
            Match match;

            regex = new Regex(@"/" + WebPage.GetNameBySysControl("GalleryCategory") + @"(/|\.|$)", RegexOptions.IgnoreCase);

            match = regex.Match(RequestPath.ToLower());
            if (match.Success && (sRewritePath == ""))
                sRewritePath = RewriteGalleryCategory(RequestPath);

            regex = new Regex(@"/" + WebPage.GetNameBySysControl("Article") + @"(/|\.|$)", RegexOptions.IgnoreCase);

            match = regex.Match(RequestPath.ToLower());
            if (match.Success && (sRewritePath == ""))
                sRewritePath = RewriteArticle(RequestPath);

            regex = new Regex(@"/" + WebPage.GetNameBySysControl("Event") + @"(/|\.|$)", RegexOptions.IgnoreCase);

            match = regex.Match(RequestPath.ToLower());
            if (match.Success && (sRewritePath == ""))
                sRewritePath = RewriteEvent(RequestPath);

            regex = new Regex(@"/" + WebPage.GetNameBySysControl("ProdCategory") + @"(/|\.|$)", RegexOptions.IgnoreCase);

            match = regex.Match(RequestPath.ToLower());
            if (match.Success && (sRewritePath == ""))
                sRewritePath = RewriteProduct(RequestPath);
 
            if (sRewritePath == "")
            {
                sRewritePath = GetGeneric(RequestPath);
                if(sRewritePath != ""){
                    this.ControlType = SystemControlType.GenericPage;
                }
            }

            this.RewritePath = sRewritePath; 
        }
        private string RewriteGalleryCategory(string RequestPath)
        {
            //bool MatchFound = false;
            this.ControlType = SystemControlType.GalleryCategory;
            WebPage SystemWP = new WebPage().SelectBySystemName("GalleryCategory");
            BreadCrumbs.Add(SystemWP);
            string sRewriteBase = SystemWP.SystemURL;
            if (sRewriteBase == "")
                sRewriteBase = "~/GalleryCategory.aspx";
            string sRewritePath = sRewriteBase + "?rp=" + RequestPath;


            string exp = string.Format(GalleryCategory.TagFmt, @"(?<tag>[a-zA-Z0-9\-\._]*?)-(?<id>[0-9]*?)");
       
            Regex regex = new Regex(exp.ToLower());
            Match match = regex.Match(RequestPath.ToLower());
            if (match.Success)
            {
                string tag = match.Groups["tag"].Value;
                string id = match.Groups["id"].Value;
                sRewritePath = sRewritePath + "&ent=GalleryCategory&tgid=" + id;
            }
          
 
            List<GalleryCategory> Matches = new GalleryCategory().SelectAllActive().Where(o => GetSubDomain() + o.URL.ToLower() == RequestPath.ToLower()).ToList();
            if (Matches.Count > 0)
            {
                sRewritePath = sRewritePath + "&cid=" + Matches[0].ID.ToString();
                BreadCrumbs.Add(Matches[0]);
            }

 
            GetPageIDFromName(ref sRewritePath, RequestPath, "GalleryCategory");
            return sRewritePath;
        }

        private string RewriteArticle(string RequestPath)
        {

            this.ControlType = SystemControlType.Article;
            WebPage SystemWP = new WebPage().SelectBySystemName("Article");
            BreadCrumbs.Add(SystemWP);
            string sRewriteBase = SystemWP.SystemURL;
            if (sRewriteBase == "")
                sRewriteBase = "~/Article.aspx";
            string sRewritePath = sRewriteBase + "?rp=" + RequestPath;
            string exp = string.Format(Article.ArchiveFmt, @"(?<mt>\d{1,2})", @"(?<yr>\d{4})(.*)");
         

            Regex regex = new Regex(exp.ToLower());
            Match match = regex.Match(RequestPath.ToLower());
            if (match.Success)
            {
                string mt = match.Groups["mt"].Value;
                string yr = match.Groups["yr"].Value; 
                sRewritePath = sRewritePath + "&mt=" + mt + "&yr=" + yr;
                BreadCrumbs.Add(new WebPage() { Caption = "Archive " + mt + " " + yr, URL = string.Format(Article.ArchiveFmt, mt, yr) });
            };

            //exp = string.Format("/" + BaseArtPage.Name + "/{0}/{1}.aspx", @"(?<cat>[a-zA-Z0-9\-]*\-[0-9]*)", @"(?<art>[a-zA-Z0-9\-]*\-[0-9]*)");
            //Regex regex = new Regex(exp.ToLower());
            //Match match = regex.Match(exp.ToLower());
            //if (match.Success)
            //{
            //    string m1 = match.Groups["cat"].Value;
            //    string m2 = match.Groups["art"].Value;
            //    sRewritePath = sRewritePath + "&cid=" + m1 + "&id=" + m2;
            //}; 

            List<Article> Matches = new Article().SelectAllActive().Where(o => GetSubDomain() + o.URL.ToLower() == RequestPath.ToLower()).ToList();
 

            if (Matches.Count > 0)
            {
                sRewritePath = sRewritePath + "&id=" + Matches[0].ID.ToString();
                BreadCrumbs.Add(Matches[0]);
            }

            if (Matches.Count < 1){
                List<ArticleCategory> CatMatches = new ArticleCategory().SelectAllActive().Where(o => GetSubDomain() + o.URL.ToLower() == RequestPath.ToLower()).ToList();
                if (CatMatches.Count > 0)
                {
                    sRewritePath = sRewritePath + "&cid=" + CatMatches[0].ID.ToString();
                    BreadCrumbs.Add(CatMatches[0]);
                }
            }
 
            GetPageIDFromName(ref sRewritePath, RequestPath, "Article");
            return sRewritePath;
        }
  
        private string RewriteEvent(string RequestPath)
        {
            this.ControlType = SystemControlType.Event;
            WebPage SystemWP = new WebPage().SelectBySystemName("Event");
            BreadCrumbs.Add(SystemWP);
            string sRewriteBase = SystemWP.SystemURL;
            if (sRewriteBase == "")
                sRewriteBase = "~/Event.aspx";
            string sRewritePath = sRewriteBase + "?rp=" + RequestPath;


            if (Regex.IsMatch(RequestPath, @"^" + WebPage.GetNameBySysControl("Event") + @"/\d{1,2}\/\d{4}$"))
            {
                string[] aPath = RequestPath.Split('/');
                string mt = aPath[aPath.Count() - 2];
                string yr = aPath[aPath.Count() - 1];
                sRewritePath = sRewritePath + "&mt=" + mt + "&yr=" + yr; 
            };
            List<Event> Matches = new Event().SelectAllActive().Where(o => GetSubDomain() + o.URL.ToLower() == RequestPath.ToLower()).ToList();
            if (Matches.Count > 0){
                sRewritePath = sRewritePath + "&id=" + Matches[0].ID.ToString();
                BreadCrumbs.Add(Matches[0]);
            }

            GetPageIDFromName(ref sRewritePath, RequestPath, "Event");
            return sRewritePath;
        }

        private string RewriteProduct(string RequestPath)
        {
            this.ControlType = SystemControlType.ProdCategory;
            WebPage SystemWP = new WebPage().SelectBySystemName("ProdCategory");
            BreadCrumbs.Add(SystemWP);
            string sRewriteBase = SystemWP.SystemURL;
            if (sRewriteBase == "")
                sRewriteBase = "~/ProductCategory.aspx";
            string sRewritePath = sRewriteBase + "?rp=" + RequestPath;


            List<ProdCategory> Matches = new ProdCategory().SelectAllActive().Where(o => o.URL.ToLower() == RequestPath.ToLower()).ToList();
            if (Matches.Count > 0){
                sRewritePath = sRewritePath + "&cid=" + Matches[0].ID.ToString();
                BreadCrumbs.Add(Matches[0]);
            }

            GetPageIDFromName(ref sRewritePath, RequestPath, "ProdCategory");
            return sRewritePath;
        }



        private void GetPageIDFromName(ref string RewritePath, string RequestPath, string ControlName)
        {
            if (RewritePath.Contains("?"))
            {
                List<WebPage> WPs = oAllWebPages.Where(o => o.SystemControl == ControlName).ToList();
                if (WPs.Count > 0)
                {
                    if (RequestPath.ToLower().Contains("/" + WPs[0].Name.ToLower()))
                    {
                        RewritePath = RewritePath + "&wpid=" + WPs[0].ID.ToString() + "&" + WPs[0].SystemParam;
                    }
                }
            }
        }

        private string GetGeneric(string RequestPath)
        {
            string ret = "";
            string pwpid = "0";

            List<WebPage> results = oAllWebPages.Where(o => RequestPath.ToLower() == GetSubDomain()+ o.URL.ToLower() && o.ExternalURL=="").ToList();
 

            if (results.Count > 0)
            { 
                WebPage WP = results[0]; 
                pwpid = WP.Parent.ToString();

                if (WP.Parent == 0) // this page is a root level page
                {
                    pwpid = WP.ID.ToString();
                }
                else // this page is a SubLevel  page
                {
                    BreadCrumbs.Add(WP.ParentPage);
                } 
 
                BreadCrumbs.Add(WP);

                if (WP.SystemURL != "")
                {
                    string page = WP.SystemURL;
                    if (!page.StartsWith("~"))
                        page = "~" + page;
                    ret = page + "?rp=" + RequestPath + "&wpid=" + WP.ID.ToString() + "&pwpid=" + pwpid;
                }
                else 
                {
                    ret = "~/Generic.aspx?rp=" + RequestPath + "&wpid=" + WP.ID.ToString() + "&pwpid=" + pwpid;
                }
                ret += "&" + WP.SystemParam; 
            }
            return ret;
        }       
    } 
}
