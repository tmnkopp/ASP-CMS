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

    [ToolboxData("<{0}:LanguageSelect runat=server id=oLanguageSelect></{0}:LanguageSelect>")]
    public class LanguageSelect : Control
    {
        #region Properties 
        public string CssClass { get; set; }
        public string AltLangCaption { get; set; }
        public string MainLangCaption { get; set; }
        public string Seperator { get; set; }
        public StringBuilder HTML = new StringBuilder();
        #endregion  
  
        protected override void OnLoad(EventArgs e)
        {

            

            if (this.MainLangCaption == null) 
                this.MainLangCaption = "English";
            if (this.AltLangCaption == null)
                this.AltLangCaption = "Spanish";
            if (this.Seperator == null)
                this.Seperator = "";
            HTML.AppendFormat("<span class='lang-select'>", "");
            HTML.AppendFormat("<a href='{0}?lng=en'>{1}</a>", Globals.GetRequestPage(), this.MainLangCaption);
            HTML.AppendFormat("{0}", this.Seperator.ToString());
            HTML.AppendFormat("<a href='{0}?lng=es'>{1}</a>", Globals.GetRequestPage(), this.AltLangCaption);
            HTML.AppendFormat("</span>", "");
            base.OnLoad(e);
        }   
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(HTML.ToString());
            base.Render(writer);  
        } 
    }
}
