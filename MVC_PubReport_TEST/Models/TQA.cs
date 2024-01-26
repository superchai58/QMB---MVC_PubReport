using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Models.QA
{

    #region TQITS_CompanyCode
    public class TQITS_CompanyCode {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompayLocal { get; set; }
        public string UserID { get; set; }
        public string TransDatetime { get; set; }
    
    }
    #endregion TQITS_CompanyCode
    #region TUser_List
    public class TUser_List
    { 
    
        public string UserID { get; set; }
        public string UserName{ get; set; }
        public string CompanyCode{ get; set; }
        public string PU{ get; set; }
        public string Type{ get; set; }
        public string DepartMent{ get; set; }
        public string UserRole{ get; set; }
        public string Tel{ get; set; }
        public string Email{ get; set; }
        public string Leader{ get; set; }
        public string OpUserID{ get; set; }
        public string TransDateTime { get; set; }
    }
    #endregion TUser_List
    #region TQITS_ModelSetting
    public class TQITS_ModelSetting
    {
        public string PU { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public string CustModel { get; set; }
        public string DVTDay { get; set; }
        public string PVTDay { get; set; }
        public string MPDay { get; set; }
        public string UserID { get; set; }
        public string TransDateTime { get; set; }
        public string QAPIC { get; set; }
        public string CompanyCode { get; set; }
        public Int64 ID { get; set; }
    }
    #endregion TQITS_ModelSetting



    #region TQITS_IssueTypeSetting

    public class TQITS_IssueTypeSetting
    { 
        public string PU{get;set;} 
        public string Type{get;set;}
        public string IssueType{get;set;} 
        public string UserID{get;set;}
        public string TransDateTime { get; set; }
        public Int64 ID { get; set; }
    }
    #endregion TQITS_IssueTypeSetting

#region TQITS_IssueList
    public class TQITS_IssueList
    { 
        public string IssueNO{get;set;}
        public string PU{get;set;}
        public string Type{get;set;}
        public string Model{get;set;}
        public string Line{get;set;}
        public string IssueCategory{get;set;}
        public string IssueType{get;set;}
        public string Yield{get;set;}
        public string RootCasuse{get;set;}
        public string ActionDesc{get;set;}
        public string ResponsibilityCategory{get;set;}
        public string ISHoldSN{get;set;}
        public Int32 HoldSNQty{get;set;}
        public string IssueDate{get;set;}
        public string ScheduleDate { get; set; }
        public string LineOwner{get;set;}
        public string QMOwner{get;set;}
        public string PEOwner{get;set;}
        public string Status{get;set;}
        public string UserID{get;set;}
        public string TransDateTime{get;set;}
        public string CreateDateTime { get; set; }
    
    }
#endregion QITS_IssueList


    public class TQA {
        private DB db;
        private string MISDB_IP;

        public TQA(string key)
        {
            
            db = new DB(key);
            MISDB_IP = System.Configuration.ConfigurationManager.AppSettings["MISDB_IP"];
        }

        #region QITS_IssueTypeSetting
        public List<TQITS_IssueTypeSetting> QITS_IssueTypeSetting_ViewAll(string PU,string Type)
        {
            List<TQITS_IssueTypeSetting> list = new List<TQITS_IssueTypeSetting>();
            string sql = "";

            sql = "SELECT ROW_NUMBER()OVER(ORDER BY IssueType) AS ID, PU,Type,IssueType,UserID,TransDateTime ";
            sql = sql + " FROM dbo.QITS_IssueTypeSetting  WHERE PU ='"+PU+"' AND Type='"+Type+"' ";

            list = DataTableTool.ToList<TQITS_IssueTypeSetting>(db.Execute(sql).Tables[0]);
            return list;

        }

        public List<TQITS_IssueTypeSetting> QITS_IssueTypeSetting_View(string PU, string Type,string IssueType)
        {
            List<TQITS_IssueTypeSetting> list = new List<TQITS_IssueTypeSetting>();
            string sql = "";

            sql = "SELECT ROW_NUMBER()OVER(ORDER BY IssueType) AS ID, PU,Type,IssueType,UserID,TransDateTime ";
            sql = sql + " FROM dbo.QITS_IssueTypeSetting  WHERE PU ='" + PU + "' AND Type='" + Type + "'  AND IssueType=N'"+IssueType+"' ";

            list = DataTableTool.ToList<TQITS_IssueTypeSetting>(db.Execute(sql).Tables[0]);
            return list;

        }
        public List<TQITS_IssueTypeSetting> QITS_IssueTypeSetting_Delete(TQITS_IssueTypeSetting t, string opUserId)
        {

            string sql = "";
            List<TQITS_IssueTypeSetting> list = new List<TQITS_IssueTypeSetting>();

            sql = "DELETE FROM dbo.QITS_IssueTypeSetting  WHERE PU ='" + t.PU + "' AND Type='" + t.Type + "'  AND IssueType=N'" + t.IssueType + "' ";
            db.Execute(sql);
            db.SaveQMSLog("QITS_IssueTypeSetting_Delete", "4", opUserId, "", sql);

            //list = QITS_IssueTypeSetting_ViewAll(t.PU,t.Type);
            return list;

        }

        public List<TQITS_IssueTypeSetting> QITS_IssueTypeSetting_Save(TQITS_IssueTypeSetting t, string opUserId)
        {

            string sql = "";
            List<TQITS_IssueTypeSetting> list = new List<TQITS_IssueTypeSetting>();

            list = QITS_IssueTypeSetting_View(t.PU, t.Type,t.IssueType);
            if (list.Count > 0)
            {
                QITS_IssueTypeSetting_Delete(t, opUserId);
            }

            sql = "INSERT INTO dbo.QITS_IssueTypeSetting(PU, Type, IssueType, UserID, TransDateTime) " +
                    " VALUES('"+ t.PU +"','"+t.Type+"',N'"+t.IssueType+"','"+opUserId+"',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
            db.Execute(sql);


            list = QITS_IssueTypeSetting_ViewAll(t.PU, t.Type);
            return list;

        }
        #endregion QITS_IssueTypeSetting

        #region QITS_IssueList
        public List<TQITS_IssueList> QITS_IssueListGetDataByModel(string PU, string Type,string Model)
        {
            List<TQITS_IssueList> list = new List<TQITS_IssueList>();
            string sql = "";

            sql = "SELECT  IssueNO, PU, Type, Model, Line, IssueCategory, IssueType, Yield, RootCasuse, ActionDesc, ResponsibilityCategory, ISHoldSN, HoldSNQty, IssueDate, ScheduleDate, LineOwner, QMOwner, PEOwner, Status, UserID, TransDateTime, CreateDateTime ";
            sql = sql + "  FROM ms.dbo.QITS_IssueList WHERE PU ='" + PU + "' AND Type='" + Type + "' AND Model ='" + Model + "' ORDER BY TransDateTime DESC ";

            list = DataTableTool.ToList<TQITS_IssueList>(db.Execute(sql).Tables[0]);
            return list;

        }

        public List<TQITS_IssueList> QITS_IssueListGetDataByUserRole(string PU, string Type, string UserRole,string UserID)
        {
            List<TQITS_IssueList> list = new List<TQITS_IssueList>();
            string sql = "";

            if (UserRole == "0") //Leader 可以看所属员工的未Close的资料
            {
                sql = "SELECT  IssueNO, a.PU, a.Type, Model, Line, IssueCategory, IssueType, Yield, RootCasuse, ActionDesc, ResponsibilityCategory, ISHoldSN, HoldSNQty, IssueDate, ScheduleDate, LineOwner, QMOwner, PEOwner, Status, UserID, TransDateTime, CreateDateTime ";
                sql = sql + " dbo.QITS_IssueList a,User_List b WHERE  a.PU = b.PU AND a.Type = b.Type and a. PU ='" + PU + "' AND a.Type='" + Type + "' AND Status <>'Close' AND b.Leader = '"+ UserID +"' ORDER BY TransDateTime DESC ";

            }
            else if (UserRole == "1") // 助理可以看未Close的资料
            {
                sql = "SELECT  IssueNO, PU, Type, Model, Line, IssueCategory, IssueType, Yield, RootCasuse, ActionDesc, ResponsibilityCategory, ISHoldSN, HoldSNQty, IssueDate, ScheduleDate, LineOwner, QMOwner, PEOwner, Status, UserID, TransDateTime, CreateDateTime ";
                sql = sql + "  FROM ms.dbo.QITS_IssueList WHERE PU ='" + PU + "' AND Type='" + Type + "' AND Status <>'Close' ORDER BY TransDateTime DESC ";

            }
            else //工程师只能看自己的未close的资料
            {
                sql = "SELECT  IssueNO, PU, Type, Model, Line, IssueCategory, IssueType, Yield, RootCasuse, ActionDesc, ResponsibilityCategory, ISHoldSN, HoldSNQty, IssueDate, ScheduleDate, LineOwner, QMOwner, PEOwner, Status, UserID, TransDateTime, CreateDateTime ";
                sql = sql + "  FROM ms.dbo.QITS_IssueList WHERE PU ='" + PU + "' AND Type='" + Type + "' AND QMOwner ='" + UserID + "' and Status <>'Close' ORDER BY TransDateTime DESC ";

            }

           
            list = DataTableTool.ToList<TQITS_IssueList>(db.Execute(sql).Tables[0]);
            return list;

        }


        public List<TQITS_IssueList> QITS_IssueListSave(string PU, string Type,string UserID, TQITS_IssueList t)
        {
            List<TQITS_IssueList> list = new List<TQITS_IssueList>();
            string QITS_IssueNO;

            string sql = "exec [QITS_GenQITSIssueNO]";
            QITS_IssueNO = db.Execute(sql).Tables[0].Rows[0]["QITSIssueNO"].ToString();
            t.IssueNO = QITS_IssueNO;

            sql = "INSERT INTO dbo.QITS_IssueList(IssueNO, PU, Type, Model, Line, IssueCategory, IssueType, Yield, RootCasuse,";
            sql = sql + "ActionDesc, ResponsibilityCategory, ISHoldSN, HoldSNQty, IssueDate, ScheduleDate, LineOwner, QMOwner, PEOwner, ";
            sql = sql + "Status, UserID, TransDateTime, CreateDateTime)";

            sql = sql + "VALUES('"+t.IssueNO+"','"+PU+"','"+Type+"','"+t.Model+"','"+t.Line+"','"+t.IssueCategory+"','"+t.IssueType + 
                  "','"+ t.Yield +"',N'"+t.RootCasuse+"',N'"+t.ActionDesc+"',N'"+t.ResponsibilityCategory+"','"+t.ISHoldSN+"', "+t.HoldSNQty+
                  " ,'" + t.IssueDate + "','" + t.ScheduleDate + "',N'" + t.LineOwner + "',N'" + t.QMOwner + "',N'" + t.PEOwner + "','" + t.Status + "','" + UserID + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";

            db.Execute(sql);

            sql = "SELECT  IssueNO, PU, Type, Model, Line, IssueCategory, IssueType, Yield, RootCasuse, ActionDesc, ResponsibilityCategory, ISHoldSN, HoldSNQty, IssueDate, ScheduleDate, LineOwner, QMOwner, PEOwner, Status, UserID, TransDateTime, CreateDateTime ";
            sql = sql + "  FROM ms.dbo.QITS_IssueList WHERE IssueNO = '"+QITS_IssueNO+"' ORDER BY TransDateTime DESC ";

            list = DataTableTool.ToList<TQITS_IssueList>(db.Execute(sql).Tables[0]);
            return list; 
        }

        public List<TQITS_IssueList> QITS_IssueListUpdate(string PU, string Type, string UserID,  TQITS_IssueList t)
        {
            List<TQITS_IssueList> list = new List<TQITS_IssueList>();
          

            string sql = "UPDATE dbo.QITS_IssueList "+
                    "SET RootCasuse = N'"+t.RootCasuse+"',ActionDesc = N'"+t.ActionDesc+"',ResponsibilityCategory = N'"+t.ResponsibilityCategory+"', IssueCategory=N'"+t.IssueCategory+
                    "',IssueType = N'"+t.IssueType+"',ISHoldSN='"+t.ISHoldSN+"',HoldSNQty ="+t.HoldSNQty+",ScheduleDate='"+t.ScheduleDate+"',LineOwner=N'"+t.LineOwner+"',"+
                        "PEOwner=N'"+t.PEOwner+"',Status='"+t.Status+"',UserID ='"+UserID+"',TransDateTime = dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS')" +
                     "WHERE IssueNO = '"+t.IssueNO+"'";


         
            db.Execute(sql);

            sql = "SELECT  IssueNO, PU, Type, Model, Line, IssueCategory, IssueType, Yield, RootCasuse, ActionDesc, ResponsibilityCategory, ISHoldSN, HoldSNQty, IssueDate, ScheduleDate, LineOwner, QMOwner, PEOwner, Status, UserID, TransDateTime, CreateDateTime ";
            sql = sql + "  FROM ms.dbo.QITS_IssueList WHERE IssueNO = '" + t.IssueNO + "' ORDER BY TransDateTime DESC ";
            //db.Execute(sql);
            list = DataTableTool.ToList<TQITS_IssueList>(db.Execute(sql).Tables[0]);
            return list;
        }


        public void QITS_IssueListDelete(string opUserId, string IssueNO)
        {
            string sql = "";
            sql = "delete from QITS_IssueList where IssueNO = '"+IssueNO+"'";
            db.Execute(sql);

            db.SaveQMSLog("QITS_IssueListDelete", "4", opUserId, "", sql);
        }
        #endregion QITS_IssueList


        #region QITS_CompanyCode
        public List<TQITS_CompanyCode> QITS_CompanyCode_View(string CompanyCode)
        {
            List<TQITS_CompanyCode> list = new List<TQITS_CompanyCode>();
            string sql = "";

            sql = "SELECT CompanyCode,CompanyName,CompayLocal,UserID FROM QITS_CompanyCode WHERE CompanyCode = '" + CompanyCode + "'";

            
            list = DataTableTool.ToList<TQITS_CompanyCode>( db.Execute(sql).Tables[0]);
            return list;

        }
        public List<TQITS_CompanyCode> QITS_CompanyCode_ViewAll()
        {
            List<TQITS_CompanyCode> list = new List<TQITS_CompanyCode>();
            string sql = "";

            sql = "SELECT CompanyCode,CompanyName,CompayLocal,UserID FROM QITS_CompanyCode ";
            
            list = DataTableTool.ToList<TQITS_CompanyCode>( db.Execute(sql).Tables[0]);
            return list;

        }

        public List<string> QITS_CompanyCode_GetCompanyCode()
        {
            List<string> list = new List<string>();
            DataTable dt = new DataTable();

            string sql = "";

            sql = "SELECT distinct CompanyCode  FROM QITS_CompanyCode order by 1";

            dt = db.Execute(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["CompanyCode"].ToString());
            
            }
            return list;
        }


        public List<TQITS_CompanyCode> QITS_CompanyCode_Delete(TQITS_CompanyCode t, string opUserId)
        {
            
            string sql = "";
            List<TQITS_CompanyCode> list = new List<TQITS_CompanyCode>();

            sql = "delete  FROM QITS_CompanyCode WHERE CompanyCode = '"+ t.CompanyCode +"'";
            db.Execute(sql);
            db.SaveQMSLog("QITS_CompanyCode_Delete", "4", opUserId, "", sql);

            list = QITS_CompanyCode_ViewAll();
            return list; 

        }

        public List<TQITS_CompanyCode> QITS_CompanyCode_Save(TQITS_CompanyCode t,string opUserId)
        {

            string sql = "";
            List<TQITS_CompanyCode> list = new List<TQITS_CompanyCode>();

            list = QITS_CompanyCode_View(t.CompanyCode);
            if (list.Count > 0)
            {
                QITS_CompanyCode_Delete(t, opUserId);
            }
             
            sql = "INSERT INTO dbo.QITS_CompanyCode( CompanyCode ,CompanyName ,CompayLocal ,UserID ,TransDatetime) " +
					" VALUES  ('"+ t.CompanyCode +"',N'"+t.CompanyName+"',N'"+t.CompayLocal+"','"+opUserId+"',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
            db.Execute(sql);
            
           
            list = QITS_CompanyCode_ViewAll();
            return list;

        }
        
        #endregion QITS_CompanyCodes
        #region User_List
        public List<TUser_List> User_List_View(string UserID)
        {
            List<TUser_List> list = new List<TUser_List>();
            string sql = "";

            sql = "SELECT UserID, UserName, CompanyCode, PU, Type, DepartMent, CASE UserRole WHEN 0 THEN N'Leader' WHEN 1 THEN N'助理' WHEN 2 THEN N'工程师' ELSE N'其他' END  AS UserRole, Tel, Email, Leader, OpUserID, TransDateTime FROM [User_List] WHERE UserID= '" + UserID + "'";


            list = DataTableTool.ToList<TUser_List>(db.Execute(sql).Tables[0]);
            return list;

        }
        public List<TUser_List> User_List_ViewAll(string PU, string Type)
        {
            List<TUser_List> list = new List<TUser_List>();
            string sql = "";

            sql = "SELECT UserID, UserName, CompanyCode, PU, Type, DepartMent, CASE UserRole WHEN 0 THEN N'Leader' WHEN 1 THEN N'助理' WHEN 2 THEN N'工程师' ELSE N'其他' END  AS UserRole, Tel, Email, Leader, OpUserID, TransDateTime FROM [User_List] WHERE PU = '" + PU + "' AND Type ='" + Type + "' ";

            list = DataTableTool.ToList<TUser_List>(db.Execute(sql).Tables[0]);
            return list;

        }

        public List<TUser_List> User_List_GetLeader(string PU, string Type)
        {
            List<TUser_List> list = new List<TUser_List>();
            DataTable dt = new DataTable();

            string sql = "";
            sql = "SELECT distinct UserID,UserName  FROM dbo.User_List WHERE CompanyCode='QTA' AND  UserRole ='0' AND PU='" + PU + "' AND Type='" + Type + "' ";

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TUser_List>(dt);
            return list;
        
        }
        public List<TUser_List> User_List_GetQA(string PU, string Type)
        {
            List<TUser_List> list = new List<TUser_List>();
            DataTable dt = new DataTable();

            string sql = "";
            sql = "SELECT distinct UserID,UserName  FROM dbo.User_List WHERE CompanyCode='QTA' AND DepartMent='QM' AND PU='" + PU + "' AND Type='" + Type + "' ";

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TUser_List>(dt);
            return list;

        }

        public List<TUser_List> User_List_Delete(TUser_List t,   string opUserId)
        {

            string sql = "";
            List<TUser_List> list = new List<TUser_List>();

            sql = "DELETE  FROM [User_List] WHERE UserID='"+ t.UserID +"'";
            db.Execute(sql);
            db.SaveQMSLog("User_List_Delete", "4", opUserId, "", sql);

            list = User_List_ViewAll(t.PU,t.Type);
            return list;

        }

        public string User_List_GetNonUserID()
        {
            string UserID = "";
            DataTable dt = new DataTable();
            string sql = "exec QITS_GetNonQTAUserID";

            
            dt = db.Execute(sql).Tables[0];
            UserID = dt.Rows[0]["UserID"].ToString();
            return UserID;
        
        }
        public List<TUser_List> User_List_Save(TUser_List t, string opUserId)
        {

            string sql = "";
            List<TUser_List> list = new List<TUser_List>();

            list = User_List_View(t.UserID);
            if (list.Count > 0)
            {
                User_List_Delete(t,  opUserId);
            }

            sql = "INSERT INTO dbo.User_List(UserID, UserName, CompanyCode, PU, Type, DepartMent, UserRole, Tel, Email, Leader, OpUserID, TransDateTime) " +
                                    " VALUES('" + t.UserID + "',N'" + t.UserName + "','" + t.CompanyCode + "','" + t.PU + "','" + t.Type + "','" + t.DepartMent + "','" +
                                        t.UserRole + "','" + t.Tel + "','" + t.Email + "','" + t.Leader + "','" + opUserId + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
   
            db.Execute(sql);


            list = User_List_ViewAll(t.PU, t.Type);
            return list;

        }
        #endregion User_List
        #region QITS_ModelSetting
        public List<TQITS_ModelSetting> QITS_ModelSetting_ViewAll(string PU, string Type)
        {
            List<TQITS_ModelSetting> list = new List<TQITS_ModelSetting>();
            string sql = "";

            sql = "SELECT ROW_NUMBER()OVER(ORDER BY Model) AS ID, PU, Type, Model, CustModel, DVTDay, PVTDay, MPDay,QAPIC,CompanyCode, UserID, TransDateTime FROM dbo.QITS_ModelSetting WHERE PU ='" + PU + "' AND Type='" + Type + "'";

            list = DataTableTool.ToList<TQITS_ModelSetting>(db.Execute(sql).Tables[0]);
            return list; 
        }
        public List<TQITS_ModelSetting> QITS_ModelSetting_View(TQITS_ModelSetting t)
        {
            List<TQITS_ModelSetting> list = new List<TQITS_ModelSetting>();
            string sql = "";

            sql = "SELECT ROW_NUMBER()OVER(ORDER BY Model) AS ID, PU, Type, Model, CustModel, DVTDay, PVTDay, MPDay,QAPIC,CompanyCode, UserID, TransDateTime FROM dbo.QITS_ModelSetting WHERE PU ='" + t.PU + "' AND Type='" + t.Type + "' AND Model = '" + t.Model + "'";

            list = DataTableTool.ToList<TQITS_ModelSetting>(db.Execute(sql).Tables[0]);
            return list;
        }
        public List<TQITS_ModelSetting> QITS_ModelSetting_Save(TQITS_ModelSetting t,string opUser)
        {
            List<TQITS_ModelSetting> list = new List<TQITS_ModelSetting>();
            string sql = "";
            
            list = QITS_ModelSetting_View(t);
            if (list.Count > 0)
            {
                sql = "UPDATE dbo.QITS_ModelSetting SET CustModel ='" + t.CustModel + "',DVTDay='" + t.DVTDay + "',PVTDay='" + t.PVTDay + "',MPDay='" + t.MPDay + "',UserID='" + opUser + "',TransDateTime=dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),QAPIC='" + t.QAPIC + "',CompanyCode='"+t.CompanyCode+"'";
                sql = sql + " WHERE PU ='" + t.PU + "' AND Type='" + t.Type + "' AND Model = '" + t.Model + "'";
            }
            else
            {
                sql = "INSERT INTO dbo.QITS_ModelSetting(PU, Type, Model, CustModel, DVTDay, PVTDay, MPDay,QAPIC, CompanyCode,UserID, TransDateTime)";
                sql = sql + "VALUES ('" + t.PU + "','" + t.Type + "','" + t.Model + "','" + t.CustModel + "','" + t.DVTDay + "','" + t.PVTDay + "','" + t.MPDay + "','"+t.QAPIC+"','"+t.CompanyCode+"','" + opUser + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
            }
            db.Execute(sql);

            list = QITS_ModelSetting_ViewAll(t.PU,t.Type);
            
            return list;
        
        }

        public void QITS_ModelSetting_Delete(TQITS_ModelSetting t,string opUserId)
        {
            string sql = "";
            sql = "Delete  FROM dbo.QITS_ModelSetting WHERE PU ='" + t.PU + "' AND Type='" + t.Type + "' AND Model = '" + t.Model + "'";
            db.Execute(sql);
            db.SaveQMSLog("QITS_ModelSetting_Delete", "4", opUserId, "", sql);
        
        }

         
        #endregion QITS_ModelSetting
    }

}