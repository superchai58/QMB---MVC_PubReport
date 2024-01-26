using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MVC_PubReport.Models.Files;
using MVC_PubReport.Models.Public;
namespace MVC_PubReport.Models.DBA
{

    #region TLinkedServer
    public class TLinkedServer
    { 
        public string LocalIP{get;set;}
        public string IP{get;set;} 
        public string UserName{get;set;}
        public string PWD{get;set;}
        public string Desc1 { get; set; }
    }
    #endregion TLinkedServer

    #region SVR_Define
    public class TSVR_Define
    {
        public string BU{get;set;}	
        public string BUType{get;set;}	
        public string Customer{get;set;}
        public string CName{get;set;}
        public string Item{get;set;}
        public string Context{get;set;}
        public string IP{get;set;}
        public string DBName{get;set;}
        public string ISNew{get;set;}
        public string Description { get; set; }
        public Int64 ID { get; set; }

    }
    #endregion SVR_Define

    #region MainDB_SysConfig
    public class TMainDB_SysConfig
    { 
        public string BU{get;set;}	
        public string Type{get;set;}
        public string ServerIP{get;set;}
        public string ServerType{get;set;}
        public string DBName{get;set;}
        public string ClusterIP{get;set;}
        public string ListenIP{get;set;}
        public string Item{get;set;}
        public string Context{get;set;}
        public string Description { get; set; }
        public string Transdatetime { get; set; }
    
    }
    #endregion MainDB_SysConfig

    #region FileServer
    public class TFileServer
    { 
        public string ClusterIP{get;set;}
        public string IP1{get;set;}
        public string IP2{get;set;}
        public string ServerDesc{get;set;}
        public string UserID { get; set; }
        public string TransDatetime{get;set;}
        public string ActualIP { get; set; }
    
    }
    #endregion FileServer

    #region DBShareFolderList
    public class TDBShareFolderList
    { 
        public string PU{get;set;}
        public string Type{get;set;}
        public string ServerIP{get;set;}
        public string Path{get;set;}
        public string DB_Name{get;set;}
        public string UserName{get;set;}
        public string Password{get;set;}
        public Int64 LimitedFileNum { get; set; }
        public Int64 LimitedFileSize{get;set;}
        public string  BackUpServerIP{get;set;}
        public Int64 ID { get; set; }
    
    }
    #endregion DBShareFolderList

    #region DBServer_List
    public class TDBServer_List
    { 
        public string BU{get;set;}
        public string TYPE{get;set;}
        public string IP{get;set;}
        public string ServerName{get;set;}
        public string DBName{get;set;}
        public string PWD{get;set;}
        public string Customer{get;set;}
        public string IS_MAIN_DB{get;set;}
        public string IS_QFMS_DB{get;set;}
        public string IS_Mirror_DB{get;set;}
        public string MirrorDBIP{get;set;}
        public string IS_AlwaysOn_DB{get;set;}
        public string FileShare_Witness{get;set;}	
        public string IsNeed_AdjustWOQty{get;set;}
        public string IsNeed_sDayOfYear{get;set;}
        public string Owner_Name { get; set; }
        public Int64 ID { get; set; }
    
    }
    #endregion DBServer_List

    #region QMS_DefineCheckSQLjobs_Detail
    public class TQMS_DefineCheckSQLjobs_Detail
    { 
        public string ServerIP{get;set;}
        public string Type{get;set;}
        public string MailGroup{get;set;}
        public string Owner{get;set;}
        public string Jobname{get;set;}
        public Int64 Max_allowed_Duration { get; set; }
        public Int64 ID { get; set; }
    
    }
    #endregion QMS_DefineCheckSQLjobs_Detail

    public class DBA_DB
    {
        private DB db;

        public DBA_DB()
        {
            db = new DB("PubReportMain");
        }

