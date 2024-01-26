using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Models.User
{
    public class TUser
    {
        public string UserID { get; set; }
        public string PassWord { get; set; }
        public string PU { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }

    }


    public class TUserDB
    {
        private DB db;

        public TUserDB(string Key)
        {
            db = new DB(Key);
        }
        /// <summary>
        /// 检查UserID 的账号是否合法
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PassWord">1,2表示查询，3表示修改密码,4查询UserID是否在HR资料中</param>
        /// <returns></returns>
        public List<TUser> ViewUser(string UserID, string PassWord,int opType)
        {
            DataTable dt = null;
            DataSet ds = null;
            List<TUser> userList = new List<TUser>();

            string sql = "EXEC [MVC_PUBReport_CheckAuth] " + opType + ", '" + UserID + "','" + PassWord + "'";
            ds = db.Execute(sql);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                userList = DataTableTool.ToList<TUser>(dt);
            }
           
            db.CloseDB();
            return userList;

        }

        public void RegisterUser(string PU,string Type,string UserID,string PassWord)
        {
            string sql;
            sql = "delete from UT_UserList where UserID='" + UserID + "' ";
            db.Execute(sql);
            sql = "INSERT INTO UT_UserList (PU, Type, UserID, Password, UserDesc, UID, TransDateTime, Department, EMAIL) " +
								"SELECT '"+PU+"','"+Type+"','"+UserID+"','"+PassWord+"',EmployeeName,'Register',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),Department,Mail "+
                                "FROM mis.dbo.MIS_HR_Employee WHERE EmployeeID = '" + UserID + "'";
            db.Execute(sql);
            
        
        }

        public bool CheckAuth(string UserID, string AuthName)
        { 
            bool result;
            string sql;
            DataTable dt = new DataTable();

            result = false;
            sql = "SELECT * FROM USER_LIST WHERE SYSTEM_NAME='" + AuthName + "' AND USERID='" + UserID + "'";
            dt = db.Execute(sql).Tables[0];

            if (dt.Rows.Count > 0)
            {
                result = true;
            }

            return result;
        
        }

      
        
    
    }
}