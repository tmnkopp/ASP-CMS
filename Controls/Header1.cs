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

    [ToolboxData("<{0}:Header1 runat=server></{0}:Header1>")]
    public class Header1 : Control
    {
        #region Properties
        public string CssClass { get; set; }
        #endregion  
  
        protected override void Render(HtmlTextWriter writer)
        {
            if (WebPage.Instance == null)
                return;

            string header = WebPage.Instance.Header;
            if (SiteSettings.Instance.HeaderFormat != "")
                header = string.Format(SiteSettings.Instance.HeaderFormat, WebPage.Instance.Header);

            writer.WriteLine(String.Format("<h1 class='page-title {0}'>{1}</h1>", CssClass, header));
            base.Render(writer);  
        } 
    }
}
