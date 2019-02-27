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

public class ArticleDAL : ASPXUtils.Data.DataEntity<Article>
{
    public ArticleDAL() { }


    public static int Add(Article obj)
    {
        ResetCache();

        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "";
        sSql += " INSERT INTO " + SiteSettings.TABLE_PREFIX + "Articles( SITE_ID, Title,ShortDesc,Content,Active,DateCreated,DateUpdated, ReleaseDate )VALUES( @SITE_ID, @Title, @ShortDesc, @Content, @Active, getdate(), getdate(), @ReleaseDate); SELECT SCOPE_IDENTITY() ;";

        objSqlHelper.AddParameterToSQLCommand("@Title", SqlDbType.VarChar, Convert.ToString( obj.Title) );
        objSqlHelper.AddParameterToSQLCommand("@ShortDesc", SqlDbType.VarChar, Convert.ToString(obj.ShortDesc)); 
        objSqlHelper.AddParameterToSQLCommand("@Content", SqlDbType.VarChar, Convert.ToString( obj.Content) );
        objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString( obj.Active) );
        objSqlHelper.AddParameterToSQLCommand("@SITE_ID", SqlDbType.VarChar, SiteSettings.SITE_ID);
        objSqlHelper.AddParameterToSQLCommand("@ReleaseDate", SqlDbType.DateTime, obj.ReleaseDate);
        
        int _id = objSqlHelper.GetExecuteScalarByCommand(sSql);
        objSqlHelper.Dispose();
        obj.ID = _id;
        ArticleCategoryDAL.MapToCats(obj);
        return _id;

    }
    public static void Update(Article obj)
    {
        ResetCache();

        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "";
        sSql = " Update " + SiteSettings.TABLE_PREFIX + "Articles SET Title = @Title, ShortDesc = @ShortDesc, ReleaseDate=@ReleaseDate, Content = @Content,Active = @Active, DateUpdated = getdate() WHERE ID = @ID;";

        objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString( obj.ID) );
        objSqlHelper.AddParameterToSQLCommand("@Title", SqlDbType.VarChar, Convert.ToString( obj.Title) );
        objSqlHelper.AddParameterToSQLCommand("@ShortDesc", SqlDbType.VarChar, Convert.ToString(obj.ShortDesc)); 
        objSqlHelper.AddParameterToSQLCommand("@Content", SqlDbType.VarChar, Convert.ToString( obj.Content) );
        objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString( obj.Active) );
        objSqlHelper.AddParameterToSQLCommand("@ReleaseDate", SqlDbType.DateTime, obj.ReleaseDate);
        
        objSqlHelper.GetExecuteNonQueryByCommand(sSql);
        ArticleCategoryDAL.MapToCats(obj);
        objSqlHelper.Dispose();

    }
    public static void Delete(int ID)
    { 
        ResetCache(); 
        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "";
        sSql = " DELETE FROM " + SiteSettings.TABLE_PREFIX + "Articles WHERE ID = @ID; DELETE FROM " + SiteSettings.TABLE_PREFIX + "ArticleToCats WHERE AID = @ID;";
        objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(ID));
        objSqlHelper.GetExecuteNonQueryByCommand(sSql);
        objSqlHelper.Dispose();

    }

    public static void ResetCache()
    {
        SiteCache.Remove("Article_GetAll");  
    } 

    public List<Article> GetAll()
    {
        string _sql = "";
        _sql += " Select " + SiteSettings.TABLE_PREFIX + "Articles.* , ACID  From " + SiteSettings.TABLE_PREFIX + "Articles";
        _sql += " LEFT OUTER JOIN " + SiteSettings.TABLE_PREFIX + "ArticleToCats ON " + SiteSettings.TABLE_PREFIX + "ArticleToCats.AID = " + SiteSettings.TABLE_PREFIX + "Articles.ID  ";
        _sql += " WHERE " + SiteSettings.TABLE_PREFIX + "Articles.SITE_ID = '" + SiteSettings.SITE_ID + "' ORDER BY " + SiteSettings.TABLE_PREFIX + "Articles.ID ";
        return GetListFromHelper(new List<SqlParameter>(), _sql, "Article_GetAll");

    } 
    private List<Article> GetListFromHelper(List<SqlParameter> oSqlParameters, string _sql, string _CacheSettingName)
    {
        DataSet objDataSet = this.GetDataSet(_sql, _CacheSettingName);
        int TempID = 0;
        List<Article> objList = new List<Article>();
        Article obj = null;
        ArticleCategory AC = null;
        foreach (DataRow oDataRow in objDataSet.Tables[0].Rows)
        {

            if (TempID != Convert.ToInt32( oDataRow["ID"] ))
            {
                TempID = Convert.ToInt32(oDataRow["ID"]);
                obj = LoadFromDataRow(oDataRow);
                obj.ArticleCategories = new List<ArticleCategory>();
                objList.Add(obj); 
            }

            if (oDataRow["ACID"] != DBNull.Value)    {
                AC = new ArticleCategory().Select(Convert.ToInt32(oDataRow["ACID"])); 
            }  else {
                AC = new ArticleCategory() { ID = 0, Title = "Uncategorized", Active = "true" };
            }

            objList[objList.Count - 1].ArticleCategories.Add(AC);
        } 
        return objList;
    }


    public override Article LoadFromDataRow(DataRow oDataRow)
    {
        Article obj = new Article(); 
        obj.ID = Convert.ToInt32(oDataRow["ID"]);
        obj.Title = Convert.ToString(oDataRow["Title"]);
        obj.ShortDesc = Convert.ToString(oDataRow["ShortDesc"]); 
        obj.Content = Convert.ToString(oDataRow["Content"]);
        obj.Active = Convert.ToString(oDataRow["Active"]);
        obj.DateCreated = Convert.ToDateTime(oDataRow["DateCreated"]);
        obj.DateUpdated = Convert.ToDateTime(oDataRow["DateUpdated"]);
        if (oDataRow["ReleaseDate"] != DBNull.Value)
            obj.ReleaseDate = Convert.ToDateTime(oDataRow["ReleaseDate"]); 
        return obj;
    }
}// End Class 


