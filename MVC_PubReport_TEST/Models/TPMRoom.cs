using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Models.PMRoom
{
    #region model
    public class  TRFC_GLUEPOST_Log
    {
     public string MANDT{get;set;}
     public string ZREFID{get;set;}
     public string ZITEM{get;set;}
     public string WERKS{get;set;}
     public string KOSTL{get;set;}
     public string LGORT{get;set;}
     public string MATNR{get;set;}
     public decimal MENGE{get;set;}
     public string MEINS{get;set;}
     public string FLAG{get;set;}
     public string MBLNR{get;set;}
     public string ZEILE{get;set;}
     public string MJAHR{get;set;}
     public string MESSAGE{get;set;}
     public string AUFNR{get;set;}
     public string Line{get;set;}
     public string IndateTime{get;set;}
     public string FinishDateTime{get;set;}
     public string UID { get; set; }
    }
    #endregion model
    public class TPMRoom
    {
        private DB db;
        public TPMRoom(string key)
        { 
            db = new DB(key); 
        }


        public List<string> GetLineByPU(string PU)
        {
            List<string> list = new List<string>();
           
            DataTable dt = new DataTable();

            string sql = "";

            sql = "SELECT DISTINCT LINE FROM dbo.IE_Line_shift with(nolock) WHERE PU like '" + PU + "%'";

            dt = db.Execute(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["LINE"].ToString());

            }
            return list;
        }


        public List<string> GetCompPN(string PU)
        {
            List<string> list = new List<string>();

            DataTable dt = new DataTable();

            string sql = "";

            sql = "SELECT DISTINCT MPn FROM dbo.Glue_Data with(nolock) WHERE PU= '" + PU + "'";

            dt = db.Execute(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["MPn"].ToString());

            }
            return list;
        }

        public string GlueRecord(string PU, string Line, string CompPN, string Qty, string UserID)
        {
            string reslut = "";
            string strSQL = "";
           
            DataSet ds = new DataSet();

            strSQL = "EXEC RFC_GluePost_ByDay 'RECORD','" + PU + "','" + CompPN + "','" + Qty + "','" + Line + "','" + UserID + "'";
            ds = db.Execute(strSQL) ;

            if (ds.Tables.Count < 0)
            {
                reslut = "OK";
            }
            else
            {
                reslut = ds.Tables[0].Rows[0]["MESSAGE"].ToString();
            }
            return reslut;
        
        }

        public List<TRFC_GLUEPOST_Log> GlueQuery(string dateTime)
        {
            List<TRFC_GLUEPOST_Log> list = new List<TRFC_GLUEPOST_Log>();
            DataTable dt = new DataTable();

            string strSQL = "SELECT TOP 100 * FROM RFC_GlueMesRecord WHERE ZREFID LIKE '" + dateTime + "%' ORDER BY ZREFID DESC ";
            dt = db.Execute(strSQL).Tables[0];

            list = DataTableTool.ToList<TRFC_GLUEPOST_Log>(dt);   
            return list;
        
        }
    }
}