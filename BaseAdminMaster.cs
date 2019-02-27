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

    public abstract class BaseAdminMaster : MasterPage
    {

        public BaseAdminMaster() { }
        public bool IncludeJquery { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Security.IsAdmin())
            {
                Response.Redirect("/");
            }

            if (IncludeJquery)
            {
            }
            //Page.ClientScript.RegisterClientScriptInclude("jquery",
            //     Page.ClientScript.GetWebResourceUrl(typeof(BaseAdminMaster), "WebSite.Core.Resources.js.jquery-latest.min.js"));

            //HttpContext.Current.Response.Write("BASE MASTER"); 
            Page.Header.Controls.Add(new AccessStyles() { });

            if (Page.Title == "Untitled Page" || Page.Title == "")
            {
                Page.Title = Reques