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
using System.IO;

/// <summary>
/// Summary description for UrlRewriteModule
/// </summary>
/// namespace WebSite.Core

namespace WebSite.Core.HTTPHandlers
{

    public class Redirect301Module : IHttpModule
    {


        public Redirect301Module() { }
        public void Init(System.Web.HttpApplication MyApp)  { 
          MyApp.BeginRequest += new System.EventHandler(Rewriting_BeginRequest);
          MyApp.AuthorizeRequest += new System.EventHandler(Rewriting_AuthorizeRequest); 
        } 

        public void Dispose()  {  }
        private void Rewriting_AuthorizeRequest(object sender, EventArgs e) {  }
      
        public void Rewriting_BeginRequest(object sender, System.EventArgs args)   {
            
            HttpApplication MyApp = (HttpApplication)sender;
            string RequestPath = MyApp.Request.RawUrl.ToLower().Trim();
            string QueryString = MyApp.Request.QueryString.ToString().ToLower();


            if (RequestPath.ToLower().EndsWith("/admin/install.aspx"))
                return;
            if (RequestPath.ToLower().EndsWith(".css"))
                return;
            if (RequestPath.ToLower().EndsWith(".js"))
                return;
            if (RequestPath.ToUpper().EndsWith("E898F2AA-1D40-4BAE-B588-7557304545CD.ASPX"))
                return;

            //MyApp.Response.Write( RequestPath + "<br>"); 
            
           
            try {
                DoRedirect(RequestPath, MyApp);
            }     
            catch (Exception ex)   
             {
                 MyApp.Response.Write(ex.Message + "  " + ex.Source + "<br>"); 
                 //throw ex;     
             }

           

           
       }
        private void DoRedirect(string RequestPath, HttpApplication MyApp)
        {
            
            TextReader tr = new StreamReader(MyApp.Server.MapPath("~/App_Data/301Redirects.txt"));
            string line = "";
            string from = "";
            string to = "";
            if (SiteSettings.Instance.SubDomain != null)
            {
                if (SiteSettings.Instance.SubDomain.Length > 0)
                {
                    RequestPath = RequestPath.Replace(SiteSettings.Instance.SubDomain, "");
                } 
            }
            
            while ((line = tr.ReadLine()) != null)
            { 
                if (!line.StartsWith("*"))
                { 
                        from = line.Split('|')[0].ToLower().Trim();
                        to = line.Split('|')[1].Trim();
                        //HttpContext.Current.Response.Write("<!-- F: " + from + " RP: " + RequestPath + "  -->\n");
                        if (from.EndsWith(RequestPath.ToLower()) && to != "")
                        {
                            MyApp.Response.Status = "301 Moved Permanently";
                            MyApp.Response.AddHeader("Location", to);
                            tr.Close();
                            MyApp.Response.End();
                            break; 
                        } 
                }
            }
            tr.Close(); 
        }
    }
}