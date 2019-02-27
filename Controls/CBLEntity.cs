using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace WebSite.Core 
{
    public abstract class CBLEntity : CheckBoxList   
    {
        protected List<ILinkable> oSelected;
        public virtual List<ILinkable> Selected
        {
            get
            {
                if (oSelected == null)
                {
                    LoadSelected();
                }
                return oSelected;
            }
            set {
                SetSelected(value);
            }
        }
        protected abstract void LoadSelected();
        protected abstract void SetSelected(List<ILinkable> oSelectedList);
        protected abstract void LoadItems();
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.RepeatColumns = 4;
            Items.Clear();
            LoadItems(); 
        }
         
    }
}
