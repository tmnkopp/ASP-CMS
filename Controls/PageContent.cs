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

    [ToolboxData("<{0}:PageContent runat=server></{0}:PageContent>")]
    public class PageContent : BaseControl
    {
        #region Properties
        public string CssClass { get; set; }
        public string EditFormPath { get; set; }
        #endregion  
  
        protected override void Render(HtmlTextWriter writer)
        {
            
            if (WebPage.Instance == null)
                return;
            if (EditFormPath == null)
                EditFormPath = SiteSettings.Instance.EditWebPagePath;

            string editPath = String.Format(EditFormPath, WebPage.Instance.ID.ToString());
            editPath = DynamicContentHelper.RenderEditableRegion(NavUtil.URLRoot() + editPath);

            string content = String.Format("<div class='page-content {0}' {2}>{1}</div>", CssClass, Utils.RenderContent(WebPage.Instance), editPath);

            writer.WriteLine(content);
            base.Render(writer);  
        } 
    }