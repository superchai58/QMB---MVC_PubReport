using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MVC_PubReport.Models.IE;
using MVC_PubReport.Models.Query;
using MVC_PubReport.Models.Public;
using MVC_PubReport.Models.Files;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.SF;
using MVC_PubReport.Models.AppSettings;

namespace MVC_PubReport.Controllers
{
    public class QueryController : Controller
    {
        
        private TUser user = new TUser();

        #region Efficiency
        public ActionResult Efficiency()
        {
            TIEDB db = new TIEDB();
            DateTime dt = new System.DateTime();
            List<SelectListItem> ModeListItem = new List<SelectListItem>();

            dt = System.DateTime.Now.AddDays(-1);
            string strDateTime = dt.ToString("yyyyMMdd");

            List<string> list = db.Efficiency_GetMode(strDateTime);

            foreach (var item in list)
            {
                ModeListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["ModeListItem"] = new SelectList(ModeListItem, "Value", "Text", "");

            return View();
        }
        public ActionResult Efficiency_GetMode(string Date)
        {
            List<string> TableData = new List<string>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIEDB db = new TIEDB();
                TableData = db.Efficiency_GetMode(Date);


                Message = "OK";

            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult Efficiency_Query(string Date ="", string Mode ="", string Line ="", string Shift ="")
        {
            List<TEfficiency> TableData = new List<TEfficiency>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TEfficiency eff = new TEfficiency();
                TIEDB db = new TIEDB();

                eff.WorkDate = Date;
                eff.Mode = Mode;
                eff.Line = Line;
                eff.Shift = Shift;

                TableData = db.Efficiency_Query(eff);

                Message = "OK";

            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion Efficiency

        #region IE_InputQty_TestWO
        public ActionResult IE_InputQty_TestWO()
        {
            TIEDB db = new TIEDB();
            DateTime dt = new System.DateTime();
            List<SelectListItem> PUListItem = new List<SelectListItem>();

            dt = System.DateTime.Now.AddDays(-1);
            string strDateTime = dt.ToString("yyyyMMdd");

            List<string> list = db.IE_InputQty_TestWO_GetPU(strDateTime);

            foreach (var item in list)
            {
                PUListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["PUListItem"] = new SelectList(PUListItem, "Value", "Text", "");

            return View();
        }
        public ActionResult IE_InputQty_TestWOGetPU(string Date)
        {
            List<string> TableData = new List<string>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIEDB db = new TIEDB();
                TableData = db.IE_InputQty_TestWO_GetPU(Date);


                Message = "OK";

            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public ActionResult IE_InputQty_TestWO_Query(string Date = "", string PU = "", string Line = "", string Shift = "", string Model = "", string WO = "")
        {
            List<TIE_InputQty_TestWO> TableData = new List<TIE_InputQty_TestWO>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_InputQty_TestWO product = new TIE_InputQty_TestWO();
                TIEDB db = new TIEDB();

                product.TransDate = Date;
                product.Mode = PU;
                product.Line = Line;
                product.Shift = Shift;
                product.Model = Model;
                product.WO = WO;

                TableData = db.IE_InputQty_TestWO_Query(product);

                Message = "OK";

            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        #endregion IE_InputQty_TestWO
        #region IE_InputQty
        public ActionResult IE_InputQty()
        {
            TIEDB db = new TIEDB();
            DateTime dt = new System.DateTime();
            List<SelectListItem> PUListItem = new List<SelectListItem>();

            dt = System.DateTime.Now.AddDays(-1);
            string strDateTime = dt.ToString("yyyyMMdd");

            List<string> list = db.IE_InputQty_GetPU(strDateTime);

            foreach (var item in list)
            {
                PUListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["PUListItem"] = new SelectList(PUListItem, "Value", "Text", "");

            return View();
        }
        public ActionResult IE_InputQty_GetPU(string Date)
        {
            List<string> TableData = new List<string>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIEDB db = new TIEDB();
                TableData = db.IE_DailyProductQty_GetPU(Date);


                Message = "OK";

            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public ActionResult IE_InputQty_Query(string Date = "", string PU = "", string Line = "", string Shift = "", string Model = "", string WO = "")
        {
            List<TIE_InputQty> TableData = new List<TIE_InputQty>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_InputQty product = new TIE_InputQty();
                TIEDB db = new TIEDB();

                product.TransDate = Date;
                product.Mode = PU;
                product.Line = Line;
                product.Shift = Shift;
                product.Model = Model;
                product.WO = WO;

                TableData = db.IE_InputQty_Query(product);

                Message = "OK";

            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion IE_InputQty
        #region IE_DailyProductQty
        public ActionResult IE_DailyProductQty()
        {
            TIEDB db = new TIEDB();
            DateTime dt = new System.DateTime();
            List<SelectListItem> PUListItem = new List<SelectListItem>();

            dt = System.DateTime.Now.AddDays(-1);
            string strDateTime = dt.ToString("yyyyMMdd");

            List<string> list = db.IE_DailyProductQty_GetPU(strDateTime);

            foreach (var item in list)
            {
                PUListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["PUListItem"] = new SelectList(PUListItem, "Value", "Text", "");

            return View();
        }
        public ActionResult IE_DailyProductQty_GetPU(string Date  )
        {
            List<string> TableData = new List<string>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIEDB db = new TIEDB();
                TableData = db.IE_DailyProductQty_GetPU(Date);
                

                Message = "OK";

            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult IE_DailyProductQty_Query(string Date = "", string PU = "", string Line = "", string Shift = "", string Model = "", string WO = "")
        {
            List<TIE_DailyProductQty> TableData = new List<TIE_DailyProductQty>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_DailyProductQty product = new TIE_DailyProductQty();               
                TIEDB db = new TIEDB();

                product.TransDate = Date;
                product.Mode = PU;
                product.Line = Line;
                product.Shift = Shift;
                product.Model = Model;
                product.WO = WO;

                TableData = db.IE_DailyProductQty_Query(product);

                Message = "OK";

            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion IE_DailyProductQty


        #region AssyReport

        public ActionResult AssyReport()
        {
            List<SelectListItem> PUListItem = new List<SelectListItem>();
            List<SelectListItem> TypeListItem = new List<SelectListItem>();

            PUListItem = AppSettingsRead.AppSettingsReadKey("PU", ';');
            TypeListItem = AppSettingsRead.AppSettingsReadKey("Type", ';');

            ViewData["PUListItem"] = new SelectList(PUListItem, "Value", "Text", "");
            ViewData["TypeListItem"] = new SelectList(TypeListItem, "Value", "Text", "");

            return View();
        }

        public ActionResult AssyReport_GetCustomer(string PU,string Type)
        {
            List<string> TableData = new List<string>();
            string Message = "";
            //if (CheckUserSession() == false)
            //{
            //    Message = "用户登录已失效，请重新登录后操作";
            //}
            //else
            //{
               
                TableData = AppSettingsRead.AppSettingsReadKeyStr(PU + "_" + Type + "_Customer", ';');
                Message = "OK";

            //}
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult AssyReport_GetLineModel(string PU, string Type)
        {
            List<string> TableDataLine = new List<string>();
            List<string> TableDataModel = new List<string>();
            string Message = "";
            //TableDataLine = null;
            //TableDataModel = null;

            //if (CheckUserSession() == false)
            //{
            //    Message = "用户登录已失效，请重新登录后操作";
            //}
            //else
            //{
                //SF sfDB = new SF(PU+"_"+Type+"_ReportDB");
                SF sfDB = new SF(PU+"_"+Type+"_LiveDb");                
                sfDB.AssyReportGetLineModel(PU, Type, ref TableDataModel, ref TableDataLine);
                //TableData = AppSettingsRead.AppSettingsReadKeyStr(PU + "_" + Type + "_Customer", ';');
                Message = "OK";

            //}
            var jsondata = new { result = Message, tableDataLine = TableDataLine, tableDataModel = TableDataModel };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult AssyReport_QueryData(string PU, string Type,string SDate,string EDate)
        {
            List<TAssyReport> TableData = new List<TAssyReport>();
 
            string Message = "";
 
            SF sfDB = new SF(PU + "_" + Type + "_ReportDB");

            TimeSpan ts = DateTime.ParseExact(EDate, "yyyyMMddHHmmss", null).Subtract(DateTime.ParseExact(SDate, "yyyyMMddHHmmss", null));
            int diffDay = ts.Days;

            //if (diffDay > 2)
            //{
            //    Message = "结束时间-开始时间 大于2天";
            //}
            //else
            //{
            //    TableData = sfDB.AssyReport_QueryData(PU, Type, SDate, EDate);

            //    Message = "OK";

            //}
            /*--Chai modify Query not cover date, 20220517--*/
            try
            {
                TableData = sfDB.AssyReport_QueryData(PU, Type, SDate, EDate);
                Message = "OK";
            }
            catch (Exception ex)
            {
                Message = ex.ToString().Trim();
            }
            /*--Chai modify Query not cover date, 20220517--*/

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion AssyReport

        #region IE_DailyRpt_ToMIS_log
        public ActionResult IE_DailyRpt_ToMIS_log()
        {
             
            return View();
        }


        public ActionResult IE_DailyRpt_ToMIS_log_QueryByDT(string BeginDT, string EndDT)
        {
            string Message = "";
            //List<TIE_DailyRpt_ToMIS_log> TableData = new List<TIE_DailyRpt_ToMIS_log>();
            TQueryDB tQuerDB = new TQueryDB();

            TimeSpan ts = DateTime.ParseExact(EndDT, "yyyyMMdd", null).Subtract(DateTime.ParseExact(BeginDT, "yyyyMMdd", null));
            int diffDay = ts.Days;

            if (diffDay > 31)
            {
                Message = "结束时间-开始时间 大于31天";
            }
            else
            {
                tQuerDB.IE_DailyRpt_ToMIS_log_QueryByDT(BeginDT, EndDT); 
                Message = "OK";
            }

            var jsondata = new { result = Message  };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        
        }
        #endregion IE_DailyRpt_ToMIS_log


        #region WIPWO
        public ActionResult WIPWO()
        { 
            return View();
        }
        public JsonResult QueryWIPWO_UploadExcel()
        {
            string Message = ""; 
            //List<TIE_STD_ManHour_NextMonth> TableData = new List<TIE_STD_ManHour_NextMonth>();
            int Row = 0;
            int RowSucc = 0;

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
                mfile = MFile.UpLoadFile(file, FilePathAtServer, user.UserID, ref uploadResult);
                FilePathAtServer = mfile.UploadPath;

                if (uploadResult == "OK")
                {
                    Message = "OK";
                    //try
                    {
                         
                        DataTable dtTemp = MFile.ExcelToTable(FilePathAtServer, 4, "", "N"); 
                     
                        int Rowcount = dtTemp.Rows.Count;
                        TQueryDB db = new TQueryDB("SSIS_Report");
                        db.Truncate_OpenWO();

                        for(int i = 0;i<Rowcount;i++)
                        {
                         
                            Row = Row + 1;

                            string WO = dtTemp.Rows[i][0].ToString();
                            db.Insert_OpenWO(WO);
                        }

                        //启动5.13 的job 生成 资料并发邮件给
                         TQueryDB ReportDB = new TQueryDB("");
                         ReportDB.StartJOB(); 
                    }
 
                }
                else
                {
                    Message = "上传Excel失败";
                }
            }


            //tableData = TableData,
            var jsondata = new { result = Message, rowMsg = "成功上传(" + RowSucc.ToString() + ")行,查询结果请查收您的邮件，若没收到，请联系QMS 添加SystemName:WIPWOQuery" };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion WIPWO

        #region IE_ModelCheck
        public ActionResult IE_ModelCheck()
        {
            TIEDB db = new TIEDB();
            DateTime dt = new System.DateTime();
            List<SelectListItem> PUListItem = new List<SelectListItem>();

            dt = System.DateTime.Now.AddDays(-1);
            string strDateTime = dt.ToString("yyyyMMdd");

            List<string> list = db.IE_DailyProductQty_GetPU(strDateTime);

            foreach (var item in list)
            {
                PUListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["PUListItem"] = new SelectList(PUListItem, "Value", "Text", "");

            return View();
        }
      
        public ActionResult IE_ModelCheck_Query(string Date = "", string PU = "" )
        {
            List<TIE_ModelCheck> TableData = new List<TIE_ModelCheck>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_ModelCheck product = new TIE_ModelCheck();
                TIEDB db = new TIEDB();

                product.TransDate = Date;
                product.Mode = PU;

                TableData = db.IE_ModelCheck_View(product);

                Message = "OK";

            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
   
        #endregion IE_ModelCheck
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