        #region LinkedServer
        public List<TLinkedServer> LinkedServer_View(string LocalIP)
        {
            List<TLinkedServer> list = new List<TLinkedServer>();
            string sql = "SELECT LocalIP, IP, UserName, PWD, Desc1 FROM dbo.LinkedServer WHERE LocalIP ='" + LocalIP + "' ORDER BY TransDatetime";
            DataTable dt =  db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TLinkedServer>(dt);

            return list;
        
        }

        public string LinkedServer_Save( string OpType,TLinkedServer LinkServer,string OPUserID)
        {
            List<TLinkedServer> list = new List<TLinkedServer>();
        
            SqlParameter[] paraList = {
                                            db.CreateInParam("@OpType",SqlDbType.VarChar,1,OpType),
                                            db.CreateInParam("@IP",SqlDbType.VarChar,20,LinkServer.IP),
                                            db.CreateInParam("@LoginUser",SqlDbType.VarChar,20,LinkServer.UserName),
                                            db.CreateInParam("@PW",SqlDbType.VarChar,30,LinkServer.PWD),
                                            db.CreateInParam("@ServerDesc",SqlDbType.VarChar,50,LinkServer.Desc1),
                                            db.CreateInParam("@OpUser",SqlDbType.VarChar,20,OPUserID)
                                        };

            DataTable dt = db.Execute("MVC_PUBReport_CreateLinkSever", paraList).Tables[0];

            string Result = dt.Rows[0]["Result"].ToString(); ;
            return Result;


        }

        #endregion LinkedServer
        #region SVR_Define
        public List<TSVR_Define> SVR_Define_View(TSVR_Define svr)
        {
            List<TSVR_Define> list = new List<TSVR_Define>();
            string sql = "	SELECT ROW_NUMBER()OVER(ORDER BY IP) AS ID, BU,BUType,Customer,CName,Item,Context,IP,DBName,ISNew,Description FROM SVR_Define WHERE 1=1 ";

            if (svr.Item != "")
            {

                sql = sql + "AND Item = '" + svr.Item + "'";
            }
            if (svr.IP != "")
            {
                sql = sql + "AND IP = '" + svr.IP + "'";
            }
            if (svr.DBName != "")
            {
                sql = sql + "AND DBName = '" + svr.DBName + "'";
            }
            sql = sql + " order by IP ";
            DataTable dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TSVR_Define>(dt);
            return list;
        
        }
        public void SVR_Define_Delete(TSVR_Define svr)
        {
            string sql = "";
            sql = "	delete from SVR_Define WHERE ip = '" + svr.IP + "' AND Item = '"+svr.Item+"' and  DBName = '" + svr.DBName + "'";
            db.Execute(sql);
        
        }
        public List<TSVR_Define> SVR_Define_Save(TSVR_Define svr)
        {
            List<TSVR_Define> list = new List<TSVR_Define>();
            string sql = "";

            list = SVR_Define_View(svr);
            if (list.Count > 0)
            {
                SVR_Define_Delete(svr);
            }
            sql = " INSERT INTO SVR_Define(BU, BUType, Customer, CName, Item, Context, IP, DBName, ISNew, Description)"
                    + "  VALUES('" + svr.BU + "','" + svr.BUType + "','" + svr.Customer + "','" + svr.CName + "','" + svr.Item + "','" + svr.Context + "','" + svr.IP + "','" + svr.DBName + "','Y','" + svr.Description + "')";

            db.Execute(sql);
            list = SVR_Define_View(svr);

            return list;


        }
        #endregion SVR_Define

        #region MainDB_SysConfig
        public List<TMainDB_SysConfig> MainDB_SysConfig_View(TMainDB_SysConfig tb)
        {
            List<TMainDB_SysConfig> list = new List<TMainDB_SysConfig>();
            string sql = "SELECT BU,Type,ServerIP,ServerType,DBName,ClusterIP,ListenIP,Item,Context, [DESC] as Description,Transdatetime FROM dbo.MainDB_SysConfig WHERE 1 = 1";
            if (String.IsNullOrEmpty(tb.ServerIP) == false && string.IsNullOrWhiteSpace(tb.ServerIP) == false)
            {
                sql = sql + " AND ServerIP = '" + tb.ServerIP + "'";
            }
            sql = sql + "ORDER BY ServerIP";
            DataTable dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TMainDB_SysConfig>(dt);
            return list;
        
        }

