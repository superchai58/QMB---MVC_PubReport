using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.DBA;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Controllers
{
    public class DBAController : Controller
    {
        private TUser user = new TUser();

        #region LinkServer
        public ActionResult DBAMaintain()
        {
            return View();
        }
        public ActionResult LinkedServer_View()
        {

            string Message = "";
            List<TLinkedServer> TableData = new List<TLinkedServer>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            { 
                DBA_DB dba = new DBA_DB();              
                TableData = dba.LinkedServer_View("10.97.1.12");
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult LinkedServer_Save(string LocalIP, string RemoteIP, string LoginUser, string PW, string ServerDesc)
        {

            string Message = "";
            List<TLinkedServer> TableData = new List<TLinkedServer>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TLinkedServer linkServer = new TLinkedServer();
                linkServer.LocalIP = LocalIP;
                linkServer.IP = RemoteIP;
                linkServer.UserName = LoginUser;
                linkServer.PWD = PW;
                linkServer.Desc1 = ServerDesc;

                DBA_DB dba = new DBA_DB();
                Message = dba.LinkedServer_Save("1", linkServer, user.UserID);
                TableData = dba.LinkedServer_View(LocalIP);

            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult LinkedServer_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TLinkedServer> TableData = JsonHelper.DeserializeJsonToList<TLinkedServer>(jsonData);

                foreach (var item in TableData)
                {
                    DBA_DB dba = new DBA_DB();
                    dba.LinkedServer_Save("2", item, user.UserID);

                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion LinkServer

        #region SVR_Define
        public ActionResult SVR_Define_View(string Item)
        {

            string Message = "";
            List<TSVR_Define> TableData = new List<TSVR_Define>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                DBA_DB dba = new DBA_DB();
                TSVR_Define svr = new TSVR_Define();
                svr.Item = Item;
                svr.IP = "";
                svr.DBName = "";
                TableData = dba.SVR_Define_View(svr);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult SVR_Define_Save(string BU, string BUType, string Customer, string CName, string Item,
                                            string Context, string IP, string DBName, string Description)
        {

            string Message = "";
            List<TSVR_Define> TableData = new List<TSVR_Define>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TSVR_Define svr = new TSVR_Define();
                svr.BU = BU;
                svr.BUType = BUType;
                svr.Customer = Customer;
                svr.CName = CName;
                svr.Item = Item;
                svr.Context = Context;
                svr.IP = IP;
                svr.DBName = DBName;
                svr.Description = Description;

                DBA_DB dba = new DBA_DB();
                TableData = dba.SVR_Define_Save(svr);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult SVR_Define_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TSVR_Define> TableData = JsonHelper.DeserializeJsonToList<TSVR_Define>(jsonData);

                foreach (var item in TableData)
                {
                    DBA_DB dba = new DBA_DB();
                    dba.SVR_Define_Delete(item);

                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion SVR_Define

        #region MainDB_SysConfig
        public ActionResult MainDB_SysConfig_View()
        {

            string Message = "";
            List<TMainDB_SysConfig> TableData = new List<TMainDB_SysConfig>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                DBA_DB dba = new DBA_DB();
                TMainDB_SysConfig tb = new TMainDB_SysConfig();
                
                tb.ServerIP = "";
                TableData = dba.MainDB_SysConfig_View(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult MainDB_SysConfig_Save(string BU, string BUType, string ServerIP, string ServerName, string ServerType,
                                            string ClusterIP, string ListenIP, string Description )
        {

            string Message = "";
            List<TMainDB_SysConfig> TableData = new List<TMainDB_SysConfig>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TMainDB_SysConfig tb = new TMainDB_SysConfig();

                tb.BU = BU;
                tb.Type = BUType;
                tb.ServerIP = ServerIP;
                tb.DBName = ServerName;
                tb.ServerType = ServerType;
                tb.ClusterIP = ClusterIP;
                tb.ListenIP = ListenIP;
                tb.Description = Description; 

                DBA_DB dba = new DBA_DB();
                dba.MainDB_SysConfig_Save (tb);
                TableData.Add(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult MainDB_SysConfig_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TMainDB_SysConfig> TableData = JsonHelper.DeserializeJsonToList<TMainDB_SysConfig>(jsonData);

                foreach (var item in TableData)
                {
                    DBA_DB dba = new DBA_DB();
                    dba.MainDB_SysConfig_Delete(item);

                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion MainDB_SysConfig

        #region FileServer
        public ActionResult FileServer_View()
        {

            string Message = "";
            List<TFileServer> TableData = new List<TFileServer>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                DBA_DB dba = new DBA_DB();
                TFileServer tb = new TFileServer();

                tb.ClusterIP = "";
                TableData = dba.FileServer_View(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult FileServer_Save(string ClusterIP, string IP1, string IP2, string ActualIP,  string Description)
        {

            string Message = "";
            List<TFileServer> TableData = new List<TFileServer>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TFileServer tb = new TFileServer();

                tb.ClusterIP = ClusterIP;
                tb.IP1 = IP1;
                tb.IP2 = IP2;
                tb.ActualIP = ActualIP;
                tb.ServerDesc = Description;
                tb.UserID = user.UserID;

                DBA_DB dba = new DBA_DB();
                dba.FileServer_Save(tb);
                TableData.Add(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult FileServer_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TFileServer> TableData = JsonHelper.DeserializeJsonToList<TFileServer>(jsonData);

                foreach (var item in TableData)
                {
                    DBA_DB dba = new DBA_DB();
                    dba.FileServer_Delete(item);

                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion FileServer


        #region DBShareFolderList
        public ActionResult DBShareFolderList_View()
        {

            string Message = "";
            List<TDBShareFolderList> TableData = new List<TDBShareFolderList>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                DBA_DB dba = new DBA_DB();
                TDBShareFolderList tb = new TDBShareFolderList();

                TableData = dba.DBShareFolderList_View(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult DBShareFolderList_Save(string PU, string Type, string ServerIP, string BackUpServerIP, string Path, string UserName, string Password,
                                                        int LimitedFileNum, int LimitedFileSize)
        {

            string Message = "";
            List<TDBShareFolderList> TableData = new List<TDBShareFolderList>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TDBShareFolderList tb = new TDBShareFolderList();
                DBA_DB dba = new DBA_DB();

                tb.PU = PU;
                tb.Type = Type;
                tb.ServerIP = ServerIP;
                tb.BackUpServerIP = BackUpServerIP;
                tb.Path = Path;
                tb.UserName = UserName;
                tb.Password = Password;
                tb.LimitedFileNum = LimitedFileNum;
                tb.LimitedFileSize = LimitedFileSize;

                dba.DBShareFolderList_Save(tb);
                TableData.Add(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult DBShareFolderList_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TDBShareFolderList> TableData = JsonHelper.DeserializeJsonToList<TDBShareFolderList>(jsonData);

                foreach (var item in TableData)
                {
                    DBA_DB dba = new DBA_DB();
                    dba.DBShareFolderList_Delete(item);

                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion DBShareFolderList

        #region DBServer_List
        public ActionResult DBServer_List_View()
        {

            string Message = "";
            List<TDBServer_List> TableData = new List<TDBServer_List>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                DBA_DB dba = new DBA_DB();
                TDBServer_List tb = new TDBServer_List();

                TableData = dba.DBServer_List_View(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult DBServer_List_Save(string BU,string TYPE,string IP,string ServerName,string DBName,
                                                string PWD,string Customer, string IS_MAIN_DB,string IS_QFMS_DB,string IS_Mirror_DB, 
                                               string  MirrorDBIP,string IS_AlwaysOn_DB,string FileShare_Witness,string IsNeed_AdjustWOQty,
                                               string IsNeed_sDayOfYear,string Owner_Name) 
        {

            string Message = "";
            List<TDBServer_List> TableData = new List<TDBServer_List>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TDBServer_List tb = new TDBServer_List();
                DBA_DB dba = new DBA_DB();

                tb.BU = BU;

                tb.TYPE = TYPE;
                tb.IP = IP;
                tb.ServerName = ServerName;
                tb.DBName = DBName;
                tb.PWD = PWD;
                tb.Customer = Customer;
                tb.IS_MAIN_DB = IS_MAIN_DB;
                tb.IS_QFMS_DB = IS_QFMS_DB;
                tb.IS_Mirror_DB = IS_Mirror_DB;
                tb.MirrorDBIP = MirrorDBIP;
                tb.IS_AlwaysOn_DB = IS_AlwaysOn_DB;
                tb.FileShare_Witness = FileShare_Witness;
                tb.IsNeed_AdjustWOQty = IsNeed_AdjustWOQty;
                tb.IsNeed_sDayOfYear = IsNeed_sDayOfYear;
                tb.Owner_Name = Owner_Name;
                

                dba.DBServer_List_Save (tb);
                TableData.Add(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult DBServer_List_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TDBServer_List> TableData = JsonHelper.DeserializeJsonToList<TDBServer_List>(jsonData);

                foreach (var item in TableData)
                {
                    DBA_DB dba = new DBA_DB();
                    dba.DBServer_List_Delete(item);

                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion DBServer_List

        #region QMS_DefineCheckSQLjobs_Detail

        public ActionResult QMS_DefineCheckSQLjob()            
        {

            DBA_DB dba = new DBA_DB();
            List<SelectListItem> ServerIP = new List<SelectListItem>();
            List<string> list = dba.DBServerIP_View();

            foreach (var item in list)
            {
                ServerIP.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["ServerIP"] = new SelectList(ServerIP, "Value", "Text", "");

            return View();
        }

        public ActionResult QMS_DefineCheckSQLjobs_Detail_View(string ServerIP,string Jobname)
        {

            string Message = "";
            List<TQMS_DefineCheckSQLjobs_Detail> TableData = new List<TQMS_DefineCheckSQLjobs_Detail>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                DBA_DB dba = new DBA_DB();
                TQMS_DefineCheckSQLjobs_Detail tb = new TQMS_DefineCheckSQLjobs_Detail();
                tb.ServerIP = ServerIP;
                tb.Jobname = Jobname;

                TableData = dba.QMS_DefineCheckSQLjobs_Detail_View(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult QMS_DefineCheckSQLjobs_Detail_Save(string ServerIP, string Type, string MailGroup, string Owner, string Jobname, int Max_allowed_Duration)
        {

            string Message = "";
            List<TQMS_DefineCheckSQLjobs_Detail> TableData = new List<TQMS_DefineCheckSQLjobs_Detail>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TQMS_DefineCheckSQLjobs_Detail tb = new TQMS_DefineCheckSQLjobs_Detail();
                DBA_DB dba = new DBA_DB();

                tb.ServerIP = ServerIP;
                tb.Type = Type;
                tb.MailGroup = MailGroup;
                tb.Owner = Owner;
                tb.Jobname = Jobname;
                tb.Max_allowed_Duration = Max_allowed_Duration;

                dba.QMS_DefineCheckSQLjobs_Detail_Save (tb);
                TableData.Add(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult QMS_DefineCheckSQLjobs_Detail_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TQMS_DefineCheckSQLjobs_Detail> TableData = JsonHelper.DeserializeJsonToList<TQMS_DefineCheckSQLjobs_Detail>(jsonData);

                foreach (var item in TableData)
                {
                    DBA_DB dba = new DBA_DB();
                    dba.QMS_DefineCheckSQLjobs_Detail_Delete (item);

                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion QMS_DefineCheckSQLjobs_Detail

        private bool CheckUserSession()
        {
            if (Session["Pub_Report_User"] == null)
            {
                return false;
            }
            else
            {
                user = Session["Pub_Report_User"] as TUser;
                return true;
            }

        }
    }
}
