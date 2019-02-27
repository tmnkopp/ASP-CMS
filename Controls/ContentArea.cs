using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPXUtils;
using ASPXUtils.Controls;
namespace WebSite.Core
{

    public enum ContentAreaType
    {
        NewWindow,
        Inline
    }

     

    [ToolboxData("<{0}:ContentArea runat=server></{0}:ContentArea>")]
    public class ContentArea : Control
    {
        #region Properties
        public string CssClass { get; set; }
        public StringBuilder HTML = new StringBuilder();  
        public Button cmdSelect;
        public PlaceHolder ControlHolder = new PlaceHolder();
        public HiddenField hidData = new HiddenField() {  };
        public CKEditor txtEditor = new CKEditor() {  UseInlineEditor = true, SourceFolder = "ckeditor4.0" };
        public bool EditMode {get;set;}
        public string ContentID { get; set; }
        #endregion

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            hidData.ID="hidData" + this.ClientID ;
            txtEditor.ID = "txtEditor" + this.ClientID;
            if (ContentID == null)
                ContentID = this.ID;

            txtEditor.UseInlineEditor = false;
            txtEditor.SourceFolder = "ckeditor";

            //RenderEditableRegion

            EditMode = true;
            ContentItem item = new ContentItem().Select(this.ContentID);
            if(item==null){
                item = new ContentItem() { ContentID = this.ContentID, Contents = SiteSettings.Instance.DefaultContent };
                int i = item.Insert();
            } 
            if (item.Contents == "" || item.Contents==null)
                item.Contents = SiteSettings.Instance.DefaultContent;

            HTML.Append(item.Contents);

            if (EditMode)
                LoadControls(); 

        } 
        protected override void Render(HtmlTextWriter writer)
        {
            if (!EditMode) 
                writer.Write(HTML.ToString());
            base.Render(writer);
        } 
        private void LoadControls()
        {

            Button cmdSelect = new Button() { ID = "cmdSelect" + this.ClientID, CssClass = "ctrlcmdSelect", OnClientClick = "Select_Click" + this.ClientID + "(this);  ", Text = "Save" };
            this.Controls.Add(ControlHolder);
            txtEditor.Text = HTML.ToString();
            cmdSelect.Click += new EventHandler(Save_Click);
            ControlHolder.Controls.Add(cmdSelect);
            ControlHolder.Controls.Add(txtEditor);
            ControlHolder.Controls.Add(hidData); 
            string scripts = String.Format(@"
                <script>
                    function Select_Click{2}(obj){{ 
                        try{{ 
                            var data = CKEDITOR.instances['{0}'].getData();
                             $('#{1}').val(data); 
                   
                        }}catch(err){{console.log(err)}} 
                    }} 
                </script>
                ", txtEditor.InlineID, hidData.ClientID, this.ClientID);
                    ControlHolder.Controls.Add(new Literal() { Text = scripts });

        }
        protected void Save_Click(object sender, EventArgs e)
        {


            ContentItem item = new ContentItem().Select(this.ContentID);
            if (item == null)
                return;
            item.Contents = hidData.Value;
            txtEditor.Text = item.Contents;
  