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
using System.Data.SqlClient;
using System.Web.Caching;
using System.Text;
using System.Xml;
using ASPXUtils;
using WebSite.Core;


namespace WebSite.Core
{ 
public class WebPageDAL : ASPXUtils.Data.DataEntity<WebPage>
{
    public WebPageDAL() { }


    public static int Add(WebPage obj)
    {
        ResetCache();

        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "";
        sSql += " INSERT INTO " + SiteSettings.TABLE_PREFIX + "Pages (PageContentAlt,MenuCaptionAlt,SystemLayout,SITE_ID,UseExactTitle, Parent,Name,MenuCaption,Description,Keywords,Title,Header,PageContent,MainMenu,ExternalURL, SystemURL, SystemParam, SystemControl, DateCreated,DateUpdated,Active,SortOrder, Notes, SubCaption) ";
        sSql += " VALUES( @PageContentAlt , @MenuCaptionAlt, @SystemLayout,@SITE_ID, @UseExactTitle, @Parent,  @Name, @MenuCaption, @Description, @Keywords, @Title, @Header, @PageContent, @MainMenu, @ExternalURL, @SystemURL, @SystemParam, @SystemControl, @DateCreated, @DateUpdated, @Active, @SortOrder, @Notes, @SubCaption); SELECT SCOPE_IDENTITY() ;";

         objSqlHelper.AddParameterToSQLCommand("@Parent", SqlDbType.VarChar, Convert.ToString(obj.Parent));
         objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString( obj.Name) );
         objSqlHelper.AddParameterToSQLCommand("@MenuCaption", SqlDbType.VarChar, Convert.ToString(obj.MenuCaption));
         objSqlHelper.AddParameterToSQLCommand("@SubCaption", SqlDbType.VarChar, Convert.ToString(obj.SubCaption)); 

         objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString( obj.ShortDesc) );
         objSqlHelper.AddParameterToSQLCommand("@Keywords", SqlDbType.VarChar, Convert.ToString( obj.Keywords) );
         objSqlHelper.AddParameterToSQLCommand("@Title", SqlDbType.VarChar, Convert.ToString( obj.Title) );
         objSqlHelper.AddParameterToSQLCommand("@Header", SqlDbType.VarChar, Convert.ToString( obj.Header) );
         objSqlHelper.AddParameterToSQLCommand("@PageContent", SqlDbType.VarChar, Convert.ToString( obj.PageContent) );
         objSqlHelper.AddParameterToSQLCommand("@MainMenu", SqlDbType.VarChar, Convert.ToString( obj.MainMenu) );
         objSqlHelper.AddParameterToSQLCommand("@ExternalURL", SqlDbType.VarChar, Convert.ToString( obj.ExternalURL) );
         objSqlHelper.AddParameterToSQLCommand("@SystemURL", SqlDbType.VarChar, Convert.ToString(obj.SystemURL));
         objSqlHelper.AddParameterToSQLCommand("@SystemParam", SqlDbType.VarChar, Convert.ToString(obj.SystemParam)); 
         objSqlHelper.AddParameterToSQLCommand("@SystemControl", SqlDbType.VarChar, Convert.ToString(obj.SystemControl));
         objSqlHelper.AddParameterToSQLCommand("@SystemLayout", SqlDbType.VarChar, Convert.ToString(obj.SystemLayout));
        

