using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.Collections;
using System.Configuration;

namespace WebSite.Core 
{
    public static class Utils
    {
        public static void GetServerVariables(){
            StringBuilder SB = new StringBuilder();
            System.Collections.Specialized.NameObjectCollectionBase.KeysCollection keys = HttpContext.Current.Request.ServerVariables.Keys;
            foreach (string key in keys)
            {
                SB.AppendFormat("<br>HttpContext.Current.Request.ServerVariables[\"{0}\"]  &nbsp;&nbsp; : <b>{1}</b> "
                    , key
                    , HttpContext.Current.Request.ServerVariables[key]);
            }
            HttpContext.Current.Response.Write(SB.ToString());
        }
        public static string ServerVariable(string ServerVariableName)
        {
            return HttpContext.Current.Request.ServerVariables[ServerVariableName];
        }

        public static void AddStylesheetInclude(Page page, string url)
        {
            
            HtmlLink link = new HtmlLink();
            link.Attributes["type"] = "text/css";
            link.Attributes["href"] = url;
            link.Attributes["rel"] = "stylesheet";
            page.Header.Controls.Add(link);
            page.Header.Controls.Add(new Literal() { Text = "\n" });
        }
      
        public static void AddJavaScriptInclude(Page page, string url, bool ForceUnique)
        {
            
            bool CanAdd = true;
            if (ForceUnique)
            {
                foreach (Control c in page.Header.Controls)
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

            if (CanAdd)
            {
                HtmlGenericControl script = new HtmlGenericControl("script");
                script.Attributes["type"] = "text/javascript";
                script.Attributes["src"] = url;
                page.Header.Controls.Add(script);
                page.Header.Controls.Add(new Literal() { Text = "\n" });
            }
        } 

        public static bool IsLocal()
        {
            if (HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"] == "127.0.0.1"
                && HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] == "127.0.0.1"
                && HttpContext.Current.Request.ServerVariables["REMOTE_HOST"] == "127.0.0.1"
                && !HttpContext.Current.Request.ServerVariables["HTTP_HOST"].Contains(".")
                && (ConfigurationManager.AppSettings["SITE_ID"] == "test" || ConfigurationManager.AppSettings["SITE_ID"] == "1C4A3678-99A8-446F-BF18-979DE18C0504")
                
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }





        public static string HTMLFriendlyScriptName() {

            return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].Replace("/", "-").Replace(SiteSettings.Instance.PageExtention, "");
        }
        public static string RenderContent(WebPage oWebPage) {

            if (oWebPage == null)
                return "";
            if (oWebPage.PageContent == null)
                return "";

            return DynamicContentHelper.InjectControls(oWebPage.PageContent); 
        }

        public static string GetDataString(object arg)
        {
            string val = Convert.ToString(arg);
            if (val == null)
                val = "";
            return val; 
        }
        public static int GetDataInt(object arg)
        {
            int val = Convert.ToInt32(arg);
            return val;
        }     
        public static string ExMessage(Exception ex, string sender){
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("{0} - {1} - {2}", sender, ex.Source, ex.Message);
            return String.Format("<span class='sysmessage'>{0}</span>", SB.ToString());
        }
        public static List<ILinkable> ConvertListToLinkable<L, T>(L linkableCollection, T objLoadedType)
            where L : List<ILinkable>
            where T : IEnumerable
        {
            if (linkableCollection.Count < 1)
            {
                foreach (ILinkable item in objLoadedType)
                    linkableCollection.Add(item);
            }
            return linkableCollection;
        }

        private static string GetCssValue(string def, string attribute)
        {
            string ret = "";
            string[] arr = def.Split(';');
            foreach (string item in arr)
            {
                if (item.Contains(attribute))
                    ret = item.Replace(attribute + ":", "");
            }
            return ret;
        }

    }
}