        public void MainDB_SysConfig_Delete(TMainDB_SysConfig tb)
        {
            string sql = "DELETE FROM MainDB_SysConfig WHERE ServerIP ='" + tb.ServerIP + "'";
            db.Execute(sql);
        
        }

        public void MainDB_SysConfig_Save(TMainDB_SysConfig tb)
        {
            List<TMainDB_SysConfig> list = new List<TMainDB_SysConfig>();
            string sql = "";
            list = MainDB_SysConfig_View(tb);
            if (list.Count > 0)
            {

                MainDB_SysConfig_Delete(tb);
            }

            sql = "INSERT INTO dbo.MainDB_SysConfig(BU, Type, ServerIP, ServerType, DBName, ClusterIP, ListenIP, Item, Context, [DESC], Transdatetime)"+
                " VALUES('" + tb.BU + "','" + tb.Type + "','" + tb.ServerIP + "','" + tb.ServerType + "','" + tb.DBName + "','" + tb.ClusterIP + "','" + tb.ListenIP + "','" + tb.Item + "','" + tb.Context + "','"+ tb.Description +"',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
            db.Execute(sql);   
        
        }


        #endregion MainDB_SysConfig

        #region FileServer
        public List<TFileServer> FileServer_View(TFileServer tb)
        {
            List<TFileServer> list = new List<TFileServer>();
            string sql = "SELECT ClusterIP,IP1,IP2,ActualIP,ServerDesc,[UID] AS UserID,TransDatetime FROM FileServer WHERE 1 = 1";
            if (String.IsNullOrEmpty(tb.ClusterIP) == false && string.IsNullOrWhiteSpace(tb.ClusterIP) == false)
            {
                sql = sql + " AND ClusterIP = '" + tb.ClusterIP + "'";
            }
            sql = sql + "ORDER BY ClusterIP";
            DataTable dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TFileServer>(dt);
            return list;

        }

        public void FileServer_Delete(TFileServer tb)
        {
            string sql = "DELETE FROM   FileServer WHERE  ClusterIP = '" + tb.ClusterIP + "'";
            db.Execute(sql);

        }

        public void FileServer_Save(TFileServer tb)
        {
            List<TFileServer> list = new List<TFileServer>();
            string sql = "";
            list = FileServer_View(tb);
            if (list.Count > 0)
            {

                FileServer_Delete(tb);
            }

            sql = "INSERT INTO dbo.FileServer(ClusterIP, IP1, IP2,ActualIP, ServerDesc, UID, TransDatetime )" +
                "  VALUES('" + tb.ClusterIP + "','" + tb.IP1 + "','" + tb.IP2 + "','" + tb.ActualIP + "','" + tb.ServerDesc + "','" + tb.UserID + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS') )";
            db.Execute(sql);

        }

        #endregion FileServer

        #region DBShareFolderList
        public List<TDBShareFolderList> DBShareFolderList_View(TDBShareFolderList tb)
        {
            List<TDBShareFolderList> list = new List<TDBShareFolderList>();
            string sql = "SELECT ROW_NUMBER()OVER(ORDER BY ServerIP) AS ID, PU,Type,ServerIP,Path,[DB_Name],UserName,Password,LimitedFileNum,LimitedFileSize,BackUpServerIP   FROM DBShareFolderList WHERE 1 = 1 ";
            if (String.IsNullOrEmpty(tb.ServerIP) == false && string.IsNullOrWhiteSpace(tb.ServerIP) == false)
            {
                sql = sql + " AND ServerIP = '"+ tb.ServerIP +"' ";
            }
            if (String.IsNullOrEmpty(tb.Path) == false && string.IsNullOrWhiteSpace(tb.Path) == false)
            {
                sql = sql + " AND Path = '"+ tb.Path +"'  ";
            }
            sql = sql + "ORDER BY ServerIP";
            DataTable dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TDBShareFolderList>(dt);
            return list;

        }