         objSqlHelper.AddParameterToSQLCommand("@DateCreated", SqlDbType.VarChar, Convert.ToString( obj.DateCreated) );
         objSqlHelper.AddParameterToSQLCommand("@DateUpdated", SqlDbType.VarChar, Convert.ToString( obj.DateUpdated) );
         objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString( obj.Active) );
         objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString( obj.SortOrder) );
         objSqlHelper.AddParameterToSQLCommand("@Notes", SqlDbType.VarChar, Convert.ToString(obj.Notes));
         objSqlHelper.AddParameterToSQLCommand("@UseExactTitle", SqlDbType.VarChar, Convert.ToString(obj.UseExactTitle));
         objSqlHelper.AddParameterToSQLCommand("@SITE_ID", SqlDbType.VarChar, SiteSettings.SITE_ID);

         objSqlHelper.AddParameterToSQLCommand("@PageContentAlt", SqlDbType.VarChar, Convert.ToString(obj.PageContentAlt));
         objSqlHelper.AddParameterToSQLCommand("@MenuCaptionAlt", SqlDbType.VarChar, Convert.ToString(obj.MenuCaptionAlt));

        int _id = objSqlHelper.GetExecuteScalarByCommand(sSql);
        objSqlHelper.Dispose();

        return _id;

    }
    public static void Update(WebPage obj)
    {
        ResetCache();

        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "";
        sSql = " Update " + SiteSettings.TABLE_PREFIX + "Pages SET PageContentAlt=@PageContentAlt, MenuCaptionAlt=@MenuCaptionAlt, SystemLayout=@SystemLayout, UseExactTitle = @UseExactTitle ,Parent = @Parent, Name = @Name, MenuCaption = @MenuCaption, Description = @Description,Keywords = @Keywords,Title = @Title,Header = @Header,PageContent = @PageContent,MainMenu = @MainMenu,ExternalURL = @ExternalURL, SystemURL=@SystemURL, SystemParam=@SystemParam, SystemControl=@SystemControl, DateUpdated = @DateUpdated,Active = @Active,SortOrder = @SortOrder, Notes = @Notes, SubCaption = @SubCaption WHERE ID = @ID;";

         objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString( obj.ID) );
         objSqlHelper.AddParameterToSQLCommand("@Parent", SqlDbType.VarChar, Convert.ToString(obj.Parent));
         objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString( obj.Name) );
         objSqlHelper.AddParameterToSQLCommand("@MenuCaption", SqlDbType.VarChar, Convert.ToString(obj.MenuCaption));
         objSqlHelper.AddParameterToSQLCommand("@SubCaption", SqlDbType.VarChar, Convert.ToString(obj.SubCaption));
         
         objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString(obj.ShortDesc));
         objSqlHelper.AddParameterToSQLCommand("@Keywords", SqlDbType.VarChar, Convert.ToString( obj.Keywords) );
         objSqlHelper.AddParameterToSQLCommand("@Title", SqlDbType.VarChar, Convert.ToString( obj.Title) );
         objSqlHelper.AddParameterToSQLCommand("@Header", SqlDbType.VarChar, Convert.ToString( obj.Header) );
         objSqlHelper.AddParameterToSQLCommand("@PageContent", SqlDbType.VarChar, Convert.ToString( obj.PageContent) );
         objSqlHelper.AddParameterToSQLCommand("@MainMenu", SqlDbType.VarChar, Convert.ToString( obj.MainMenu) ); 
         objSqlHelper.AddParameterToSQLCommand("@ExternalURL", SqlDbType.VarChar, Convert.ToString( obj.ExternalURL) );
         objSqlHelper.AddParameterToSQLCommand("@SystemURL", SqlDbType.VarChar, Convert.ToString(obj.SystemURL));
         objSqlHelper.AddParameterToSQLCommand("@SystemParam", SqlDbType.VarChar, Convert.ToString(obj.SystemParam)); 
         objSqlHelper.AddParameterToSQLCommand("@SystemControl", SqlDbType.VarChar, Convert.ToString(obj.SystemControl));  
         objSqlHelper.AddParameterToSQLCommand("@DateUpdated", SqlDbType.VarChar, Convert.ToString( obj.DateUpdated) );
         objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString( obj.Active) );
         objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString( obj.SortOrder) );
         objSqlHelper.AddParameterToSQLCommand("@Notes", SqlDbType.VarChar, Convert.ToString(obj.Notes));
         objSqlHelper.AddParameterToSQLCommand("@UseExactTitle", SqlDbType.VarChar, Convert.ToString(obj.UseExactTitle));
         objSqlHelper.AddParameterToSQLCommand("@SystemLayout", SqlDbType.VarChar, Convert.ToString(obj.SystemLayout));

         objSqlHelper.AddParameterToSQLCommand("@PageContentAlt", SqlDbType.VarChar, Convert.ToString(obj.PageContentAlt));
         objSqlHelper.AddParameterToSQLCommand("@MenuCaptionAlt", SqlDbType.VarChar, Convert.ToString(obj.MenuCaptionAlt));
        
        objSqlHelper.GetExecuteNonQueryByCommand(sSql);
        objSqlHelper.Dispose();

    }
    public static void Delete(int ID)
    { 
        ResetCache(); 
        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "DELETE FROM " + SiteSettings.TABLE_PREFIX + "Pages WHERE ID = @ID;"; 
        objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(ID));
        objSqlHelper.GetExecuteNonQueryByCommand(sSql);
        objSqlHelper.Dispose(); 
    }

    public static void ResetCache() {
        SiteCache.Remove("WebPage_GetAll");
        WebPage.Instance = null;
    }
    public List<WebPage> GetAll()
    {
        return this.GetListFromDS(this.GetDataSet("Select * From " + SiteSettings.TABLE_PREFIX + "Pages  WHERE SITE_ID = '" + SiteSettings.SITE_ID + "'  Order By SortOrder Asc ", "WebPage_GetAll"));
    }   
    public override WebPage LoadFromDataRow(DataRow oDataRow)
    {
        WebPage obj = new WebPage();
        obj.ID = Convert.ToInt32(oDataRow["ID"]);
        obj.Parent = Convert.ToInt32(oDataRow["Parent"]);
        obj.Name = Convert.ToString(oDataRow["Name"]);
        obj.MenuCaption = Convert.ToString(oDataRow["MenuCaption"]);
        obj.SubCaption = Convert.ToString(oDataRow["SubCaption"]);  
        obj.ShortDesc = Convert.ToString(oDataRow["Description"]);
        obj.Keywords = Convert.ToString(oDataRow["Keywords"]);
        obj.Title = Convert.ToString(oDataRow["Title"]);
        obj.Header = Convert.ToString(oDataRow["Header"]);
        obj.PageContent = Convert.ToString(oDataRow["PageContent"]);
        obj.MainMenu = Convert.ToString(oDataRow["MainMenu"]);
        obj.ExternalURL = Convert.ToString(oDataRow["ExternalURL"]); 
        obj.SystemURL = Convert.ToString(oDataRow["SystemURL"]);
        obj.SystemParam = Convert.ToString(oDataRow["SystemPar