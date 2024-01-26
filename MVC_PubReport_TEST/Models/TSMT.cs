using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Models.SMT
{
    public class TSMT
    {
    }

    #region PCBA_CPU_MaterialMapping
    public class TPCBA_CPU_MaterialMapping
    {
        public string PCBAPN { get; set; }
        public string NBDescription { get; set; }
        public string CPUPN { get; set; }
        public string CPUDescription { set; get; }
        public string CPUDes { get; set; }
        public string Plant { set; get; }
        public string Status { set; get; }
        public string UserID { set; get; }
        public string TransDateTime { get; set; }
    
    }
#endregion PCBA_CPU_MaterialMapping


    public class SMT
    {      
       private DB db;
       private string MISDB_IP;


       public SMT(string key)
        {
            
            db = new DB(key);
            
        }
        #region PCBA_CPU_MaterialMapping

        public List<TPCBA_CPU_MaterialMapping> PCBA_CPU_MaterialMappingView(TPCBA_CPU_MaterialMapping t)
        {
            List<TPCBA_CPU_MaterialMapping> list = new List<TPCBA_CPU_MaterialMapping>();
            DataTable dt = new DataTable();

            string sql = "SELECT PCBAPN,NBDescription,CPUPN,CPUDescription,CPUDes,Plant,Status,UserID,TransDateTime FROM PCBA_CPU_MaterialMapping WHERE 1=1 ";

            if (String.IsNullOrEmpty(t.PCBAPN) == false && string.IsNullOrWhiteSpace(t.PCBAPN) == false)
            {
                sql = sql + " and  PCBAPN ='" + t.PCBAPN + "'";

            }

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TPCBA_CPU_MaterialMapping>(dt);
            return list;
                 
        
        }
        public List<TPCBA_CPU_MaterialMapping> PCBA_CPU_MaterialMappingQuery(TPCBA_CPU_MaterialMapping t)
        {
            List<TPCBA_CPU_MaterialMapping> list = new List<TPCBA_CPU_MaterialMapping>();
            DataTable dt = new DataTable();

            string sql = "SELECT PCBAPN,NBDescription,CPUPN,CPUDescription,CPUDes,Plant,Status,UserID,TransDateTime FROM PCBA_CPU_MaterialMapping WHERE 1=1 ";

            if (String.IsNullOrEmpty(t.PCBAPN) == false && string.IsNullOrWhiteSpace(t.PCBAPN) == false)
            {
                sql = sql + " and  PCBAPN ='" + t.PCBAPN + "'";

            }
            if (String.IsNullOrEmpty(t.NBDescription) == false && string.IsNullOrWhiteSpace(t.NBDescription) == false)
            {
                sql = sql + " and  NBDescription ='" + t.NBDescription + "'";

            }
            if (String.IsNullOrEmpty(t.CPUPN) == false && string.IsNullOrWhiteSpace(t.CPUPN) == false)
            {
                sql = sql + " and  CPUPN ='" + t.CPUPN + "'";

            }
            if (String.IsNullOrEmpty(t.CPUDescription) == false && string.IsNullOrWhiteSpace(t.CPUDescription) == false)
            {
                sql = sql + " and  CPUDescription ='" + t.CPUDescription + "'";

            }
            if (String.IsNullOrEmpty(t.CPUDes) == false && string.IsNullOrWhiteSpace(t.CPUDes) == false)
            {
                sql = sql + " and  CPUDes ='" + t.CPUDes + "'";

            }
            if (String.IsNullOrEmpty(t.Plant) == false && string.IsNullOrWhiteSpace(t.Plant) == false)
            {
                sql = sql + " and  Plant ='" + t.Plant + "'";

            }
            if (String.IsNullOrEmpty(t.Status) == false && string.IsNullOrWhiteSpace(t.Status) == false)
            {
                sql = sql + " and  Status ='" + t.Status + "'";

            }

            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TPCBA_CPU_MaterialMapping>(dt);
            return list;


        }

        public void PCBA_CPU_MaterialMappingSave(TPCBA_CPU_MaterialMapping t)
        {
            List<TPCBA_CPU_MaterialMapping> list = new List<TPCBA_CPU_MaterialMapping>();
            string sql = "";

            list = PCBA_CPU_MaterialMappingView(t);
            if (list.Count > 0)
            {
                sql = "DELETE FROM PCBA_CPU_MaterialMapping WHERE   PCBAPN='"+ t.PCBAPN +"'";
                db.Execute(sql);
            }
         

            sql = "INSERT INTO PCBA_CPU_MaterialMapping (PCBAPN, NBDescription, CPUPN, CPUDescription, CPUDes, Plant, Status, UserID, TransDateTime) " +
                                " VALUES('" + t.PCBAPN + "','" + t.NBDescription + "','" + t.CPUPN + "','" + t.CPUDescription + "','" + t.CPUDes + "','" + t.Plant + "','" + t.Status + "','" + t.UserID + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
            db.Execute(sql);

        }

        public void PCBA_CPU_MaterialMappingDelete(TPCBA_CPU_MaterialMapping t,string opUserID)
        {
            string sql = "";
            sql = "DELETE FROM PCBA_CPU_MaterialMapping WHERE   PCBAPN='" + t.PCBAPN + "'";
            db.Execute(sql);

            db.SaveQMSLog("PCBA_CPU_MaterialMappingDelete", "4", opUserID, "", sql);
        
        }

        #endregion PCBA_CPU_MaterialMapping
    }
}