        public void DBShareFolderList_Delete(TDBShareFolderList tb)
        {
            string sql = "DELETE  FROM DBShareFolderList WHERE ServerIP = '"+ tb.ServerIP +"' AND Path ='"+ tb.Path +"' ";
            db.Execute(sql);

        }

        public void DBShareFolderList_Save(TDBShareFolderList tb)
        {
            List<TDBShareFolderList> list = new List<TDBShareFolderList>();
            string sql = "";
            list = DBShareFolderList_View(tb);
            if (list.Count > 0)
            {

                DBShareFolderList_Delete(tb);
            }

            sql = "INSERT INTO dbo.DBShareFolderList(PU, Type, ServerIP, Path, UserName, Password, LimitedFileNum, LimitedFileSize, BackUpServerIP)" +
                "  VALUES('"+ tb.PU +"','"+ tb.Type +"','"+ tb.ServerIP +"','"+ tb.Path +"','"+ tb.UserName +"','"+ tb.Password +"',"+ tb.LimitedFileNum +","+ tb.LimitedFileSize+",'"+ tb.BackUpServerIP +"')";
            db.Execute(sql);

        }
        #endregion DBShareFolderList

        #region  DBServer_List
        public List<TDBServer_List> DBServer_List_View(TDBServer_List tb)
        {
            List<TDBServer_List> list = new List<TDBServer_List>();
            string sql;
            sql = "SELECT ROW_NUMBER()OVER(ORDER BY IP) AS ID,BU,TYPE,IP,ServerName,DBName,PWD,Customer,IS_MAIN_DB,IS_QFMS_DB,IS_Mirror_DB,MirrorDBIP,IS_AlwaysOn_DB,FileShare_Witness,IsNeed_AdjustWOQty,IsNeed_sDayOfYear,Owner_Name "+
                    " FROM dbo.DBServer_LIST WHERE 1 = 1";


            if (String.IsNullOrEmpty(tb.IP) == false && string.IsNullOrWhiteSpace(tb.IP) == false)
            {
                sql = sql + " AND IP = '" + tb.IP + "' ";
            }
            if (String.IsNullOrEmpty(tb.DBName) == false && string.IsNullOrWhiteSpace(tb.DBName) == false)
            {
                sql = sql + " AND DBName = '" + tb.DBName + "'  ";
            }
            sql = sql + "ORDER BY IP";
            DataTable dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TDBServer_List>(dt);
            return list;

        }

        public void DBServer_List_Delete(TDBServer_List tb)
        {
            string sql = "DELETE FROM dbo.DBServer_LIST WHERE   IP = '"+ tb.IP +"' AND DBName ='"+ tb.DBName +"' ";
            db.Execute(sql);

        }

        public void DBServer_List_Save(TDBServer_List tb)
        {
            List<TDBServer_List> list = new List<TDBServer_List>();
            string sql = "";
            list = DBServer_List_View(tb);
            if (list.Count > 0)
            {

                DBServer_List_Delete(tb);
            }

            sql = "INSERT INTO dbo.DBServer_LIST(BU, TYPE, IP, ServerName, DBName, PWD, Customer, IS_MAIN_DB, IS_QFMS_DB, IS_Mirror_DB, MirrorDBIP, IS_AlwaysOn_DB, FileShare_Witness, IsNeed_AdjustWOQty, IsNeed_sDayOfYear, Owner_Name)" +
                    " VALUES('" + tb.BU + "','" + tb.TYPE + "','" + tb.IP + "','" + tb.ServerName + "','" + tb.DBName + "','" + tb.PWD + "','" + tb.Customer + "','" + tb.IS_MAIN_DB + "','" + tb.IS_QFMS_DB + "','" +
                        tb.IS_Mirror_DB + "','" + tb.MirrorDBIP + "','" + tb.IS_AlwaysOn_DB + "','" + tb.FileShare_Witness + "','" + tb.IsNeed_AdjustWOQty + "','" + tb.IsNeed_sDayOfYear + "','" + tb.Owner_Name + "')";
            
            db.Execute(sql);

        }
        #endregion DBServer_List


