using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WebSite.Core 
{
    public class SocialIcons : Control
    {
        #region Properties
        #endregion
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            writer.WriteLine("<style> ");
            writer.WriteLine(" ");
            writer.WriteLine(" .facebook{background:url('/images/social-network-icon.png') no-repeat  -31px -5px }");
            writer.WriteLine(" .twitter{background:url('/images/social-network-icon.png') no-repeat   -31px -42px}");

            writer.WriteLine(" .linkedin{background:url('/images/social-network-icon.png') no-repeat -31px -77px  }");
            writer.WriteLine(" .rss{background:url('/images/social-network-icon.png') no-repeat   -31px  -112px }");
            writer.WriteLine("</style>");

            if (SiteSettings.Instance.FacebookLink != null && SiteSettings.Instance.FacebookLink != "")
                writer.WriteLine("<a href=\"" + SiteSettings.Instance.FacebookLink + "\" target='_blank' class=\"social-icon facebook\">&nbsp;</a>");
            if (SiteSettings.Instance.TwitterLink != null && SiteSettings.Instance.TwitterLink != "")
                writer.WriteLine("<a href=\"" + SiteSettings.Instance.TwitterLink + "\"  target='_blank' class=\"social-icon twitter\">&nbsp;</a>");
            if (SiteSettings.Instance.LinkedInLink != null && SiteSettings.Instance.LinkedInLink != "")
                writer.WriteLine("<a href=\"" + SiteSettings.Instance.LinkedInLink + "\"  target='_blank' class=\"social-icon linkedin\">&nbsp;</a>");
            if (SiteSettings.Instance.RssLink != null && SiteSettings.Instance.RssLink != "")
                writer.WriteLine("<a href=\"" + SiteSettings.Instance.RssLink + "\"  target='_blank' class=\"social-icon rss\">&nbsp;</a>");

        }
    }

}
