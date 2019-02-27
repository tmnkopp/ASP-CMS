using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using ASPXUtils;
using System.Web.Compilation;
using System.Reflection;
using System.Configuration;
using ASPXUtils.Controls;

namespace WebSite.Core 
{
    public  class LoginForm : UserControl
    {  

        protected Panel ControlHolder = new Panel() { CssClass = "LoginForm" };

        private Button cmdSubmit = new Button() { Text = "Login" }; 
        private ExtendedTextBox txtUsername = new ExtendedTextBox() { ID = "txtUsername", Required = true };
        private ExtendedTextBox txtPassword = new ExtendedTextBox() { ID = "txtPassword", Required = true, TextMode = TextBoxMode.Password };
        private Label lblFeedBack = new Label() { ID = "lblFeedBack" };
        protected override void OnInit(EventArgs e) 
        {
            base.OnInit(e); 
            cmdSubmit.Click += new EventHandler(cmdSubmit_Click); 
             
            Controls.Add(ControlHolder);
            
            ControlHolder.Controls.Add(lblFeedBack);
            ControlHolder.Controls.Add(new Literal() { Text = "<br><label>Username</label>" }); 
            ControlHolder.Controls.Add(txtUsername);
            ControlHolder.Controls.Add(new Literal() { Text = "<br><label>Password</label>" });
            ControlHolder.Controls.Add(txtPassword);
            ControlHolder.Controls.Add(new Literal() { Text = "<br><label> </label>" });
            ControlHolder.Controls.Add(cmdSubmit);
            
        }

        protected void cmdSubmit_Click(object source, EventArgs e)
        {
            lblFeedBack.Visible = true;
            lblFeedBack.CssClass = "ok";
            string SITEU = ConfigurationSettings.AppSettings["SITEU"];
            string SITEP = ConfigurationSettings.AppSettings["SITEP"]; 
            if ( SITEU != null && SITEP != null && txtUsername.Text != "" && txtPassword.Text != "" )
            {
                if (txtUsername.Text == SITEU && txtPassword.Text == SITEP)
                {
                    Security.SetAdmin();
                    lblFeedBack.Text = "<b>Login Successful</b>"; 
                    lblFeedBack.Text += "<br><a href='/admin/default.aspx'>Go To Admin</a>";
                }
                else
                    SetInvalid();  
            }
            else 
                SetInvalid();
           
    }

    private void SetInvalid() {
        Security.ClearAdmin();
        lblFeedBack.Text = "<b>Invalid User Name / Password. Please try again</b>";
        lblFeedBack.CssClass = "ko";  
 
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HTTPUtils.QString("logout") != "")
            {
                Security.ClearAdmin();
                Settings.SaveSetting("admin", "");
                Response.Redirect("/");
            }
        }
    } 
}
 