        #region  QMS_DefineCheckSQLjobs_Detail
        public List<string> DBServerIP_View()
        {
            List<string> list = new List<string>();
            DataTable dt = new DataTable();

            string sql = "SELECT DISTINCT ServerIP FROM report.dbo.SNA_SVR_Config WHERE DBPass <>'NA:NA' order by ServerIP";

            dt = db.Execute(sql).Tables[0];
            int rowCount = dt.Rows.Count;
            for (int i = 0; i < rowCount ;i++ )
            {
                list.Add(dt.Rows[i]["ServerIP"].ToString());
            }


            return list;
        
        
        }


        public List<TQMS_DefineCheckSQLjobs_Detail> QMS_DefineCheckSQLjobs_Detail_View(TQMS_DefineCheckSQLjobs_Detail tb)
        {
            List<TQMS_DefineCheckSQLjobs_Detail> list = new List<TQMS_DefineCheckSQLjobs_Detail>();
            string sql;
            sql = "SELECT ROW_NUMBER()OVER(ORDER BY ServerIP) AS ID, ServerIP, Type, MailGroup, Owner, Jobname, Max_allowed_Duration, Trans_DateTime" +
                    "  FROM SF_Maintain.dbo.QMS_DefineCheckSQLjobs_Detail WHERE 1 = 1";


            if (String.IsNullOrEmpty(tb.ServerIP) == false && string.IsNullOrWhiteSpace(tb.ServerIP) == false)
            {
                sql = sql + " AND ServerIP = '" + tb.ServerIP + "' ";
            }
            if (String.IsNullOrEmpty(tb.Jobname) == false && string.IsNullOrWhiteSpace(tb.Jobname) == false)
            {
                sql = sql + " AND Jobname = '" + tb.Jobname + "'  ";
            }
            sql = sql + "ORDER BY ServerIP,Jobname";
            DataTable dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TQMS_DefineCheckSQLjobs_Detail>(dt);
            return list;

        }

        public void QMS_DefineCheckSQLjobs_Detail_Delete(TQMS_DefineCheckSQLjobs_Detail tb)
        {
            string sql = " DELETE FROM SF_Maintain.dbo.QMS_DefineCheckSQLjobs_Detail WHERE ServerIP = '"+ tb.ServerIP +"' AND Jobname='"+ tb.Jobname +"' ";
            db.Execute(sql);

        }

        public void QMS_DefineCheckSQLjobs_Detail_Save(TQMS_DefineCheckSQLjobs_Detail tb)
        {
            List<TQMS_DefineCheckSQLjobs_Detail> list = new List<TQMS_DefineCheckSQLjobs_Detail>();
            string sql = "";
            list = QMS_DefineCheckSQLjobs_Detail_View(tb);
            if (list.Count > 0)
            {

                QMS_DefineCheckSQLjobs_Detail_Delete(tb);
            }

            sql = "INSERT INTO SF_Maintain.dbo.QMS_DefineCheckSQLjobs_Detail(ServerIP, Type, MailGroup, Owner, Jobname, Max_allowed_Duration, Trans_DateTime)";
            sql = sql + " VALUES('" + tb.ServerIP + "','" + tb.Type + "','" + tb.MailGroup + "','" + tb.Owner + "','" + tb.Jobname + "','" + tb.Max_allowed_Duration + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
            db.Execute(sql);

        }
        #endregion QMS_DefineCheckSQLjobs_Detail
    }
    
}