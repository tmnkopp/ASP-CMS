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
    public class GalleryCategoryDAL : ASPXUtils.Data.DataEntity<GalleryCategory>
    {
        public GalleryCategoryDAL() { }


        public static int Add(GalleryCategory obj)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql += " INSERT INTO " + SiteSettings.TABLE_PREFIX + "GalleryCategories(SITE_ID,Name,ShortDesc,GalleryType,Description,DateCreated,DateUpdated,Active,SortOrder, DisplayType, ImgMaxHeight, ImgMaxWidth )VALUES( @SITE_ID, @Name, @ShortDesc, @GalleryType, @Description, getdate(), getdate(), @Active, @SortOrder, @DisplayType, @ImgMaxHeight, @ImgMaxWidth); SELECT SCOPE_IDENTITY() ;";

            objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString(obj.Name));
            objSqlHelper.AddParameterToSQLCommand("@ShortDesc", SqlDbType.VarChar, Convert.ToString(obj.ShortDesc)); 
            objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString(obj.Description));
            objSqlHelper.AddParameterToSQLCommand("@GalleryType", SqlDbType.VarChar, Convert.ToString(obj.GalleryType));
            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString(obj.Active));
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString(obj.SortOrder));
            objSqlHelper.AddParameterToSQLCommand("@SITE_ID", SqlDbType.VarChar, SiteSettings.SITE_ID);
            objSqlHelper.AddParameterToSQLCommand("@DisplayType", SqlDbType.VarChar, Convert.ToString(obj.DisplayType));
            objSqlHelper.AddParameterToSQLCommand("@ImgMaxHeight", SqlDbType.VarChar, Convert.ToString(obj.ImgMaxHeight));
            objSqlHelper.AddParameterToSQLCommand("@ImgMaxWidth", SqlDbType.VarChar, Convert.ToString(obj.ImgMaxWidth));

            int _id = objSqlHelper.GetExecuteScalarByCommand(sSql);
            EntityTagDAL.MapEntityToTags( _id, obj.EntityTagCollection);
            objSqlHelper.Dispose();

            return _id;

        }
        public static void Update(GalleryCategory obj)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql = " Update " + SiteSettings.TABLE_PREFIX + @"GalleryCategories SET Name = @Name, ShortDesc = @ShortDesc, GalleryType = @GalleryType
                        , Description = @Description,DateUpdated = getdate() ,Active = @Active,SortOrder = @SortOrder
                        , DisplayType = @DisplayType,ImgMaxHeight=@ImgMaxHeight, ImgMaxWidth=@ImgMaxWidth WHERE CATID = @CATID;";

            objSqlHelper.AddParameterToSQLCommand("@CATID", SqlDbType.VarChar, Convert.ToString(obj.CATID));
            objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString(obj.Name));
            objSqlHelper.AddParameterToSQLCommand("@ShortDesc", SqlDbType.VarChar, Convert.ToString(obj.ShortDesc)); 
            objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString(obj.Description));
            objSqlHelper.AddParameterToSQLCommand("@GalleryType", SqlDbType.VarChar, Convert.ToString(obj.GalleryType));
            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString(obj.Active));
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString(obj.SortOrder));
            objSqlHelper.AddParameterToSQLCommand("@DisplayType", SqlDbType.VarChar, Convert.ToString(obj.DisplayType));
            objSqlHelper.AddParameterToSQLCommand("@ImgMaxHeight", SqlDbType.VarChar, Convert.ToString(obj.ImgMaxHeight));
            objSqlHelper.AddParameterToSQLCommand("@ImgMaxWidth", SqlDbType.VarChar, Convert.ToString(obj.ImgMaxWidth));

            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            EntityTagDAL.MapEntityToTags(obj.CATID, obj.EntityTagCollection);
            objSqlHelper.Dispose();

        }
        public static void Delete(int ID)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql = " DELETE FROM " + SiteSettings.TABLE_PREFIX + "GalleryCategories WHERE CATID = @CATID;";
            objSqlHelper.AddParameterToSQLCommand("@CATID", SqlDbType.VarChar, Convert.ToString(ID));
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();

        }
        public static void ResetCache()
        {
            SiteCache.Remove("GalleryCategory_GetAll");
        }

        public List<GalleryCategory> GetAll()
        {
            string sql = String.Format(@"Select * From {0}GalleryCategories
                            LEFT JOIN (SELECT 
                            {0}EntityTags.ID as EntityTagID
                            , {0}EntityTags.Name
                            , {0}EntityTags.ShortDesc
                            , {0}EntityTags.HTMLDesc
                            , {0}EntityTags.SortOrder
                            , {0}EntityTags.Entity
                            , {0}EntityTagMap.EntityID
                            From {0}EntityTags
                            INNER JOIN {0}EntityTagMap 
	                            ON {0}EntityTags.ID = {0}EntityTagMap.EntityTagID ) VW

                            ON VW.EntityID = CATID
                            WHERE {0}GalleryCategories.SITE_ID = '{1}'
                            ", SiteSettings.TABLE_PREFIX, SiteSettings.SITE_ID);
           // return this.GetListFromDS(this.GetDataSet("Select * From " + SiteSettings.TABLE_PREFIX + "GalleryCategories WHERE SITE_ID = '" + SiteSettings.SITE_ID + "' Order By SortOrder Asc", "GalleryCategory_GetAll"));

            return GetListFromHelper(new List<SqlParameter>(), sql, "GalleryCategory_GetAll");

        }
        private List<GalleryCategory> GetListFromHelper(List<SqlParameter> oSqlParameters, string _sql, string _CacheSettingName)
        {
            DataSet objDataSet = this.GetDataSet(_sql, _CacheSettingName);
            int TempID = 0;
            List<GalleryCategory> objList = new List<GalleryCategory>();
            GalleryCategory obj = null;
            EntityTag ET = null;
            foreach (DataRow oDataRow in objDataSet.Tables[0].Rows)
            { 
                if (TempID != Convert.ToInt32(oDataRow["CATID"]))
                {
                    TempID = Convert.ToInt32(oDataRow["CATID"]);
                    obj = LoadFromDataRow(oDataRow);
                    obj.EntityTagCollection = new List<EntityTag>();
                    objList.Add(obj);
                }

                if (oDataRow["EntityTagID"] != DBNull.Value) 
                    ET = new EntityTag().Select(Convert.ToInt32(oDataRow["EntityTagID"])); 
                else
                    ET = new EntityTag() { ID = 0 };
                  
                objList[objList.Count - 1].EntityTagCollection.Add(ET);
            }
            return objList;
        }


 
        public override GalleryCategory LoadFromDataRow(DataRow oDataRow)
        {
            GalleryCategory obj = new GalleryCategory();
            obj.CATID = Convert.ToInt32(oDataRow["CATID"]);
            obj.Name = Convert.ToString(oDataRow["Name"]); 
            obj.ShortDesc = Convert.ToString(oDataRow["ShortDesc"]);
            obj.GalleryType = Convert.ToString(oDataRow["GalleryType"]);
            obj.Description = Convert.ToString(oDataRow["Description"]);
            obj.DateCreated = Convert.ToDateTime(oDataRow["DateCreated"]);
            obj.DateUpdated = Convert.ToDateTime(oDataRow["DateUpdated"]);
            obj.Active = Convert.ToString(oDataRow["Active"]);
            obj.SortOrder = Convert.ToInt32(oDataRow["SortOrder"]);
            obj.DisplayType = Convert.ToString(oDataRow["DisplayType"]);
            obj.ImgMaxHeight = Convert.ToString(oDataRow["ImgMaxHeight"]);
            obj.ImgMaxWidth = Convert.ToString(oDataRow["ImgMaxWidth"]);
  
            obj.GalleryImages = new GalleryImage().SelectByCatID(obj.CATID); 
            return obj;
        }
    }// End Class 


}