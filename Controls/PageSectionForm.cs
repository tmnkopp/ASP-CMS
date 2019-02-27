using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ASPXUtils;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace WebSite.Core 
{
    public class PageSectionForm :  CompositeControl
    {
        private ArrayList ArrSectionIds;
        private ArrayList ArrPTypeIds;
        private ArrayList ArrControlTypes;
 

        private PlaceHolder phContainer;
        private HiddenField hidSize = new HiddenField() { ID = "hidSize" };

        private bool sectionsLoaded { get; set; }
        public string _namespace = "wsc";
        public string Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }
        private string _text = "";
        public string Text {
            get { 
                if(_text=="")
                    _text = GetContent();
                return _text;
            }
            set{_text = value;}
        }
        private string _layoutPath = "~/generic.aspx";
        public string LayoutPath
        {
            get { return _layoutPath; }
            set { _layoutPath = value; }
        }       
        private void CreateControlHierarchy()
        {
            phContainer = new PlaceHolder() { ID = "phContainer" }; 
            Controls.Add(new Literal() { Text="<div id='container'>" });   
            Controls.Add( phContainer );
            Controls.Add(new Literal() { Text = "</div>" });
            phContainer.Controls.Add(hidSize);

            LoadPageSectionArray();
            LoadPageSections(); 

        }

        public string GetContent()
        {
            StringBuilder SB = new StringBuilder();
            PageSection PS;
  
            if (ArrSectionIds != null)
            {
                foreach (string id in ArrSectionIds)
                {
                    PS = (PageSection)phContainer.FindControl(id);
                    if (PS != null)
                    {
                        string value = "";
                        switch (PS.ControlType.ToString().ToLower())
                        {
                            case "textarea":
                                value = PS.Editor.Text;
                                break;
                            case "imageselect": 
                                value = String.Format("{0}", PS.oImageSelect.Value);
                                break;
                            default:
                                if (PS.Editor != null)
                                    value = PS.Editor.Text;
                                break;
                        }
                        string sSize = "250";
                        SB.AppendFormat("<{0}" + id + " size=\"" + sSize + "\"><![CDATA[ " + value + " ]]></{0}" + id + ">", this.Namespace + ":");
                    }
                }
            } else { 
                PS = (PageSection)phContainer.FindControl("center");
                if (PS != null) 
                    SB.AppendFormat("<{0}center><![CDATA[ " + PS.Editor.Text + " ]]></{0}center>", this.Namespace + ":" );
                
            } 
            return String.Format("<{0}pagesection xmlns:{1}='http://www.w3.org/'>" + SB.ToString() + "</{0}pagesection>", this.Namespace + ":", this.Namespace);
        }
        protected void LoadPageSections()
        {
            StringBuilder SB = new StringBuilder(); 
             
            if (ArrSectionIds != null)
            {
                sectionsLoaded = true;
                SB.Append(@"	<script type='text/javascript'>
	             var pageLayoutOptions = {
		        name: 'pageLayout' , closable: false ");
                for (int i = 0; i < ArrSectionIds.Count; i++)
                { 
                    string sCType = ArrControlTypes[i].ToString();
                    PageSectionControlType Ctype;
                    if (sCType != "")
                        Ctype = ArrControlTypes[i].ToString().ConvertToEnum<PageSectionControlType>();
                    else
                        Ctype = PageSectionControlType.fulleditor;
 
                    PageSection oPageSection = new PageSection()
                    {
                        ID = ArrSectionIds[i].ToString(),
                        ControlType = Ctype,
                        Edit = true,
                        PaneClass = "ui-layout-" + ArrSectionIds[i].ToString(),
                        SectionContent = this.Text
                    };
                    phContainer.Controls.Add(oPageSection);
                    oPageSection.LoadSection();
                    
                     
                    SB.Append(", " + ArrPTypeIds[i].ToString() + ": { paneSelector:\".ui-layout-" + ArrSectionIds[i].ToString() + "\", size:" + oPageSection.Size + "  } \n");
                }
                SB.Append(@" }; 
	            $(document).ready(function () { $('#container').layout(pageLayoutOptions); });
                </script>
                ");
                Page.ClientScript.RegisterStartupScript(GetType(), "pageLayoutOptions", SB.ToString());
 
                
            }
            else 
            {
                phContainer.Controls.Add(
                       new PageSection() { ID = "center", Edit = true, PaneClass = "ui-layout-" , SectionContent =  this.Text }
                    ); 
            }  
            
        }
        protected void LoadPageSectionArray()
        {
            ArrSectionIds = (ArrayList)ViewState["PageSectionArray"];
            ArrPTypeIds = (ArrayList)ViewState["PaneTypeArray"];
            ArrControlTypes = (ArrayList)ViewState["PaneCTArray"];

            if ((ArrSectionIds == null || ArrPTypeIds == null) && (this.LayoutPath != "" && this.LayoutPath != null))
            {
                TextReader tr = new StreamReader(HttpContext.Current.Server.MapPath(this.LayoutPath));
                string content = tr.ReadToEnd();
                tr.Close();

                Regex regex = new Regex(@"<WebCore:PageSection id=""(?<id>\w*?)""(.*?)>(.*?)</WebCore:PageSection>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                MatchCollection myMatches = regex.Matches(content);

                if (myMatches.Count > 0)
                {
                    ArrSectionIds = new ArrayList(myMatches.Count);
                    ArrPTypeIds = new ArrayList(myMatches.Count);
                    ArrControlTypes = new ArrayList(myMatches.Count);
                    foreach (Match myMatch in myMatches)
                    { 
                        string match = myMatch.ToString().Replace("WebCore:","");
                        string sId = XmlHelper.GetAttributeValue(match, "id");
                        string sPaneType = XmlHelper.GetAttributeValue(match, "PaneType");
                        string sControlType = XmlHelper.GetAttributeValue(match, "ControlType");
                        ArrSectionIds.Add(sId);
                        ArrPTypeIds.Add(sPaneType);
                        ArrControlTypes.Add(sControlType); 
                    }
                }
                ViewState.Add("PageSectionArray", ArrSectionIds);
                ViewState.Add("PaneTypeArray", ArrPTypeIds);
                ViewState.Add("PaneCTArray", ArrControlTypes);
                
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            StringBuilder SB = new StringBuilder();
            string scripts = "jquery-1.3.2.min.js|jquery-ui-latest.js|jquery.layout-latest.js";
            foreach (string script in scripts.Split('|'))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), script,
                 "<script type=\"text/javascript\" src=\"/js/" + script + "\"></script>");
            }
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "layout-default-latest.css",
            "<link type=\"text/css\" rel=\"stylesheet\" href=\"/css/layout-default-latest.css\" />");

            if (this.sectionsLoaded)
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "layout-default-styles",
                    @" 
	                <style type='text/css'>
	                html, body {
		                background:	#666;  width:100%; height:100%; padding:	0; margin:	0; overflow:auto;  
	                }
	                #container {
		                background:	#999; height:		900px; margin:		0 auto; width:		100%; max-width:	1200px; min-width:	1000px; _width:		1000px;  
	                }
	                .pane { 	display:	none;  }
	                #contentdiv    {  width:1200px;  }
	                </style>
                    ");
            }
        }
        protected override  void CreateChildControls()
        {
            Controls.Clear();
            CreateControlHierarchy();
        }
        public override ControlCollection Controls
        {
            get { EnsureChildControls(); return base.Controls; }
        }
        
    }

}
