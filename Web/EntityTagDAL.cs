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


public class EntityTagDAL: ASPXUtils.Data.DataEntity<EntityTag> 
{
    public EntityTagDAL() { }


    public static int Add(EntityTag obj)  {
        ResetCache(); 
        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "";
            sSql = String.Format(@" INSERT INTO  {0}EntityTags(		
                Name,		
                Entity,		
                Active,		
                SortOrder,		
                DateCreated,		
                DateUpdated,		
                SITE_ID		
            )VALUES(		
                @Name,		
                @Entity,		
                @Active,		
                @SortOrder,		
                getdate(),		
                getdate(),		
                @SITE_ID		
            ); 
            SELECT SCOPE_IDENTITY() ;", SiteSettings.TABLE_PREFIX); 
            objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString( obj.Name) );
            objSqlHelper.AddParameterToSQLCommand("@Entity", SqlDbType.VarChar, Convert.ToString( obj.Entity) );
            objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString( obj.Active) );
            objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToInt32( obj.SortOrder) );
 
            objSqlHelper.AddParameterToSQLCommand("@SITE_ID", SqlDbType.VarChar, Convert.ToString(SiteSettings.SITE_ID));

        int _id = objSqlHelper.GetExecuteScalarByCommand(sSql);
        objSqlHelper.Dispose(); 
        return _id; 
    }
    public static void Update(EntityTag obj) {
        ResetCache();

        SqlHelper objSqlHelper = new SqlHelper();
        string   sSql= String.Format(@" Update  {0}EntityTags SET 
            Name = @Name,
            Entity = @Entity,
            Active = @Active,
            SortOrder = @SortOrder, 
            DateUpdated = getdate(),
            SITE_ID = @SITE_ID
        WHERE ID = @ID;", SiteSettings.TABLE_PREFIX );

        objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToInt32( obj.ID) );
        objSqlHelper.AddParameterToSQLCommand("@Name", SqlDbType.VarChar, Convert.ToString( obj.Name) );
        objSqlHelper.AddParameterToSQLCommand("@Entity", SqlDbType.VarChar, Convert.ToString( obj.Entity) );
        objSqlHelper.AddParameterToSQLCommand("@Active", SqlDbType.VarChar, Convert.ToString( obj.Active) );
        objSqlHelper.AddParameterToSQLCommand("@SortOrder", SqlDbType.VarChar, Convert.ToInt32( obj.SortOrder) ); 
        objSqlHelper.AddParameterToSQLCommand("@SITE_ID", SqlDbType.VarChar, Convert.ToString(SiteSettings.SITE_ID));

        objSqlHelper.GetExecuteNonQueryByCommand(sSql);
        objSqlHelper.Dispose();

    }
    public static void Delete(int ID)   { 
        ResetCache(); 
        SqlHelper objSqlHelper = new SqlHelper();
        string sSql = "";
        sSql = " DELETE FROM  " + SiteSettings.TABLE_PREFIX + "EntityTags WHERE ID = @ID;";
        sSql += " DELETE FROM  " + SiteSettings.TABLE_PREFIX + "EntityTagMap WHERE EntityTagID = @ID;";
        objSqlHelper.AddParameterToSQLCommand("@ID", SqlDbType.VarChar, Convert.ToString(ID));
        objSqlHelper.GetExecuteNonQueryByCommand(sSql);
        objSqlHelper.Dispose(); 
    }

    public static void MapEntityToTags( int EntityID, List<EntityTag> oTags)
    {
        if (oTags == null)
            return;

        ResetCache();
        SqlHelper objSqlHelper = new SqlHelper(); 
        string sSql = String.Format(@" DELETE FROM  {0}EntityTagMap 
            WHERE   EntityID=@EntityID AND Entity=@Entity AND SITE_ID=@SITE_ID;    
        ", SiteSettings.TABLE_PREFIX) ;

        int cnt=0;
        string sEntity = "";
        foreach (EntityTag et in oTags)
        {
            sEntity = et.Entity;
            sSql += String.Format(@" INSERT INTO  {0}EntityTagMap(		
                EntityTagID,		
                EntityID,
                Entity,	 		
                SITE_ID		
            )VALUES(		
                @EntityTagID{1},		
                @EntityID,	
                @Entity,		 		
                @SITE_ID  ); ", SiteSettings.TABLE_PREFIX, cnt.ToString());

            objSqlHelper.AddParameterToSQLCommand("@EntityTagID" + cnt.ToString(), SqlDbType.VarChar, et.ID.ToString());
            cnt++;
        }
        objSqlHelper.AddParameterToSQLCommand("@EntityID", SqlDbType.VarChar, Convert.ToInt32(EntityID));
        objSqlHelper.AddParameterToSQLCommand("@Entity", SqlDbType.VarChar, Convert.ToString(sEntity));
        objSqlHelper.AddParameterToSQLCommand("@SITE_ID" , SqlDbType.VarChar, Convert.ToString(SiteSettings.SITE_ID));
 
        objSqlHelper.GetExecuteNonQueryByCommand(sSql);
        objSqlHelper.Dispose();
       
    }

    public static void ResetCache()  { 
        SiteCache.Remove("EntityTag_GetAll");
    }    
  
    public List<EntityTag> GetAll()
    {
        string _sql = "Select * From  " + SiteSettings.TABLE_PREFIX + "EntityTags";
        return base.GetListFromDS(base.GetDataSet(_sql, "EntityTag_GetAll"));
    }  
    public override EntityTag LoadFromDataRow(DataRow oDataRow )
    {
        EntityTag obj = new EntityTag();
        obj.ID = Convert.ToInt32( oDataRow["ID"] );
        obj.Name = Convert.ToString( oDataRow["Name"] );
        obj.Entity = Convert.ToString( oDataRow["Entity"] );
        obj.Active = Convert.ToString( oDataRow["Active"] );
        obj.SortOrder = Convert.ToInt32( oDataRow["SortOrder"] );
        obj.DateCreated = Convert.ToDateTime( oDataRow["DateCreated"] ); 
        return obj;
    }

}// End Class 


