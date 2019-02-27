using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPXUtils; 
namespace WebSite.Core
{ 
    
    [ToolboxData("<{0}:LoginLink runat=server></{0}:LoginLink>")]
    public class LoginLink : Control
    {
        #region Properties
        public string LoginPageUrl { get; set; }
        public string NavigateUrl { get; set; }
        public string CssClass { get; set; }
        public bool ShowLogin { get; set; }
        #endregion  
  
        protected override void Render(HtmlTextWriter writer)
        {
            string _LoginPageUrl = "/l0g1n.aspx?1=1";
            
            if (LoginPageUrl != null) {
                _LoginPageUrl = this.LoginPageUrl;
            }
            writer.WriteLine("<span id='loginlinks' class='" + this.CssClass + "'>");
            if (Security.IsAdmin())
            {
                this.NavigateUrl = _LoginPageUrl + "&logout=1";

                writer.WriteLine("<a href='" + this.NavigateUrl + "&logout=1" + "'>Logout</a>");
                writer.WriteLine(" | <a href='/admin/default.aspx" + "'>Admin</a> ");
            }  else { 
                if(this.ShowLogin){
                    this.NavigateUrl = _LoginPageUrl + "";
                    writer.WriteLine("<a href='" + this.NavigateUrl + "&logout=1" + "'>Login</a> ");
                } 
            }
            writer.WriteLine("</span>");
            base.Render(writer);  
        } 
    }
}
