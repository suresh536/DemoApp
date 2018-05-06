using Softlabs.bo;
using Softlabs.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.DataAccess
{
    public class DBCommon
    {
        public string connStr = String.Empty;
        string SP_M_APPCONFIG_GETCONFIGVALUE = "M_APPCONFIG_GETCONFIGVALUE";
        string SP_Category_GetContentCount = "Category_GetContentCount";

        string SP_PARAM_RETURNVALUE = "@ReturnValue";
        string SP_PARAM_CONFIGNAME = "@configNAME";
        string SP_PARAM_CATEGORYID = "@CategoryID";

        public DBCommon()
        {
            //connStr = ConfigurationSettings.AppSettings["corporateConnection"];
            connStr = ConfigurationManager.ConnectionStrings["SchoolDB"].ConnectionString;
        }
        public String InsertUpdate(String SP, Hashtable ht)
        {
            DataManager dmObj = new DataManager();
            dmObj.ConnString = connStr;
            dmObj.ID = "1";
            string strMessage = dmObj.ProcessData(SP, ht);
            return strMessage;
        }

        public string DeleteRecord(String SP, Hashtable ht)
        {

            DataManager dmObj = new DataManager();
            dmObj.ConnString = connStr;
            dmObj.ID = "1";
            string strMessage = dmObj.ProcessData(SP, ht);
            return strMessage;
        }

        public string Rolename(int roleid)
        {
            DataTable Dt = new DataTable();
            string varRoleName = string.Empty;
            string varroleid = string.Empty;
            SPDataAccess spObj = new SPDataAccess(connStr);
            Hashtable ht = new Hashtable();
            ht.Add("ROLEID", roleid);
            Dt = spObj.GetDataTableForSP("SP_ROLENAME", ht);
            if (Dt.Rows.Count > 0)
                varroleid = Dt.Rows[0]["PKID"].ToString().Trim();
            varRoleName = Dt.Rows[0]["RoleName"].ToString().Trim();
            return varRoleName;
        }

        public string Pagenamename(string strPagename, string roleid)
        {
            Hashtable ht = new Hashtable();
            string varRoleID = string.Empty;
            string RoleID = string.Empty;
            ht.Add("Pagename", strPagename);
            DataSet ds = new DataSet();
            ds = DataSetforSP("Role_GetRoleFromPageName", ht);
            if (ds.Tables[0].Rows.Count > 0)
            {
                varRoleID = ds.Tables[0].Rows[0]["rolefkid"].ToString().Trim();
                ht.Clear();
                ht.Add("RoleIdComma", varRoleID);
                ht.Add("RoleId", roleid);
                ds = DataSetforSP("Role_CheckForPage", ht);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    RoleID = ds.Tables[0].Rows[0]["tocken_id"].ToString().Trim();
                }
            }

            return RoleID;
            //Role_CheckForPage
        }


     

        public DataTable TableforSP(String SP, Hashtable ht)
        {
            DataTable dt;
            SPDataAccess dbm = new SPDataAccess(connStr);
            dt = dbm.GetDataTableForSP(SP, ht);
            return dt;
        }

        public DataSet DataSetforSP(String SP, Hashtable ht)
        {
            DataSet dS;
            SPDataAccess dbm = new SPDataAccess(connStr);
            dS = dbm.GetDataSetForSP(SP, ht);
            return dS;
        }

        public int GetContentCountForCategory(int categoryid)
        {

            int intcount = 0;
            SqlDataReader Dr;
            SqlParameter objReurnValueParam = SqlHelper.CreateParameter(SP_PARAM_RETURNVALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            Dr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.StoredProcedure, SP_Category_GetContentCount,
                SqlHelper.CreateParameter(SP_PARAM_CATEGORYID, SqlDbType.Int, categoryid, ParameterDirection.Input, 4, false), objReurnValueParam);
            while (Dr.Read())
            {
                if (Dr["contentcount"] != DBNull.Value)
                {
                    intcount = Convert.ToInt32(Dr["contentcount"]);
                }
            }
            return intcount;
        }
        public int ReturnIntforSP(String SP, Hashtable ht)
        {
            int i;
            SPDataAccess dbm = new SPDataAccess(connStr);
            i = dbm.GetIntResultForSP(SP, ht);
            return i;
        }
        public int Delete(int intId)
        {
            int intRetVal = 0;
            SqlParameter objReurnValueParam = SqlHelper.CreateParameter(SP_PARAM_RETURNVALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.StoredProcedure, "User_Content_delete",
            SqlHelper.CreateParameter("@USERID", SqlDbType.Int, intId, ParameterDirection.Input, 0, false), objReurnValueParam);
            if (objReurnValueParam != null)
            {
                intRetVal = (int)objReurnValueParam.Value;
            }
            return intRetVal;
        }

        public string GetAppConfigValue(string strnName)
        {

            string strname = string.Empty;
            SqlDataReader Dr;
            SqlParameter objReurnValueParam = SqlHelper.CreateParameter(SP_PARAM_RETURNVALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            Dr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.StoredProcedure, SP_M_APPCONFIG_GETCONFIGVALUE,
                SqlHelper.CreateParameter(SP_PARAM_CONFIGNAME, SqlDbType.NVarChar, strnName, ParameterDirection.Input, 100, false), objReurnValueParam);
            while (Dr.Read())
            {
                if (Dr["configvalue"] != DBNull.Value)
                {
                    strname = Dr["configvalue"].ToString();
                }
            }
            return strname;
        }


        public string GetUserName(int userid)
        {

            string strname = string.Empty;
            SqlDataReader Dr;
            SqlParameter objReurnValueParam = SqlHelper.CreateParameter(SP_PARAM_RETURNVALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            Dr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.StoredProcedure, "User_GetName",
                SqlHelper.CreateParameter("@PKId", SqlDbType.Int, userid, ParameterDirection.Input, 4, false), objReurnValueParam);
            while (Dr.Read())
            {
                if (Dr["UserName"] != DBNull.Value)
                {
                    strname = Dr["UserName"].ToString();
                }
            }
            return strname;
        }
        public int DeleteMapping(int mappingId)
        {
            int intRetVal = 0;
            SqlParameter objReurnValueParam = SqlHelper.CreateParameter(SP_PARAM_RETURNVALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.StoredProcedure, "Concours_Mapping_DELETE",
            SqlHelper.CreateParameter("@mappingID", SqlDbType.Int, mappingId, ParameterDirection.Input, 0, false), objReurnValueParam);
            if (objReurnValueParam != null)
            {
                intRetVal = (int)objReurnValueParam.Value;
            }
            return intRetVal;
        }

        


        public int newPageurl(string strPageurl, string roleid)
        {
            int cnt = 0;
            Hashtable ht = new Hashtable();
            string varRoleID = string.Empty;
            string RoleID = string.Empty;
            ht.Add("url", strPageurl);
            ht.Add("roleid", roleid);
            DataSet ds = new DataSet();
            ds = DataSetforSP("Role_GetRoleFromPageUrlcount", ht);
            if (ds.Tables[0].Rows.Count > 0)
            {

                cnt = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }
            return cnt;

        }

        public int auth_newPageurl(string strPageurl, string roleid)
        {
            int cnt = 0;
            Hashtable ht = new Hashtable();
            string varRoleID = string.Empty;
            string RoleID = string.Empty;
            ht.Add("url", strPageurl);
            ht.Add("roleid", roleid);
            DataSet ds = new DataSet();
            ds = DataSetforSP("auth_GetRoleFromPageUrlcount", ht);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cnt = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }
            return cnt;
        }


        public string auth_Rolename(int roleid)
        {
            DataTable Dt = new DataTable();
            string varRoleName = string.Empty;
            string varroleid = string.Empty;
            SPDataAccess spObj = new SPDataAccess(connStr);
            Hashtable ht = new Hashtable();
            ht.Add("ROLEID", roleid);
            Dt = spObj.GetDataTableForSP("auth_ROLENAME", ht);
            if (Dt.Rows.Count > 0)
                varroleid = Dt.Rows[0]["roleid"].ToString().Trim();
            varRoleName = Dt.Rows[0]["RoleName"].ToString().Trim();
            return varRoleName;
        }


        public DataTable TableforSP()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DataTable TableforSP(string p)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object InsertUpdate(string p)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
