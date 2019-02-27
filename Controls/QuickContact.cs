using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ASPXUtils;
using WebSite.Core;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Net.Mail;
using ASPXUtils.Controls;

namespace WebSite.Core
{
    public class QuickContact : UserControl
    { 
        public PlaceHolder ControlHolder;
        public UpdatePanel UP;
        public ExtendedTextBox txtName;
        public ExtendedTextBox txtEmail;
        public TextBox txtMessage;
        public Button cmdSubmit;
        public Label divLoading;
        public string HeaderLabel { get; set; }
        public string ButtonLabel { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ControlHolder = new PlaceHolder();
            Controls.Add(ControlHolder);  
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadForm();
        }
        protected void LoadForm()
        {
            if (ButtonLabel == null)
                ButtonLabel = "Send";
            UP = new UpdatePanel() { ID = "UP" };
            cmdSubmit = new Button() { CssClass = "qc_cmdsend", ID = "cmdSend", Text = ButtonLabel };
            cmdSubmit.Click += new EventHandler(cmdSubmit_Click);
            cmdSubmit.OnClientClick = "send();return;";
            txtName = new ExtendedTextBox() { CssClass = "qc_input", ID = "txtName" };
            txtEmail = new ExtendedTextBox() { CssClass = "qc_input", ID = "txtEmail", Required=true };
            txtMessage = new TextBox() { CssClass = "qc_input", ID = "txtMessage", TextMode = TextBoxMode.MultiLine, Height = 100 };
            divLoading = new Label() { ID = "divLoading", CssClass = "hide divLoading", Text = "<img style='display:block;position:relative;left:70px; top:170px; ' src='/images/loading.gif'>" };

            ControlHolder.Controls.Add(new Literal() { Text = "<div class='qc_container'>" });
            ControlHolder.Controls.Add(new ScriptManager());
            ControlHolder.Controls.Add(divLoading);
            UP.ContentTemplateContainer.Controls.Add(new Literal() { Text = "<label class='qc_header'>" + this.HeaderLabel + "</label>" });
            UP.ContentTemplateContainer.Controls.Add(new Literal() { Text = "<label class='qc_name'>Name</label>" });
            UP.ContentTemplateContainer.Controls.Add(txtName);
            UP.ContentTemplateContainer.Controls.Add(new Literal() { Text = "<label class='qc_email'>Email</label>" });
            UP.ContentTemplateContainer.Controls.Add(txtEmail);
            UP.ContentTemplateContainer.Controls.Add(new Literal() { Text = "<label class='qc_message'>Message</label>" });
            UP.ContentTemplateContainer.Controls.Add(txtMessage);
            UP.ContentTemplateContainer.Controls.Add(new Literal() { Text = "<br><br>" });
            UP.ContentTemplateContainer.Controls.Add(cmdSubmit);
            ControlHolder.Controls.Add(UP);
            ControlHolder.Controls.Add(new Literal() { Text = "</div>" } );

            string script = @"
            function setvisible(){
                document.getElementById('" + divLoading.ClientID + @"').style.visibility='visible'; 
            } 
            function sethidden(){ 
                document.getElementById('" + divLoading.ClientID + @"').innerHTML='<span style=display:block;position:relative;left:50px;top:200px; class=ok>Email Sent</span>';
                document.forms[0]['" + txtMessage.ClientID + @"'].value = '';        
            }
            function send(){
                if( document.forms[0]['" + txtEmail.ClientID + @"'].value != '' ){
                    setvisible();
                    var t=setTimeout('sethidden()',1500);
                }else{
                    document.forms[0]['" + txtEmail.ClientID + @"'].value='[Enter Email]';
                } 
            }";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "quick_contact", script, true);
        }

        protected void cmdSubmit_Click(object source, EventArgs e)
        {

            SaveForm();
        }
        protected void SaveForm()
        {
            if (txtEmail.Text.IndexOf("@") < 1)
                return;
            string message = @"
                Name: " + txtName.Text + @"
                <br>Email: " + txtEmail.Text + @" 
                <br>Message: " + txtMessage.Text + @" 
                <br>From Page: " + HTTPUtils.QString("rp") + @"  
            ";

            MailMessage oMailMessage = new MailMessage();
            oMailMessage.From = new MailAddress("WebContact@" + Request.ServerVariables["SERVER_NAME"]);
            oMailMessage.To.Add(new MailAddress(SiteSettings.Instance.ContactTo));
            oMailMessage.IsBodyHtml = true;
            oMailMessage.Subject = "Website Quick Contact";
            oMailMessage.Body = message;

            SmtpClient client = new SmtpClient();
            try
            {
                client.Send(oMailMessage);
            }
            catch (Exception ex)
            {
                txtMessage.Text = "[Message Sent]";
                Trace.Write(ex.Message);
                //ControlHolder.Controls.Add(new Label() { Text = "<span id='emailerr' style='display:none;'>" + ex.Message + "</span>" });  
            }
        } 
    } 
}


