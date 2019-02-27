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
using WebSite.Core;
using ASPXUtils;

namespace WebSite.Core
{
    public   class ProdCategoryDAL : ASPXUtils.Data.DataEntity<ProdCategory>
    {
        public ProdCategoryDAL() { }


        public static int Add(ProdCategory obj)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql += " INSERT INTO " + SiteSettings.TABLE_PREFIX + "ProdCategories( Parent, LOC, Name,Description,SortOrder, Active, ImageURL, SITE_ID)";
            sSql += " VALUES( @Parent, @LOC, @Name, @Description, @SortOrder, @Active, @ImageURL, @SITE_ID); ";
            sSql += " SELECT SCOPE_IDENTITY() ;";

            objSqlHelper.AddParameterToSQLCommand("@Parent", SqlDbType.VarChar, Convert.ToString(obj.Parent));
            objSqlHelper.AddParameterToSQLCommand("@ImageURL", SqlDbType.VarChar, Convert.ToString(obj.ImageURL));
            objSqlHelper.AddParameterToSQLCommand("@LOC", SqlDbType.VarChar, Convert.ToString(obj.LOC));
            objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString(obj.Name));
            objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString(obj.Description));
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString(obj.SortOrder));
            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString(obj.Active));
            objSqlHelper.AddParameterToSQLCommand("@SITE_ID", SqlDbType.VarChar, SiteSettings.SITE_ID);

            int _id = objSqlHelper.GetExecuteScalarByCommand(sSql);
            objSqlHelper.Dispose();

            return _id;

        }
        public static void Update(ProdCategory obj)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql = " Update " + SiteSettings.TABLE_PREFIX + "ProdCategories SET ";
            sSql += " Parent = @Parent, Active = @Active,  LOC = @LOC, Name = @Name, Description = @Description,SortOrder = @SortOrder, ImageURL = @ImageURL";
            sSql += " WHERE ID = @ID;";

            objSqlHelper.AddParameterToSQLCommand("@Parent", SqlDbType.VarChar, Convert.ToString(obj.Parent));
            objSqlHelper.AddParameterToSQLCommand("@ImageURL", SqlDbType.VarChar, Convert.ToString(obj.ImageURL));
            objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(obj.ID));
            objSqlHelper.AddParameterToSQLCommand("@LOC", SqlDbType.VarChar, Convert.ToString(obj.LOC));
            objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString(obj.Name));
            objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString(obj.Description));
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString(obj.SortOrder));
            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString(obj.Active));
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();

        }
        public static void Delete(int ID)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql += " DELETE FROM " + SiteSettings.TABLE_PREFIX + "ProdItemToCats WHERE CatID = @ID; ";
            sSql += " DELETE FROM " + SiteSettings.TABLE_PREFIX + "ProdCategories WHERE Parent = @ID; ";
            sSql += " DELETE FROM " + SiteSettings.TABLE_PREFIX + "ProdCategories WHERE ID = @ID; ";
            objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(ID));
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();

        }

        public static void AddMenuItem(int ItemID, int CatID)
        {
            AddMenuItem(ItemID, CatID, 0);
        }

        public static void AddMenuItem(int ItemID, int CatID, int Sort)
        { 
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql += " DELETE FROM " + SiteSettings.TABLE_PREFIX + "ProdItemToCats WHERE CatID = @CatID AND ItemID = @ItemID ; ";
            sSql += " INSERT INTO " + SiteSettings.TABLE_PREFIX + "ProdItemToCats( CatID, ItemID, Sort  ) VALUES( @CatID, @ItemID, @Sort ); ";
            objSqlHelper.AddParameterToSQLCommand("@Sort", SqlDbType.VarChar, Convert.ToString(Sort));
            objSqlHelper.AddParameterToSQLCommand("@ItemID", SqlDbType.VarChar, Convert.ToString(ItemID));
            objSqlHelper.AddParameterToSQLCommand("@CatID", SqlDbType.VarChar, Convert.ToString(CatID));
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();
        }

        public static void ResetCache()
        {
            SiteCache.Remove("MenuItem_GetAll");
            SiteCache.Remove("MenuCategory_GetAll");
            SiteCache.Remove("FULL_MENU");
        }


        public static DataView GetOrdered()
        { 
            DataSet objDataSet = (DataSet)SiteCache.GetDataSet("FULL_MENU"); 
            if (objDataSet == null)
            {
                string _sql = "  Select * From vwFullMenu WHERE SITE_ID = '" + SiteSettings.SITE_ID + "'  Order By SortOrder ASC, MASTERID ASC, Parent DESC, SortOrder2 ASC, ID ASC, CatItemSort ASC ";
                SqlHelper objSqlHelper = new SqlHelper();
                objDataSet = objSqlHelper.GetDatasetByCommand(_sql);
                objSqlHelper.Dispose(); 
                SiteCache.Add("FULL_MENU", objDataSet);
            }
            DataView oDataView = objDataSet.Tables[0].DefaultView;
            return oDataView;
        }

        public  List<ProdCategory> GetAll()
        {
            string sSql = "Select * From " + SiteSettings.TABLE_PREFIX + "ProdCategories WHERE SITE_ID = '" + SiteSettings.SITE_ID + "' Order By " + SiteSettings.TABLE_PREFIX + "ProdCategories.SortOrder ASC ";
            return this.GetListFromDS(this.GetDataSet(sSql, "MenuCategory_GetAll"));
        }
       public override ProdCategory LoadFromDataRow(DataRow oDataRow)
        { 
            ProdCategory obj = new ProdCategory();
            obj.ID = Convert.ToInt32(oDataRow["ID"]);
            obj.Parent = Convert.ToInt32(oDataRow["Parent"]);
            obj.ImageURL = Convert.ToString(oDataRow["ImageURL"]);
            obj.Active = Convert.ToString(oDataRow["Active"]);
            obj.LOC = Convert.ToString(oDataRow["LOC"]);
            obj.Name = Convert.ToString(oDataRow["Name"]);
            obj.Description = Convert.ToString(oDataRow["Description"]);
            obj.SortOrder = Convert.ToInt32(oDataRow["SortOrder"]);
            return obj; 
       }  
    }// End Class  
}