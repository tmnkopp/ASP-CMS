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

namespace WebSite.Core { 
public class GalleryImageDAL : ASPXUtils.Data.DataEntity<GalleryImage>
{
    public GalleryImageDAL() { }


    public static int Add(GalleryImage obj)
    {
        ResetCache();

        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "";
        sSql += " INSERT INTO " + SiteSettings.TABLE_PREFIX + "GalleryImages(SITE_ID,CATID,AbsPath,Name,Description,ClickURL,HTMLContent,DateCreated,DateUpdated,Active,SortOrder )VALUES(@SITE_ID,  @CATID, @AbsPath, @Name, @Description, @ClickURL, @HTMLContent, @DateCreated, @DateUpdated, @Active, @SortOrder); SELECT SCOPE_IDENTITY() ;";

                 objSqlHelper.AddParameterToSQLCommand("@CATID", SqlDbType.VarChar, Convert.ToString( obj.CATID) );
         objSqlHelper.AddParameterToSQLCommand("@AbsPath", SqlDbType.VarChar, Convert.ToString( obj.AbsPath) );
         objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString( obj.Name) );
         objSqlHelper.AddParameterToSQLCommand("@HTMLContent", SqlDbType.VarChar, Convert.ToString(obj.HTMLContent));
         objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString( obj.Description) );
         objSqlHelper.AddParameterToSQLCommand("@ClickURL", SqlDbType.VarChar, Convert.ToString(obj.ClickURL));  
         objSqlHelper.AddParameterToSQLCommand("@DateCreated", SqlDbType.VarChar, Convert.ToString( obj.DateCreated) );
         objSqlHelper.AddParameterToSQLCommand("@DateUpdated", SqlDbType.VarChar, Convert.ToString( obj.DateUpdated) );
         objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString( obj.Active) );
         objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString( obj.SortOrder) );
         objSqlHelper.AddParameterToSQLCommand("@SITE_ID", SqlDbType.VarChar, SiteSettings.SITE_ID);
        int _id = objSqlHelper.GetExecuteScalarByCommand(sSql);
        objSqlHelper.Dispose();

        return _id;

    }
    public static void Update(GalleryImage obj)
    {
        ResetCache();

        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "";
        sSql = " Update " + SiteSettings.TABLE_PREFIX + "GalleryImages SET CATID = @CATID,AbsPath = @AbsPath, HTMLContent = @HTMLContent, Name = @Name, ClickURL = @ClickURL , Description = @Description,DateCreated = @DateCreated,DateUpdated = @DateUpdated,Active = @Active,SortOrder = @SortOrder WHERE ID = @ID;";
        
         objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString( obj.ID) );
         objSqlHelper.AddParameterToSQLCommand("@CATID", SqlDbType.VarChar, Convert.ToString( obj.CATID) );
         objSqlHelper.AddParameterToSQLCommand("@AbsPath", SqlDbType.VarChar, Convert.ToString( obj.AbsPath) );
         objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString( obj.Name) );
         objSqlHelper.AddParameterToSQLCommand("@HTMLContent", SqlDbType.VarChar, Convert.ToString(obj.HTMLContent));
        
         objSqlHelper.AddParameterToSQLCommand("@ClickURL", SqlDbType.VarChar, Convert.ToString(obj.ClickURL));  
         objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString( obj.Description) );
         objSqlHelper.AddParameterToSQLCommand("@DateCreated", SqlDbType.VarChar, Convert.ToString( obj.DateCreated) );
         objSqlHelper.AddParameterToSQLCommand("@DateUpdated", SqlDbType.VarChar, Convert.ToString( obj.DateUpdated) );
         objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString( obj.Active) );
         objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString( obj.SortOrder) );

        objSqlHelper.GetExecuteNonQueryByCommand(sSql);
        objSqlHelper.Dispose();

    }
    public static void Delete(int ID)
    { 
        ResetCache();

        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "";
        sSql = " DELETE FROM " + SiteSettings.TABLE_PREFIX + "GalleryImages WHERE ID = @ID;";
        objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(ID));
        objSqlHelper.GetExecuteNonQueryByCommand(sSql);
        objSqlHelper.Dispose();

    }

    public static void DeleteByCAT(int ID)  {
        ResetCache(); 
        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "DELETE FROM " + SiteSettings.TABLE_PREFIX + "GalleryImages WHERE CATID = @ID;"; 
        objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(ID));
        objSqlHelper.GetExecuteNonQueryByCommand(sSql);
        objSqlHelper.Dispose(); 
    } 

    public static void ResetCache()  {
        SiteCache.Remove("GalleryImage_GetAll");
    }
    public List<GalleryImage> GetAll()  {
        return this.GetListFromDS(this.GetDataSet("Select * From " + SiteSettings.TABLE_PREFIX + "GalleryImages WHERE SITE_ID = '" + SiteSettings.SITE_ID + "' Order By SortOrder Asc ", "GalleryImage_GetAll"));
    } 
  
    public override GalleryImage LoadFromDataRow(DataRow oDataRow)
    {
        GalleryImage obj = new GalleryImage();
        obj.ID = Convert.ToInt32(oDataRow["ID"]);
        obj.CATID = Convert.ToInt32(oDataRow["CATID"]);
        obj.AbsPath = Convert.ToString(oDataRow["AbsPath"]);
        obj.Name = Convert.ToString(oDataRow["Name"]);
        obj.ClickURL = Convert.ToString(oDataRow["ClickURL"]);
        obj.HTMLContent = Convert.ToString(oDataRow["HTMLContent"]);
        obj.Description = Convert.ToString(oDataRow["Description"]);
        obj.DateCreated = Convert.ToString(oDataRow["DateCreated"]);
        obj.DateUpdated = Convert.ToString(oDataRow["DateUpdated"]);
        obj.Active = Convert.ToString(oDataRow["Active"]);
        obj.SortOrder = Convert.ToInt32(oDataRow["SortOrder"]);
        
        return obj;
    }
}// End Class 


}