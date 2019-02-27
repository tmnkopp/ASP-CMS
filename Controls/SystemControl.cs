using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Core 
{
    public enum SystemControlType
    {
        GenericPage,
        GalleryCategory,
        ArticleCategory,
        Article,
        Event,
        ProdCategory,
        Testimonial
    }
    public class SystemControl
    {  
        public string Name { get; set; }
        public string PhysicalPage { get; set; }
        public bool ShowSubMenu { get; set; }
        public string ClassName { get { return this.Type.ToString(); } }
        public SystemControlType Type { get; set; } 
        public SystemControl()    {       }
        public SystemControl(SystemControlType type, string name,string physicalPage, bool showSubMenu)
        {
            this.Type = type; 
            this.Name = name;
            this.PhysicalPage = physicalPage;
            this.ShowSubMenu = showSubMenu;
        }
        public SystemControl SelectByClass(string type)
        {
            var oResult = ((from o in SelectAll().AsQueryable() where o.Type.ToString() == type select o).Take(1)).ToList();
            return (oResult.Count > 0) ? oResult[0] : null;
        }
        public SystemControl SelectByType(SystemControlType type)
        {
            var oResult = ((from o in SelectAll().AsQueryable() where o.Type  == type select o).Take(1)).ToList();
            return (oResult.Count > 0) ? oResult[0] : null;
        } 
        List<SystemControl> SystemControlList { get; set; }
        public List<SystemControl> SelectAll()
        {
            if (this.SystemControlList == null) {
                this.SystemControlList = new List<SystemControl>();
                SystemControlList.Add(new SystemControl(SystemControlType.Article, "Articles", "Article.aspx", false));
                SystemControlList.Add(new SystemControl(SystemControlType.ArticleCategory, "Article Categories", "ArticleCategory.aspx", true));
                SystemControlList.Add(new SystemControl(SystemControlType.GalleryCategory, "Gallery Categories", "GalleryCategory.aspx", true));
                SystemControlList.Add(new SystemControl(SystemControlType.Event, "Events", "Event.aspx", false));
                SystemControlList.Add(new SystemControl(SystemControlType.ProdCategory, "Product Categories", "ProductCategory.aspx", true));
                SystemControlList.Add(new SystemControl(SystemControlType.Testimonial, "Testimonials", "Testimonial.aspx", false)); 
            }
            return this.SystemControlList;
        }
        public static Dictionary<string, string> GetSystemControlList()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (SystemControl SC in new SystemControl().SelectAll())
            {
                dict.Add(SC.ClassName, SC.Name);
            }
            return dict;
        }
   
    } 
}
