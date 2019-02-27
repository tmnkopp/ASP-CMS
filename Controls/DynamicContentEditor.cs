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
using ASPXUtils.Controls;

namespace WebSite.Core 
{
   /*
    Depencanies
    <script type='text/javascript' src='/js/ckeditor/ckeditor.js'></script> 
    */
    [ToolboxData("<{0}:DynamicContentEditor runat=Server></{0}:DynamicContentEditor>")]

    public class DynamicContentEditor : UserControl
    {

        private int _itemCount = 0;
        private string _formElements = "";


        private string _filePath = "";
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        private bool _closeVisible = false;
        public bool CloseVisible
        {
            get { return _closeVisible; }
            set { _closeVisible = value; }
        } 

        private string _beginAtNode = "";
        public string BeginAtNode
        {
            get
            {
                if (_beginAtNode == "" || _beginAtNode == null)
                {
                    XmlDocument oXmlDocument = new XmlDocument();
                    oXmlDocument.Load(this.AbsoluteFilePath);
                    _beginAtNode = oXmlDocument.FirstChild.Name;
                }
                return _beginAtNode;
            }
            set { _beginAtNode = value; }
        }

        public string AbsoluteFilePath
        {
            get { return HttpContext.Current.Server.MapPath(this.FilePath); }
        }
        public string EditorView { get; set; }

        Table tbl = new Table();
        PlaceHolder pnlForm = new PlaceHolder();
        Button cmdSave = new Button(){ Text = "Save" };
        HtmlInputButton cmdClose = new HtmlInputButton() { Value = "Close Window" };
        Button cmdSaveClose = new Button() { Text = "Save And Close" };
        Literal litScripts = new Literal();

        protected void Page_Load(object sender, EventArgs e)
        {

            this.EditorView = HttpContext.Current.Request.QueryString["view"];


            this.FilePath = HttpContext.Current.Request.QueryString["u"];
            this.BeginAtNode = HttpContext.Current.Request.QueryString["ban"]; 

            cmdSave.Click += new EventHandler(cmdSave_Click);
            cmdSaveClose.Click += new EventHandler(cmdSaveClose_Click);
            cmdClose.Attributes.Add("onclick", "window.opener.location.reload(true); window.close();");

        
            pnlForm.Controls.Add(cmdSave);
            pnlForm.Controls.Add(cmdClose);
            pnlForm.Controls.Add(cmdSaveClose);
            pnlForm.Controls.Add(new Literal(){Text="<br />"});
            Controls.Add(pnlForm); 

            Controls.Add(litScripts);
          
        }

        protected void cmdSave_Click(object sender, EventArgs e)  {

            Save();
            Response.Redirect(Request.RawUrl); 

        }
        protected void cmdSaveClose_Click(object sender, EventArgs e)  {

            Save();
            Response.Write("<script>window.opener.location.reload(true);  window.close();</script>");

        }

        private void Save()
        {
            XmlDocument oXmlDocument;
            _itemCount = 0;
           
            string[] _elements = (string[])ViewState["Elements"].ToString().Split('$');

            oXmlDocument = new XmlDocument();
            oXmlDocument.Load(this.AbsoluteFilePath);
            XmlNode oRootNode = oXmlDocument.SelectSingleNode("//" + this.BeginAtNode);

            if (XmlHelper.GetAttributeValue(oRootNode, "multi") == "true")
            {
                for (int i = 0; i < oRootNode.ChildNodes.Count; i++) {
                    XmlNode oNode = oRootNode.ChildNodes[i];
                    SaveField(ref oNode);  
                } 
            }
            else
                SaveField(ref oRootNode);
            
            oXmlDocument.Save(this.AbsoluteFilePath);
        }
        private void SaveField(ref  XmlNode oRootNode)
        { 
            string _val = "";
            string _controlType = ""; 
            foreach (XmlNode oFormElement in oRootNode.ChildNodes)
            {
                _val = RequestForm(oFormElement.Name + _itemCount).Trim();
                _controlType = XmlHelper.GetAttributeValue(oFormElement, "controltype");
                if (_controlType == "textbox-multi")
                    _val = _val.Replace("\r", "<br />");

                _val = HttpContext.Current.Server.HtmlDecode(_val);
                oFormElement.InnerXml = "<![CDATA[" + _val + "]]>";
                _itemCount++;
            }

        }

        private void Page_PreRender(object sender, System.EventArgs e)
        { 
            CreateForm();
        }

