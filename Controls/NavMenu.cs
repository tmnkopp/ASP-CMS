using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using ASPXUtils;
using System.Text.RegularExpressions;
namespace WebSite.Core
{

    [ToolboxData("<{0}:NavMenu runat=server></{0}:NavMenu>")]
    public class NavMenu : BaseNavMenu
    {
        #region Properties
        public NavStyleType NavType { get; set; }
        public bool RenderAllSubMenus { get; set; }
        public bool RenderSelectedSubMenu { get; set; }
        public bool IncludeHome { get; set; }
        public bool Selected { get; set; }
        #endregion

        //http://nettuts.s3.amazonaws.com/699_nav/navCode/nav.html#
        //http://demos.99points.info/fancy_menus/
        //http://demo.lateralcode.com/jquery-menu/
        //http://mediatemple.net/company/contact.php

       
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            List<WebPage> WebPages = new WebPage().SelectAllMain().Where(o => o.MainMenu == "True" && o.Active == "True").ToList();

            if (!this.IncludeHome)
                this.IncludeHome = SiteSettings.Instance.IncludeMenuHome;

            if (this.CssClass==null)  
                this.CssClass = "mainmenu";
             
            writer.WriteLine("<ul class=\"" + this.CssClass + "\">");
            if (this.IncludeHome)
            {
                writer.Write("<li><a href='" + NavUtil.URLRoot() + ""+SiteSettings.Instance.NavRootDocumentName+"'>Home</a></li>");
                if (this.Seperator != null)
                    writer.Write(this.Seperator);
            }

            string sel = "";
            string url = "";
            int count = 0;
            foreach (WebPage WP in WebPages)
            {
                count++;
                this.Selected = this.IsSelected("/" + WP.Name);
                if (this.Selected)
                    sel = "sel";

                writer.Write("<li id=\"\" class=\"" + sel + " item-" + count + "\">");

                url = WP.URL;
                if (WP.ExternalURL  != "")
                    url = WP.ExternalURL;
                //if (WP.SystemURL != "")
                //    url = WP.SystemURL;

                if (WP.SubCaption != "")
                    WP.MenuCaption = string.Format("{0}<span class='nav-sub-caption'>{1}</span>", WP.Caption, WP.SubCaption);

                RenderMenuItem(writer, url, WP.Caption, sel);



                if (this.NavType == NavStyleType.SubInline)
                {
                    if (this.RenderAllSubMenus || (this.Selected && this.RenderSelectedSubMenu))
                    {
                        GetChildren(writer, WP);
                    } 
                } 

 
                writer.Write("</li>");


                if (this.Seperator != null && count < WebPages.Count)
                    writer.Write(this.Seperator);

                sel = "";
                
            }
            writer.WriteLine("</ul>");

            if (this.NavType == NavStyleType.SubBottom)
            { 
                foreach (WebPage WP in WebPages)
                {
                    this.Selected = this.IsSelected("/" + WP.Name);

                    if (this.RenderAllSubMenus || (this.Selected && this.RenderSelectedSubMenu))
                    {
                        GetChildren(writer, WP);
                    }  
                } 
            }  
        }
        private void GetChildren(HtmlTextWriter writer, WebPage WP) {
 
            SubMenu SM = new SubMenu();
            SM.PWPID = WP.ID;
            if (WP.SystemControl == "")
            {
                SM.MenuItemType = SystemControlType.GenericPage;
            }
            //else
            //{
            //    if (new SystemControl().SelectByClass(WP.SystemControl).ShowSubMenu)
            //    {
            //        SM.MenuItems = NavUtil.GetLinkableByName(WP.SystemControl).SelectLinkable();
            //    } 
            //} 
            SM.RenderControl(writer);
        }

        protected override bool IsSelected(string arg)
        { 
            bool ret = false;
            arg = arg.ToLower();

            Regex regex = new Regex("^" + arg + @"(/|\.)", RegexOptions.IgnoreCase);
            Match match = regex.Match(NavUtil.RequestPath.ToLower());
            if (match.Success) 
                ret = true;

            if (!ret)
            {
                regex = new Regex("^" + arg + @"-([A-Za-z0-9]*)/", RegexOptions.IgnoreCase);
                match = regex.Match(NavUtil.RequestPath.ToLower());
                if (match.Success)
                    ret = true;
            }

                
            return ret;
        }
