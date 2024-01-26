using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.SF;
using MVC_PubReport.Models.Public;
using MVC_PubReport.Models.Files;
using System.Data;
using System.IO;
using MVC_PubReport.Models.AppSettings;

namespace MVC_PubReport.Controllers
{
    public class SFController : Controller
    {
        private TUser user = new TUser();
        public double ProcessRate = 0.0;

        #region Mail
        public ActionResult Mail()
        {
            return View();
        }

        public ActionResult Mail_View(string SystemName)
        {
            string Message = "";
            List<TMail> TableData = new List<TMail>();
 
            SF sf = new SF("PubReportMain");
            TMail t = new TMail();
            t.System_Name = SystemName;
            TableData = sf.Mail_View(t); 
            Message = "OK"; 
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }

        public ActionResult Mail_Save(string SystemName,string RMailAccount,string CMailAccount)
        {
            string Message = "";
            List<TMail> TableData = new List<TMail>();
            SF sf = new SF("PubReportMain");
            

            if (String.IsNullOrEmpty(RMailAccount) == false && string.IsNullOrWhiteSpace(RMailAccount) == false)
            {
                string[] mailAccount = RMailAccount.Split(new char[] { ';' });
                for (int i = 0; i < mailAccount.Length; i++)
                {
                    TMail t = new TMail();
                    t.System_Name = SystemName;
                    t.Mail_Account = mailAccount[i];
                    t.Mail_Type = "1";
                    TableData.Add(t);
                    sf.Mail_Save(t);
                
                } 
            }

            if (String.IsNullOrEmpty(CMailAccount) == false && string.IsNullOrWhiteSpace(CMailAccount) == false)
            {
                string[] mailAccount = CMailAccount.Split(new char[] { ';' });
                for (int i = 0; i < mailAccount.Length; i++)
                {
                    TMail t = new TMail();
                    t.System_Name = SystemName;
                    t.Mail_Account = mailAccount[i];
                    t.Mail_Type = "2";
                    TableData.Add(t);
                    sf.Mail_Save(t);
                }
            }

            
            Message = "OK";
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult Mail_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TMail> TableData = JsonHelper.DeserializeJsonToList<TMail>(jsonData);

                foreach (var item in TableData)
                {
                    SF sf = new SF("PubReportMain");
                    item.Trans_id = user.UserID;
                    sf.Mail_Delete(item);
                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion Mail

        #region QIMS_DutyInfo
        public ActionResult QIMS_DutyInfo()
        {
            return View();
        }

        public ActionResult QIMS_DutyInfo_View(string Line,string Shift)
        {
            string Message = "";
            List<TQIMS_DutyInfo> TableData = new List<TQIMS_DutyInfo>();

            SF sf = new SF("PubReportMain");
            TQIMS_DutyInfo t = new TQIMS_DutyInfo();
            t.Line = Line;
            t.Shift = Shift;
            TableData = sf.QIMS_DutyInfo_View(t);
            Message = "OK";
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult QIMS_DutyInfo_Delete(string jsonData)
        {
            string Message = "";

            List<TQIMS_DutyInfo> TableData = JsonHelper.DeserializeJsonToList<TQIMS_DutyInfo>(jsonData);
          
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                SF sf = new SF("PubReportMain");
                foreach (var item in TableData)
                {
                    TQIMS_DutyInfo t = new TQIMS_DutyInfo();
                    t = item;
                    sf.QIMS_DutyInfo_View_Delete(t);

                }
                Message = "OK";
            }          
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult QIMS_DutyInfo_View_InsertUpdate(string Line, string Shift, string type, string EmployeeID, string DutyName, string DutyPhone)
        {       
            List<TQIMS_DutyInfo> TableData = new List<TQIMS_DutyInfo>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                SF sf = new SF("PubReportMain");
                TQIMS_DutyInfo t = new TQIMS_DutyInfo();
                t.Line = Line;
                t.Shift = Shift;
                t.Type = type;
                t.EmployeeID = EmployeeID;
                t.DutyName = DutyName;
                t.DutyPhone = DutyPhone;
                sf.QIMS_DutyInfo_View_InsertUpdate(t,user.UserID);
                TableData = sf.QIMS_DutyInfo_View(t);
                Message = "OK";
                //return QIMS_DutyInfo_View(Line, Shift); 
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
            
        }
        public JsonResult QIMS_DutyInfo_UploadExcel()
        {
            string Message = "";
            List<TQIMS_DutyInfo> TableData = new List<TQIMS_DutyInfo>();


            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                MFile mfile = new MFile();
                string FilePathAtServer = @"../Upload/Users/";
                HttpPostedFileBase file = Request.Files["file"];//接收客户端传递过来的数据.
                string uploadResult = "";
                //mfile = MFile.UpLoadFile(file, user.UserID, FilePathAtServer, "IE_ModelStage_Temp", ref uploadResult);
                mfile = MFile.UpLoadFile(file, FilePathAtServer, user.UserID, ref uploadResult);
                FilePathAtServer = mfile.UploadPath;

                if (uploadResult == "OK")
                {
                    Message = "OK";
                    try
                    {
                        DataTable dtTemp = MFile.ExcelToTable(FilePathAtServer, 0, "", "Y");

                        List<TQIMS_DutyInfo> list = new List<TQIMS_DutyInfo>();
                        //List<TIE_DepartMent> listFail = new List<TIE_DepartMent>();
                        SF db = new SF("PubReportMain");

                        list = DataTableTool.ToList<TQIMS_DutyInfo>(dtTemp);
                        double listCount = list.Count;
                        for (int i = 0; i < listCount; i++)
                        {
                             
                            string Line = list[i].Line;
                            string Type = list[i].Type;
                            string Shift = list[i].Shift;
                            string EmployeeID = list[i].EmployeeID;
                            string DutyName = list[i].DutyName;
                            string DutyPhone = list[i].DutyPhone;
                            string Remark = list[i].Remark;

                            string RowFail = "";
                            if (Line.Trim() == "" || String.IsNullOrEmpty(Line) == true)
                            {
                                RowFail = RowFail + "[Line]不能为空 ";
                                list[i].Line = RowFail;
                            }
                            if (Shift.Trim() == "" || String.IsNullOrEmpty(Shift) == true)
                            {
                                RowFail = RowFail + "[Shift]不能为空 ";
                                list[i].Shift = RowFail;
                            }
                            if (EmployeeID.Trim() == "" || String.IsNullOrEmpty(EmployeeID) == true)
                            {
                                RowFail = RowFail + "[EmployeeID]不能为空 ";
                                list[i].EmployeeID = RowFail;
                            }
                            if (DutyPhone.Trim() == "" || String.IsNullOrEmpty(DutyPhone) == true)
                            {
                                RowFail = RowFail + "[Depart_No]不能为空 ";
                                list[i].DutyPhone = RowFail;
                            }
 

                            if (RowFail.Trim() != "")
                            {
                                TableData.Add(list[i]);
                            }
                            else
                            {
                                db.QIMS_DutyInfo_View_InsertUpdate(list[i],user.UserID);
                            }

                            ProcessRate = ((i + 1) / listCount) * 100;

                        }

                    }
                    catch
                    {
                        Message = "处理Excel出现异常，请联系QMS!";
                    }
                }
                else
                {
                    Message = "上传Excel失败";
                }
            }



            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion QIMS_DutyInfo

        #region Wechart
        public ActionResult WeChart()
        {
            SF db = new SF("QIS");
            List<SelectListItem> System_NameListItem = new List<SelectListItem>();
            List<string> list = db.GetSystem_Name();

            foreach (var item in list)
            {
                System_NameListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["System_NameListItem"] = new SelectList(System_NameListItem, "Value", "Text", "");

            return View();
        }

        public ActionResult WeChart_View(string PU,string Type,string SystemName,string EmployeeID)
        {
            string Message = "";
            List<TWeChart> TableData = new List<TWeChart>();

            SF sf = new SF("QIS");
            TWeChart t = new TWeChart();            

            t.PU = PU;
            t.Type = Type;
            t.System_Name = SystemName;
            t.EmployeeID = EmployeeID;

            TableData = sf.WeChart_View(t);
            Message = "OK";
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult WeChart_Save(string PU, string Type, string SystemName, string EmployeeID)
        {
            List<TWeChart> TableData = new List<TWeChart>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                SF sf = new SF("QIS");
                TWeChart t = new TWeChart();

                t.PU = PU;
                t.Type = Type;
                t.System_Name = SystemName;
                t.EmployeeID = EmployeeID;

                sf.WeChart_Save(t, user.UserID);
                TableData = sf.WeChart_View (t);
                Message = "OK"; 
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult; 
        }

        public ActionResult Wechart_Delete(string jsonData)
        {
            string Message = "";

            List<TWeChart> TableData = JsonHelper.DeserializeJsonToList<TWeChart>(jsonData);

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                SF sf = new SF("QIS");
                foreach (var item in TableData)
                {
                    TWeChart t = new TWeChart();
                    t = item;
                    sf.WeChart_Delete(t,user.UserID);

                }
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }

        public JsonResult WeChart_UploadExcel()
        {
            string Message = "";
            List<TWeChart> TableData = new List<TWeChart>();


            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                MFile mfile = new MFile();
                string FilePathAtServer = @"../Upload/Users/";
                HttpPostedFileBase file = Request.Files["file"];//接收客户端传递过来的数据.
                string uploadResult = "";
                //mfile = MFile.UpLoadFile(file, user.UserID, FilePathAtServer, "IE_ModelStage_Temp", ref uploadResult);
                mfile = MFile.UpLoadFile(file, FilePathAtServer, user.UserID, ref uploadResult);
                FilePathAtServer = mfile.UploadPath;

                if (uploadResult == "OK")
                {
                    Message = "OK";
                    try
                    {
                        DataTable dtTemp = MFile.ExcelToTable(FilePathAtServer, 0, "", "Y");
                        SF sf = new SF("QIS");
                        List<TWeChart> list = new List<TWeChart>();   
                        list = DataTableTool.ToList<TWeChart>(dtTemp);
                        double listCount = list.Count;
                        for (int i = 0; i < listCount; i++)
                        {
                            string PU = list[i].PU;
                            string Type = list[i].Type;
                            string SystemName = list[i].System_Name;
                            string EmployeeID = list[i].EmployeeID;
                           
                            string RowFail = "";
                            if (PU.Trim() == "" || String.IsNullOrEmpty(PU) == true)
                            {
                                RowFail = RowFail + "[PU]不能为空 ";
                                list[i].PU = RowFail;
                            }
                            if (Type.Trim() == "" || String.IsNullOrEmpty(Type) == true)
                            {
                                RowFail = RowFail + "[Type]不能为空 ";
                                list[i].Type = RowFail;
                            }
                            if (EmployeeID.Trim() == "" || String.IsNullOrEmpty(EmployeeID) == true)
                            {
                                RowFail = RowFail + "[EmployeeID]不能为空 ";
                                list[i].EmployeeID = RowFail;
                            }
                            if (SystemName.Trim() == "" || String.IsNullOrEmpty(SystemName) == true)
                            {
                                RowFail = RowFail + "[SystemName]不能为空 ";
                                list[i].System_Name = RowFail;
                            }


                            if (RowFail.Trim() != "")
                            {
                                TableData.Add(list[i]);
                            }
                            else
                            {
                                sf.WeChart_Save(list[i], user.UserID);
                            }

                            ProcessRate = ((i + 1) / listCount) * 100;

                        }

                    }
                    catch
                    {
                        Message = "处理Excel出现异常，请联系QMS!";
                    }
                }
                else
                {
                    Message = "上传Excel失败";
                }
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion wechart
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

        public JsonResult ProcessGetStatus()
        {
            string Message = "OK"; double TableData = ProcessRate;

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }

        #region LineStopMail
        public ActionResult LineStopMail()
        {
            return OnView();
        }

        public JsonResult Mail_Query(string PU, string Type, string UID)
        {
            string Message = "OK";
            List<TLineStopMail> TableData = new List<TLineStopMail>();
            if (Message == "OK")
            {
                TLineStop t = new TLineStop();
                t.PU = PU;
                t.Type = Type;
                string DBKey = PU + "_" + Type + "_PubDb";

                TLineStopDB db = new TLineStopDB(DBKey);

                TableData = db.Mail_View(UID);
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult LineStopMail_Save(string PU, string Type, string UID, string MailAccount, string HostName, string MailLevel, string FixLevel)
        {
            var Message = "";
            List<TLineStopMail> TableData = new List<TLineStopMail>();
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {

                try
                {
                    string DBKey = PU + "_" + Type + "_PubDb";
                    TLineStopDB db = new TLineStopDB(DBKey);
                    TLineStopMail t = new TLineStopMail();
                    TLineStop m = new TLineStop();
                    t.UID = UID;
                    t.MailAccount = MailAccount.Trim();
                    t.HostName = HostName.Trim();
                    t.MailLevel = MailLevel.Trim();
                    t.FixLevel = FixLevel.Trim();
                    m.PU = PU;
                    m.Type = Type;
                    Message = db.Mail_Save(t, m);
                    if (Message == "OK")
                    {
                        TableData.Add(t);
                    }
                }
                catch (Exception ex)
                {

                    Message = "保存失败" + ex.ToString();
                }
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        #endregion

        #region Upload
        public ActionResult OnView()
        {
            List<SelectListItem> PUListItem = new List<SelectListItem>();
            List<SelectListItem> TypeListItem = new List<SelectListItem>();

            PUListItem = AppSettingsRead.AppSettingsReadKey("PU", ';');
            TypeListItem = AppSettingsRead.AppSettingsReadKey("Type", ';');

            ViewData["PUListItem"] = new SelectList(PUListItem, "Value", "Text", "");
            ViewData["TypeListItem"] = new SelectList(TypeListItem, "Value", "Text", "");
            return View();
        }
        public JsonResult Upload_QueryID(string PU, string Type, string Auth)
        {
            List<string> TableData = new List<string>();
            List<SelectListItem> Staton = new List<SelectListItem>();
            string DBKey = PU + "_" + Type + "_PubDb";

            List<SelectListItem> IDListItem = new List<SelectListItem>();

            JsonResult jsonResult = new JsonResult();
            if (Auth == "FPY" || Auth == "WaitRepair" || Auth == "WaitTest" || Auth == "SeverityLevel")
            {
                List<string> TableDataStaton = new List<string>();
                List<TLineStopFPY> TableDataViewFPY = new List<TLineStopFPY>();
                List<TLineStopWaitRepair> TableDataWaitRepair = new List<TLineStopWaitRepair>();
                List<TSeverityLevel> TableDataTSeverityLevel = new List<TSeverityLevel>();
                TLineStopDB db = new TLineStopDB(DBKey);
                TableData = db.Upload_GetID(Auth);
                if (Auth == "FPY")
                {
                    TableDataViewFPY = db.FPY_View(PU, Type, "", "", "");
                    Staton = AppSettingsRead.AppSettingsReadKey(Type, ';');
                    for (int i = 0; i < Staton.Count; i++)
                    {
                        TableDataStaton.Add(Staton[i].Value.ToString());
                    }
                }
                if (Auth == "WaitRepair" || Auth == "WaitTest")
                {
                    TableDataWaitRepair = db.WaitRepair_View(PU, Type, "", "", "", Auth);
                    if (Auth == "WaitTest")
                    {
                        Staton = AppSettingsRead.AppSettingsReadKey(Type, ';');
                        for (int i = 0; i < Staton.Count; i++)
                        {
                            TableDataStaton.Add(Staton[i].Value.ToString());
                        }
                    }
                }
                if (Auth == "SeverityLevel")
                {
                    TableDataTSeverityLevel = db.SeverityLevel_View("");
                }

                var jsondata = new { tableData = TableData, tableData2 = TableDataStaton, tableData3 = TableDataViewFPY, tableData4 = TableDataWaitRepair, tableData5 = TableDataTSeverityLevel };
                jsonResult.Data = jsondata;
            }
            else if (Auth == "Mail")
            {
                TLineStopDB db = new TLineStopDB(DBKey);
                List<TLineStopMail> TableDataTMail = new List<TLineStopMail>();
                TableDataTMail = db.Mail_View("");
                var jsondata = new { tableData = TableDataTMail };
                jsonResult.Data = jsondata;
            }
            else
            {
                TErrorCodeDB db = new TErrorCodeDB(DBKey);
                TableData = db.Upload_GetID(Auth);
                var jsondata = new { tableData = TableData };
                jsonResult.Data = jsondata;
            }


            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public string UpLoadFileToServer()
        {

            MFile mfile = new MFile();
            string FilePathAtServer = @"../Upload/Users/";
            HttpPostedFileBase file = Request.Files["file"];//接收客户端传递过来的数据.


            string uploadResult = "";
            //mfile = MFile.UpLoadFile(file, user.UserID, FilePathAtServer, file.FileName, ref uploadResult);
            mfile = MFile.UpLoadFile(file, FilePathAtServer, user.UserID, ref uploadResult);
            FilePathAtServer = mfile.UploadPath;

            if (uploadResult == "OK")
            {
                //dt = MFile.ExcelToTable(FilePathAtServer, 4, SheetName, "N");
                return FilePathAtServer;
            }
            else
            {
                return "";
            }
        }

        public JsonResult Upload_Query(string PU, string Type, string Auth, string Table, string ID)
        {
            string Message = "OK";
            List<TErrorCode> TableData = new List<TErrorCode>();
            if (Message == "OK")
            {
                TErrorCodePU t = new TErrorCodePU();
                t.PU = PU;
                t.Type = Type;
                string DBKey = PU + "_" + Type + "_PubDb";
                string selID = ID;
                TErrorCodeDB db = new TErrorCodeDB(DBKey);

                TableData = db.ErrorCode_View(Table, selID);
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult Upload_Delete(string jsonData, string PU, string Type, string Auth, string Table)
        {
            string Message = "OK";

            if (CheckUserSession())
            {
                if (Message == "OK")
                {
                    List<TErrorCode> TableData = JsonHelper.DeserializeJsonToList<TErrorCode>(jsonData);
                    string DBKey = PU + "_" + Type + "_PubDb";
                    TErrorCodeDB db = new TErrorCodeDB(DBKey);
                    foreach (var item in TableData)
                    {
                        db.ErrorCode_Delete(item, user.UserID, Table);
                    }


                    Message = "OK";
                }
            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion Upload

    }
}
