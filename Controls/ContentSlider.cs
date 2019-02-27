using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.IO;
using ASPXUtils;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
namespace WebSite.Core 
{

    public enum SlideType{
       SingleImage,
       ImageSlider,
       FadingSlideshow,
       jcarousel
    }
 

    public class ContentSlider : Control 
    {
        public SlideType SliderType { get; set; }
        public int SlideDuration { get; set; }
        private string slideDuration= "5000";
        public string ImageIDParam { get; set; }
        public string GalleryIDParam { get; set; } 
        public int UseGalleryID { get; set; }
        public int UseImageID { get; set; }
        public string UseGalleryName { get; set; }
        List<GalleryImage> Images;
        bool HasImages = false;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Easing { get; set; }
        public bool AutoDetect { get; set; }
        public bool JqueryExclude { get; set; }
        string StyleSheetInclude = "<link rel='stylesheet' text='text/css' href='{0}' />";
        protected override void OnInit(EventArgs e) {
            //WebSite.Core.jquery-1.3.2.min.js
 
            base.OnInit(e);
            if (JqueryExclude)
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery",
                   Page.ClientScript.GetWebResourceUrl(this.GetType(), "WebSite.Core.Resources.js.jquery-latest.min.js"));    
            }
       
          


            if (this.Easing != null && this.Easing != "")
            {
                Page.ClientScript.RegisterClientScriptInclude("jquery-easing",
                    Page.ClientScript.GetWebResourceUrl(this.GetType(), "WebSite.Core.Resources.js.jquery-easing.js"));
            }

            this.Visible = !this.AutoDetect;
            if (AutoDetect)
            {
                if (StringUtils.IsInteger(HTTPUtils.QString("csimid")))
                {
                    this.Visible = true;
                    this.UseImageID = Convert.ToInt32(HTTPUtils.QString("csimid"));
                    this.SliderType = SlideType.SingleImage;
                }
            }

            slideDuration = "1200";
            if (SlideDuration > 1)
                slideDuration = SlideDuration.ToString();

