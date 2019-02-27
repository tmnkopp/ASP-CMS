 using System;
  using System.Data;
  using System.Configuration;
  using System.Linq;
  using System.Web;
  using System.Web.Security;
  using System.Web.UI;
  using System.Web.UI.HtmlControls;
  using System.Web.UI.WebControls;
  using System.Web.UI.WebControls.WebParts;
  using System.Xml.Linq;
  using System.Collections.Generic;
using WebSite.Core;
using ASPXUtils;
using ASPXUtils.Controls;

namespace WebSite.Core
{
    [DataEntityAttribute(TableName = "ContentItems", TablePrefixAppKey = "TABLE_PREFIX")]
    [EntityAttribute("ContentItem")] 
    public class ContentItem :  IBusinessEntity 
    { 
        [EntityGridAttribute("ID", IsID=true)]  
        [DatafieldAttribute("ID" , IsPrimary=true, DataType = SqlDbType.Int )] 
        public int ID{ get; set; }
 
        [DatafieldAttribute("ContentID" )] 
        public string ContentID{ get; set; }
 
        [DatafieldAttribute("Contents" )] 
        public string Contents{ get; set; }

        [DatafieldAttribute("ContentsSm")]
        public string ContentsSm { get; set; }
         
        private string _SITE_ID = SiteSettings.SITE_ID;
        [DatafieldAttribute("SITE_ID")] 
        public string SITE_ID  
        {
            get { return _SITE_ID; }
            set { _SITE_ID = value; }
        }

        private List<ContentItem> Collection; 
   
  
     #region Methods 
        public ContentItem Select(int ID)
        {
            List<ContentItem> oResult = SelectAll().Where(o => o.ID == ID).Take(1).ToList();
            return (oResult.Count > 0) ? oResult[0] : null;
        }
        public ContentItem Select(string ContentID)
        {
            List<ContentItem> oResult = SelectAll().Where(o => o.ContentID == ContentID).Take(1).ToList();
            return (oResult.Count > 0) ? oResult[0] : null;
        }
        public int Insert()
        {
            return ASPXUtils.Data.GenericDAL<ContentItem>.Insert(this);
        } 
        public void Update()
        {
            ASPXUtils.Data.GenericDAL<ContentItem>.Update(this);  
        }
        public void Delete()
        {
            ASPXUtils.Data.GenericDAL<ContentItem>.Delete(this);
        }
        public List<ContentItem> SelectAll()
        {
            if (Collection == null)
                Collection = ASPXUtils.Data.GenericDAL<Conten