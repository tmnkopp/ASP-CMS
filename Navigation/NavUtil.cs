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

namespace WebSite.Core
{ 
  
    public enum NavStyleType  {  MainOnly,   SubInline,    SubBottom  }
 
    public static class NavUtil
    { 
        public static string RequestPath
        {
            get {

               // HttpContext.Current.Response.Write(HTTPUtils.QString("rp") + " ::<br>");
                    return HTTPUtils.QString("rp");
             
            }
        }
        public static ILinkable GetLinkableByName(string className)
        {
            Type module = BuildManager.GetType("WebSite.Core." + className, true);
            var customModule = (ILinkable)Activator.CreateInstance(module); 
            return customModule; 
        } 
        public static string URLRoot(){
            string root = ""; 
            //root += "/MP";
            return root;
        } 
    } 
}
