using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebSite.Core 
{
    class GalleryImageUploader: ASPXUtils.Controls.FileUploader
    {
        public int CATID { get; set; }
        protected override void UploadComplete()
        { 
            //HttpContext.Current.Response.Write("<br>" + this.SavePath);
            //HttpContext.Current.Response.Write("<br>" + filename);

            GalleryImage obj;
            foreach(string filename in this._UploadedFileNameList){
                
                obj = new GalleryImage() { CATID = this.CATID, AbsPath = filename, Name = filename };
                obj.Insert();
            } 
            string url = HttpContext.Current.Request.RawUrl;
            HttpContext.Current.Response.Redirect(url, true);
        }
    }
}
