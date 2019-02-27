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
    public   class ProdItemDAL : ASPXUtils.Data.DataEntity<ProdItem>
    {
        public ProdItemDAL() { }


        public static int Add(ProdItem obj)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql += " INSERT INTO " + SiteSettings.TABLE_PREFIX + "ProdItems ";
            sSql += " (ItemName,Description, Comments, ImageURL,Price,PriceInfo,Active,SortOrder, LOC, ItemType, DisplayType, IsVeg, IsFeatured, SITE_ID ) ";
            sSql += " VALUES(  @ItemName, @Description, @Comments, @ImageURL, @Price, @PriceInfo, @Active, @SortOrder, @LOC, @ItemType, @DisplayType,  @IsVeg, @IsFeatured, @SITE_ID ); ";
            sSql += " SELECT SCOPE_IDENTITY() ; ";

            objSqlHelper.AddParameterToSQLCommand("@ItemName", SqlDbType.VarChar, Convert.ToString(obj.ItemName));
            objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString(obj.Description));
            objSqlHelper.AddParameterToSQLCommand("@Comments", SqlDbType.VarChar, Convert.ToString(obj.Comments));
            objSqlHelper.AddParameterToSQLCommand("@ImageURL", SqlDbType.VarChar, Convert.ToString(obj.ImageURL));
            objSqlHelper.AddParameterToSQLCommand("@Price", SqlDbType.VarChar, Convert.ToString(obj.Price));
            objSqlHelper.AddParameterToSQLCommand("@PriceInfo", SqlDbType.VarChar, Convert.ToString(obj.PriceInfo));
            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString(obj.Active));
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString(obj.SortOrder));
            objSqlHelper.AddParameterToSQLCommand("@LOC", SqlDbType.VarChar, Convert.ToString(obj.LOC));
            objSqlHelper.AddParameterToSQLCommand("@ItemType", SqlDbType.VarChar, Convert.ToString(obj.ItemType));
            objSqlHelper.AddParameterToSQLCommand("@DisplayType", SqlDbType.VarChar, Convert.ToString(obj.DisplayType));
            objSqlHelper.AddParameterToSQLCommand("@IsVeg", SqlDbType.VarChar, Convert.ToString(obj.IsVeg));
            objSqlHelper.AddParameterToSQLCommand("@IsFeatured", SqlDbType.VarChar, Convert.ToString(obj.IsFeatured));
            objSqlHelper.AddParameterToSQLCommand("@SITE_ID", SqlDbType.VarChar, SiteSettings.SITE_ID);
            int _id = objSqlHelper.GetExecuteScalarByCommand(sSql);
            objSqlHelper.Dispose();

            UpdateCategories(_id, obj.MenuCategories);

            return _id;

        }

        public static int GetMenuCatSort(int ItemID, int CatID)
        {
            string _sql = "";
            int _sort = 0;
            SqlHelper objSqlHelper = new SqlHelper();
            _sql = " Select " + SiteSettings.TABLE_PREFIX + "ProdItemToCats.Sort FROM " + SiteSettings.TABLE_PREFIX + "ProdItemToCats WHERE ItemID = " + Convert.ToString(ItemID) + " AND CATID = " + Convert.ToString(CatID) + "; ";
            SqlDataReader objSqlDataReader = objSqlHelper.GetReaderByCmd(_sql);
            while (objSqlDataReader.Read())
            {
                if (objSqlDataReader["Sort"] != DBNull.Value)
                    _sort = Convert.ToInt32(objSqlDataReader["Sort"]);
            }
            objSqlDataReader.Close();
            objSqlHelper.Dispose();
            return _sort;
        }

        public static void UpdateCategories(int _id, List<ProdCategory> mCats)
        {

            SqlHelper objSqlHelper = new SqlHelper();

            string sSql = "";
            sSql += " DELETE FROM " + SiteSettings.TABLE_PREFIX + "ProdItemToCats WHERE ItemID = @ID; ";
            if (mCats != null)
            {
                foreach (ProdCategory m in mCats)
                {
                    sSql += " INSERT INTO " + SiteSettings.TABLE_PREFIX + "ProdItemToCats (ItemID, CatID, Sort) ";
                    sSql += " VALUES(  @ID , '" + Convert.ToString(m.ID) + "', '" + GetMenuCatSort(_id, m.ID) + "'); ";
                }
            }
            objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(_id));
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();

        }


        public static void Update(ProdItem obj)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql += " Update " + SiteSettings.TABLE_PREFIX + "ProdItems SET ";
            sSql += " ItemName = @ItemName,Description = @Description, Comments = @Comments, ImageURL = @ImageURL, LOC = @LOC, ";
            sSql += " Price = @Price,PriceInfo = @PriceInfo,Active = @Active,SortOrder = @SortOrder, ItemType = @ItemType, DisplayType = @DisplayType, IsVeg = @IsVeg, IsFeatured = @IsFeatured ";
            sSql += " WHERE ID = @ID; ";

            objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(obj.ID));
            objSqlHelper.AddParameterToSQLCommand("@ItemName", SqlDbType.VarChar, Convert.ToString(obj.ItemName));
            objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString(obj.Description));
            objSqlHelper.AddParameterToSQLCommand("@Comments", SqlDbType.VarChar, Convert.ToString(obj.Comments));
            objSqlHelper.AddParameterToSQLCommand("@ImageURL", SqlDbType.VarChar, Convert.ToString(obj.ImageURL));
            objSqlHelper.AddParameterToSQLCommand("@Price", SqlDbType.VarChar, Convert.ToString(obj.Price));
            objSqlHelper.AddParameterToSQLCommand("@PriceInfo", SqlDbType.VarChar, Convert.ToString(obj.PriceInfo));
            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString(obj.Active));
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString(obj.SortOrder));
            objSqlHelper.AddParameterToSQLCommand("@LOC", SqlDbType.VarChar, Convert.ToString(obj.LOC));
            objSqlHelper.AddParameterToSQLCommand("@DisplayType", SqlDbType.VarChar, Convert.ToString(obj.DisplayType));
            objSqlHelper.AddParameterToSQLCommand("@ItemType", SqlDbType.VarChar, Convert.ToString(obj.ItemType));
            objSqlHelper.AddParameterToSQLCommand("@IsVeg", SqlDbType.VarChar, Convert.ToString(obj.IsVeg));
            objSqlHelper.AddParameterToSQLCommand("@IsFeatured", SqlDbType.VarChar, Convert.ToString(obj.IsFeatured));
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();

            UpdateCategories(obj.ID, obj.MenuCategories);

        }

        public static void UpdateCatSort(int CATID, int ItemID, int Sort)
        {

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql += " Update " + SiteSettings.TABLE_PREFIX + "ProdItemToCats SET  Sort = @Sort ";
            sSql += " WHERE ItemID = @ItemID AND CATID = @CATID ; ";
            objSqlHelper.AddParameterToSQLCommand("@Sort", SqlDbType.VarChar, Convert.ToString(Sort));
            objSqlHelper.AddParameterToSQLCommand("@ItemID", SqlDbType.VarChar, Convert.ToString(ItemID));
            objSqlHelper.AddParameterToSQLCommand("@CATID", SqlDbType.VarChar, Convert.ToString(CATID));
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();
            ResetCache();
        }

        public static void Delete(int ID)
        {
            ResetCache();

            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql += " DELETE FROM " + SiteSettings.TABLE_PREFIX + "ProdItemToCats WHERE ItemID = @ID; ";
            sSql += " DELETE FROM " + SiteSettings.TABLE_PREFIX + "ProdItems WHERE ID = @ID; ";
            objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(ID));
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();

        }

        public static void ResetCache()
        { 
            SiteCache.Remove("MenuCategory_GetAll");
            SiteCache.Remove("MenuItem_GetAll");
            SiteCache.Remove("FULL_MENU");
        }
        public List<ProdItem> GetAll()
        {
            //string sSql = "  Select * FROM " + SiteSettings.TABLE_PREFIX + "ProdItems  WHERE SITE_ID = '" + SiteSettings.SITE_ID + "'  Order By SortOrder Asc   "; 
            //return this.GetListFromDS(this.GetDataSet(sSql, "MenuItem_GetAll"));
   
            string sSql = "   Select " + SiteSettings.TABLE_PREFIX + "ProdItems.*, " + SiteSettings.TABLE_PREFIX + "ProdItemToCats.CatID, ISNULL(" + SiteSettings.TABLE_PREFIX + "ProdItemToCats.Sort,0) Sort From " + SiteSettings.TABLE_PREFIX + "ProdItems  ";
            sSql += " Left Outer Join " + SiteSettings.TABLE_PREFIX + "ProdItemToCats ON  " + SiteSettings.TABLE_PREFIX + "ProdItems.ID = " + SiteSettings.TABLE_PREFIX + "ProdItemToCats.ItemID ";
            sSql += "   WHERE SITE_ID = '" + SiteSettings.SITE_ID + "'";
            sSql += "   Order By ID Asc  , CatID Asc , Sort Asc   ";

            return GetListFromHelper(new List<SqlParameter>(), sSql, "MenuItem_GetAll");

        }

        private static List<ProdItem> GetListFromHelper(List<SqlParameter> oSqlParameters, string _sql, string _CacheSettingName)
        {
            DataSet objDataSet = (DataSet)SiteCache.GetDataSet(_CacheSettingName); 
            if (objDataSet == null)
            {
                SqlHelper objSqlHelper = new SqlHelper();
                foreach (SqlParameter p in oSqlParameters) objSqlHelper.AddParameterToSQLCommand(p.ParameterName, SqlDbType.VarChar, Convert.ToString(p.Value)); 
                objDataSet = objSqlHelper.GetDatasetByCommand(_sql);
                objSqlHelper.Dispose();
                SiteCache.Add(_CacheSettingName, objDataSet);
            }

            List<ProdItem> objList = new List<ProdItem>();
            ProdItem obj = new ProdItem();

            List<ProdCategory> _MenuCategoryList = new List<ProdCategory>();

            int _id = -1;

            foreach (DataRow oDataRow in objDataSet.Tables[0].Rows)
            {

                if (_id != Convert.ToInt32(oDataRow["ID"]))// ONLY ONCE PER MENU ITEM
                {
                    if (_id != -1)
                    {
                        obj.MenuCategories = _MenuCategoryList;
                        objList.Add(obj);
                        _MenuCategoryList = new List<ProdCategory>();
                    }
                    obj = new ProdItem();
                    obj.ID = Convert.ToInt32(oDataRow["ID"]);
                    obj.ItemName = Convert.ToString(oDataRow["ItemName"]);
                    obj.Description = Convert.ToString(oDataRow["Description"]);
                    obj.Comments = Convert.ToString(oDataRow["Comments"]);
                    obj.ImageURL = Convert.ToString(oDataRow["ImageURL"]);
                    obj.Price = Convert.ToDecimal(oDataRow["Price"]);
                    obj.PriceInfo = Convert.ToString(oDataRow["PriceInfo"]);
                    obj.Active = Convert.ToString(oDataRow["Active"]);
                    obj.SortOrder = Convert.ToInt32(oDataRow["SortOrder"]);
                    obj.LOC = Convert.ToString(oDataRow["LOC"]);
                    obj.ItemType = Convert.ToString(oDataRow["ItemType"]);
                    obj.DisplayType = Convert.ToString(oDataRow["DisplayType"]);
                    obj.IsVeg = Convert.ToString(oDataRow["IsVeg"]);
                    obj.IsFeatured = Convert.ToString(oDataRow["IsFeatured"]);
                    obj.CatSort = Convert.ToInt32(oDataRow["Sort"]);
                    _id = Convert.ToInt32(oDataRow["ID"]);

                }

                if (oDataRow["CatID"] != System.DBNull.Value)// FOR EVERY CAT
                {
                    ProdCategory mc = new ProdCategory().Select(Convert.ToInt32(oDataRow["CatID"]));
                    mc.ItemCatSort = Convert.ToInt32(oDataRow["Sort"]);
                    _MenuCategoryList.Add(mc);
                }

            }
            obj.MenuCategories = _MenuCategoryList;
            objList.Add(obj);

            return objList;
        }
        public override ProdItem LoadFromDataRow(DataRow oDataRow)
        {
            ProdItem obj = new ProdItem();
            obj.ID = Convert.ToInt32(oDataRow["ID"]);
            obj.ItemName = Convert.ToString(oDataRow["ItemName"]);
            obj.Description = Convert.ToString(oDataRow["Description"]);
            obj.Comments = Convert.ToString(oDataRow["Comments"]);
            obj.ImageURL = Convert.ToString(oDataRow["ImageURL"]);
            obj.Price = Convert.ToDecimal(oDataRow["Price"]);
            obj.PriceInfo = Convert.ToString(oDataRow["PriceInfo"]);
            obj.Active = Convert.ToString(oDataRow["Active"]);
            obj.SortOrder = Convert.ToInt32(oDataRow["SortOrder"]);
            obj.LOC = Convert.ToString(oDataRow["LOC"]);
            obj.ItemType = Convert.ToString(oDataRow["ItemType"]);
            obj.DisplayType = Convert.ToString(oDataRow["DisplayType"]);
            obj.IsVeg = Convert.ToString(oDataRow["IsVeg"]);
            obj.IsFeatured = Convert.ToString(oDataRow["IsFeatured"]);
            obj.CatSort = Convert.ToInt32(oDataRow["Sort"]); 
            return obj;
        }  
 
    }// End Class 


}