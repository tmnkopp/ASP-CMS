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
using System.ComponentModel;
using System.Xml;
using ASPXUtils;
using ASPXUtils.Controls;


namespace WebSite.Core 
{

    /// <summary> 
        /// <WebCore:PageSection  id="center" runat="server">
        /// <ItemTemplate></ItemTemplate>
        /// </WebCore:PageSection> 
    /// </summary>

    public enum PageSectionControlType
    {
        textarea,
        imageselect,
        fulleditor
    }

    public class PageSection : CompositeControl
    {
        public PlaceHolder ControlHolder;
        public PageSection()   : base()
        {
            ControlHolder = new PlaceHolder { ID = "ControlHolder" };
        }

        private ITemplate itemTemplate;
        [Browsable(false), Description("Item template"),
        PersistenceMode(PersistenceMode.InnerProperty),
        TemplateContainer(typeof(BasicTemplateContainer))]
        public ITemplate ItemTemplate
        {
            get { return itemTemplate; }
            set { itemTemplate = value; }
        }
        private ITemplate iheadTemplate;
        [Browsable(false), Description("Head template"),
        PersistenceMode(PersistenceMode.InnerProperty),
        TemplateContainer(typeof(BasicTemplateContainer))]
        public ITemplate HeadTemplate
        {
            get { return iheadTemplate; }
            set { iheadTemplate = value; }
        }
        private ITemplate ifootTemplate;
        [Browsable(false), Description("Foot template"),
        PersistenceMode(PersistenceMode.InnerProperty),
        TemplateContainer(typeof(BasicTemplateContainer))]
        public ITemplate FootTemplate
        {
            get { return ifootTemplate; }
            set { ifootTemplate = value; }
        }

        private string _pane = "";
        public string Pane
        {
            get  {
                if (_pane == "") 
                    _pane = this.ID; 
                return _pane;
            }
            set { _pane = value; }
        } 
        
        public string _SectionContent =""; 
        public string SectionContent {
            get { 
                if(_SectionContent==""){
                    if (WebPage.Instance != null)
                        _SectionContent = WebPage.Instance.PageContent;
                }
                return _SectionContent; 
            }
            set { _SectionContent = value; } 
        }
        public string _paneSelector = "";
        public string PaneSelector
        {
            get { return _paneSelector; }
            set { _paneSelector = value; } 
        }
        public string _paneType = "";
        public string PaneType
        {
            get { return _paneType; }
            set { _paneType = value; }
        }
        public PageSectionControlType ControlType { get; set; }
 
        public string _namespace = "wsc:";
        public string Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }
        public string _divID = "";
        public string DivID
        {
            get {
                if(_divID!="")
                    return _divID; 
                else
                    return this.ID; 
            }
            set { _divID = value; }
        }
        public string _size = "200";
        public string Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public int EditorWidth { get; set; }
        public int EditorHeight { get; set; }

        public bool Edit { get; set; }
        public string PaneClass { get; set; }
        public CKEditor Editor;
        public ImageSelect oImageSelect;

        private string _formattedContent = "";
        public void LoadSection() {

            if (_formattedContent != "")
                return;

            if (SectionContent == null)
                SectionContent = "";

            XmlDocument oTempXmlDoc = new XmlDocument();
            XmlAttribute xAttns = oTempXmlDoc.CreateAttribute("xmlns:wsc");
            xAttns.Value = "http://www.w3.org/";
            XmlNamespaceManager namespaces = new XmlNamespaceManager(oTempXmlDoc.NameTable);
            namespaces.AddNamespace("wsc", "http://www.w3.org/");
            SectionContent = SectionContent.Trim();

             

            if (SectionContent.Contains(String.Format("</{0}" + this.Pane.ToString().ToLower() + ">", this.Namespace)))
            {  
                try {
                    oTempXmlDoc.LoadXml(SectionContent);
                    XmlNode oNode = oTempXmlDoc.SelectSingleNode(String.Format("//{0}" + this.Pane.ToString().ToLower(), this.Namespace), namespaces);
                    _formattedContent = oNode.InnerText;

                    if(oNode.Attributes["size"] != null) 
                        this.Size = oNode.Attributes["size"].Value;  
                }
                catch(Exception ex)
                {
                    HttpContext.Current.Response.Write(HttpContext.Current.Server.HtmlEncode(SectionContent) + "<Br>");
                    HttpContext.Current.Response.Write(ex.Message + "<Br>");
                    HttpContext.Current.Response.Write(ex.Source + "<Br>");
                }

            }
            if (_formattedContent == "" && this.Pane == String.Format("{0}center", this.Namespace))
                _formattedContent = SectionContent;

            if (!Edit && this.ControlType == PageSectionControlType.imageselect) {
                _formattedContent = String.Format("<img src=\"{0}\" >", _formattedContent);
            } 
    
        
        } 

        private void CreateControlHierarchy()
        { 
            Controls.Add(ControlHolder);
            LoadSection();

            if (HeadTemplate != null)
            {
                BasicTemplateContainer item = new BasicTemplateContainer();
                HeadTemplate.InstantiateIn(item); 
                ControlHolder.Controls.Add(item);  
            } 
            if (this.Edit)
            {

                switch ( this.ControlType.ToString().ToLower())
	            {
                   case "textarea":
                        Editor = new CKEditor() { ID = "CKEDIT", Text = _formattedContent, Toolbar="Basic" }; 
                        Editor.Height = 400;
                        Editor.Width = 300;
                        Controls.Add(Editor);
                        break;
                   case "imageselect":
                        oImageSelect = new ImageSelect() { Value = _formattedContent };
                        Controls.Add(oImageSelect);
                        break;
                   default:                
                        Editor = new CKEditor() { ID = "CKEDIT", Text = _formattedContent };
                        if (this.EditorWidth > 0)
                            Editor.Width = this.EditorWidth;
                        if (this.EditorHeight > 0)
                            Editor.Height = this.EditorHeight;

                        Editor.Height = 400;
                        Editor.Width = 700;
                        Controls.Add(Editor);
                        break;
	            } 
            }
            else
            {
                Controls.Add(new Literal() { Text = _formattedContent });
            }
            if (FootTemplate != null)
            {
                BasicTemplateContainer item = new BasicTemplateContainer();
                FootTemplate.InstantiateIn(item);
                ControlHolder.Controls.Add(item);
            }
        } 

        public class BasicTemplateContainer : WebControl, INamingContainer
        {
            public BasicTemplateContainer()
                : base("") { }
            protected override void Render(System.Web.UI.HtmlTextWriter writer)
            { 
                foreach (Control childControl in Controls)
                    childControl.RenderControl(writer); 
            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        { } 
        public override void RenderEndTag(HtmlTextWriter writer)
        { }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteLine("<div id=\"" + this.DivID + "\" class=\"pane " + this.PaneClass + "\">");
            foreach (Control childControl in Controls)
                childControl.RenderControl(writer);
            writer.WriteLine("</div>");
        }
        override protected void CreateChildControls()
        {
            Controls.Clear();
            CreateControlHierarchy();
        }
        public override ControlCollection Controls
        {
            get { EnsureChildControls(); return base.Controls; }
        }
        public override void DataBind()
        {
            CreateChildControls();
            ChildControlsCreated = true;
            base.DataBind();
        }
    }

}
