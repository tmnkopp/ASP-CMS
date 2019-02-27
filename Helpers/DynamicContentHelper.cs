using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Collections.Specialized;
using ASPXUtils;
using System.Web;
using System.Web.Compilation;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.UI.WebControls; 
namespace WebSite.Core
{
    public static class DynamicContentHelper
    {
        public static void RenderEditableRegion(Panel p, string _url)
        {
 
            if(Security.IsAdmin()){
                //Literal lit = new Literal() { ID = "", Text = "<div id='edt-" + p.ClientID + "' class='edt-tab'  ' onclick=\"openwin('" + _url + "&modal=1')\">EDIT</div>" };
                //p.Controls.AddAt(0, lit);
                //p.Attributes.Add("onmouseover", "this.style.border='6px dashed #000'; document.getElementById('edt-" + p.ClientID + "').style.display='block';");
                //p.Attributes.Add("onmouseout", "this.style.border='0px'; document.getElementById('edt-" + p.ClientID + "').style.display='none';");

                p.Attributes.Add(" onmouseover", " $(this).addClass('edit-over') ");
                p.Attributes.Add(" onmouseout", "  $(this).removeClass('edit-over') ");
                p.Attributes.Add("onclick", @"  openwin('" + _url + "&modal=1')"); 
                  
             
            }

        }
        public static string RenderEditableRegion(string _url, bool modal)
        {
            string ret = "";
            if (Security.IsAdmin())
            {
                StringBuilder SB = new StringBuilder();
                SB.Append("  onmouseover=\"$(this).addClass('edit-over')\" onmouseout=\"$(this).removeClass('edit-over') \" ");
                if(modal)
                    SB.Append(" onclick=\"openwin('" + _url + "&modal=1');\" ");
                else
                    SB.Append(" onclick=\"window.location.href='" + _url + "';\" ");
                ret = SB.ToString();
            }
            return ret;
        }
        public static string RenderEditableRegion(string _url)
        {
            return RenderEditableRegion(_url, true);
        }

        public static string InjectControls(string content){

            StringBuilder SB = new StringBuilder();
            //content = content.Replace(@"""web-control"">", @"""web-control"" >");

            Regex regex = new Regex(@"<wsc:div(.*?)data-type=""web-control(.*?)></wsc:div>", RegexOptions.IgnoreCase);
            MatchCollection myMatches = regex.Matches(content);
            if (myMatches.Count > 0) { 
               
                int currentPosition = 0;
                foreach (Match myMatch in myMatches)
                {
                    SB.Append(" " + content.Substring(currentPosition, myMatch.Index - currentPosition));
                    SB.Append(" " + ParseControlTag(myMatch.Value));
                    currentPosition = myMatch.Index + myMatch.Groups[0].Length;
                }
                SB.Append(content.Substring(currentPosition));
            }
            else 
            {
                SB.Append("" + content);
            }

            //SB.Remove(0, SB.Length);
            content = InjectLinks(SB.ToString()); 

            return content;
        }


        private static string InjectLinks(string arg){

            string content = arg;
            Regex regex = new Regex(@"<a href=""~[0-9]*~"">(.*?)</a>", RegexOptions.IgnoreCase);
            MatchCollection myMatches = regex.Matches(content);
            StringBuilder SB = new StringBuilder();
            if (myMatches.Count > 0)
            {
                int currentPosition = 0;
                foreach (Match myMatch in myMatches)
                {
                    string pageid = ParseLink(myMatch.Value) ;

                    string format = myMatch.Value.Replace(pageid,"{0}");
                    string url = GetURL(Convert.ToInt32(  pageid.Replace("~", "") ));

                    string link = String.Format(format, url);
                    SB.Append(" " + content.Substring(currentPosition, myMatch.Index - currentPosition));
                    SB.AppendFormat(" {0} ", link);
                    currentPosition = myMatch.Index + myMatch.Groups[0].Length;
                }
                SB.Append(content.Substring(currentPosition));
            }
            else 
                SB.Append("" + content);
            
            return SB.ToString(); 
        
        }

        private static string ParseLink(string atag)
        {
            int begin = atag.IndexOf("href=") + 6;
            int end = atag.IndexOf("~", begin+1) + 1  ;
            string id = atag.Substring(begin, end - begin);
        
            return id;
        }
        private static string GetURL(int id)
        {
            string ret = "";
            WebPage wp = new WebPage().Select(id);
            if(wp != null) 
                ret = wp.URL;
         
            return ret;
        }
        private static string ParseControlTag(string controltag)
        {
            string _return = "";
            string controlname = "";
            string cssclass = "";
            string itemcnt = "";
            ILinkable oLinkable;
            LinkableList list;
            XmlDocument oXmlDocument = new XmlDocument();

            controltag = controltag.Replace("wsc:div", "wscdiv");

            try   { 
                oXmlDocument.LoadXml(controltag); 
            }  catch (Exception ex)   { 
                throw ex;
            }

            XmlNode oNode = oXmlDocument.FirstChild; 

            controlname = XmlHelper.GetAttributeValue(oNode, "data-class");
            cssclass = XmlHelper.GetAttributeValue(oNode, "cssclass");
            itemcnt = XmlHelper.GetAttributeValue(oNode, "itemcnt");
 
       
            //if(controlname != ""){

            //    if (StringUtils.IsInteger(controlname))
            //    {
            //        var subpages = from wps in new WebPage().SelectSubByParent(Convert.ToInt32(controlname))
            //                          select wps.ID; 

            //         List<ILinkable> ListItems  = (from w in new WebPage().SelectLinkable()
            //                      where subpages.Contains(w.ID)  select w).ToList();

            //         list = new LinkableList() { Items = ListItems, CssClass = cssclass + " " + controlname };
            //    } else {
            //        oLinkable = NavUtil.GetLinkableByName(controlname);
            //        list = new LinkableList() { Item = oLinkable, CssClass = cssclass + " " + controlname };
            //    }
 
            //    if (StringUtils.IsInteger(itemcnt))
            //        list.Max = Convert.ToInt32(itemcnt);

            //    _return = list.GetHTML();
            //}  

            return _return; 
        }
    } 
}
