using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI; 
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using ASPXUtils;
using ASPXUtils.Controls;
 
namespace WebSite.Core
{
    public abstract class BaseControl : UserControl
    {

        public string _rp = HTTPUtils.QString("rp");
        public string _wpid = HTTPUtils.QString("wpid");
        public string _cid = HTTPUtils.QString("cid");
        public string _id = HTTPUtils.QString("id");
        public string _mt = HTTPUtils.QString("mt");
        public string _yr = HTTPUtils.QString("yr");
        protected override void OnInit(EventArgs e)
        { 
            base.OnInit(e);
        }     
    }
}

