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
using WebSite.Core;
using System.Text.RegularExpressions;
using ASPXUtils;

/// <summary>
/// Summary description for UrlRewriteModule
/// </summary>
/// namespace WebSite.Core

namespace WebSite.Core.HTTPHandlers
{

    public class UrlRewriteModule : IHttpModule
    { 
        

	    public UrlRewriteModule() {  }
        public void Init(System.Web.HttpApplication MyApp)  { 
          MyApp.BeginRequest += new System.EventHandler(Rewriting_BeginRequest);
          MyApp.AuthorizeRequest += new System.EventHandler(Rewriting_AuthorizeRequest); 
        } 

        public void Dispose()  {  }
        private void Rewriting_AuthorizeRequest(object sender, EventArgs e) {  }
      
        public void Rewriting_BeginRequest(object sender, System.EventArgs args)   {
 

            HttpApplication MyApp = (HttpApplication)sender;
            string sRewritePath = ""; 

            string RequestPath = MyApp.Request.Path;
            string QueryString = MyApp.Request.QueryString.ToString().ToLower();


            //HttpContext.Current.Response.Write(RequestPath + " !!! <br>"); 



            if (RequestPath.ToLower().EndsWith("/admin/install.aspx"))
                return;
            if (RequestPath.ToLower().EndsWith(".css"))
                return;
            if (RequestPath.ToLower().EndsWith(".js"))
                return;
            if (RequestPath.ToUpper().EndsWith("E898F2AA-1D40-4BAE-B588-7557304545CD.ASPX"))
                return;

            sRewritePath = new URLTranslator(RequestPath).RewritePath;

            //MyApp.Response.Write("<br>" + RequestPath);
     
            if (sRewritePath != "")
                MyApp.Context.RewritePath(sRewritePath + QueryString); 
       }  
    }
}