using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ASPXUtils;

namespace WebSite.Core 
{
    public class CBLTags : CBLEntity 
    {
        private string _EntityType = HTTPUtils.QString("etype");
        public string EntityType {
            get { return _EntityType; }
            set { _EntityType = value; } 
        }
        public override List<ILinkable> Selected
        {
            get
            {
                if (base.oSelected == null)
                    LoadSelected();
                return base.oSelected;
            }
        }
        protected override void LoadSelected( )
        {
                base.oSelected = new List<ILinkable>();
                foreach (ListItem LI in this.Items)
                {
                    if (LI.Selected)
                        base.oSelected.Add(new EntityTag().Select(Convert.ToInt32(LI.Value)));
                } 
 
        }
        protected override void SetSelected(List<ILinkable> oSelectedList)
        {
            foreach (ILinkable item in oSelectedList)
                this.Items.FindByValue(Convert.ToString(item.ID)).Selected = true;
        }
        protected override void LoadItems()
        {//etype
            List<EntityTag> oList = new EntityTag().SelectByEntity(this.EntityType);
            foreach (EntityTag item in oList)
                Items.Add(new ListItem(item.Caption, Convert.ToString(item.ID))); 
        }
    }
}
