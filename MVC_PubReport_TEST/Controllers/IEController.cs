using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_PubReport.Models.IE;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.Public;
using MVC_PubReport.Models.Files;
using System.Data;
using System.IO;


namespace MVC_PubReport.Controllers
{
    public class IEController : Controller
    {
       
        private TUser user = new TUser();
        public  double ProcessRate =  0.0;

        #region IE_FQALineMapping

        public ActionResult IE_FQALineMapping()
        {
            TIEDB db = new TIEDB();          
            List<SelectListItem> PUListItem = new List<SelectListItem>();
            List<string> list = db.IE_DepartMent_GetDepartNO();

            foreach (var item in list)
            {
                PUListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["DepartNOListItem"] = new SelectList(PUListItem, "Value", "Text", "");

            return View();
        }

        public ActionResult IE_FQALineMapping_GetMode(string DepartNo )
        {
            List<string> TableData = new List<string>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {


                TableData = db.IE_DepartMent_GetMode(DepartNo);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public ActionResult IE_FQALineMapping_GetData(string DepartNo = "", string Mode = "", string QLineNo = "", string LineNo = "")
        {
            List<TIE_FQALineMapping> TableData = new List<TIE_FQALineMapping>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_FQALineMapping fqaLine = new TIE_FQALineMapping();
                fqaLine.Depart_No = DepartNo;
                fqaLine.Mode = Mode;
                fqaLine.FQA_Line_No = QLineNo;
                fqaLine.Line_No = LineNo;

                TableData = db.IE_FQALineMapping_GetData(fqaLine);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult IE_FQALineMapping_SaveData(string DepartNo , string Mode, string QLineNo, string LineNo ,string QShift,string Shift,string QLine,string Line)
        {
            List<TIE_FQALineMapping> TableData = new List<TIE_FQALineMapping>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_FQALineMapping fqaLine = new TIE_FQALineMapping();
                fqaLine.Depart_No = DepartNo;
                fqaLine.Mode = Mode;
                fqaLine.FQA_Line_No = QLineNo;
                fqaLine.Line_No = LineNo;
                fqaLine.FQA_Shift = QShift;
                fqaLine.Shift = Shift;
                fqaLine.FQA_Line = QLine;
                fqaLine.Line = Line;

                TableData = db.IE_FQALineMapping_SaveData(fqaLine,user.UserID);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public ActionResult IE_FQALineMapping_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TIE_FQALineMapping> TableData = JsonHelper.DeserializeJsonToList<TIE_FQALineMapping>(jsonData);

                foreach (var item in TableData)
                {
                    TIE_FQALineMapping fqaLine = new TIE_FQALineMapping();
                    TIEDB db = new TIEDB();

                    fqaLine = item;
                    db.IE_FQALineMapping_Delete(fqaLine, user.UserID);

                }

                Message = "OK";

            }
            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion IE_FQALineMapping


        #region IE_BaseFactor
        public ActionResult IE_BaseFactor()
        {
            TIEDB db = new TIEDB();          
            List<SelectListItem> PUListItem = new List<SelectListItem>(); 
            List<string> list = db.IE_DailyProductQty_GetPU("");

            foreach (var item in list)
            {
                PUListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["modeListItem"] = new SelectList(PUListItem, "Value", "Text", "");

            return View();
        }

        public ActionResult IE_BaseFactor_GetData(string Mode = "", string Section = "", string Line = "",
                                                    string Stage = "", double Base=0, double Factor=0, Int64 CountTimes=0)
        {
            List<TIE_BaseFactor> TableData = new List<TIE_BaseFactor>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_BaseFactor baseFactor = new TIE_BaseFactor();
                baseFactor.Mode = Mode;
                baseFactor.Section = Section;
                baseFactor.Line = Line;
                baseFactor.Stage = Stage;
                baseFactor.Base = Base;
                baseFactor.Factor = Factor;
                baseFactor.CountTimes = CountTimes;

                TableData = db.IE_BaseFactor_GetData(baseFactor);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        
        public ActionResult IE_BaseFactor_SavaData(string Mode, string Section, string Line,
                                                    string Stage, double Base, double Factor, Int64 CountTimes)
        {
            List<TIE_BaseFactor> TableData = new List<TIE_BaseFactor>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_BaseFactor baseFactor = new TIE_BaseFactor();
                baseFactor.Mode = Mode;
                baseFactor.Section = Section;
                baseFactor.Line = Line;
                baseFactor.Stage = Stage;
                baseFactor.Base = Base;
                baseFactor.Factor = Factor;
                baseFactor.CountTimes = CountTimes;

                TableData = db.IE_BaseFactor_SavaData(baseFactor, user.UserID);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public ActionResult IE_BaseFactor_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TIE_BaseFactor> TableData = JsonHelper.DeserializeJsonToList<TIE_BaseFactor>(jsonData);

                foreach (var item in TableData)
                {
                    TIE_BaseFactor baseFactor = new TIE_BaseFactor();
                    TIEDB db = new TIEDB();

                    baseFactor = item;
                    db.IE_BaseFactor_Delete(baseFactor, user.UserID);

                }

                Message = "OK";

            }
            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion IE_BaseFactor

        #region IE_DepartMent
        public ActionResult IE_DepartMent()
        {

            return View();
        }

        public JsonResult IE_DepartMent_UploadExcel()
        {
            string Message = "";
            List<TIE_DepartMent> TableData = new List<TIE_DepartMent>();
     

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

                        List<TIE_DepartMent> list = new List<TIE_DepartMent>();
                         //List<TIE_DepartMent> listFail = new List<TIE_DepartMent>();
                        TIEDB db = new TIEDB();

                        list = DataTableTool.ToList<TIE_DepartMent>(dtTemp);
                        double listCount = list.Count;
                        for (int i = 0; i < listCount; i++)
                        {
                            string Line = list[i].Line;
                            string Shift = list[i].Shift;
                            string Line_No = list[i].Line_No;
                            string Depart_No = list[i].Depart_No;
                            string Mode = list[i].Mode;

                            string RowFail = "";
                            if (Line.Trim() == "" || String.IsNullOrEmpty(Line) == true)
                            {
                                RowFail = RowFail+ "[Line]不能为空 ";
                                list[i].Line = RowFail;
                            }
                            if (Shift.Trim() == "" || String.IsNullOrEmpty(Shift) == true)
                            {
                                RowFail = RowFail + "[Shift]不能为空 ";
                                list[i].Shift = RowFail;
                            }
                            if (Line_No.Trim() == "" || String.IsNullOrEmpty(Line_No) == true)
                            {
                                RowFail = RowFail + "[Line_No]不能为空 ";
                                list[i].Line_No = RowFail;
                            }
                            if (Depart_No.Trim() == "" || String.IsNullOrEmpty(Depart_No) == true)
                            {
                                RowFail = RowFail + "[Depart_No]不能为空 ";
                                list[i].Depart_No = RowFail;
                            }
                            if (Mode.Trim() == "" || String.IsNullOrEmpty(Mode) == true)
                            {
                                RowFail = RowFail + "[Mode]不能为空 ";
                                list[i].Mode = RowFail;
                            }

                            if (RowFail.Trim() != "")
                            {
                                TableData.Add(list[i]);
                            }
                            else
                            {
                                db.IE_DepartMent_SavaData(list[i], user.UserID);
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




        public ActionResult IE_DepartMent_Delete(string jsonData)
        { 
            string Message = ""; 
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TIE_DepartMent> TableData = JsonHelper.DeserializeJsonToList<TIE_DepartMent>(jsonData);

                foreach (var item in TableData)
                {
                    TIE_DepartMent ie_DepartMent = new TIE_DepartMent();
                    TIEDB db = new TIEDB();

                    ie_DepartMent = item;
                    db.IE_DepartMent_Delete(ie_DepartMent, user.UserID);

                }

                Message = "OK"; 
 
            }
            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        

        public ActionResult IE_DepartMent_SavaData(string Line , string Shift, string LineNo , string DepartNo , string Mode )
        {
            List<TIE_DepartMent> TableData = new List<TIE_DepartMent>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_DepartMent ie_DepartMent = new TIE_DepartMent();
                ie_DepartMent.Line = Line;
                ie_DepartMent.Shift = Shift;
                ie_DepartMent.Line_No = LineNo;
                ie_DepartMent.Depart_No = DepartNo;
                ie_DepartMent.Mode = Mode;
                TableData = db.IE_DepartMent_SavaData(ie_DepartMent, user.UserID);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult IE_DepartMent_GetData(string Line = "", string Shift = "", string LineNo = "", string DepartNo = "", string Mode = "")
        {
            List<TIE_DepartMent> TableData = new List<TIE_DepartMent>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_DepartMent ie_DepartMent = new TIE_DepartMent();
                ie_DepartMent.Line = Line;
                ie_DepartMent.Shift = Shift;
                ie_DepartMent.Line_No = LineNo;
                ie_DepartMent.Depart_No = DepartNo;
                ie_DepartMent.Mode = Mode;
                TableData = db.IE_DepartMent_GetData(ie_DepartMent);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion IE_DepartMent
        
        #region IE_STD_ManHour
        public ActionResult IE_STD_ManHour()
        {
            TIEDB db = new TIEDB();
            List<SelectListItem> factoryListItem = new List<SelectListItem>();
            List<string> list = new List<string>();

            list = db.TIE_STD_ManHour_GetFactory();
            foreach (var item in list)
            {
                factoryListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["factoryListItem"] = new SelectList(factoryListItem, "Value", "Text", "");
            return View();
        }
        public JsonResult IE_STD_ManHour_GetPU(string Factory)
        {
            List<string> TableData = new List<string>();
           
            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TableData = db.TIE_STD_ManHour_GetPU(Factory);               
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData};
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult IE_STD_ManHour_GetMode(string Factory,string PU)
        {
            List<string> TableData = new List<string>();
           

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TableData = db.TIE_STD_ManHour_GetMode(Factory,PU);  
                Message = "OK";
            }

            var jsondata = new { result = Message, tableData = TableData};
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult IE_STD_ManHour_GetLine(string Factory, string PU,string Mode)
        {
            List<string> TableData = new List<string>();
            //List<TIE_STD_ManHour> DetailData = new List<TIE_STD_ManHour>();
            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TableData = db.TIE_STD_ManHour_GetLine(Factory, PU, Mode);
                //DetailData = db.TIE_STD_ManHour_GetData(Factory, PU, Mode, "", "");
                Message = "OK";
            }

            var jsondata = new { result = Message, tableData = TableData  };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult IE_STD_ManHour_GetData(string Factory, string PU, string Mode,string Line,string Model)
        {
            //List<string> TableData = new List<string>();
            List<TIE_STD_ManHour> TableData = new List<TIE_STD_ManHour>();
            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                //TableData = db.TIE_STD_ManHour_GetLine(Factory, PU, Mode);
                TableData = db.TIE_STD_ManHour_GetData(Factory, PU, Mode, Line, Model);
                Message = "OK";
            }
            
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult IE_STD_ManHour_GetSaveData1(string Factory, string PU, string Mode, string Line, string Model,double CycleTime,
                                                        double ManHour, Int64 OnlineMan, double OfflineMan, double ShareRate,string Remarks)
        {
            List<TIE_STD_ManHour> TableData = new List<TIE_STD_ManHour>();
            //List<TIE_STD_ManHour> TableData = new List<TIE_STD_ManHour>();
           
            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                //TableData = db.TIE_STD_ManHour_GetLine(Factory, PU, Mode);
                TIE_STD_ManHour tIE_STD_ManHour = new TIE_STD_ManHour();
                tIE_STD_ManHour.Factory = Factory;
                tIE_STD_ManHour.BU = PU;
                tIE_STD_ManHour.Mode = Mode;
                tIE_STD_ManHour.Line = Line;
                tIE_STD_ManHour.Model = Model;
                tIE_STD_ManHour.CycleTime = CycleTime;
                tIE_STD_ManHour.ManHour = ManHour;
                tIE_STD_ManHour.Online_Man = OnlineMan;
                tIE_STD_ManHour.Offline_Man = OfflineMan;
                tIE_STD_ManHour.Share_Rate = ShareRate;
                tIE_STD_ManHour.Remarks = Remarks;

                TableData = db.IE_STD_ManHour_GetSaveData1(tIE_STD_ManHour,user.UserID);
                Message = "OK";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }


        public JsonResult IE_STD_ManHour_Delete(string jsonData)
        { 
            string Message = ""; 
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TIE_STD_ManHour> TableData = JsonHelper.DeserializeJsonToList<TIE_STD_ManHour>(jsonData);

                foreach (var item in TableData)
                {
                    TIE_STD_ManHour iE_STD_ManHour = new TIE_STD_ManHour();
                    TIEDB db = new TIEDB();

                    iE_STD_ManHour = item;
                    db.IE_STD_ManHour_Detele(iE_STD_ManHour, user.UserID);

                }
               
                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult IE_STD_ManHour_UploadExcel()
        {
            string Message = ""; List<TIE_STD_ManHour> TableData = new List<TIE_STD_ManHour>();
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
                //mfile = MFile.UpLoadFile(file, user.UserID, FilePathAtServer, file.FileName, ref uploadResult);
                mfile = MFile.UpLoadFile(file, FilePathAtServer, user.UserID, ref uploadResult);
                FilePathAtServer = mfile.UploadPath;

                if (uploadResult == "OK")
                {
                    Message = "OK";
                    try
                    {
                        //string str = "select [Mode],[Model],[Section],[Stage] from [IE_ModelStage$] where [Mode] <>'' ";
                        DataTable dtTemp = MFile.ExcelToTable(FilePathAtServer, 4, "","N");
                        DataTable dtFail = new DataTable();
                        TIEDB db = new TIEDB();
                        string strLine = "";

                        dtFail = dtTemp.Clone();
                        dtFail.Clear();

                        int Rowcount = dtTemp.Rows.Count;
                        
                        while (dtTemp.Rows.Count > 0)
                        {
                            int i = 0;
                            Row = Row + 1;
                            TIE_STD_ManHour ie_Std_ManHour = new TIE_STD_ManHour();
                            string rowOK = "";

                            if (dtTemp.Rows[i][0].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][0].ToString()) == false)
                            {
                                ie_Std_ManHour.BU = dtTemp.Rows[i][0].ToString();
                            }else{
                                rowOK = rowOK + " BU 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][1].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][1].ToString()) == false)
                            {
                                ie_Std_ManHour.Model = dtTemp.Rows[i][1].ToString();
                            }else{
                                rowOK = rowOK+" Model 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][2].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][2].ToString()) == false)
                            {
                                ie_Std_ManHour.Factory = dtTemp.Rows[i][2].ToString();
                            }
                            else
                            {
                                rowOK = rowOK + " Factory 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][3].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][3].ToString()) == false)
                            {
                                ie_Std_ManHour.Mode = dtTemp.Rows[i][3].ToString();
                            }else{
                                rowOK = rowOK + " Mode 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][4].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][4].ToString()) == false)
                            {
                                strLine = dtTemp.Rows[i][4].ToString();
                            }else{
                                rowOK = rowOK + " Line 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][5].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][5].ToString()) == false)
                            {
                                ie_Std_ManHour.Online_Man = Convert.ToDouble(dtTemp.Rows[i][5].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " Online_Man 不能为空 ||";
                            }
                            if (dtTemp.Rows[i][6].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][6].ToString()) == false)
                            {

                                ie_Std_ManHour.Offline_Man = Convert.ToDouble(  dtTemp.Rows[i][6].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " Offline_Man 不能为空 ||";
                            }
                            if (dtTemp.Rows[i][7].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][7].ToString()) == false)
                            {

                                ie_Std_ManHour.Share_Rate = Convert.ToDouble(dtTemp.Rows[i][7].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " Share_Rate 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][8].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][8].ToString()) == false)
                            {
                                ie_Std_ManHour.Share_Man = Convert.ToDouble(dtTemp.Rows[i][8].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " Share_Man 不能为空 ||";
                            }
                            if (dtTemp.Rows[i][9].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][9].ToString()) == false)
                            {
                                ie_Std_ManHour.STD_Manpower = Convert.ToDouble(dtTemp.Rows[i][9].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " STD_Manpower 不能为空 ||";
                            }
                            if (dtTemp.Rows[i][10].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][10].ToString()) == false)
                            {
                                ie_Std_ManHour.CycleTime = Convert.ToDouble(dtTemp.Rows[i][10].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " CycleTime 不能为空 ||";
                            }



                            if (dtTemp.Rows[i][11].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][11].ToString()) == false)
                            {
                                ie_Std_ManHour.ManHour = Convert.ToDouble(dtTemp.Rows[i][11].ToString()); ;
                            }
                            else if (dtTemp.Rows[i][12].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][12].ToString()) == false)
                            {
                                ie_Std_ManHour.ManHour = Convert.ToDouble(dtTemp.Rows[i][12].ToString()); ;
                            }
                            else if (dtTemp.Rows[i][13].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][13].ToString()) == false)
                            {
                                ie_Std_ManHour.ManHour = Convert.ToDouble(dtTemp.Rows[i][13].ToString()); ;
                            }
                            else if (dtTemp.Rows[i][14].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][14].ToString()) == false)
                            {
                                ie_Std_ManHour.ManHour = Convert.ToDouble(dtTemp.Rows[i][14].ToString()); ;
                            }
                            else if (dtTemp.Rows[i][15].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][15].ToString()) == false)
                            {
                                ie_Std_ManHour.ManHour = Convert.ToDouble(dtTemp.Rows[i][15].ToString()); ;
                            }
                            else
                            {
                                rowOK = rowOK + " ManHour 不能为空 ||";
                            }
                            

                            if (dtTemp.Rows[i][16].ToString() != "")
                            {
                                ie_Std_ManHour.Output_8Hrs = Convert.ToInt64(dtTemp.Rows[i][16].ToString()); //(long)dtTemp.Rows[i][16];
                            }
                            else
                            {
                                rowOK = rowOK + " Output_8Hrs 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][17].ToString() != "")
                            {
                                ie_Std_ManHour.Output_12Hrs = Convert.ToInt64(dtTemp.Rows[i][17].ToString());// (long)dtTemp.Rows[i][17];
                            }
                            else
                            {
                                rowOK = rowOK + " Output_12Hrs 不能为空 ||";
                            }


                            if (dtTemp.Rows[i][18].ToString() != "")
                            {
                                ie_Std_ManHour.Balance_Efficiency = dtTemp.Rows[i][18].ToString();
                            }

                            if (dtTemp.Rows[i][19].ToString() != "")
                            {
                                ie_Std_ManHour.Issue_Date = dtTemp.Rows[i][19].ToString();
                            }

                            if (dtTemp.Rows[i][21].ToString() != "")
                            {
                                ie_Std_ManHour.Remarks = dtTemp.Rows[i][21].ToString();
                            }
                            else
                            {
                                rowOK = rowOK + "Remarks 不能为空 ||";
                            }

                            //if (ie_Std_ManHour.ManHour == 0 || ie_Std_ManHour.Online_Man == 0 ||
                            //     ie_Std_ManHour.Offline_Man == 0 || ie_Std_ManHour.Share_Man ==0 ||
                            //    ie_Std_ManHour.CycleTime == 0
                            //    )

                            if (rowOK != "")
                            {
                                ie_Std_ManHour.Remarks = rowOK;
                                TableData.Add(ie_Std_ManHour); 
                                //dtTemp.Rows[i][21] = rowOK;
                                //DataRow dr;

                                //dr = dtFail.NewRow();
                                //dr.ItemArray = dtTemp.Rows[i].ItemArray;

                                //dtFail.Rows.Add(dr);
                                dtTemp.Rows[i].Delete();
                                
                            }
                            else
                            { //插入数据库
                               //分割Line，每条线塞一笔记录。
                                string[] sArrayLine = strLine.Split(',');
                                foreach (string Line in sArrayLine)
                                {
                                    ie_Std_ManHour.Line = Line;
                                    db.IE_STD_ManHour_GetSaveData2(ie_Std_ManHour, user.UserID);
                                   
                                }
                                dtTemp.Rows[i].Delete();
                                RowSucc = RowSucc + 1;
                            }

                            ProcessRate = (Row / Rowcount) * 100;
                             
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



            var jsondata = new { result = Message, tableData = TableData, rowMsg = "成功上传(" + RowSucc.ToString() + ")行,失败了(" + (Row - RowSucc).ToString() + ")行---失败原因请见 [IE_STD_ManHour详细信息 Remark 栏位]" };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion IE_STD_ManHour
       
        #region IE_ModelStage
        public ActionResult IE_ModelStage()
        {

            TIEDB db = new TIEDB();;
            List<SelectListItem> modelListItem = new List<SelectListItem>(); 
            List<string > list = db.IE_ModelStage_GetMode();

            foreach (var item in list)
            {
                modelListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["modelListItem"] = new SelectList(modelListItem, "Value", "Text", "");
            
            return View();
        }

        public JsonResult IE_ModelStage_UploadExcel()
        {
            string Message = ""; List<TIE_ModelStage> TableData = new List<TIE_ModelStage>();

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
                        //string str = "select [Mode],[Model],[Section],[Stage] from [IE_ModelStage$] where [Mode] <>'' ";
                        //DataTable dtTemp = MFile.ExcelSqlConnection(FilePathAtServer, "IE_ModelStage", str, "").Tables[0];
                        DataTable dtTemp = MFile.ExcelToTable(FilePathAtServer, 0, "", "Y");
                        
                        List<TIE_ModelStage> list = new List<TIE_ModelStage>();
                        TIEDB db = new TIEDB();

                        list = DataTableTool.ToList<TIE_ModelStage>(dtTemp);
                        double listCount = list.Count;
                        for (int i = 0; i < listCount; i++)
                        {
                            db.IE_ModelStage_ByOpType(list[i], 2, user.UserID);
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

        [HttpPost]
        public JsonResult IE_ModelStage_ByOpType(string Mode, string Model, string Stage, string Section, int opType)
        {
            string Message = "";

            List<TIE_ModelStage> TableData = new List<TIE_ModelStage>();

            //string TableData = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_ModelStage IE_ModelStage = new TIE_ModelStage();
                TIEDB db = new TIEDB();

                IE_ModelStage.Mode = Mode;
                IE_ModelStage.Model = Model;
                IE_ModelStage.Stage = Stage;
                IE_ModelStage.Section = Section;
                Message = "OK";
                TableData = db.IE_ModelStage_ByOpType(IE_ModelStage, opType, user.UserID);

            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }


        public JsonResult IE_ModelStage_Delete(string jsonData)
        {
            //List<TIE_ModelStage> TableData = new List<TIE_ModelStage>();
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TIE_ModelStage> TableData = JsonHelper.DeserializeJsonToList<TIE_ModelStage>(jsonData);

                foreach (var item in TableData)
                {
                    TIE_ModelStage IE_ModelStage = new TIE_ModelStage();
                    TIEDB db = new TIEDB();

                    IE_ModelStage = item;
                    db.IE_ModelStage_ByOpType(IE_ModelStage, 4, user.UserID);

                }
                Message = "OK";
            }


            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion IE_ModelStage

        #region IE_Line_Shift
        public ActionResult IE_Line_Shift()
        {
            return View();
        }
        public ActionResult IE_Line_Shift_SavaData(string PU,string Type,string Line,string ShiftNo)
        {
            List<TIE_Line_Shift> TableData = new List<TIE_Line_Shift>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_Line_Shift ie = new TIE_Line_Shift();
                ie.PU = PU;
                ie.Line = Line;
                ie.ShiftNo = ShiftNo;

                db.IE_Line_Shift_Save(ie, user.UserID);
                //TableData = db.IE_Line_Shift_View(ie);
                //Message = "OK";
                string Key = PU + "_" + Type + "_PubDb";
                if (db.BuildPubConnection(Key) == false)
                {

                    Message = "连接 " + Key + "DB 失败";
                }
                else
                {

                    db.IE_Line_Shift_Save(ie, user.UserID);
                    TableData = db.IE_Line_Shift_View(ie);
                    Message = "OK";
                }


            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult IE_Line_Shift_View(string PU, string Type)
        {
            List<TIE_Line_Shift> TableData = new List<TIE_Line_Shift>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TIE_Line_Shift ie = new TIE_Line_Shift();
                string Key = PU + "_" + Type + "_PubDb";
                if (db.BuildPubConnection(Key) == false)
                {

                    Message = "连接 " + Key + " 失败,请联系QMS添加配置文件";
                }
                else
                {
                    ie.PU = PU;

                    TableData = db.IE_Line_Shift_View( ie);
                    Message = "OK";
                }
           }
                
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public ActionResult IE_Line_Shift_Delete(string jsonData,string PU, string Type)
        {       
            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TIE_Line_Shift> TableData = new List<TIE_Line_Shift>();
                TableData = JsonHelper.DeserializeJsonToList<TIE_Line_Shift>(jsonData);
                foreach (var item in TableData)
                {
                    db.IE_Line_Shift_Delete(item, user.UserID);
                
                }

                string Key = PU + "_" + Type + "_PubDb";
                if (db.BuildPubConnection(Key) == false)
                {

                    Message = "连接 " + Key + " 失败,请联系QMS添加配置文件";
                }
                else
                {
                    foreach (var item in TableData)
                    {
                        db.IE_Line_Shift_Delete(item, user.UserID);

                    }
                    Message = "OK";
                }
               
                
            }
            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion IE_Line_Shift

        #region IE_STD_ManHour_NextMonth
        public ActionResult IE_STD_ManHour_NextMonth()
        {
            TIEDB db = new TIEDB();
            List<SelectListItem> factoryListItem = new List<SelectListItem>();
            List<string> list = new List<string>();

            list = db.TIE_STD_ManHour_NextMonth_GetFactory();
            foreach (var item in list)
            {
                factoryListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["factoryListItem"] = new SelectList(factoryListItem, "Value", "Text", "");
            return View();
        }
        public JsonResult IE_STD_ManHour_NextMonth_GetPU(string Factory)
        {
            List<string> TableData = new List<string>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TableData = db.TIE_STD_ManHour_NextMonth_GetPU(Factory);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult IE_STD_ManHour_NextMonth_GetMode(string Factory, string PU)
        {
            List<string> TableData = new List<string>();


            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TableData = db.TIE_STD_ManHour_NextMonth_GetMode(Factory, PU);
                Message = "OK";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult IE_STD_ManHour_NextMonth_GetLine(string Factory, string PU, string Mode)
        {
            List<string> TableData = new List<string>();
            //List<TIE_STD_ManHour> DetailData = new List<TIE_STD_ManHour>();
            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TableData = db.TIE_STD_ManHour_NextMonth_GetLine(Factory, PU, Mode);
                //DetailData = db.TIE_STD_ManHour_GetData(Factory, PU, Mode, "", "");
                Message = "OK";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult IE_STD_ManHour_NextMonth_GetData(string Factory, string PU, string Mode, string Line, string Model)
        {
            //List<string> TableData = new List<string>();
            List<TIE_STD_ManHour_NextMonth> TableData = new List<TIE_STD_ManHour_NextMonth>();
            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                //TableData = db.TIE_STD_ManHour_GetLine(Factory, PU, Mode);
                TableData = db.TIE_STD_ManHour_NextMonth_GetData(Factory, PU, Mode, Line, Model);
                Message = "OK";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult IE_STD_ManHour_NextMonth_GetSaveData1(string Factory, string PU, string Mode, string Line, string Model, double CycleTime,
                                                        double ManHour, Int64 OnlineMan, double OfflineMan, double ShareRate, string Remarks)
        {
            List<TIE_STD_ManHour_NextMonth> TableData = new List<TIE_STD_ManHour_NextMonth>();
            //List<TIE_STD_ManHour> TableData = new List<TIE_STD_ManHour>();

            string Message = "";
            TIEDB db = new TIEDB();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                //TableData = db.TIE_STD_ManHour_GetLine(Factory, PU, Mode);
                TIE_STD_ManHour_NextMonth tIE_STD_ManHour = new TIE_STD_ManHour_NextMonth();
                tIE_STD_ManHour.Factory = Factory;
                tIE_STD_ManHour.BU = PU;
                tIE_STD_ManHour.Mode = Mode;
                tIE_STD_ManHour.Line = Line;
                tIE_STD_ManHour.Model = Model;
                tIE_STD_ManHour.CycleTime = CycleTime;
                tIE_STD_ManHour.ManHour = ManHour;
                tIE_STD_ManHour.Online_Man = OnlineMan;
                tIE_STD_ManHour.Offline_Man = OfflineMan;
                tIE_STD_ManHour.Share_Rate = ShareRate;
                tIE_STD_ManHour.Remarks = Remarks;

                TableData = db.IE_STD_ManHour_NextMonth_GetSaveData1(tIE_STD_ManHour, user.UserID);
                Message = "OK";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }


        public JsonResult IE_STD_ManHour_NextMonth_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TIE_STD_ManHour_NextMonth> TableData = JsonHelper.DeserializeJsonToList<TIE_STD_ManHour_NextMonth>(jsonData);

                foreach (var item in TableData)
                {
                    TIE_STD_ManHour_NextMonth iE_STD_ManHour = new TIE_STD_ManHour_NextMonth();
                    TIEDB db = new TIEDB();

                    iE_STD_ManHour = item;
                    db.IE_STD_ManHour_NextMonth_Detele(iE_STD_ManHour, user.UserID);

                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult IE_STD_ManHour_NextMonth_UploadExcel()
        {
            string Message = ""; List<TIE_STD_ManHour_NextMonth> TableData = new List<TIE_STD_ManHour_NextMonth>();
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
                //mfile = MFile.UpLoadFile(file, user.UserID, FilePathAtServer, file.FileName, ref uploadResult);
                mfile = MFile.UpLoadFile(file, FilePathAtServer, user.UserID, ref uploadResult);
                FilePathAtServer = mfile.UploadPath;

                if (uploadResult == "OK")
                {
                    Message = "OK";
                    //try
                    {
                        //string str = "select [Mode],[Model],[Section],[Stage] from [IE_ModelStage$] where [Mode] <>'' ";
                        DataTable dtTemp = MFile.ExcelToTable(FilePathAtServer, 4, "", "N");
                        DataTable dtFail = new DataTable();
                        TIEDB db = new TIEDB();
                        string strLine = "";

                        dtFail = dtTemp.Clone();
                        dtFail.Clear();

                        int Rowcount = dtTemp.Rows.Count;

                        while (dtTemp.Rows.Count > 0)
                        {
                            int i = 0;
                            Row = Row + 1;
                            TIE_STD_ManHour_NextMonth ie_Std_ManHour = new TIE_STD_ManHour_NextMonth();
                            string rowOK = "";

                            if (dtTemp.Rows[i][0].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][0].ToString()) == false)
                            {
                                ie_Std_ManHour.BU = dtTemp.Rows[i][0].ToString();
                            }
                            else
                            {
                                rowOK = rowOK + " BU 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][1].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][1].ToString()) == false)
                            {
                                ie_Std_ManHour.Model = dtTemp.Rows[i][1].ToString();
                            }
                            else
                            {
                                rowOK = rowOK + " Model 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][2].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][2].ToString()) == false)
                            {
                                ie_Std_ManHour.Factory = dtTemp.Rows[i][2].ToString();
                            }
                            else
                            {
                                rowOK = rowOK + " Factory 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][3].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][3].ToString()) == false)
                            {
                                ie_Std_ManHour.Mode = dtTemp.Rows[i][3].ToString();
                            }
                            else
                            {
                                rowOK = rowOK + " Mode 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][4].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][4].ToString()) == false)
                            {
                                strLine = dtTemp.Rows[i][4].ToString();
                            }
                            else
                            {
                                rowOK = rowOK + " Line 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][5].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][5].ToString()) == false)
                            {
                                ie_Std_ManHour.Online_Man = Convert.ToDouble(dtTemp.Rows[i][5].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " Online_Man 不能为空 ||";
                            }
                            if (dtTemp.Rows[i][6].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][6].ToString()) == false)
                            {

                                ie_Std_ManHour.Offline_Man = Convert.ToDouble(dtTemp.Rows[i][6].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " Offline_Man 不能为空 ||";
                            }
                            if (dtTemp.Rows[i][7].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][7].ToString()) == false)
                            {

                                ie_Std_ManHour.Share_Rate = Convert.ToDouble(dtTemp.Rows[i][7].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " Share_Rate 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][8].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][8].ToString()) == false)
                            {
                                ie_Std_ManHour.Share_Man = Convert.ToDouble(dtTemp.Rows[i][8].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " Share_Man 不能为空 ||";
                            }
                            if (dtTemp.Rows[i][9].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][9].ToString()) == false)
                            {
                                ie_Std_ManHour.STD_Manpower = Convert.ToDouble(dtTemp.Rows[i][9].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " STD_Manpower 不能为空 ||";
                            }
                            if (dtTemp.Rows[i][10].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][10].ToString()) == false)
                            {
                                ie_Std_ManHour.CycleTime = Convert.ToDouble(dtTemp.Rows[i][10].ToString());
                            }
                            else
                            {
                                rowOK = rowOK + " CycleTime 不能为空 ||";
                            }



                            if (dtTemp.Rows[i][11].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][11].ToString()) == false)
                            {
                                ie_Std_ManHour.ManHour = Convert.ToDouble(dtTemp.Rows[i][11].ToString()); ;
                            }
                            else if (dtTemp.Rows[i][12].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][12].ToString()) == false)
                            {
                                ie_Std_ManHour.ManHour = Convert.ToDouble(dtTemp.Rows[i][12].ToString()); ;
                            }
                            else if (dtTemp.Rows[i][13].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][13].ToString()) == false)
                            {
                                ie_Std_ManHour.ManHour = Convert.ToDouble(dtTemp.Rows[i][13].ToString()); ;
                            }
                            else if (dtTemp.Rows[i][14].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][14].ToString()) == false)
                            {
                                ie_Std_ManHour.ManHour = Convert.ToDouble(dtTemp.Rows[i][14].ToString()); ;
                            }
                            else if (dtTemp.Rows[i][15].ToString() != "" && String.IsNullOrEmpty(dtTemp.Rows[i][15].ToString()) == false)
                            {
                                ie_Std_ManHour.ManHour = Convert.ToDouble(dtTemp.Rows[i][15].ToString()); ;
                            }
                            else
                            {
                                rowOK = rowOK + " ManHour 不能为空 ||";
                            }


                            if (dtTemp.Rows[i][16].ToString() != "")
                            {
                                ie_Std_ManHour.Output_8Hrs = Convert.ToInt64(Convert.ToDouble (dtTemp.Rows[i][16].ToString())); //(long)dtTemp.Rows[i][16];
                            }
                            else
                            {
                                rowOK = rowOK + " Output_8Hrs 不能为空 ||";
                            }

                            if (dtTemp.Rows[i][17].ToString() != "")
                            {
                                ie_Std_ManHour.Output_12Hrs = Convert.ToInt64(Convert.ToDouble (dtTemp.Rows[i][17].ToString()));// (long)dtTemp.Rows[i][17];
                            }
                            else
                            {
                                rowOK = rowOK + " Output_12Hrs 不能为空 ||";
                            }


                            if (dtTemp.Rows[i][18].ToString() != "")
                            {
                                ie_Std_ManHour.Balance_Efficiency = dtTemp.Rows[i][18].ToString();
                            }

                            if (dtTemp.Rows[i][19].ToString() != "")
                            {
                                ie_Std_ManHour.Issue_Date = dtTemp.Rows[i][19].ToString();
                            }

                            if (dtTemp.Rows[i][21].ToString() != "")
                            {
                                ie_Std_ManHour.Remarks = dtTemp.Rows[i][21].ToString();
                            }
                            else
                            {
                                rowOK = rowOK + "Remarks 不能为空 ||";
                            }

                            //if (ie_Std_ManHour.ManHour == 0 || ie_Std_ManHour.Online_Man == 0 ||
                            //     ie_Std_ManHour.Offline_Man == 0 || ie_Std_ManHour.Share_Man ==0 ||
                            //    ie_Std_ManHour.CycleTime == 0
                            //    )

                            if (rowOK != "")
                            {
                                ie_Std_ManHour.Remarks = rowOK;
                                TableData.Add(ie_Std_ManHour);
                                //dtTemp.Rows[i][21] = rowOK;
                                //DataRow dr;

                                //dr = dtFail.NewRow();
                                //dr.ItemArray = dtTemp.Rows[i].ItemArray;

                                //dtFail.Rows.Add(dr);
                                dtTemp.Rows[i].Delete();

                            }
                            else
                            { //插入数据库
                                //分割Line，每条线塞一笔记录。
                                string[] sArrayLine = strLine.Split(',');
                                foreach (string Line in sArrayLine)
                                {
                                    ie_Std_ManHour.Line = Line;
                                    db.IE_STD_ManHour_NextMonth_GetSaveData2(ie_Std_ManHour, user.UserID);

                                }
                                dtTemp.Rows[i].Delete();
                                RowSucc = RowSucc + 1;
                            }

                            ProcessRate = (Row / Rowcount) * 100;

                        }


                    }
                    //catch
                    //{
                    //    Message = "处理Excel出现异常，请联系QMS!";
                    //}
                }
                else
                {
                    Message = "上传Excel失败";
                }
            }



            var jsondata = new { result = Message, tableData = TableData, rowMsg = "成功上传(" + RowSucc.ToString() + ")行,失败了(" + (Row - RowSucc).ToString() + ")行---失败原因请见 [IE_STD_ManHour详细信息 Remark 栏位]" };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion IE_STD_ManHour_NextMonth
        public JsonResult ProcessGetStatus()
        {
            string Message = "OK"; double TableData = ProcessRate ;

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
         
 
        private bool CheckUserSession ()
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
