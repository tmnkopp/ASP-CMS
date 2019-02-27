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
using ASPXUtils;

namespace WebSite.Core 
{
    public static class Globals
    {
        public static string _lng = HTTPUtils.QString("lng");
        public static string defaultLangCode = "en";
        public static string GetLang()
        {
            if (HTTPUtils.QString("lng") != "")
            {
                Settings.SaveSetting("lang", HTTPUtils.QString("lng"));
            }
            if (HTTPUtils.QString("lng") == "en")
            {
                Settings.SaveSetting("lang", "");
            } 
            return Settings.GetSetting("lang");
        } 
        public static string GetRequestPage()
        {
            return HTTPUtils.QString("rp");
        } 
        
    }
}
