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
    [EntityAttribute("EntityTags")]
    [FormAttribute("Tags")]
    public class EntityTag: ILinkable
    {
        private int _id;
        public int ID
        {
            get { return   _id; }
            set { _id = value; }
        }

        private string _name;
        [EntityGridAttribute("Name")]
        [Formfield("Name", Required = true, MaxLength = 100, Width = 100,
            ValidateRegEx = @"^[a-zA-Z0-9\-\&\\\(\)\%\:]*$", ToolTip = @".<br> 
      <li>Must begin with a letter / 100 Character limit / Valid Characters (Letters, Numbers -\&()%: ) </li>"
            )]
        public string Name
        {
            get { return _name == null ? "" : _name; }
            set { _name = value; }
        }

        private string _entity;
        public string Entity
        {
            get { return _entity == null ? "" : _entity; }
            set { _entity = value; }
        }

        private string _active;
        [Formfield("Active", Width = 5, ControlType = typeof(DDTrueFalse))]
        public string Active
        {
            get { return _active == null ? "1" : _active; }
            set { _active = value; }
        }


        private int _sortorder; 
        [Formfield("Sort Order", Width = 50, ValidateInteger = true)]
        [EntityGridAttribute("Sort Order")]
        public int SortOrder
        {
            get { return _sortorder == 0 ? 1 : _sortorder; }
            set { _sortorder = value; }
        }


        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
 

        private List<EntityTag> _EntityTagCollection;
        public List<EntityTag> EntityTagCollection
        {
            get { return _EntityTagCollection; }
            set { _EntityTagCollection = value; }
        }
        private List<EntityTag> _EntityTagMappings;
        public List<EntityTag> EntityTagMappings
        {
            get { return _EntityTagMappings; }
            set { _EntityTagMappings = value; }
        }
 
        public int Insert()
        {
            return Insert(this);
        }
        public int Insert(EntityTag obj)
        {
            return EntityTagDAL.Add(obj);
        }
        public void Update()
        {
            Update(this);
        }
        public void Update(EntityTag obj)
        {
            EntityTagDAL.Update(obj);
        }
        public void Delete()
        {
            Delete(this);
        }
        public void Delete(EntityTag obj)
        {
            EntityTagDAL.Delete(obj.ID);
        }
        public EntityTag Select(int ID)
        {
            List<EntityTag> oResult = ((from o in SelectAll() where o.ID == ID select o).Take(1)).ToList();
            return (oResult.Count > 0) ? oResult[0] : null;
        }
        public int SelectCount()
        {
            List<EntityTag> oResult = ((from o in SelectAll() select o)).ToList();
            return (oResult.Count >= 0) ? oResult.Count : 0;
        }
        public List<EntityTag> SelectAllActive()
        {
            List<EntityTag> oResult = ((from o in SelectAll() where o.Active == "True" select o)).ToList();
            return oResult;
        }
        public List<EntityTag> SelectByEntity(string Entity)
        {
            List<EntityTag> oResult = ((from o in SelectAll() where o.Entity == Entity select o)).ToList();
            return oResult;
        }
        public List<EntityTag> SelectByEntityID(int EntityID, string Entity)
        {
            List<EntityTag> oResult = ((from o in SelectAll() where o.Entity == Entity && o.ID == EntityID select o)).ToList();
            return oResult;
        }

        public List<EntityTag> SelectAll()
        {
            if (this.EntityTagCollection == null)
                this.EntityTagCollection = new EntityTagDAL().GetAll();
            return this.EntityTagCollection;
        }

        #region ILinkable Members


        public string Caption
        {
            get { return this.Name; }
        }

        public string URL
        {
            get { return ""; }
        }

        public string SystemURL { get { return ""; } set { this.SystemURL = value; } }
        public string ShortDesc   {  get { return ""; }   }
 
        #endregion
    }

}