        private void CreateForm()
        {
            XmlDocument oXmlDocument = new XmlDocument();
            try { 
                oXmlDocument.Load(this.AbsoluteFilePath);
            }
            catch (Exception ex) 
            {
                Response.Write(this.AbsoluteFilePath + "<br>");
                Response.Write(ex.Message + "<br>");
                Response.Write(ex.Source + "<br>"); 
                Response.Write(this.ID + "<br>");
                Response.Write(this.ClientID + "<br>");
                return;
            }
            
            XmlNode oRootNode = oXmlDocument.SelectSingleNode("//" + this.BeginAtNode);

            
            tbl.ID = "tblForm";

             _itemCount = 0;
            _formElements = "";

            if(XmlHelper.GetAttributeValue(oRootNode, "multi")== "true")
            {
                foreach (XmlNode oFormElement in oRootNode.ChildNodes)
                    CreateField(oFormElement);
          
            }else{
                CreateField(oRootNode);
            } 

            ViewState["ItemCount"] = Convert.ToString(_itemCount);
            ViewState["Elements"] = _formElements.Substring(0, _formElements.Length - 1);

            pnlForm.Controls.Add(tbl);
 
        }

        private void CreateField(XmlNode oRootNode)
        {
            string _controlType = "";
            string _val = "";
            foreach (XmlNode oFormElement in oRootNode.ChildNodes)
            {
                 _formElements += oFormElement.Name + _itemCount.ToString() + "$";

                _val = oFormElement.InnerText;
                _controlType = XmlHelper.GetAttributeValue(oFormElement, "controltype");
                if (_controlType == "textbox-multi")
                    _val = _val.Replace("<br />", "\r");

                ConvertElementToField(oFormElement, _itemCount, _val);
                _itemCount++;
            }
    
        }
        private void ConvertElementToField(XmlNode oFormElement, int _itemcnt, string _defaultValue)
        {
            string _controltype = "";
            string _controlHeight = "100";
            string _cssClass = "";
            if (oFormElement.Attributes != null)
            {
                if (oFormElement.Attributes["controltype"] != null)
                    _controltype = (string)oFormElement.Attributes["controltype"].Value;
                if (oFormElement.Attributes["control-h"] != null)
                    _controlHeight = (string)oFormElement.Attributes["control-h"].Value;
                if (oFormElement.Attributes["cssclass"] != null)
                    _cssClass = (string)oFormElement.Attributes["cssclass"].Value;
            }

            Control cntl;
            string _id = oFormElement.Name + _itemcnt;
            switch (_controltype.ToLower())
            {

                case "textbox-multi":
                    cntl = new TextBox()
                    {
                        ID = _id,
                        Text = _defaultValue,
                        TextMode = TextBoxMode.MultiLine,
                        CssClass = _cssClass,
                        Height = Unit.Parse(_controlHeight),
                        Width = 500
                    };
                    break;
                case "imageselect":
                    cntl = new ImageSelect
                    {
                        ID = _id, 
                        Value = _defaultValue
                    };


                    break;
                case "ckeditor":

                    if (_controlHeight != "")
                    {
                       if (Convert.ToInt32(_controlHeight) < 100)
                            _controlHeight = "400";
                    }
                    cntl = new CKEditor
                           {
                               ID = _id,
                               Height = Unit.Parse(_controlHeight),
                               Width = Unit.Parse( SiteSettings.Instance.CKEditWidth ),
                               Rows = 60,
                               Text = _defaultValue
                           };

                    break;

                case "ckeditor-basic":

                    if (_controlHeight != "")
                    {
                        if (Convert.ToInt32(_controlHeight) < 100)
                            _controlHeight = "400";
                    }
                    cntl = new CKEditor
                    {
                        ID = _id,
                        Toolbar="Basic", 
                        Height = Unit.Parse(_controlHeight),
                        Width = Unit.Parse(SiteSettings.Instance.CKEditWidth),
                        Rows = 60,
                        Text = _defaultValue
                    };

                    break;


                default:
                    cntl = new TextBox()
                    {
                        ID = _id,
                        Text = _defaultValue,
                        Width = 500
                    };
                    break;
            }

            AddFormRow(
                 new Label() { ID = "lbl" + _id, Text = StringUtils.Pcase(oFormElement.Name.ToString().Replace("_", " ")) }
                 ,
                    cntl
                 );

        }

 

        private void AddFormRow(Control ctrl1, Control ctrl2)
        {
            TableRow tr;
            TableCell td;
            tr = new TableRow();
            td = new TableCell();
            td.VerticalAlign = VerticalAlign.Top;
            td.Font.Bold = true;
            td.Style.Add("padding-top", "4px");
            td.Style.Add("font-size", "1.2em");
            td.Controls.Add(ctrl1);
            td.Controls.Add(new Literal() { Text = "<br />" });
            td.Controls.Add(ctrl2);
            tr.Cells.Add(td);
           
            tbl.Rows.Add(tr);
        }
 

        public string RequestForm(string sControlID)
        {
            string _return = "";
            string[] _keyParts;
            char _splitChar = '$';
            if (sControlID == "" || sControlID == null)
                return _return;

            foreach (string key in HttpContext.Current.Request.Form.Keys)
            {
                _splitChar = '$';
                if (key.StartsWith("_content_"))
                    _splitChar = '_';

                _keyParts = key.Split(_splitChar);
                for (int i = _keyParts.Length - 1; i >= 0; i--)
                {
                    if (_keyParts[i] == sControlID)
                    {
                        _return = HttpContext.Current.Request.Form[key];
                        break;
                    }
           