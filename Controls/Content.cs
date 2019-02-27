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
using System.Web.UI.HtmlControls;
namespace WebSite.Core
{

    public enum ContentEditBehavior 
    {
        NewWindow,
        Inline
    }
    public enum ContentType
    {
        Text,
        Label
    }

    [ToolboxData("<{0}:ContentEditor runat=server></{0}:ContentEditor>")]
    public class ContentEditor : Control
    {
        public string ContentID { get; set; }
        public CKEditor oCKEditor;
        PlaceHolder pnlForm = new PlaceHolder();
        Button cmdSave = new Button() { Text = "Save" };
        HtmlInputButton cmdClose = new HtmlInputButton() { Value = "Close Window" };
        Button cmdSaveClose = new Button() { Text = "Save And Close" };

        private ContentType _ContentType = ContentType.Text;
        public ContentType EditContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        } 

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            oCKEditor = new CKEditor
                           {
                               ID = "oCKEditor",
                               Height = Unit.Parse("400"),
                               Width = Unit.Parse("900" ),
                               Rows = 60 
                           };

            cmdSave.Click += new EventHandler(cmdSave_Click);
            cmdSaveClose.Click += new EventHandler(cmdSaveClose_Click);
            cmdClose.Attributes.Add("onclick", "window.opener.location.reload(true); window.close();");  

            pnlForm.Controls.Add(cmdSave);
            pnlForm.Controls.Add(cmdClose);
            pnlForm.Controls.Add(cmdSaveClose);
            pnlForm.Controls.Add(new Literal() { Text = "<br />" });
            pnlForm.Controls.Add(oCKEditor);
            Controls.Add(pnlForm);

            ContentItem item = new ContentItem().Select(this.ContentID);
            if (item == null)
            {
                item = new ContentItem()
                {
                    ContentID = this.ContentID
                    ,  Contents = SiteSettings.Instance.DefaultContent
                    ,  ContentsSm = SiteSettings.Instance.DefaultContent
                };

                int i = item.Insert();
            } 

            oCKEditor.Text = item.Contents;
        }
        protected void Save()
        {
            ContentItem item = new ContentItem().Select(this.ContentID);
            if (item == null)
                return;
            item.Contents = oCKEditor.Text;
            item.Update();
        } 
        protected void cmdSaveClose_Click(object sender, EventArgs e)
        {
            Save();
            HttpContext.Current.Response.Write("<script>window.opener.location.reload(true);  window.close();</script>");
        } 

        protected void cmdSave_Click(object sender, EventArgs e)
        { 
            Save();
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);  
        } 

    }



    [ToolboxData("<{0}:Content runat=server></{0}:Content>")]
    public class Content : Control
    {
        #region Properties
        public string CssClass { get; set; }
        public StringBuilder HTML = new StringBuilder();  
        public Button cmdSelect;
        public PlaceHolder ControlHolder = new PlaceHolder();
        public HiddenField hidData = new HiddenField() {  };
        public CKEditor txtEditor = new CKEditor() {  UseInlineEditor = false, SourceFolder = "ckeditor" };
        public bool EditMode {get;set;}
        public string ContentID { get; set; }

        private ContentType _ContentType = ContentType.Text;
        public ContentType EditContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        } 

        private ContentEditBehavior _EditBehavior = ContentEditBehavior.NewWindow;
        public ContentEditBehavior EditBehavior
        {
            get { return _EditBehavior; }
            set { _EditBehavior = value; }
        }  

        private string _TargetEditURL = "/Admin/Default.aspx?cid={0}&view=ce";
        public string TargetEditURL
        {
            get { return _TargetEditURL; }
            set { _TargetEditURL = value; }
        }  
        #endregion
 
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (ContentID == null)
                ContentID = this.ID;
             
            EditMode = Security.IsAdmin(); 
            //EditMode = false;

            ContentItem item = new ContentItem().Select(this.ContentID);
            if (item == null)
            {
                item = new ContentItem() { 
                    ContentID = this.ContentID
                    , Contents = SiteSettings.Instance.DefaultContent
                    ,  ContentsSm = SiteSettings.Instance.DefaultContent
                };
                int i = item.Insert();
            }

            if (this.EditBehavior == ContentEditBehavior.Inline)
            { 
                
                txtEditor.UseInlineEditor = true;
                txtEditor.SourceFolder = "ckeditor4.0";
                hidData.ID = "hidData" + this.ClientID;
                txtEditor.ID = "txtEditor" + this.ClientID; 
                HTML.Append(item.Contents); 

                if (EditMode)
                    LoadInlineControls(); 
            } 
            else 
            { 
                string renderString = DynamicContentHelper.RenderEditableRegion(String.Format(TargetEditURL, this.ContentID)); 
                string content = string.Format("<div {0}>{1}</div>", renderString, item.Contents);
                HTML.Append(content);
                ASPXUtils.Log.Insert(this.ContentID + " " + item.Contents);
            } 
        } 

        protected override void Render(HtmlTextWriter writer)
        {
            //If (!EditMode) 
                writer.Write(HTML.ToString());
            base.Render(writer);
        }

        private void LoadInlineControls()
        {

            Button cmdSelect = new Button() { 
                ID = "cmdSelect" + this.ClientID
                , CssClass = "ctrlcmdSelect"
                , OnClientClick = "Select_Click" + this.ClientID + "(this);  "
                , Text = "Save" 
            };

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
            item.Update();
            
            //HttpContext.Current.Response.Write("<br>!! " + hidData.Value);
            //HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl, true);
        } 
    }
}
