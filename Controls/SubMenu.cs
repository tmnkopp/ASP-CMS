using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using ASPXUtils;
using System.Web.UI.WebControls;

namespace WebSite.Core
{
    [ToolboxData("<{0}:SubMenu runat=server></{0}:SubMenu>")]
    public class SubMenu : BaseNavMenu
    {
        #region Properties
 
        public List<ILinkable> MenuItems { get; set; } 
        public string Selected { get; set; }
        public bool AutoDetect { get; set; }
        public bool IncludeRoot { get; set; }

        private int _PWPID = 0;
        public int PWPID { 
            get{return _PWPID;} set{_PWPID=value;} 
        }
        public SystemControlType MenuItemType { get; set; }  
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);



        }
      
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            if (this.AutoDetect)
            {
                this.MenuItemType = URLTranslator.Instance.ControlType;
            }
            if (MenuItems == null)
            {
                if (this.MenuItemType.ToString() == "" || this.MenuItemType == SystemControlType.GenericPage)
                    GetWebPages(); 
            } 

            if (this.CssClass == null)
            {
                this.CssClass = "submenu";
            }

            if (MenuItems != null)
            { 
                if (MenuItems.Count > 0) 
                    this.RenderList(writer, MenuItems); 
            } 
        }  

        private void GetWebPages()
        {
            if (this.PWPID == 0)  { 
                if (StringUtils.IsInteger(HTTPUtils.QString("pwpid")))    {
                    this.PWPID = Convert.ToInt32(HTTPUtils.QString("pwpid"));
                } 
            }

            List<WebPage> SubItems = new WebPage().SelectSubByParent(this.PWPID).Where(o => o.Active == "True" && o.MainMenu=="True").ToList();

            MenuItems = new List<ILinkable>();
            if (this.IncludeRoot && SubItems.Count > 0)
            {
                if (StringUtils.IsInteger(HTTPUtils.QString("pwpid")))
                    MenuItems.Add(new WebPage().Select(Convert.ToInt32(HTTPUtils.QString("pwpid"))));
            }


            foreach (WebPage WP in SubItems)
            { 
                MenuItems.Add(WP);
            }
                
           
        } 
    }
}
