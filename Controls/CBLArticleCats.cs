using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace WebSite.Core 
{
    public class CBLArticleCats : CBLEntity 
    {
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
                        base.oSelected.Add(new ArticleCategory().Select(Convert.ToInt32(LI.Value)));
                } 
 
        }
        protected override void SetSelected(List<ILinkable> oSelectedList)
        {
            foreach (ILinkable item in oSelectedList)
                this.Items.FindByValue(Convert.ToString(item.ID)).Selected = true;
        }
        protected override void LoadItems()
        {
            List<ArticleCategory> oList = new ArticleCategory().SelectAll(); 
            foreach (ArticleCategory item in oList)
                Items.Add(new ListItem(item.Caption, Convert.ToString(item.ID))); 
        }
    }
}