            if (this.UseImageID > 0)
                this.SliderType = SlideType.SingleImage;
            if (this.Width < 1)
                this.Width = Convert.ToInt32(SiteSettings.Instance.MaxImageSize);
            if (this.SliderType == null)
                this.SliderType = SlideType.ImageSlider;

            
        }
        protected override void OnLoad(EventArgs e)
        { 
            base.OnLoad(e); 
            Page.ClientScript.RegisterClientScriptInclude("sliderjs",
            Page.ClientScript.GetWebResourceUrl(this.GetType(), string.Format("WebSite.Core.Resources.{0}.core.js", this.SliderType.ToString())));

            string includeTemplate = StyleSheetInclude;
            string includeLocation = Page.ClientScript.GetWebResourceUrl(this.GetType(), string.Format("WebSite.Core.Resources.{0}.core.css", this.SliderType.ToString()));
            LiteralControl include =   new LiteralControl(String.Format(includeTemplate, includeLocation));
                ((HtmlHead)Page.Header).Controls.Add(include);

        }


        protected override void Render(HtmlTextWriter writer)
        { 
            LoadMain();
            
            if (this.HasImages)
            {
                string main = "";
                if (this.SliderType.ToString() == "SingleImage")
                    main = GetSingleImage();
                if (this.SliderType.ToString() == "ImageSlider")
                    main = GetImageSlider();
                if (this.SliderType.ToString() == "FadingSlideshow")
                    main = GetFadingSlideshow();
                if (this.SliderType.ToString().ToLower() == "jcarousel")
                    main = GetJCarousel();
   
                writer.Write(main); 
                base.Render(writer);

            }
            else {
                writer.Write("<!--  ContentSlider: no content images -->"); 
                return ;
            } 
        }
        private string GetSingleImage() {

             
            return String.Format(@"<div style='background:url({0}) ;
                                        width:{1}px;height:{2}px;' class='content-slider-single'>  
                 
                        {3}
                   
             </div> ", Images[0].AbsPath, this.Width, this.Height, DynamicContentHelper.InjectControls(  Images[0].HTMLContent ) );
            
        }
        private string GetJCarousel() {

            if (this.Easing == null)
                this.Easing = "";

            StringBuilder sb_imagereel = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            
            sb.AppendFormat("<!--  begin JCarousel-->");
            if (this.Width > 0)
                sb.AppendFormat("<style>.jcarousel-skin .jcarousel-container-horizontal {{  width: {0}px; !important; }} .jcarousel-skin .jcarousel-item {{width: {0}px !important; }}\n .window {{width: {0}px !important;}}</style>", Convert.ToString(this.Width));
           
            if (this.Height > 0)
                sb.AppendFormat("<style>.jcarousel-skin .jcarousel-item {{height: {0}px !important; }}\n  .window {{height: {0}px !important;}}</style>", Convert.ToString(this.Height));
            string catid = "";
            foreach (GalleryImage img in Images)
            {
                catid = img.CATID.ToString();
                sb_imagereel.AppendFormat(@"  <li  style='background:url({0}) no-repeat;'  data-sort='{2}' >
                        <div class='content-slider-single'>{1}<div>   </li>", img.AbsPath, DynamicContentHelper.InjectControls( img.HTMLContent ),  img.SortOrder.ToString()); 
            }
            sb.AppendFormat(StyleSheetInclude, "/css/jcarousel-skin.css");
 

             sb.AppendFormat(@"<script>jQuery(document).ready(function() {{
                 jQuery('#mycarousel').jcarousel({{
                     scroll: 1, auto:5 , animation:1200, easing:'{0}', wrap: 'last'
                 }});
             }}); </script>", this.Easing.ToString());


            return String.Format(@"{1}<div id='wrap'>  
                  <ul {2} id='mycarousel' class='jcarousel-skin'> 
                        {0}
                  </ul>  
             </div> ", sb_imagereel.ToString(), sb.ToString(), DynamicContentHelper.RenderEditableRegion("/Admin/entitygridform.aspx?id=" + catid + "&etype=GalleryCategory"));
             
            
        }

        private string GetFadingSlideshow() { 
            int cnt = 0; 
            StringBuilder sb_imagereel = new StringBuilder();
            sb_imagereel.Append("<div id='slideshow'>");
            foreach (GalleryImage img in Images)
            {
                cnt++;
                string cls = "";
                if (cnt == 1)
                    cls = "active";
                sb_imagereel.AppendFormat("<img src='{0}' alt='Slideshow Image {1}' class='{2}'  />", img.AbsPath, cnt.ToString(), cls); 
            }
            sb_imagereel.Append("</div>");
            sb_imagereel.AppendFormat("<style>#slideshow {{  height: {0}px !important; width: {1}px !important;  }}</style>", this.Height, this.Width);

            //sb_imagereel.AppendFormat("<script>  setInterval( slideSwitch, {0} ); </script>", slideDuration);

            sb_imagereel.AppendFormat("<script>  setInterval(slideSwitch, {0} );   </script>", slideDuration);
            

            return   sb_imagereel.ToString()  ;  
        }


        private string GetImageSlider() { 
            int cnt = 0;
            StringBuilder sb_imagereel = new StringBuilder();
            StringBuilder sb_paging = new StringBuilder();
            StringBuilder sb = new StringBuilder();

            if (this.Width > 0)
                sb.AppendFormat("<style>.window {{  width: {0}px !important;  }}</style>", Convert.ToString(this.Width));

            if (this.Height > 0)
                sb.AppendFormat("<style>.window {{  height: {0}px !important;  }}</style>", Convert.ToString(this.Height)); 
              

            
            foreach (GalleryImage img in Images)
            {
                cnt++;
                sb_imagereel.AppendFormat("<a href='#'><img src='{0}' alt='' /></a>", img.AbsPath);
                //sb_imagereel.AppendFormat("{0}", img.AbsPath);
                sb_paging.AppendFormat("<a href='#' rel='{0}'>{0}</a>", cnt.ToString());
            }

  

            return string.Format(@"
                   {2} <div class='main_view'>
                        <div class='window'>
                            <div class='image_reel'>
                                {0}
                            </div>
                        </div>
                        <div class='paging'>
                            {1}
                        </div>
                    </div>
            ", sb_imagereel.ToString(), sb_paging.ToString(), sb.ToString() );
        

        }

        protected void LoadMain() {
            
            List<GalleryCategory> GC;
            if (this.UseImageID > 0)
            { 
                GalleryImage GI = new GalleryImage().Select(this.UseImageID);
                if (GI != null) {
                    Images = new List<GalleryImage>();
                    Images.Add(GI);
                }
                  
            }

            
            if (this.GalleryIDParam != null && this.GalleryIDParam != "")
            {
                if(StringUtils.IsInteger(HTTPUtils.QString(this.GalleryIDParam))){
                    this.UseGalleryID = Convert.ToInt32(HTTPUtils.QString(this.GalleryIDParam));
                } 
            }

            if (this.UseGalleryID > 0)
            {
                GC = new GalleryCategory().SelectAll().Where(o => o.ID == this.UseGalleryID).ToList();
            }
            else
            {
                if (this.UseGalleryName != null && this.UseGalleryName != "")
                {
                    GC = new GalleryCategory().SelectAll().Where(o => o.Name == this.UseGalleryName).ToList();
                }
                else 
                {
                    GC = new GalleryCategory().SelectAll().Where(o => o.Caption == this.SliderType.ToString()).ToList();
                } 
            }
            
            if (GC == null) {
                HasImages = false; 
                return;
            }
  
            if (GC.Count > 0) 
                Images = GC[0].GalleryImages.Where(o=>o.Active=="True").ToList();
            
            if (Images != 