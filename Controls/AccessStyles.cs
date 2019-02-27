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
    public class AccessStyles : Control
    {
        #region Properties
 
        #endregion  
        
        protected override void Render(HtmlTextWriter writer)
        {
            if (HTTPUtils.QString("tim") != "") 
                Settings.SaveSetting("tim", "1");
            if (HTTPUtils.QString("tim") == "0")
                Settings.SaveSetting("tim", "");  
         
            writer.WriteLine("<style>");
            if (ASPXUtils.Settings.GetSetting("tim") != "")
            {
                writer.WriteLine(".fa_SiteConfig   {   visibility:visible;   } ");
            }  else {
                writer.WriteLine(".fa_SiteConfig   {   visibility:hidden; position: absolute; height: 0px;   } ");
            }

            writer.WriteLine(".ReqOnlyOmit    {  visibility:hidden; position: absolute; height: 0px;  } ");
            writer.WriteLine("");

         
            writer.WriteLine("</style>");
            base.Render(writer);  
        } 
    }
}
