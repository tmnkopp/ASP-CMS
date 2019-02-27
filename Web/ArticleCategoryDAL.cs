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
    public class ArticleCategoryDAL : ASPXUtils.Data.DataEntity<ArticleCategory>
    {
        public ArticleCategoryDAL() { }


        public static int Add(ArticleCategory obj)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = " INSERT INTO " + SiteSettings.TABLE_PREFIX + "ArticleCategories(SITE_ID, Title,ShortDesc,Active,SortOrder,DateCreated,DateUpdated ) VALUES( @SITE_ID,  @Title, @ShortDesc, @Active, @SortOrder, getdate(), getdate()); SELECT SCOPE_IDENTITY() ;";
            
            objSqlHelper.AddParameterToSQLCommand("@ShortDesc", SqlDbType.VarChar, Convert.ToString(obj.ShortDesc));
            objSqlHelper.AddParameterToSQLCommand("@Title", SqlDbType.VarChar, Convert.ToString(obj.Title));
            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString(obj.Active));
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString(obj.SortOrder));
            objSqlHelper.AddParameterToSQLCommand("@SITE_ID", SqlDbType.VarChar, SiteSettings.SITE_ID);
            
            int _id = objSqlHelper.GetExecuteScalarByCommand(sSql);
            objSqlHelper.Dispose();

            return _id;

        }
        public static void Update(ArticleCategory obj)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql = " Update " + SiteSettings.TABLE_PREFIX + "ArticleCategories SET Title = @Title, ShortDesc = @ShortDesc , Active = @Active,SortOrder = @SortOrder, DateUpdated = getdate() WHERE ID = @ID;";

            objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(obj.ID));
            objSqlHelper.AddParameterToSQLCommand("@ShortDesc", SqlDbType.VarChar, Convert.ToString(obj.ShortDesc));
            objSqlHelper.AddParameterToSQLCommand("@Title", SqlDbType.VarChar, Convert.ToString(obj.Title));
            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString(obj.Active));
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString(obj.SortOrder));

            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();

        }
        public static void Delete(int ID)
        {
            ResetCache();
            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql = "DELETE FROM " + SiteSettings.TABLE_PREFIX + "ArticleCategories WHERE ID = @ID; DELETE FROM " + SiteSettings.TABLE_PREFIX + "ArticleToCats WHERE ACID = @ID;";
            objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(ID));
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();
        }
        public static void MapToCats(Article obj)
        {
            ResetCache();
            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "DELETE FROM " + SiteSettings.TABLE_PREFIX + "ArticleToCats WHERE AID = " + obj.ID + ";";
            foreach (ArticleCategory AC in obj.ArticleCategories)
            {
                sSql += " INSERT INTO " + SiteSettings.TABLE_PREFIX + "ArticleToCats (AID, ACID) VALUES (" + obj.ID + "," + AC.ID + "); ";
            }
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();
        }


        public static void ResetCache()
        {
            SiteCache.Remove("ArticleCategory_GetAll");
            SiteCache.Remove("Article_GetAll");
        }

        public List<ArticleCategory> GetAll()
        {
            return this.GetListFromDS(this.GetDataSet("Select * From " + SiteSettings.TABLE_PREFIX + "ArticleCategories WHERE SITE_ID = '" + SiteSettings.SITE_ID + "' Order By SortOrder Asc", "ArticleCategory_GetAll"));
        }

        public override ArticleCategory LoadFromDataRow(DataRow oDataRow)
        {
            ArticleCategory obj = new ArticleCategory();
            obj.ID = Convert.ToInt32(oDataRow["ID"]);
            obj.ShortDesc = Convert.ToString(oDataRow["ShortDesc"]);
            obj.Title = Convert.ToString(oDataRow["Title"]);
            obj.Active = Convert.ToString(oDataRow["Active"]);
            obj.SortOrder = Convert.ToInt32(oDataRow["SortOrder"]);
            obj.DateCreated = Convert.ToDateTime(oDataRow["DateCreated"]);
            obj.DateUpdated = Convert.ToDateTime(oDataRow["DateUpdated"]);
            return obj;
        }
    }// End Class 


}