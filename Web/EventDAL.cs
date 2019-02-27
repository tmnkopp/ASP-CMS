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
using ASPXUtils;
using WebSite.Core;
using ASPXUtils.Data;
 
namespace WebSite.Core
{
 
    public class EventDAL : ASPXUtils.Data.DataEntity<Event>
    {
        public EventDAL() { }

        public static int Add(Event objEvent)
        {
            ResetCache();
            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql += " INSERT INTO " + SiteSettings.TABLE_PREFIX + "Events(SITE_ID, LOC,Name,ShortDesc,Description,ImageURL,EventStartDate,EventEndDate,RecurDays,RecurDOW, Active,Featured,SortOrder )VALUES( @SITE_ID, @LOC, @Name, @ShortDesc, @Description, @ImageURL, @EventStartDate, @EventEndDate, @RecurDays, @RecurDOW,  @Active, @Featured, @SortOrder); SELECT SCOPE_IDENTITY() ;";
            objSqlHelper.AddParameterToSQLCommand("@LOC", SqlDbType.VarChar, Convert.ToString(objEvent.LOC));
            objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString(objEvent.Name));
            objSqlHelper.AddParameterToSQLCommand("@ShortDesc", SqlDbType.VarChar, Convert.ToString(objEvent.ShortDesc));
            objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString(objEvent.Description));
            objSqlHelper.AddParameterToSQLCommand("@ImageURL", SqlDbType.VarChar, Convert.ToString(objEvent.ImageURL));
            objSqlHelper.AddParameterToSQLCommand("@EventStartDate", SqlDbType.VarChar, Convert.ToString(objEvent.EventStartDate));
            objSqlHelper.AddParameterToSQLCommand("@EventEndDate", SqlDbType.VarChar, Convert.ToString(objEvent.EventEndDate));
            objSqlHelper.AddParameterToSQLCommand("@RecurDays", SqlDbType.VarChar, Convert.ToString(objEvent.RecurDays));
            objSqlHelper.AddParameterToSQLCommand("@RecurDOW", SqlDbType.VarChar, Convert.ToString(objEvent.RecurDOW));

            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString(objEvent.Active));
            objSqlHelper.AddParameterToSQLCommand("@Featured", SqlDbType.VarChar, Convert.ToString(objEvent.Featured));
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString(objEvent.SortOrder));
            objSqlHelper.AddParameterToSQLCommand("@SITE_ID", SqlDbType.VarChar, SiteSettings.SITE_ID);
            int _id = objSqlHelper.GetExecuteScalarByCommand(sSql);
            objSqlHelper.Dispose();
            return _id;

        }
        public static void Update(Event objEvent)
        {
            ResetCache();
            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql = " Update " + SiteSettings.TABLE_PREFIX + "Events SET LOC = @LOC,Name = @Name, ShortDesc = @ShortDesc,Description = @Description,ImageURL = @ImageURL,EventStartDate = @EventStartDate,EventEndDate = @EventEndDate,RecurDays = @RecurDays,RecurDOW = @RecurDOW, Active = @Active,Featured = @Featured,SortOrder = @SortOrder WHERE id = @id;";
            objSqlHelper.AddParameterToSQLCommand("@id", SqlDbType.VarChar, Convert.ToString(objEvent.ID));
            objSqlHelper.AddParameterToSQLCommand("@LOC", SqlDbType.VarChar, Convert.ToString(objEvent.LOC));
            objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString(objEvent.Name));
            objSqlHelper.AddParameterToSQLCommand("@ShortDesc", SqlDbType.VarChar, Convert.ToString(objEvent.ShortDesc)); 
            objSqlHelper.AddParameterToSQLCommand("@Description", SqlDbType.VarChar, Convert.ToString(objEvent.Description));
            objSqlHelper.AddParameterToSQLCommand("@ImageURL", SqlDbType.VarChar, Convert.ToString(objEvent.ImageURL));
            objSqlHelper.AddParameterToSQLCommand("@EventStartDate", SqlDbType.VarChar, Convert.ToString(objEvent.EventStartDate));
            objSqlHelper.AddParameterToSQLCommand("@EventEndDate", SqlDbType.VarChar, Convert.ToString(objEvent.EventEndDate));
            objSqlHelper.AddParameterToSQLCommand("@RecurDays", SqlDbType.VarChar, Convert.ToString(objEvent.RecurDays));
            objSqlHelper.AddParameterToSQLCommand("@RecurDOW", SqlDbType.VarChar, Convert.ToString(objEvent.RecurDOW));

            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString(objEvent.Active));
            objSqlHelper.AddParameterToSQLCommand("@Featured", SqlDbType.VarChar, Convert.ToString(objEvent.Featured));
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToString(objEvent.SortOrder));
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose();

        }
        public static void Delete(int ID)
        {
            ResetCache();
            SqlHelper objSqlHelper = new SqlHelper();
            string sSql = "";
            sSql = " DELETE FROM " + SiteSettings.TABLE_PREFIX + "Events WHERE ID = @id;";
            objSqlHelper.AddParameterToSQLCommand("@id", SqlDbType.VarChar);
            objSqlHelper.SetSQLCommandParameterValue("@id", ID);
            objSqlHelper.GetExecuteNonQueryByCommand(sSql);
            objSqlHelper.Dispose(); 
        }

        public static void ResetCache()
        {
            SiteCache.Remove("Events_GetAll"); 
        }
     
        public List<Event> GetAll()
        {
            return this.GetListFromDS(this.GetDataSet("Select * From " + SiteSettings.TABLE_PREFIX + "Events WHERE SITE_ID = '" + SiteSettings.SITE_ID + "' Order By EventStartDate Desc ", "Events_GetAll"));
        }  
        public override Event LoadFromDataRow(DataRow oDataRow)
        {
            Event objEvent = new Event();
            objEvent.ID = Convert.ToInt32(oDataRow["ID"]);
            objEvent.LOC = Convert.ToString(oDataRow["LOC"]);
            objEvent.Name = Convert.ToString(oDataRow["Name"]); 
            objEvent.ShortDesc = Convert.ToString(oDataRow["ShortDesc"]);
            objEvent.Description = Convert.ToString(oDataRow["Description"]);
            objEvent.ImageURL = Convert.ToString(oDataRow["ImageURL"]);
            objEvent.EventStartDate = Convert.ToDateTime(oDataRow["EventStartDate"]);
            objEvent.EventEndDate = Convert.ToDateTime(oDataRow["EventEndDate"]);
            objEvent.RecurDays = Convert.ToInt32(oDataRow["RecurDays"]);
            objEvent.RecurDOW = Convert.ToInt32(oDataRow["RecurDOW"]);

            objEvent.Active = Convert.ToString(oDataRow["Active"]);
            objEvent.Featured = Convert.ToString(oDataRow["Featured"]);
            objEvent.SortOrder = Convert.ToInt32(oDataRow["SortOrder"]);
            return objEvent;
        }
    }// End Class 


}