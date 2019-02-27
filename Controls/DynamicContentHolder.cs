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
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using ASPXUtils;
using System.Web.Caching;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace WebSite.Core 
{
    [ToolboxData("<{0}:Updatable runat=Server></{0}:Updatable>")]
    public class DynamicContentHolder : UserControl
    { 
        public string DataFilePath { get; set; }
        public string DataFile { get; set; }
        public string TitleCaption { get; set; }  
        public string StyleSheet { get; set; }  
        public string BeginAtNode { get; set; }
        public string CssClass { get; set; }

        private string _TargetEditURL = "/Admin/Default.aspx?u={0}&ban={1}&view=dch";
        public string TargetEditURL
        {
            get { return _TargetEditURL; }
            set { _TargetEditURL = value; }
        }  
        
        private string content = "";
        public StringBuilder WriteLine = new StringBuilder(); 

        XmlDocument xmlDoc = new XmlDocument();
        Panel Editable = new Panel();
        Literal litResult = new Literal();


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.DataFilePath == null || this.DataFilePath == "")
            {
                string _fname; 
                if (this.DataFile != null && this.DataFile != "")
                {
                    _fname = this.DataFile;
                }  else { 
                    _fname = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToLower();
                    _fname = _fname.Replace("/", "-").TrimStart('-');
                    _fname = _fname.Split('.')[0]; 
                } 
                this.DataFilePath = string.Format("~/App_Data/Content/{0}.xml", _fname); 
            }
        }
 
        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.Controls.Add(Editable);
            Editable.Controls.Add(litResult);

            if (this.StyleSheet == null) 
                this.StyleSheet = "basic";

            // /Admin/EditRegion.aspx
         
            DynamicContentHelper.RenderEditableRegion((Panel)Editable 
                ,  string.Format(TargetEditURL, this.DataFilePath ,this.BeginAtNode )  
                );
       
            if (this.DataFilePath != "" && this.DataFilePath != null) 
                LoadContent();

            litResult.Text = String.Format("<div id=\"{1}\" class=\"{2}\">{0}</div>"
                , WriteLine.ToString()
                , this.ID
                , Utils.HTMLFriendlyScriptName().Replace("-", "") + "-" + this.ID + " " + Convert.ToString(this.CssClass));

        }

        protected void LoadContent()
        {
            StringWriter SW = new StringWriter();
            XmlDocument xmlDocTemp = new XmlDocument();
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(HttpContext.Current.Server.MapPath("~/xsl/" + this.StyleSheet + ".xslt"));

            try  {
                xmlDocTemp.LoadXml(GetData().InnerXml);
            }
            catch (Exception ex)  {
                //Utils.SendLog("ContentPlaceholder.ascx / GetData()");
                throw ex;
            }

           
            if (this.BeginAtNode != "" && this.BeginAtNode != null)
            {
                XmlNode oNode = xmlDocTemp.GetElementsByTagName(this.BeginAtNode)[0]; 
                try
                {
                    xmlDocTemp.LoadXml("<root>" + oNode.InnerXml + "</root>");
                }
                catch (Exception ex)
                {
                    //xmlDocTemp.LoadXml("<root><err></err></root>");
                    //Trace.Write(ex.Message);
                    throw ex;
                    //Utils.SendLog("ContentPlaceholder.ascx / transform.Transform ");
                }
            }

            this.content = xmlDocTemp.InnerXml;
            this.content = DynamicContentHelper.InjectControls(this.content);

            try   {
                xmlDocTemp.LoadXml(this.content);
            }
            catch (Exception ex)  {
                throw ex; //Utils.SendLog("ContentPlaceholder.ascx / this.BeginAtNode");
            }

            try   {
                transform.Transform(xmlDocTemp, null, SW);
                string _result = HttpContext.Current.Server.HtmlDecode(SW.ToString());
                WriteLine.Append( _result + "&nbsp;");  
            }
            catch (Exception ex)  {
                throw ex;
                //Utils.SendLog("ContentPlaceholder.ascx / transform.Transform ");
            }

        }

        protected XmlDocument GetData()
        {
 
            string _settingName = FileUtils.PathToSettingName(this.DataFilePath);
            try
            {
                if (Cache[_settingName] == null)
                {
                    xmlDoc.Load(HttpContext.Current.Server.MapPath(this.DataFilePath));
                    Cache.Insert(_settingName, xmlDoc, new CacheDependency(HttpContext.Current.Server.MapPath(this.DataFilePath)));
                }
                xmlDoc = (XmlDocument)Cache[_settingName];
            }
            catch (Exception ex) {
                throw ex;
            }
            return xmlDoc;
        }

        protected void ConfirmFile(string pPath) {

            if (!File.Exists(pPath) && this.BeginAtNode != "" && this.BeginAtNode != null)
            {
  
            }


        }

    }
}
