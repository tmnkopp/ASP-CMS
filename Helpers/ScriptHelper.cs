using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Collections.Specialized;
using ASPXUtils;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using System.IO; 
namespace WebSite.Core
{  
    public static class ScriptHelper
    {  
         
        public static string CKDefaultConfigControls()
        {

            StringBuilder SB = new StringBuilder();
            SB.Append(" [ "); 
            foreach (SystemControl SC in new SystemControl().SelectAll())
            {
                SB.Append(" [ '" + SC.ClassName + "' , '" + SC.ClassName + "'] ,");
            }

            List<WebPage> oList = new WebPage().SelectAllMain().Where(o=>o.SystemControl=="").ToList();
            foreach (WebPage WP in oList)
            {

                SB.Append(" [  'Menu Items " + WP.ID.ToString() + "-" +SanitizeForJS( WP.MenuCaption )+ "' ,'" + WP.ID.ToString() + "'] ,");
            }


            SB.Remove( SB.Length - 1 , 1);
            SB.Append("  ] "); 
            return SB.ToString();  
        }
        public static string CKDefaultConfigControlValues()
        {
            LinkableList list;
            ILinkable oLinkable;
            string sListHtml = "";
            StringBuilder SB = new StringBuilder();
            SB.Append(" [ ");
            foreach (SystemControl SC in new SystemControl().SelectAll())
            {
                oLinkable = NavUtil.GetLinkableByName(SC.ClassName);
                list = new LinkableList() { Item = oLinkable, CssClass = SC.ClassName };

                sListHtml = SanitizeForJS(list.GetHTML());
                SB.Append(" [ '" + SC.ClassName + "' , '" + SanitizeForJS(sListHtml) + "'] ,");
            }

            List<WebPage> oList = new WebPage().SelectAllMain().Where(o => o.SystemControl == "").ToList(); 
            foreach (WebPage WP in oList)
            {
                sListHtml = WebPagesToList(WP.SelectSubByParent(WP.ID));
                SB.Append(" [  '" + WP.ID.ToString() + "'  ,  '" + SanitizeForJS(sListHtml) + "' ] ,");
            }

            SB.Remove(SB.Length - 1, 1);
            SB.Append("  ] ");
            return SB.ToString(); 
        }

        public static string CKDefaultConfigGetWebPages()
        { 
            StringBuilder SB = new StringBuilder();
            SB.Append(" [ ");  
            List<WebPage> oList = new WebPage().SelectAllActive().ToList();
            foreach (WebPage WP in oList)
            {
                SB.AppendFormat(" [  '{1} ({2})'  ,  '{0}' ] ,", WP.ID.ToString(),  SanitizeForJS(WP.MenuCaption), SanitizeForJS(WP.URL));
                //SB.Append(" [  '" + WP.ID.ToString() + "'  ,  '" + + "' ] ,");
            }

            SB.Remove(SB.Length - 1, 1);
            SB.Append("  ] ");
            return SB.ToString();
        }
        public static string SanitizeForJS(string arg)
        { 
            arg = arg.Replace("'", ""); 
            return arg;
        }

        public static string WebPagesToList(List<WebPage> oList)
        { 
            StringBuilder SB = new StringBuilder();
            SB.Append("<ul>");
            foreach (WebPage WP in oList)
            {
                SB.Append("<li>" + WP.Caption+ "</li>");
            }
            SB.Append("</ul>");
            return SB.ToString();

        }
    } 
}
