using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MVC_PubReport.Models.Files;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Models.Query
{
    #region TIE_DailyRpt_ToMIS_log
    public class TIE_DailyRpt_ToMIS_log
    {
        public string TransDate{get;set;}
        public string Depart_No{get;set;} 
        public string Mode{get;set;}
        public string Line{get;set;}
        public string Line_NO{get;set;}
        public string Shift{get;set;}
        public string Model{get;set;}
        public string Stage{get;set;}
        public Int64 OutPut{get;set;}
        public Int64 InPut{get;set;}
        public Int64 InPut_TestModel{get;set;}
        public double Manhour{get;set;}
        public double Out{get;set;}
        public string QtyType{get;set;}
        public Int64 CountTimes{get;set;}
        public double Out_BK{get;set;}
        public string BUTYPE{get;set;}
        public string PN{get;set;}
        public string WO { get; set; }

    }
    #endregion TIE_DailyRpt_ToMIS_log

    public class TQueryDB
    {
         private DB db;
 
        public TQueryDB()
        {
            db = new DB("PubReportMain");
        }

        public TQueryDB(string Key)
        {
            db = new DB(Key);
        }

        public bool BuildPubConnection(string Key)
        {
            try
            {
                db = new DB(Key);
              
                return true;
            }

            catch

            {
                return false;
            }
           
        
        }
        #region IE_DailyRpt_ToMIS_log
        public void IE_DailyRpt_ToMIS_log_QueryByDT(string BeginDT, string EndDT)
        {
             
            string strSQL = "exec [MVC_PUBReport_Query_IE_DailyRpt_ToMIS_log] '" + BeginDT + "','" + EndDT + "'";
            db.Execute(strSQL);
           
             
        
        }
        #endregion IE_DailyRpt_ToMIS_log

        #region WIPWO
        public void Truncate_OpenWO()
        {
            string strSQL = "truncate table openwo";
            db.Execute(strSQL);
        
        }
        public void Insert_OpenWO(string WO)
        {
            string strSQL = "insert into OPENWO(Work_Order) values('"+WO+"')";
            db.Execute(strSQL);
        }

        public void StartJOB()
        {

            string strSQL = "  EXEC msdb. dbo.sp_start_job N'WIPWOQuery'";
            db.Execute(strSQL);
        }
        #endregion WIPWO

    }
}