using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Models.SF
{
    public class TSF
    {
    }

    #region TMail
    public class TMail
    {
        public string System_Name { get; set; }
        public string Mail_Account { get; set; }
        public string Mail_Type { get; set; }
        public string Trans_id { get; set; }
        public string Trans_date { get; set; }
        public Int64 ID { get; set; }
    
    }
    #endregion TMail

    #region TQIMS_DutyInfo
    public class TQIMS_DutyInfo
    {
        public Int64 ID { get; set; }
        public string Type { get; set; }
        public string Line { get; set; }        
        public string Shift { get; set; }
        public string EmployeeID { get; set; }
        public string DutyName { get; set; }
        public string DutyPhone { get; set; }
        public string Remark { get; set; }
       
    }
    #endregion
    #region AssyReport
    public class TAssyReport
    {
        public string Line { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }

    }
    #endregion AssyReport

    #region TWeChart
    public class TWeChart
    {
        public Int64 ID { get; set; }
        public string System_Name { get; set; }
        public string EmployeeID { get; set; }
        public string PU { get; set; }
        public string Type { get; set; }

    }
    #endregion TWeChart
    public class SF
    {  
       private DB db;
       private string MISDB_IP;


       public SF(string key )
        {
            
            db = new DB(key);
            MISDB_IP = System.Configuration.ConfigurationManager.AppSettings["MISDB_IP"];
        }
        #region Mail

        public List<TMail> Mail_View(TMail t)
        {
            List<TMail> list = new List<TMail>();
            DataTable dt = new DataTable();

            string sql = "SELECT ROW_NUMBER()OVER(ORDER BY Mail_Account) AS ID, System_Name,Mail_Account,Mail_Type,Trans_id,Trans_date FROM dbo.Mail WHERE 1= 1 ";

            if (String.IsNullOrEmpty(t.System_Name) == false && string.IsNullOrWhiteSpace(t.System_Name) == false)
            {
                sql = sql + " and  System_Name ='"+ t.System_Name +"'";
            
            }
            if (String.IsNullOrEmpty(t.Mail_Account) == false && string.IsNullOrWhiteSpace(t.Mail_Account) == false)
            {
                sql = sql + " AND Mail_Account ='"+ t.Mail_Account +"'";

            }

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TMail>(dt);

            return list;
        
        }

        public void Mail_Delete(TMail t)
        {
            string sql = "DELETE  FROM dbo.Mail WHERE System_Name ='"+ t.System_Name +"' AND Mail_Account ='"+ t.Mail_Account +"'";
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report_Mail_Delete", "4", t.Trans_id, "", sql);
        }

        public void Mail_Save(TMail t)
        {
            List<TMail> list = new List<TMail>();
            string sql = "";

            list = Mail_View(t);

            if (list.Count > 0)
            {
                Mail_Delete(t);
            
            }

            sql = "INSERT INTO dbo.Mail(System_Name,Mail_Account,Mail_Type,Trans_id,Trans_date)";
            sql = sql + "VALUES('" + t.System_Name + "','" + t.Mail_Account + "','" + t.Mail_Type + "','" + t.Trans_id + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
            db.Execute(sql);
        
        }
        #endregion Mail

        #region QIMS_DutyInfo
        public List<TQIMS_DutyInfo> QIMS_DutyInfo_View(TQIMS_DutyInfo t)
        {
            List<TQIMS_DutyInfo> list = new List<TQIMS_DutyInfo>();
            DataTable dt = new DataTable();

            string sql = " SELECT ROW_NUMBER()OVER(ORDER BY EmployeeID) AS ID, site,type,line,Shift,EmployeeID,DutyName,DutyPhone,Remark FROM  Report.dbo.QIMS_DutyInfo WHERE 1= 1";

            if (String.IsNullOrEmpty(t.Line) == false && string.IsNullOrWhiteSpace(t.Line) == false)
            {
                sql = sql + " and Line = '"+t.Line+"'";
            }
            if (String.IsNullOrEmpty(t.Shift) == false && string.IsNullOrWhiteSpace(t.Shift) == false)
            {
                sql = sql + " and Shift = '" + t.Shift + "'";
            }
            dt = db.Execute(sql).Tables[0];

            list = DataTableTool.ToList<TQIMS_DutyInfo>(dt);
            return list;
        
        }
        public void QIMS_DutyInfo_View_InsertUpdate(TQIMS_DutyInfo t, string UserID)
        {
            if (QIMS_DutyInfo_View(t).Count > 0)
            {
                QIMS_DutyInfo_View_Update(t,UserID);
            }
            else
            { 
                string sql = "insert into Report.dbo.QIMS_DutyInfo(Site, Type, Line, Section, Shift, EmployeeID, DutyName, DutyPhone, Remark, UID, TransDateTime)"
                                + " values('QCMC','" + t.Type + "','" + t.Line + "','','" + t.Shift + "','" + t.EmployeeID + "',N'" + t.DutyName + "','" + t.DutyPhone + "','','"+UserID+"',GETDATE())";
                db.Execute(sql);
            }
        
        }
        public void QIMS_DutyInfo_View_Update(TQIMS_DutyInfo t,string UserID)
        {
            string sql = "UPDATE Report.dbo.QIMS_DutyInfo SET EmployeeID ='" + t.EmployeeID + "' ,DutyName = N'" + t.DutyName + "',DutyPhone='" + t.DutyPhone + "' ,UID='" + UserID + "'  WHERE Line ='" + t.Line + "' AND Shift ='" + t.Shift + "'";
            db.Execute(sql);
        }
        public void QIMS_DutyInfo_View_Delete(TQIMS_DutyInfo t)
        {
            string sql = "Delete Report.dbo.QIMS_DutyInfo  WHERE Line ='" + t.Line + "' AND Shift ='" + t.Shift + "'";
            db.Execute(sql);
        }
        #endregion TQIMS_DutyInfo
        #region AssyReport
        public void AssyReportGetLineModel(string PU, string Type, ref List<string> Model, ref List<string> Line)
        {
            string sql = "exec FindLineModel ";
            DataTable dtModel = new DataTable();
            DataTable dtLine = new DataTable();

            DataSet ds = db.Execute(sql);
            dtLine = ds.Tables[0];
            dtModel = ds.Tables[1];

            for (int i = 0; i < dtLine.Rows.Count; i++)
            {
                Line.Add(dtLine.Rows[i]["Line"].ToString());
            }

            for (int i = 0; i < dtModel.Rows.Count; i++)
            {
                Model.Add(dtModel.Rows[i]["Model"].ToString());
            }
            
        
        }

        public List<TAssyReport> AssyReport_QueryData(string PU, string Type, string SDate, string EDate)
        {

            List<TAssyReport> list = new List<TAssyReport>();
            string sql = "";

            //SDate = SDate + "080000";
            //EDate = EDate + "200000";
            //sql = "EXEC Assy_Report_Pub '','','" + SDate + "','" + EDate;
            sql = "EXEC Assy_Report_Pub '" + PU + "','" + " " + "','" + SDate + "','" + EDate+"'";


            list = DataTableTool.ToList<TAssyReport>(db.Execute(sql).Tables[0]);
            return list;
        }
        #endregion AssyReport

        #region WeChart
        public List<string> GetSystem_Name()
        {
            List<string> list = new List<string>();
            DataTable dt = new DataTable();
            string strSQL = "select distinct System_Name from WeChart with(nolock) order by System_Name";
            dt = db.Execute(strSQL).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["System_Name"].ToString());
            }

                return list;
        
        }
        public List<TWeChart> WeChart_View(TWeChart t)
        {
            List<TWeChart> list = new List<TWeChart>();
            DataTable dt = new DataTable();

            string sql = " SELECT ROW_NUMBER()OVER(ORDER BY Job_Number) AS ID, PU,Type,System_Name,Job_Number as EmployeeID FROM  WeChart with(nolock) WHERE 1= 1";

            if (String.IsNullOrEmpty(t.EmployeeID) == false && string.IsNullOrWhiteSpace(t.EmployeeID) == false)
            {
                sql = sql + " and Job_Number = '" + t.EmployeeID + "'";
            }
            if (String.IsNullOrEmpty(t.System_Name) == false && string.IsNullOrWhiteSpace(t.System_Name) == false)
            {
                sql = sql + " and System_Name = '" + t.System_Name + "'";
            }
            if (String.IsNullOrEmpty(t.PU) == false && string.IsNullOrWhiteSpace(t.PU) == false)
            {
                sql = sql + " and PU = '" + t.PU + "'";
            }
            if (String.IsNullOrEmpty(t.Type) == false && string.IsNullOrWhiteSpace(t.Type) == false)
            {
                sql = sql + " and Type = '" + t.Type + "'";
            }
            dt = db.Execute(sql).Tables[0];

            list = DataTableTool.ToList<TWeChart>(dt);
            return list;
        }

        public void WeChart_Save(TWeChart t, string UserID)
        {
            if (WeChart_View(t).Count > 0)
            {
                WeChart_Delete(t, UserID);
            }
            string sql = "insert into WeChart(System_Name,Job_Number,PU,Type,UID,TransDateTime) values('"+t.System_Name+"','"+t.EmployeeID+"','"+t.PU+"','"+t.Type+"','"+UserID+"',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
            //string sql = "UPDATE QFactory.dbo.QIMS_DutyInfo SET EmployeeID ='" + t.EmployeeID + "' ,DutyName = N'" + t.DutyName + "',DutyPhone='" + t.DutyPhone + "' ,UID='" + UserID + "'  WHERE Line ='" + t.Line + "' AND Shift ='" + t.Shift + "'";
            db.Execute(sql);
        }
        public void WeChart_Delete(TWeChart t,string UserID)
        {
            string sql = "Delete from WeChart where System_Name ='"+t.System_Name+"' and Job_Number='"+t.EmployeeID+"' and PU='"+t.PU+"' and type ='"+t.Type+"'";
            db.Execute(sql);
            SaveQMSLog("WeChart_Delete", "1", UserID, sql.Replace("'",""));
        }
        #endregion WeChart


        private void SaveQMSLog(string Process_Name, string Event_No,string userID,string desc1)
        {

            string sql = "insert into Report.dbo.QMS_LOG(Process_Name,Event_No,Info1,Info2,Desc1,Trans_Date) values('" + Process_Name + "','" + Event_No + "','" + userID + "','','" + desc1 + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
            db.Execute(sql);
        }

    }

    #region LineStop
    public class TLineStop
    {

        public string PU { get; set; }
        public string Type { get; set; }
    }
    public class TLineStopFPY
    {
        public Int64 ID { get; set; }
        public string Model { get; set; }
        public string Station { get; set; }
        public string SeverityLevel { get; set; }
        public string FPY { get; set; }
        public int ErrorCodeCnt { get; set; }
        public int LocationCnt { get; set; }
        public int MinTestCnt { get; set; }
        public string VPY { get; set; }
    }

    public class TLineStopWaitRepair
    {
        public Int64 ID { get; set; }
        public string Line { get; set; }
        public string Status { get; set; }
        public string Station { get; set; }
        public string SeverityLevel { get; set; }
        public string WaitRepair { get; set; }
        public string WaitRepairResume { get; set; }
        public string WaitTest { get; set; }
        public string WaitTestResume { get; set; }
        public string Key { get; set; }
    }
    public class TGroupName
    {
        public Int64 ID { get; set; }
        public string GroupName { get; set; }
        public string desc { get; set; }
        public string Key { get; set; }
    }
    public class TUnWO
    {
        public Int64 ID { get; set; }
        public string WO { get; set; }
        public string UID { get; set; }
        public string Description { get; set; }
        public string ActiveDateTime { get; set; }
        public string ActiveFlag { get; set; }
        public string InActiveDateTime { get; set; }
    }
    public class TUnModel
    {
        public Int64 ID { get; set; }
        public string Line { get; set; }
        public string Model { get; set; }
        public string UID { get; set; }
        public string Description { get; set; }
        public string ActiveDateTime { get; set; }
        public string ActiveFlag { get; set; }
        public string InActiveDateTime { get; set; }
    }
    public class TSeverityLevel
    {
        public Int64 ID { get; set; }
        public string Line { get; set; }
        public string SeverityLevel { get; set; }
        public string Description { get; set; }
    }
    public class TLineStopMail
    {
        public Int64 ID { get; set; }
        public string UID { get; set; }
        public string MailAccount { get; set; }
        public string HostName { get; set; }
        public string MailLevel { get; set; }
        public string FixLevel { get; set; }
    }
    public class TLineStopPhone
    {
        public Int64 ID { get; set; }
        public string UID { get; set; }
        public string PhoneNo { get; set; }
        public int Priority { get; set; }
    }

    public class TLineStopDB
    {
        private DB db;
        public TLineStopDB(string Key)
        {
            db = new DB(Key);
        }
        public List<string> Upload_GetID(string Table)
        {
            List<string> list = new List<string>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = "";
            if (Table == "FPY" || Table == "WaitRepair" || Table == "WaitTest" || Table == "SeverityLevel")
            {
                sql = "EXEC FindLineModel";
            }
            ds = db.Execute(sql);
            if (ds.Tables.Count > 0)
            {
                if (Table == "FPY")
                {
                    dt = ds.Tables[1];
                }
                else
                {

                    dt = ds.Tables[0];
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(dt.Rows[i][0].ToString());
                }
            }
            db.CloseDB();
            return list;
        }
        public List<TLineStopFPY> FPY_View(string PU, string Type, string Model, string Station, string SeverityLevel)
        {
            List<TLineStopFPY> list = new List<TLineStopFPY>();
            DataTable dt = new DataTable();
            string sql;
            if ((PU == "ASBU" || PU == "NB1") && Type == "SMT")
            {
                sql = "Select Model,rtrim(Station) as Station,rtrim(SeverityLevel) as SeverityLevel,FPY,ErrorCodeCnt,LocationCnt,MinTestCnt from LineStopFPY where  Model like '" + Model + "%' and Station Like '" + Station + "%' and SeverityLevel like '" + SeverityLevel + "%' order by model,station,severitylevel";
            }
            else if (PU == "PU5" && Type == "SMT")
            {
                sql = "Select Model,rtrim(Station) as Station,rtrim(SeverityLevel) as SeverityLevel,FPY,ErrorCodeCnt,LocationCnt,MinTestCnt,VPY from LineStopFPY where  Model like '" + Model + "%' and Station Like '" + Station + "%' and SeverityLevel like '" + SeverityLevel + "%' order by model,station,severitylevel";
            }
            else
            {
                sql = "Select Model,rtrim(Station) as Station,rtrim(SeverityLevel) as SeverityLevel,FPY,ErrorCodeCnt,MinTestCnt from LineStopFPY where  Model like '" + Model + "%' and Station Like '" + Station + "%' and SeverityLevel like '" + SeverityLevel + "%' order by model,station,severitylevel";
            }
            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TLineStopFPY>(dt);
            return list;
        }

        public void FPY_Save(TLineStopFPY t, TLineStop m)
        {
            List<TLineStopFPY> list = new List<TLineStopFPY>();
            string sql = "";

            list = FPY_View(m.PU, m.Type, t.Model, t.Station, t.SeverityLevel);

            if (list.Count > 0)
            {
                if ((m.PU == "ASBU" || m.PU == "NB1") && m.Type == "SMT")
                {
                    sql = "Update LineStopFPY Set FPY='" + t.FPY + "', ErrorCodeCnt='" + t.ErrorCodeCnt + "',LocationCnt='" + t.LocationCnt + "',MinTestCnt='" + t.MinTestCnt + "' Where Model='" + t.Model + "' and Station='" + t.Station + "' and SeverityLevel='" + t.SeverityLevel + "'  ";
                }
                else if (m.PU == "PU5" && m.Type == "SMT")
                {
                    sql = "Update LineStopFPY Set FPY='" + t.FPY + "', VPY='" + t.VPY + "',ErrorCodeCnt='" + t.ErrorCodeCnt + "',LocationCnt='" + t.LocationCnt + "',MinTestCnt='" + t.MinTestCnt + "' Where Model='" + t.Model + "' and Station='" + t.Station + "' and SeverityLevel='" + t.SeverityLevel + "'  ";
                }
                else
                {
                    sql = "Update LineStopFPY Set FPY='" + t.FPY + "',ErrorCodeCnt='" + t.ErrorCodeCnt + "',MinTestCnt='" + t.MinTestCnt + "' Where Model='" + t.Model + "' and Station='" + t.Station + "' and SeverityLevel='" + t.SeverityLevel + "'";
                }
            }
            else
            {
                if ((m.PU == "ASBU" || m.PU == "NB1") && m.Type == "SMT")
                {
                    sql = "Insert Into LineStopFPY(Model,Station,SeverityLevel,FPY,ErrorCodeCnt,LocationCnt,MinTestCnt)  Values('" + t.Model + "','" + t.Station + "','" + t.SeverityLevel + "','" + t.FPY + "' ,'" + t.ErrorCodeCnt + "','" + t.LocationCnt + "','" + t.MinTestCnt + "')";
                }
                else if (m.PU == "PU5" && m.Type == "SMT")
                {
                    sql = "Insert Into LineStopFPY(Model,Station,SeverityLevel,FPY,ErrorCodeCnt,LocationCnt,MinTestCnt,VPY)  Values('" + t.Model + "','" + t.Station + "','" + t.SeverityLevel + "','" + t.FPY + "' ,'" + t.ErrorCodeCnt + "','" + t.LocationCnt + "','" + t.MinTestCnt + "','" + t.VPY + "')";
                }
                else
                {
                    sql = "Insert Into LineStopFPY(Model,Station,SeverityLevel,FPY,ErrorCodeCnt,MinTestCnt)  Values('" + t.Model + "','" + t.Station + "','" + t.SeverityLevel + "','" + t.FPY + "' ,'" + t.ErrorCodeCnt + "','" + t.MinTestCnt + "')";
                }
            }
            db.Execute(sql);
        }

        public void WaitRepair_Save(TLineStopWaitRepair t, TLineStop m)
        {
            List<TLineStopWaitRepair> list = new List<TLineStopWaitRepair>();
            string sql = "";
            var tablename = "";
            var Col = "";
            if (t.Key == "WaitRepair")
            {
                tablename = "LineStopWaitRep";
                Col = "status";
            }
            if (t.Key == "WaitTest")
            {
                tablename = "LineStopWaitTest";
                Col = "station";
            }
            list = WaitRepair_View(m.PU, m.Type, t.Line, t.Status, t.SeverityLevel, t.Key);

            if (list.Count > 0)
            {
                sql = "Update " + tablename + " Set " + t.Key + "='" + t.WaitRepair + "'," + t.Key + "Resume='" + t.WaitRepairResume + "' Where Line='" + t.Line + "' and " + Col + "='" + t.Status + "' and SeverityLevel='" + t.SeverityLevel + "' ";
            }
            else
            {

                sql = "Insert Into " + tablename + "(Line," + Col + ",SeverityLevel," + t.Key + "," + t.Key + "Resume)  Values('" + t.Line + "','" + t.Status + "','" + t.SeverityLevel + "','" + t.WaitRepair + "' ,'" + t.WaitRepairResume + "')";

            }
            db.Execute(sql);
        }


        public List<TLineStopWaitRepair> WaitRepair_View(string PU, string Type, string Line, string Status, string SeverityLevel, string Key)
        {
            List<TLineStopWaitRepair> list = new List<TLineStopWaitRepair>();
            DataTable dt = new DataTable();
            string sql = "";
            if (Key == "WaitRepair")
            {
                sql = "Select Line,Status,SeverityLevel,WaitRepair,WaitRepairResume From LineStopWaitRep Where Line like '" + Line + "%' and Status Like '" + Status + "%' and SeverityLevel like '" + SeverityLevel + "%' order by Line,Status,severitylevel";
            }
            if (Key == "WaitTest")
            {
                sql = "Select Line,Station,SeverityLevel,WaitTest,WaitTestResume From LineStopWaitTest Where Line like '" + Line + "%' and Station Like '" + Status + "%' and SeverityLevel like '" + SeverityLevel + "%' order by Line,Station,severitylevel";
            }

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TLineStopWaitRepair>(dt);
            return list;
        }

        public List<TGroupName> GroupName_View(string GroupName)
        {
            List<TGroupName> list = new List<TGroupName>();
            DataTable dt = new DataTable();
            string sql = "";
            sql = "select * from LineStopGroupName Where GroupName like '" + GroupName + "%'";

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TGroupName>(dt);
            return list;
        }

        public List<TUnWO> UnWO_View(string WO)
        {
            List<TUnWO> list = new List<TUnWO>();
            DataTable dt = new DataTable();
            string sql = "";
            sql = "select * from LineStopunWO where WO like '" + WO + "%'";

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TUnWO>(dt);
            return list;
        }

        public List<TUnModel> UnModel_View(string Line, string Model, string Type)
        {
            List<TUnModel> list = new List<TUnModel>();
            DataTable dt = new DataTable();
            string sql = "";
            if (Type == "1")
            {
                sql = "select * from LineStopunModel where line like '" + Line + "%' and Model like'" + Model + "%' order by line,model,activedatetime desc";
            }
            if (Type == "2")
            {
                sql = "select * from LineStopunModel where line like '" + Line + "%' and Model like'" + Model + "%' and activeflag='Y'";
            }

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TUnModel>(dt);
            return list;
        }

        public List<TSeverityLevel> SeverityLevel_View(string Line)
        {
            List<TSeverityLevel> list = new List<TSeverityLevel>();
            DataTable dt = new DataTable();
            string sql = "";
            sql = "Select Line,SeverityLevel From RoutingLine where Line like '" + Line + "%' order by Line";

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TSeverityLevel>(dt);
            return list;
        }

        public List<TLineStopMail> Mail_View(string UID)
        {
            List<TLineStopMail> list = new List<TLineStopMail>();
            DataTable dt = new DataTable();
            string sql = "";
            sql = "Select UID,MailAccount,HostName,MailLevel,FixLevel From LineStopMailUser Where UID like '" + UID + "%' ";

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TLineStopMail>(dt);
            return list;
        }
        public List<TLineStopPhone> Phone_View(string PhoneNo)
        {
            List<TLineStopPhone> list = new List<TLineStopPhone>();
            DataTable dt = new DataTable();
            string sql = "";
            sql = "Select PhoneNo,UID,Priority from LineStopPhone Where PhoneNo like '" + PhoneNo + "%' ";

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TLineStopPhone>(dt);
            return list;
        }
        public string GroupName_Save(TGroupName t, TLineStop m)
        {
            List<TGroupName> list = new List<TGroupName>();
            string sql = "";

            list = GroupName_View(t.GroupName);

            if (list.Count > 0)
            {
                return ("该数据已存在！");
            }
            else
            {
                sql = "Insert Into LineStopGroupName(GroupName,gDESC)  Values('" + t.GroupName + "','" + t.desc + "')";
                db.Execute(sql);
                return ("OK");
            }
        }

        public string UnWO_Save(TUnWO t, TLineStop m)
        {
            List<TUnWO> list = new List<TUnWO>();
            string sql = "";

            list = UnWO_View(t.WO);

            if (list.Count > 0)
            {
                return ("该数据已存在！");
            }
            else
            {
                sql = "Insert Into LineStopUnWO(WO,UID,Description,ActiveDateTime,ActiveFlag)  Values('" + t.WO + "','" + t.UID + "','" + t.Description + "','" + t.ActiveDateTime + "','" + t.ActiveFlag + "')";
                db.Execute(sql);
                return ("OK");
            }
        }

        public string UnModel_Save(TUnModel t, TLineStop m)
        {
            List<TUnModel> list = new List<TUnModel>();
            string sql = "";

            list = UnModel_View(t.Line, t.Model, "2");

            if (list.Count > 0)
            {
                return ("该数据已存在！");
            }
            else
            {
                sql = "Insert Into LineStopUnModel(Line,Model,UID,Description,ActiveDateTime,ActiveFlag)  Values('" + t.Line + "','" + t.Model + "','" + t.UID + "','" + t.Description + "','" + t.ActiveDateTime + "','" + t.ActiveFlag + "')";
                db.Execute(sql);
                return ("OK");
            }
        }

        public string SeverityLevel_Save(TSeverityLevel t, TLineStop m)
        {
            List<TSeverityLevel> list = new List<TSeverityLevel>();
            string sql = "";

            list = SeverityLevel_View(t.Line);

            if (list.Count > 0)
            {
                sql = "Update RoutingLine Set SeverityLevel='" + t.SeverityLevel + "' where Line = '" + t.Line + "' ";
            }
            else
            {

                sql = "Insert Into  RoutingLine(Line,SeverityLevel,Description) Values('" + t.Line + "','" + t.SeverityLevel + "','" + t.Description + "')";

            }
            db.Execute(sql);
            return ("OK");
        }

        public string Mail_Save(TLineStopMail t, TLineStop m)
        {
            List<TLineStopMail> list = new List<TLineStopMail>();
            string sql = "";

            list = Mail_View(t.UID);

            if (list.Count > 0)
            {
                sql = "Update LineStopMailUser Set MailAccount='" + t.MailAccount + "',HostName='" + t.HostName + "',MailLevel='" + t.MailLevel + "',FixLevel='" + t.FixLevel + "' where UID = '" + t.UID + "' ";
            }
            else
            {

                sql = "Insert Into  LineStopMailUser(UID,MailAccount,HostName,MailLevel,FixLevel,Password) Values('" + t.UID + "','" + t.MailAccount + "','" + t.HostName + "','" + t.MailLevel + "','" + t.FixLevel + "','qms')";

            }
            db.Execute(sql);
            return ("OK");
        }
        public string Phone_Save(TLineStopPhone t, TLineStop m)
        {
            List<TLineStopPhone> list = new List<TLineStopPhone>();
            string sql = "";

            list = Phone_View(t.PhoneNo);

            if (list.Count > 0)
            {
                sql = "Update LineStopPhone Set UID='" + t.UID + "',Priority='" + t.Priority + "' where PhoneNo = '" + t.PhoneNo + "' ";
            }
            else
            {

                sql = "Insert Into LineStopPhone(PhoneNo,UID,Priority) Values('" + t.PhoneNo + "','" + t.UID + "','" + t.Priority + "')";

            }
            db.Execute(sql);
            return ("OK");
        }

        public string UnModel_Disable(TUnModel t, TLineStop m)
        {
            List<TUnModel> list = new List<TUnModel>();
            string sql = "";

            list = UnModel_View(t.Line, t.Model, "2");

            if (list.Count == 0)
            {
                return ("There is no Line & model in the DB,Please check");
            }
            else
            {
                sql = "update LineStopUnModel set ActiveFlag='N',InActiveDateTime='" + t.InActiveDateTime + "' where ActiveFlag='Y' and line='" + t.Line + "' and Model='" + t.Model + "'";
                db.Execute(sql);
                return ("OK");
            }
        }

        public void LineStopFPY_Delete(TLineStopFPY t, string PU, string Type, string Auth)
        {
            string sql = "";
            if (Auth == "FPY")
            {
                sql = "Delete from LineStopFPY where Model='" + t.Model + "' and Station='" + t.Station + "' and SeverityLevel='" + t.SeverityLevel + "' ";
                db.Execute(sql);
            }
        }
        public void LineStopWaitRepair_Delete(TLineStopWaitRepair t, string PU, string Type, string Auth)
        {
            string sql = "";
            if (Auth == "WaitRepair")
            {
                sql = "Delete from LineStopWaitRep where Line='" + t.Line + "' and Status='" + t.Status + "' and SeverityLevel='" + t.SeverityLevel + "' ";
            }
            if (Auth == "WaitTest")
            {
                sql = "Delete from LineStopWaitTest where Line='" + t.Line + "' and Station='" + t.Status + "' and SeverityLevel='" + t.SeverityLevel + "' ";
            }

            db.Execute(sql);

        }

        public void LineStopGroupName_Delete(TGroupName t, string PU, string Type, string Auth)
        {
            string sql = "";
            if (Auth == "GroupName")
            {
                sql = "Delete from LineStopGroupName where groupName='" + t.GroupName + "' ";
                db.Execute(sql);
            }
        }

        public string LineStopUnWO_Delete(TUnWO t, string PU, string Type, string Auth)
        {
            string sql = "";

            List<TUnWO> list = new List<TUnWO>();

            list = UnWO_View(t.WO);

            if (list.Count == 0)
            {
                return ("该数据不存在！");
            }
            else
            {
                sql = "Update LineStopUnWO set ActiveFlag='N',InActiveDateTime='" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' where WO = '" + t.WO + "'";
                db.Execute(sql);
                return ("OK");
            }

        }


        public void LineStopUnModel_Delete(TUnModel t, string PU, string Type, string Auth)
        {
            string sql = "";
            if (Auth == "UnModel")
            {
                sql = "Delete from LineStopUnModel where Line='" + t.Line + "' and Model='" + t.Model + "' ";
                db.Execute(sql);
            }
        }

        public void LineStopSeverityLevel_Delete(TSeverityLevel t, string PU, string Type, string Auth)
        {
            string sql = "";
            if (Auth == "SeverityLevel")
            {
                sql = "Delete from RoutingLine Where Line='" + t.Line + "' ";
                db.Execute(sql);
            }
        }

        public void LineStopMail_Delete(TLineStopMail t, string PU, string Type, string Auth)
        {
            string sql = "";
            if (Auth == "Mail")
            {
                sql = "Delete from LineStopMailUser Where UID='" + t.UID + "' ";
                db.Execute(sql);
            }
        }
        public void LineStopPhone_Delete(TLineStopPhone t, string PU, string Type, string Auth)
        {
            string sql = "";
            if (Auth == "Phone")
            {
                sql = "Delete from LineStopPhone Where PhoneNo='" + t.PhoneNo + "' ";
                db.Execute(sql);
            }
        }
    }
    #endregion LineStop

    #region ErrorCode
    public class TErrorCodePU
    {

        public string PU { get; set; }
        public string Type { get; set; }
    }
    public class TErrorCode
    {
        public Int64 ID { get; set; }
        public string System_Name { get; set; }
        public string EmployeeID { get; set; }
        //ErrorCode
        public string ErrType { get; set; }
        public string ErrCode { get; set; }
        public string Description { get; set; }
        public string CHDescription { get; set; }
        //RepairCode
        public string RepCode { get; set; }
        public string SWAP { get; set; }
        public string SB_Flag { get; set; }
        public string GroupName { get; set; }
        public string RohsPart_Flag { get; set; }
        public string KeyPart_Flag { get; set; }
        public string SB_ChkFlag { get; set; }
        public string Solution_Code { get; set; }
        //SolutionCode
        public string SolCode { get; set; }
        //KeyPartSupplier
        public string Comp_PN { get; set; }
        public string Model { get; set; }
        public string SWDL_STR { get; set; }
        public string Supplier { get; set; }
        public string Vendor { get; set; }
        public string User_ID { get; set; }
        public string TransDateTime { get; set; }
        public string Type { get; set; }
        public string DelateFlag { get; set; }
        public string Vendor_Desc { get; set; }
        //RepGrade
        public string UID { get; set; }
        public string CName { get; set; }
        public string JobFunc { get; set; }
        public string Password { get; set; }
        public string RepTime { get; set; }
        public string Grade { get; set; }
    }
    public class TErrorCodeDB
    {
        private DB db;
        private DB dbLog;
        public TErrorCodeDB(string Key)
        {
            db = new DB(Key);
        }

        public List<TErrorCode> ErrorCode_View(string Table, string selID)
        {
            List<TErrorCode> list = new List<TErrorCode>();
            DataTable dt = new DataTable();
            if (Table == "ErrorCode")
            {
                string sql = "select TOP 2000 * from error_code WHERE 1=1 ";
                if (string.IsNullOrEmpty(selID) == false)
                {
                    sql = sql + "and ErrType='" + selID + "'";
                }
                sql = sql + " order by ErrCode";
                dt = db.Execute(sql).Tables[0];
            }
            if (Table == "RepairCode")
            {
                string sql = "select TOP 2000 * from Repair_Code order by RepCode";
                dt = db.Execute(sql).Tables[0];
            }
            if (Table == "SolutionCode")
            {
                string sql = "select TOP 2000 * from Solution_Code order by solcode";
                dt = db.Execute(sql).Tables[0];
            }
            if (Table == "KeyPartSupplier")
            {
                string sql = "Select TOP 3500 * from KeyPart_Supplier WHERE 1=1 ";
                if (string.IsNullOrEmpty(selID) == false)
                {
                    sql = sql + " and RepCode='" + selID + "'";
                }
                sql = sql + "  order by RepCode,Comp_PN";
                dt = db.Execute(sql).Tables[0];
            }
            if (Table == "Repair_Man")
            {
                string sql = "Select * from Repair_Man ";
                dt = db.Execute(sql).Tables[0];
            }
            if (Table == "Repair_Grade")
            {
                string sql = "Select * from Repair_Grade ";
                dt = db.Execute(sql).Tables[0];
            }
            db.CloseDB();
            list = DataTableTool.ToList<TErrorCode>(dt);
            return list;

        }
        public List<string> Upload_GetID(string Table)
        {
            List<string> list = new List<string>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = "";
            if (Table == "ErrorCode")
            {
                sql = "select DISTINCT ErrType from error_code ";
            }
            if (Table == "KeyPartSupplier")
            {
                sql = "Select DISTINCT RepCode from KeyPart_Supplier ";
            }

            ds = db.Execute(sql);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(dt.Rows[i][0].ToString());
                }
            }
            db.CloseDB();
            return list;
        }
        public List<TErrorCode> Upload_View(string Table, TErrorCode t)
        {
            List<TErrorCode> list = new List<TErrorCode>();
            DataTable dt = new DataTable();
            if (Table == "ErrorCode")
            {
                string sql = "select TOP 1 1 from Error_Code where   ErrType = '" + t.ErrType.Trim() + "' and ErrCode = '" + t.ErrCode.Trim() + "'";
                dt = db.Execute(sql).Tables[0];
            }
            if (Table == "RepairCode")
            {
                string sql = "select TOP 1 1 from Repair_Code where  RepCode = '" + t.RepCode.Trim() + "'";
                dt = db.Execute(sql).Tables[0];
            }
            if (Table == "SolutionCode")
            {
                string sql = "select TOP 1 1 from Solution_Code where solcode = '" + t.SolCode.Trim() + "'";
                dt = db.Execute(sql).Tables[0];
            }
            if (Table == "KeyPartSupplier")
            {
                string sql = "Select TOP 1 1 from KeyPart_Supplier where Comp_PN='" + t.Comp_PN.Trim() + "'";
                dt = db.Execute(sql).Tables[0];
            }
            if (Table == "RepairMan")
            {
                string sql = "Select TOP 1 1 from Repair_Man where UID = '" + t.UID.Trim() + "'";
                dt = db.Execute(sql).Tables[0];
            }
            if (Table == "RepairGrade")
            {
                string sql = "Select TOP 1 1 from Repair_Grade where Model = '" + t.Model.Trim() + "' and RepCode = '" + t.RepCode.Trim() + "'";
                dt = db.Execute(sql).Tables[0];
            }
            db.CloseDB();
            list = DataTableTool.ToList<TErrorCode>(dt);
            return list;

        }
        public void ErrorCode_Save(TErrorCode t, string PU)
        {
            string sql;
            if (t.DelateFlag == "Y")
            {
                sql = "Delete from Error_Code where ErrType = '" + t.ErrType.Trim() + "' and ErrCode = '" + t.ErrCode.Trim() + "'";
            }
            else
            {
                if (Upload_View("ErrorCode", t).Count > 0)
                {
                    if (PU == "PU7-HUA")
                    {
                        sql = "Update Error_Code set ErrType = '" + t.ErrType.Trim() + "',ErrCode = '" + t.ErrCode.Trim() + "',Description='" + t.Description.Trim() + "',CHDescription=N'" + t.CHDescription.Trim() + "' where ErrType = '" + t.ErrType.Trim() + "' and ErrCode = '" + t.ErrCode.Trim() + "'";
                    }
                    else
                    {
                        sql = "Update Error_Code set ErrType = '" + t.ErrType.Trim() + "',ErrCode = '" + t.ErrCode.Trim() + "',Description='" + t.Description.Trim() + "' where ErrType = '" + t.ErrType.Trim() + "' and ErrCode = '" + t.ErrCode.Trim() + "'";
                    }
                }
                else
                {
                    if (PU == "PU7-HUA")
                    {
                        sql = "Insert into Error_Code(ErrType,ErrCode,Description,CHDescription) values('" + t.ErrType.Trim() + "','" + t.ErrCode.Trim() + "','" + t.Description.Trim() + "',N'" + t.CHDescription.Trim() + "')";
                    }
                    else
                    {
                        sql = "Insert into Error_Code(ErrType,ErrCode,Description) values('" + t.ErrType.Trim() + "','" + t.ErrCode.Trim() + "','" + t.Description.Trim() + "')";
                    }
                }
            }
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report_ErrorCode_Upload", "03", "UploadErrorCode", t.EmployeeID, sql);
            db.CloseDB();
        }
        public void RepairCode_Save(TErrorCode t)
        {
            List<TErrorCode> list = new List<TErrorCode>();
            string sql = "";

            list = Upload_View("RepairCode", t);

            if (list.Count > 0)
            {
                sql = "Update Repair_Code set Description='" + t.Description.Trim() + "',SWAP='" + t.SWAP.Trim() + "', " +
                     " SB_Flag='" + t.SB_Flag.Trim() + "',GroupName='" + t.GroupName.Trim() + "',RohsPart_Flag='" + t.RohsPart_Flag.Trim() + "'," +
                        " KeyPart_Flag='" + t.KeyPart_Flag.Trim() + "',SB_ChkFlag='" + t.SB_ChkFlag.Trim() + "', Solution_Code = '" + t.Solution_Code.Trim() + "'  where RepCode='" + t.RepCode.Trim() + " '";
            }
            else
            {
                sql = "Insert into Repair_Code(RepCode,Description,SWAP,SB_Flag,GroupName,RohsPart_Flag,KeyPart_Flag,SB_ChkFlag,Solution_Code) values('" + t.RepCode.Trim() + "','" + t.Description.Trim() + "','" + t.SWAP.Trim() + "','" + t.SB_Flag.Trim() + "','" + t.GroupName.Trim() + "','" + t.RohsPart_Flag.Trim() + "','" + t.KeyPart_Flag.Trim() + "','" + t.SB_ChkFlag.Trim() + "','" + t.Solution_Code.Trim() + "')";
            }
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report_RepairCode_Upload", "03", "UploadRepairCode", t.EmployeeID, sql);
            db.CloseDB();
        }
        public void SolCode_Save(TErrorCode t)
        {
            List<TErrorCode> list = new List<TErrorCode>();
            string sql = "";

            list = Upload_View("SolutionCode", t);

            if (list.Count > 0)
            {
                sql = "Update Solution_Code set SolCode='" + t.SolCode.Trim() + "',Description='" + t.Description.Trim() + "', " +
                     " SB_Flag='" + t.SB_Flag.Trim() + "',GroupName='" + t.GroupName.Trim() + "',RohsPart_Flag='" + t.RohsPart_Flag.Trim() + "'," +
                        " KeyPart_Flag='" + t.KeyPart_Flag.Trim() + "',SB_ChkFlag='" + t.SB_ChkFlag.Trim() + "', Solution_Code = '" + t.Solution_Code.Trim() + "'  where RepCode='" + t.RepCode.Trim() + " '";
            }
            else
            {
                sql = "Insert into Solution_Code(SolCode,Description) values('" + t.SolCode.Trim() + "','" + t.Description.Trim() + "')";
            }
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report_SolutionCode_Upload", "03", "UploadSolutionCode", t.EmployeeID, sql);
            db.CloseDB();
        }
        public void KeyPartSupplier_Save(TErrorCode t, string PU, string Type)
        {
            List<TErrorCode> list = new List<TErrorCode>();
            string sql = "";
            if (t.DelateFlag == "Y")
            {
                sql = "delete from Keypart_Supplier where  Comp_PN='" + t.Comp_PN.Trim() + "'";
            }
            else
            {
                list = Upload_View("KeypartSupplier", t);
                if (list.Count > 0)
                {
                    if ((PU == "PAD" || PU == "NB4") && (Type == "PAD" || Type == "STN"))
                    {
                        sql = "Update Keypart_Supplier set RepCode='" + t.RepCode.Trim() + "',Supplier='" + t.Supplier.Trim() + "',Vendor='" + t.Vendor.Trim() + "',Type='" + t.Type.Trim() + "',Vendor_Desc='" + t.Vendor_Desc.Trim() + "'  where Comp_PN='" + t.Comp_PN.Trim() + "' ";
                    }
                    else
                    {
                        sql = "Update Keypart_Supplier set RepCode='" + t.RepCode.Trim() + "',Supplier='" + t.Supplier.Trim() + "',Vendor='" + t.Vendor.Trim() + "',Type='" + t.Type.Trim() + "'  where Comp_PN='" + t.Comp_PN.Trim() + "' ";
                    }
                }
                else
                {
                    if ((PU == "PAD" || PU == "NB4") && (Type == "PAD" || Type == "STN"))
                    {
                        sql = "Insert into Keypart_Supplier(RepCode,Comp_PN,Supplier,Vendor,Type,Vendor_Desc) values('" + t.RepCode.Trim() + "','" + t.Comp_PN.Trim() + "','" + t.Supplier.Trim() + "','" + t.Vendor.Trim() + "','" + t.Type.Trim() + "','" + t.Vendor_Desc.Trim() + "')";
                    }
                    else
                    {
                        sql = "Insert into Keypart_Supplier(RepCode,Comp_PN,Supplier,Vendor,Type) values('" + t.RepCode.Trim() + "','" + t.Comp_PN.Trim() + "','" + t.Supplier.Trim() + "','" + t.Vendor.Trim() + "','" + t.Type.Trim() + "')";
                    }
                }
            }
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report_KeyPartSupplier_Upload", "03", "UploadKeyPartSupplier", t.EmployeeID, sql);
            db.CloseDB();
        }
        public void RepairMan_Save(TErrorCode t)
        {
            List<TErrorCode> list = new List<TErrorCode>();
            string sql = "";

            list = Upload_View("RepairMan", t);

            if (list.Count > 0)
            {
                sql = "Update Repair_Man SET cname=N'" + t.CName.Trim() + "',JobFunc=N'" + t.JobFunc.Trim() + "',Password=N'" + t.Password.Trim() + "' where UID='" + t.UID.Trim() + "'";
            }
            else
            {
                sql = "Insert into Repair_Man(UID,CNAME,JobFunc,Password) values('" + t.UID.Trim() + "',N'" + t.CName.Trim() + "',N'" + t.JobFunc.Trim() + "',N'" + t.Password.Trim() + "')";
            }
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report_RepairMan_Upload", "03", "UploadRepGrade", t.EmployeeID, sql);
            db.CloseDB();
        }
        public void RepairGrade_Save(TErrorCode t)
        {
            List<TErrorCode> list = new List<TErrorCode>();
            string sql = "";

            list = Upload_View("RepairGrade", t);

            if (list.Count > 0)
            {
                sql = "Update Repair_Grade SET RepCode=N'" + t.RepCode.Trim() + "',RepTime=N'" + t.RepTime.Trim() + "' where Grade='" + t.Grade.Trim() + "' and Model=N'" + t.Model.Trim() + "'";
            }
            else
            {
                sql = "Insert into Repair_Grade(Model,RepCode,RepTime,Grade) values('" + t.Model.Trim() + "',N'" + t.RepCode.Trim() + "',N'" + t.RepTime.Trim() + "',N'" + t.Grade.Trim() + "')";
            }
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report_RepairGrade_Upload", "03", "UploadRepGrade", t.EmployeeID, sql);
            db.CloseDB();
        }
        public void ErrorCode_Delete(TErrorCode t, string opUser, string Table)
        {
            string sql = "";
            if (Table == "ErrorCode")
            {
                sql = "delete from Error_Code where ErrType='" + t.ErrType + "' and ErrCode='" + t.ErrCode + "' and Description='" + t.Description + "' ";
                db.Execute(sql);
                db.SaveQMSLog("MVC_PUB_Report_ErrorCode_Delete", "04", "UploadErrorCode", t.EmployeeID, sql);
            }
            if (Table == "RepairCode")
            {
                sql = "delete from Repair_Code where RepCode='" + t.RepCode + "'  ";
                db.Execute(sql);
                db.SaveQMSLog("MVC_PUB_Report_RepairCode_Delete", "04", "UploadRepairCode", t.EmployeeID, sql);
            }
            if (Table == "SolutionCode")
            {
                sql = "delete from Solution_Code where SolCode='" + t.SolCode + "'  ";
                db.Execute(sql);
                db.SaveQMSLog("MVC_PUB_Report_SolutionCode_Delete", "04", "UploadSolutionCode", t.EmployeeID, sql);
            }
            if (Table == "KeyPartSupplier")
            {
                sql = "delete from KeyPart_Supplier where Comp_PN='" + t.Comp_PN + "'  ";
                db.Execute(sql);
                db.SaveQMSLog("MVC_PUB_Report_KeyPartSupplier_Delete", "04", "UploadKeyPartSupplier", t.EmployeeID, sql);
            }
            if (Table == "Repair_Man")
            {
                sql = "delete from Repair_Man where UID='" + t.UID + "'  ";
                db.Execute(sql);
                db.SaveQMSLog("MVC_PUB_Report_RepairMan_Delete", "04", "UploadRepGrade", t.EmployeeID, sql);
            }
            if (Table == "Repair_Grade")
            {
                sql = "delete from Repair_Grade where Model='" + t.Model + "' and RepCode = '" + t.RepCode + "' ";
                db.Execute(sql);
                db.SaveQMSLog("MVC_PUB_Report_RepairGrade_Delete", "04", "UploadRepGrade", t.EmployeeID, sql);
            }
            db.CloseDB();
        }
    }
    #endregion ErrorCode
}