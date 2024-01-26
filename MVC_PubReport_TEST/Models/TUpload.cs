using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Models.Upload
{
    public class TUpload
    {
    }

    #region TCapacity_Plan
    public class TCapacity_Plan
    { 
    
        public string Department{get;set;}	
        public string PU{get;set;}
        public string Type{get;set;}
        public string DateTime{get;set;}
        public string Factory{get;set;}
        public string Line{get;set;}
        public string HRLine{get;set;}
        public string Shift{get;set;}
        public string IsDocking{get;set;}
        public Int64 Quantity{get;set;}
        public string UserID{get;set;}
        public string TransDateTime{get;set;}
        public string IsDisplay{get;set;}
        public Int64 Buffer_Qty { get; set; }
        public Int64 ID { get; set; }
        public string Remark { get; set; }
 
    }
    #endregion TCapacity_Plan


    public class Upload
    {
       private DB db;
       private string MISDB_IP;


       public Upload(string key)
        {
            
            db = new DB(key);
            MISDB_IP = System.Configuration.ConfigurationManager.AppSettings["MISDB_IP"];
        }
        #region Capacity_Plan

       public List<TCapacity_Plan> Capacity_Plan_ViewAllMonth(TCapacity_Plan t)
       {
           List<TCapacity_Plan> list = new List<TCapacity_Plan>();
           DataTable dt = new DataTable();

           string sql = "select ROW_NUMBER()OVER (ORDER BY DateTime) AS ID, Department, PU, Type, DateTime, Factory, Line, HRLine, Shift, IsDocking, Quantity, UserID, TransDateTime, IsDisplay, Buffer_Qty from Capacity_Plan where Department='" + t.Department + "' and PU='" + t.PU + "' and Type='" + t.Type +
                           "' and DateTime like '" + t.DateTime + "%'";
           dt = db.Execute(sql).Tables[0];

           list = DataTableTool.ToList<TCapacity_Plan>(dt);
           return list;
       
       }

       public List<TCapacity_Plan> Capacity_Plan_View(TCapacity_Plan t)
       {
           List<TCapacity_Plan> list = new List<TCapacity_Plan>();
           DataTable dt = new DataTable();

           string sql = "select ROW_NUMBER()OVER (ORDER BY DateTime) AS ID, Department, PU, Type, DateTime, Factory, Line, HRLine, Shift, IsDocking, Quantity, UserID, TransDateTime, IsDisplay, Buffer_Qty from Capacity_Plan where Department='" + t.Department + "' and PU='" + t.PU + "' and Type='" + t.Type +
                            "' and DateTime='" + t.DateTime + "'";

      
               sql = sql + "and Line='" + t.Line + "'";
           
            
               sql = sql + "and Shift='" + t.Shift + "'";

           
               sql = sql + "and Factory='" + t.Factory + "'";

           
               sql = sql + "and IsDocking='" + t.IsDocking + "'";

           
                          
           dt = db.Execute(sql).Tables[0];

           list = DataTableTool.ToList<TCapacity_Plan>(dt);


           return list;
       
       }

       public void Capacity_Plan_Delete(TCapacity_Plan t, string opUser)
       {
           string sql = "";

           sql = "delete from Capacity_Plan where Department='" + t.Department + "' and PU='" + t.PU + "' and Type='" + t.Type +
                            "' and DateTime='" + t.DateTime + "'";

           sql = sql + "and Line='" + t.Line + "'";


           sql = sql + "and Shift='" + t.Shift + "'";


           sql = sql + "and Factory='" + t.Factory + "'";


           sql = sql + "and IsDocking='" + t.IsDocking + "'";



           db.Execute(sql);
           db.SaveQMSLog("MVC_PUB_Report", "4", opUser, "", sql);
       
       }
       public void Capacity_Plan_Save(TCapacity_Plan t,string opUser)
       {
           List<TCapacity_Plan> list = new List<TCapacity_Plan>();
           string sql = "";

           list = Capacity_Plan_View(t);

           if (list.Count > 0)
           {
               sql = "update Capacity_Plan set Quantity='" + t.Quantity + "',userid='" + opUser + "',TransDateTime = dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS')" +
                    " where Department='" + t.Department + "' and PU='" + t.PU + "' and Type='" + t.Type + "' and DateTime='" + t.DateTime + "'" +
                            " and Line='" + t.Line + "' and Shift='" + t.Shift + "' and factory='" + t.Factory + "' and   IsDocking='" + t.IsDocking + "'";
           }
           else
           { 
               sql =  "insert into Capacity_Plan(Department,PU,Type,DateTime,Factory,Line,Shift,IsDocking,Quantity,UserID,TransDateTime) values "+
                   "('"+ t.Department +"','"+ t.PU +"','"+ t.Type +"','"+ t.DateTime +"'," +
                         "'" + t.Factory + "','" + t.Line + "','" + t.Shift + "','" + t.IsDocking + "','" + t.Quantity + "','" + opUser + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS') )";
           
           }
           db.Execute(sql);
       
       }
        #endregion Capacity_Plan
    }

}