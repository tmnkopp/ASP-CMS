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
using ASPXUtils;
using ASPXUtils.Controls;
namespace WebSite.Core {

    [FormAttribute("Caption" )]
    public class GalleryImage : ILinkable, IBusinessEntity 
    {

        #region Properties

        private int _id;
        public int ID
        {
            get { return  _id; }
            set { _id = value; }
        }

        private int _catid;
        public int CATID
        {
            get { return _catid; }
            set { _catid = value; }
        }


        public string VirPath
        {
            get { return ASPXUtils.ImgUtils.ImagePathToThumb(this.AbsPath); }
        }
       

        private string _name;
        [EntityGridAttribute("Name")]
        [Formfield("Name", Required = true )]
        public string Name
        {
            get { return _name == null ? "" : _name; }
            set { _name = value; }
        }

        private string _description;
        [Formfield("Description", ControlType = typeof(ASPXUtils.Controls.TextArea))]
        public string Description
        {
            get { return _description == null ? "" : _description; }
            set { _description = value; }
        }

        private string _datecreated;
        public string DateCreated
        {
            get { return _datecreated == null ? System.DateTime.Now.ToString() : _datecreated; }
            set { _datecreated = value; }
        }

        private string _dateupdated;
        public string DateUpdated
        {
            get { return _dateupdated == null ? System.DateTime.Now.ToString() : _dateupdated; }
            set { _dateupdated = value; }
        }

        private string _active;
        [Formfield("Active", Width = 5, ControlType = typeof(DDTrueFalse))]
        public string Active
        {
            get { return _active == null ? "1" : _active; }
            set { _active = value; }
        }

        private int _sortorder;
        [Formfield("SortOrder", Width = 50, ValidateInteger = true, Required = true )]
        public int SortOrder
        {
            get {
                if (_sortorder==0)
                {
                    _sortorder = ID;
                }
                return  _sortorder; 
            }
            set { _sortorder = value; }
        }
        private string _abspath;
        //[Formfield("FullImage", ControlType = typeof(Literal))]
        public string AbsPath
        {
            get { return _abspath == null ? "" : _abspath.Replace("~", ""); }
            set { _abspath = value; }
        }

        private string _ClickURL;
        public string ClickURL
        {
            get { return _ClickURL == null ? "" : _ClickURL; }
            set { _ClickURL = value; }
        }
        private string _HTMLContent;
        [Formfield("Image", Width = 900, ControlType = typeof(CKEditor))]
        public string HTMLContent
        {
            get { return _HTMLContent == null ? "" : _HTMLContent; }
            set { _HTMLContent = value; }
        }       


        private List<GalleryImage> _GalleryImageCollection;
        public List<GalleryImage> GalleryImageCollection
        {
            get { return _GalleryImageCollection; }
            set { _GalleryImageCollection = value; }
        }
        private GalleryCategory _GalleryCategory;
        public GalleryCategory ImageGalleryCategory
        {
            get {
                if (_GalleryCategory == null)
                    _GalleryCategory = new GalleryCategory().Select(this.CATID);
                return _GalleryCategory; 
            }
            set { _GalleryCategory = value; }
        } 
        #endregion

        #region Methods
        public int Insert()
        {
            return Insert(this);
        }
        public int Insert(GalleryImage obj)
        {
            return GalleryImageDAL.Add(obj);
        }

        public void Update()
        {
            Update(this);
        }
        public void Update(GalleryImage obj)
        {
            GalleryImageDAL.Update(obj);
        }

        public void Delete()
        {
            Delete(this);
        }
        public void Delete(GalleryImage obj)
        {
            GalleryImageDAL.Delete(obj.ID);
        }

     
        public List<GalleryImage> SelectByCatID(int CATID)
        {
            var oResult = ((from o in SelectAll().AsQueryable() where o.CATID == CATID select o)).ToList();
            return oResult;
        }
        public GalleryImage Select(int ID)
        {
            var oResult = ((from o in SelectAll().AsQueryable() where o.ID == ID select o).Take(1)).ToList();
            return (oResult.Count > 0) ? oResult[0] : null;
        }
        public int SelectCount()
        {
            return SelectAll().Count();
        }
        public List<GalleryImage> SelectAllActive()
        {
            var oResult = ((from o in SelectAll().AsQueryable() where o.Active == "True" select o)).ToList();
            return oResult;
        }
        public List<GalleryImage> SelectAll()
        {
            if (this.GalleryImageCollection == null)
                this.GalleryImageCollection = new GalleryImageDAL().GetAll();
            return this.GalleryImageCollection;
        }   
        #endregion

        #region ILinkable Members


        public string Caption
        {
            get { return this.Name; }
        }
        

        public string URL
        {
            get { return this.AbsPath; }
        }

        public string SystemURL
        {
            get
            {
                return this.AbsPath;
            }
            set
            {
                this.AbsPath = value;
            }
        }

        public string ShortDesc
        {
            get { return this.Description; }
        } 

        #endregion
    } 

}