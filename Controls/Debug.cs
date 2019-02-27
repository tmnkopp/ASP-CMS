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

    [ToolboxData("<{0}:Debug runat=server></{0}:Debug>")]
    public class Debug : Control
    {
        #region Properties
        public bool ShowDebug{get;set;}
        #endregion  
        protected override void OnInit(EventArgs e)
        {
            if (HTTPUtils.QString("debug") != "")
                Settings.SaveSetting("debug", "1");
            if (HTTPUtils.QString("debug") == "0")
                Settings.SaveSetting("debug", "");
            this.Visible = false;
            if ( Settings.GetSetting("debug") != "") 
                ShowDebug = true;

            this.Visible = ShowDebug;
          
            base.OnInit(e);
        }
        protected override void Render(HtmlTextWriter writer)
        { 
            Page.Trace.IsEnabled = ShowDebug; 
            base.Render(writer);  
        } 
    }
}
