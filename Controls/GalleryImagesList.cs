using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ASPXUtils;
using System.Web;
using System.Web.UI.WebControls;
using System.Diagnostics;
using ASPXUtils.Controls;
using System.Configuration;
namespace WebSite.Core 
{
    public class GalleryImagesList : CompositeControl
    {
        
        public string LinkFormat { get; set; }
        public List<GalleryImage> GalleryImages { get; set; } 
        private string ent_type = "GalleryImagesList";
        public int CATID;
        Literal ulImages = new Literal() { ID = "ulImages" };
        GalleryImageUploader oUploader;
        public bool ShowUploader = true;
        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            StringBuilder SB = new StringBuilder();
             
            if(StringUtils.IsInteger(HTTPUtils.QString("id"))){
                CATID = Convert.ToInt32(HTTPUtils.QString("id"));
            }
            
            if (CATID > 0)
            { 
                GalleryCategory GC = new GalleryCategory().Select(CATID);
                if (GC!= null) 
                    GalleryImages = new GalleryCategory().Select(CATID).GalleryImages; 
            }

           

            if (GalleryImages != null)
            {
                if (this.LinkFormat == null)
                    this.LinkFormat = "/admin/GalleryImages/Default.aspx?id={0}&cid={1}";
                SB.Append("<div class='begin-GalleryImagesList'></div>");
                SB.Append("<ul class='" + ent_type + " " + this.CssClass + "'>");
                foreach (GalleryImage item in GalleryImages)
                {
                    this.CATID = item.CATID;
                    string url = string.Format(this.LinkFormat, item.ID.ToString(), item.CATID.ToString()  );
                    string listitem = string.Format("<li><a href='{0}' ><img src='{1}' style='border:0px;' /></a></li>"
                        , url, item.VirPath);
                    SB.Append(listitem);
                }
                SB.Append("</ul>");
                SB.Append("<div class='end-GalleryImagesList'></div>");
            }
            ulImages.Text = SB.ToString(); 

           
        }
        protected override void CreateChildControls()
        { 
            Controls.Clear();
            this.Controls.Add(ulImages);

            int iFileCnt = 5;
            if (StringUtils.IsInteger(SiteSettings.Instance.ImgUploadCnt))
                iFileCnt = Convert.ToInt32(SiteSettings.Instance.ImgUploadCnt);
            
 
            if (StringUtils.IsInteger(HTTPUtils.QString("id")))
                CATID = Convert.ToInt32(HTTPUtils.QString("id"));
            if (CATID > 0 && ShowUploader)
            {
                oUploader = new GalleryImageUploader() { CATID = CATID, ShowUpload = true };
                oUploader.SavePath = ConfigurationManager.AppSettings["GalleryPath"] + CATID.ToString()+"/";
                oUploader.CreateThumb = true;
                oUploader.FileCount = iFileCnt;
                this.Controls.Add(oUploader);
            }

        } 
         
    }
}
