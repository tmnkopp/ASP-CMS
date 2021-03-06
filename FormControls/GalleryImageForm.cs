﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using ASPXUtils;
using System.Reflection;
using System.IO;
using ASPXUtils.Controls;

namespace WebSite.Core
{ 
    public class GalleryImageForm : EntityForm
    {
        GalleryImage objItem;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            objItem = new GalleryImage().Select(Convert.ToInt32(base._ID));

            int CATID = base._CID;
            if(CATID < 1){ 
                if (objItem != null)
                {
                    CATID = objItem.CATID;
                }
            }
 
            if (CATID > 0)
            { 
                GalleryImagesList oGalleryImagesList = new GalleryImagesList();
                oGalleryImagesList.CATID = CATID;
                oGalleryImagesList.ShowUploader = false; 
                this.Controls.Add(oGalleryImagesList);
            }
            
        }
        protected override void DeleteForm(){
             
            string CAT = Convert.ToString(objItem.CATID);
            string _fullpath = objItem.AbsPath;
            string _thumb = HttpContext.Current.Server.MapPath(ImgUtils.ImagePathToThumb(_fullpath));

            FileInfo TheFile = new FileInfo(_thumb);
            if (TheFile.Exists)
            {
                File.Delete(_thumb);
            }
            TheFile = new FileInfo(HttpContext.Current.Server.MapPath(_fullpath));
            if (TheFile.Exists)
            {
                File.Delete(HttpContext.Current.Server.MapPath(_fullpath));
            } 
            objItem.Delete();
            HttpContext.Current.Response.Redirect( String.Format("/admin/default.aspx" , CAT) , true);
        }
        protected override void SaveForm()
        {
            GalleryImage objItem = new GalleryImage();
            if ( base._ID > 0)
                objItem = new GalleryImage().Select(Convert.ToInt32(base._ID));

            objItem.Name = (string)GetValueFromControl("Name");
            objItem.Description = (string)GetValueFromControl("Description");
            objItem.Active = (string)GetValueFromControl("Active");
            objItem.SortOrder = Convert.ToInt32((string)GetValueFromControl("SortOrder"));
            objItem.HTMLContent = (string)GetValueFromControl("HTMLContent");

 
            int _newid = 0;
            if ( base._ID > 0 )
            {
                objItem.Update();
                _newid = Convert.ToInt32(base._ID);
            }
            else
            {
                _newid = objItem.Insert();
            }

            string url = HttpContext.Current.Request.RawUrl;
            if (url.IndexOf("id=") < 1)
            { 
                if (url.IndexOf("?") < 1)
                    url = url + "?id=" + _newid.ToString();
                else
                    url = url + "&id=" + _newid.ToString();
            } 
            HttpContext.Current.Response.Redirect(url, true);
        }
        protected override void LoadForm()
        {
            GalleryImage objItem = new GalleryImage();
            if (base._ID > 0)
                objItem = new GalleryImage().Select(Convert.ToInt32(base._ID));

            SetControlValue("Name", objItem.Name, null);
            SetControlValue("Description", objItem.Description, null);
            SetControlValue("Active", objItem.Active, null);
            SetControlValue("SortOrder", objItem.SortOrder, null);
            //SetControlValue("AbsPath", objItem.AbsPath, null); 
            SetControlValue("HTMLContent", objItem.HTMLContent, null);
  
            CKEditor obj = (CKEditor)base.ControlHolder.FindControl("HTMLContent");
            obj.Height = 350;
            if (objItem.AbsPath != "")
            {
                obj.BackgroundStyle = " background :#333 url(" + objItem.AbsPath + ") no-repeat !important;  ";
                obj.BlockEditorCss = true;
            }
            

            //UserControl uc = (UserControl)Page.LoadControl("~/Controls/Uploader.ascx");
            //if (uc != null)
            //    pUpdatePanel.Controls.Add(uc);
 
            //SetControlValue("DefaultTitle", SiteSettings.Instance.DefaultTitle, null); 
            
        } 
  
    }
}
                                    