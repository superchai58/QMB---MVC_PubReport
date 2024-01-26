using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Models.ESD
{
    public class TESD
    {
    }

    #region TESD_DeptCode
    public class TESD_DeptCode
    {  
        public string DeptCode{get;set;}
        public string Department{get;set;}
        public string TransID{get;set;}
        public string TransDateTime { get; set; }
        public Int64 ID { get; set; }
    }
    #endregion TESD_DeptCode

    #region TESD_User
    public class TESD_User
    { 
        public string EmployeeID{get;set;}
        public string CardID{get;set;}
        public string EmployeeName{get;set;}
        public string DeptCode{get;set;}
        public string Duty{get;set;}
        public string Phone{get;set;}
        public string Mail{get;set;}
        public string Assistant{get;set;}
        public string Shift{get;set;}
        public string NeedCHK{get;set;}
        public string UserID{get;set;}
        public string TransDateTime{get;set;}
        public string EnterTime { get; set; }
    
    }
    #endregion TESD_User

    public class ESD
    {
       private DB db;
       private string MISDB_IP;


       public ESD(string PU,string Type )
        {
            string key = PU + "_" + Type + "_PubDb";
            db = new DB(key);
            MISDB_IP = System.Configuration.ConfigurationManager.AppSettings["MISDB_IP"];
        }



        #region ESD_DeptCode
       public List<TESD_DeptCode> ESD_DeptCode_View(TESD_DeptCode esd)
       {
           List<TESD_DeptCode> list = new List<TESD_DeptCode>();
           DataTable dt = new DataTable();

           string sql = "";

           sql = "SELECT ROW_NUMBER() OVER(ORDER BY DeptCode) AS ID,DeptCode,Department,TransID,TransDateTime FROM ESD_DeptCode WHERE 1= 1 ";
           if (String.IsNullOrEmpty(esd.DeptCode) == false && string.IsNullOrWhiteSpace(esd.DeptCode) == false)
           {
               sql = sql + "AND  DeptCode ='" + esd.DeptCode + "'";
           
           }
           if (String.IsNullOrEmpty(esd.Department) == false && string.IsNullOrWhiteSpace(esd.Department) == false)
           {
               sql = sql + "AND  Department =N'" + esd.Department + "'";

           }
           sql = sql + " order by DeptCode ";
           dt = db.Execute(sql).Tables[0];
           list = DataTableTool.ToList<TESD_DeptCode>(dt);
           return list;
       
       }

       public List<string> ESD_DeptCode_GetDepartment(string DeptCode)
       {
           string sql = "";
           DataTable dt = new DataTable();
           List<string> list = new List<string>();

           sql = "Select TOP 1 Department from " +MISDB_IP+ ".MIS.dbo.MIS_HR_Employee where DeptCode ='" + DeptCode + "'";
           dt = db.Execute(sql).Tables[0];
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               list.Add(dt.Rows[i]["Department"].ToString());
           
           }
           return list;
                
                
       
       }

       public void ESD_DeptCode_Delete(TESD_DeptCode esd) 
       {
           string sql = "DELETE FROM ESD_DeptCode WHERE DeptCode ='"+ esd.DeptCode +"' AND Department=N'"+ esd.Department +"'";
           db.Execute(sql);

           ESD_TraceDB("DefineDept", "003", esd.DeptCode, sql, esd.TransID);
                
       }

       public void ESD_DeptCode_Save(TESD_DeptCode esd)
       {
           List<TESD_DeptCode> list = new List<TESD_DeptCode>();
           string sql = "";

           list = ESD_DeptCode_View(esd);
           if (list.Count > 0)
           {
               ESD_DeptCode_Delete(esd);
           
           }
           sql = "INSERT INTO dbo.ESD_DeptCode(DeptCode, Department, TransID, TransDateTime)";
           sql = sql + "VALUES ('"+ esd.DeptCode +"',N'"+ esd.Department +"','"+ esd.TransID +"',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";

           db.Execute(sql);
       }

        
        #endregion ESD_DeptCode

       #region ESD_User
       public List<TESD_User> ESD_User_View(TESD_User esd)
       {
           List<TESD_User> list = new List<TESD_User>();
           DataTable dt = new DataTable();

           string sql = "";

           sql = "SELECT EmployeeID, CardID, EmployeeName, DeptCode, Duty, Phone, Mail, Assistant, Shift, NeedCHK, UserID, TransDateTime, EnterTime FROM ESD_User WHERE 1= 1 ";

           if (String.IsNullOrEmpty(esd.DeptCode) == false && string.IsNullOrWhiteSpace(esd.DeptCode) == false)
           {
               sql = sql + "AND  DeptCode ='" + esd.DeptCode + "'";

           }
           if (String.IsNullOrEmpty(esd.EmployeeID) == false && string.IsNullOrWhiteSpace(esd.EmployeeID) == false)
           {
               sql = sql + "AND  EmployeeID ='" + esd.EmployeeID + "'";

           }

           sql = sql + " order by EnterTime desc";
           dt = db.Execute(sql).Tables[0];
           list = DataTableTool.ToList<TESD_User>(dt);
           return list;

       }

       public void ESD_User_Delete(TESD_User esd)
       {
           string sql = "DELETE FROM dbo.ESD_User WHERE EmployeeID ='"+esd.EmployeeID+"'";
           db.Execute(sql);

           ESD_TraceDB("User_Maintain", "003", esd.EmployeeID, sql, esd.UserID);

       }

       public void ESD_User_Save(TESD_User esd)
       {
           List<TESD_User> list = new List<TESD_User>();
           string sql = "";
           sql = "ESD_MaintainUser '" + esd.EmployeeID + "' ,'Y','" + esd.UserID + "'";

           db.Execute(sql);
       }

       #endregion ESD_User

       private void ESD_TraceDB(string Function_Name, string Event_NO, string EmployeeID, string Desc1, string User_Name)
       { 
             Desc1 = Desc1.Replace("'", " ");

            string sql = "INSERT into ESD_log(Function_Name,Event_No,EmployeeID,Desc1,User_Name) " +
                   " values( '" + Function_Name + "','" + Event_NO + "','" +EmployeeID + "',N'" + Desc1 + "','" + User_Name + "') ";
            db.Execute(sql);
       }
       public bool CheckUserRight(string UserID,string SystemName)
       {
           bool result = false;
           string sql = "";
           DataTable dt = new DataTable();

           sql = "SELECT * FROM USER_LIST WHERE SYSTEM_NAME='" + SystemName + "' AND USERID='" + UserID + "'  ";

           dt = db.Execute(sql).Tables[0];

           if (dt.Rows.Count > 0)
           {
               result = true;
           }
           else
           {
               sql = "SELECT * FROM " + MISDB_IP + ".MIS.dbo.MIS_HR_Employee WHERE EmployeeID = '" + UserID + "' AND DeptCode ='0A921'"; //若是QMS 0A921部门的自动有权限。
               dt = db.Execute(sql).Tables[0];

               if (dt.Rows.Count > 0)
               {
                   result = true;
               }
           }

           return result;

       
       }


    }

}