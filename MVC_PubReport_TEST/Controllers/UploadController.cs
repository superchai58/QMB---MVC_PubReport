using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MVC_PubReport.Models.Upload;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.Public;
using MVC_PubReport.Models.Files;
using MVC_PubReport.Models.AppSettings;

namespace MVC_PubReport.Controllers
{
    public class UploadController : Controller
    {
        private TUser user = new TUser();

       

#region capacity_Plan
        public ActionResult Capacity_Plan()
        {

            List<SelectListItem> PUListItem = new List<SelectListItem>();
            List<SelectListItem> TypeListItem = new List<SelectListItem>();

            PUListItem = AppSettingsRead.AppSettingsReadKey("PU", ';');
            TypeListItem = AppSettingsRead.AppSettingsReadKey("Type", ';');

            ViewData["PUListItem"] = new SelectList(PUListItem, "Value", "Text", "");
            ViewData["TypeListItem"] = new SelectList(TypeListItem, "Value", "Text", "");
            return View();

        }

        public JsonResult Capacity_Plan_Delete(string jsonData)
        {
            string Message = "OK";           
            
            if (CheckUserSession())
            {
                List<TCapacity_Plan> TableData = JsonHelper.DeserializeJsonToList<TCapacity_Plan>(jsonData);

                foreach (var item in TableData)
                {                     
                    Upload db = new Upload("PubReportMain");                   
                    db.Capacity_Plan_Delete (item, user.UserID);
                }

                Message = "OK";
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

        public JsonResult Capacity_Plan_Query(string Month,string PU,string Type,string Department)
        {
            string Message = "OK";
            List<TCapacity_Plan> TableData = new List<TCapacity_Plan>();
            TCapacity_Plan t = new TCapacity_Plan();

            t.DateTime = Month;
            t.PU = PU;
            t.Type = Type;
            t.Department = Department;
            Upload db = new Upload("PubReportMain");

            TableData = db.Capacity_Plan_ViewAllMonth(t);

            var jsondata = new { result = Message, tableData = TableData};
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult Capacity_Plan_UploadExcel()
        {
            string Message = "";
            List<TCapacity_Plan> TableData = new List<TCapacity_Plan>();
            //List<TCapacity_Plan> FailTableData = new List<TCapacity_Plan>();
            int iCell = 0;
            int iRow = 0;          
            int RowSucc = 0;
            var SheetName = Request.Params["SheetName"];

            var PU = Request.Params["PU"];
            var Type = Request.Params["Type"];
            var Month = Request.Params["Month"];

            Month = Month.Substring(0,6);

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {   
                    Message = "OK";
                    try
                    {

                        string FilePathAtServer = UpLoadFileToServer();
                        DataTable dtTemp = new DataTable();

                        if (FilePathAtServer != "")
                        {
                            dtTemp = MFile.ExcelToTable(FilePathAtServer, 0, SheetName, "N");
                        }
                        //DataTable dtFail = new DataTable();
                        Upload db = new Upload("PubReportMain");

                        //dtFail = dtTemp.Clone();
                        //dtFail.Clear();

                        int Rowcount = dtTemp.Rows.Count;
                        
                        int CellCount =  dtTemp.Columns.Count;

                        for (int Row = 1; Row < Rowcount;Row=Row+2 )
                        {
                        //string Factory = "";
                        //string Shift = "";
                        //string Line = "";

                            iRow = Row;
                            string rowISOK = "";
                            TCapacity_Plan t = new TCapacity_Plan();
                            //Check 数据的合法性
                            t.Factory = dtTemp.Rows[Row][0].ToString();
                            t.Line = dtTemp.Rows[Row][1].ToString();
                            t.Shift = dtTemp.Rows[Row][2].ToString();
                            t.Department = "PMC";
                            t.Type = Type;
                            t.PU = PU;


                            if (t.Factory == "" || t.Line == "" || t.Shift == "")
                            {
                                rowISOK = "Factory or Line or shift 不能为空-->第" + (Row + 1 + 3).ToString();
                            }
                         

                            if (rowISOK != "")
                            {
                                t.Remark = rowISOK;
                                TableData.Add(t);
 

                            }
                            else
                            {
                                //循环写入每条线每天的资料。
                                int Cell = 4;
                                int TotalCell = dtTemp.Columns.Count;
                                for (int i = 4; i < TotalCell; i++)
                                {
                                    string IsDocking = "";
                                    string strDateTime = dtTemp.Rows[0][i].ToString();
                                    string strQuantity = dtTemp.Rows[Row][i].ToString();
                                    strQuantity = strQuantity == "" ? "0" : strQuantity;

                                    IsDocking = dtTemp.Rows[Row + 1][i].ToString();
                                    t.IsDocking = (IsDocking == "" ? "N" : IsDocking);

                                    int DateTime, Quantity;
                                    bool resultDateTime = int.TryParse(strDateTime, out DateTime);
                                    bool resultQuantity = int.TryParse(strQuantity, out Quantity);
                                    if (resultDateTime)
                                    {
                                        t.DateTime = Month + string.Format("{0:00}", DateTime);
                                    }

                                    if (resultQuantity)
                                    {
                                        t.Quantity = Quantity;
                                    }

                                    if (resultDateTime == false || resultQuantity == false)
                                    {
                                        t.Remark = strDateTime + "/" + strQuantity + "-->DateTime/Quantity 不能转化成数字格式";
                                        TableData.Add(t);
                                    }
                                    else
                                    {
                                        db.Capacity_Plan_Save(t, user.UserID);
                                    }
                                }





                              /*  while ( dtTemp.Rows[0][Cell].ToString() != "" )
                                {
                                //Int64 Quantity = 0;
                                  

                                    string IsDocking = "";
                                    string strDateTime =  dtTemp.Rows[0][Cell].ToString() ;
                                    string strQuantity = dtTemp.Rows[Row][Cell].ToString();
                                    strQuantity = strQuantity == "" ? "0": strQuantity;

                                    IsDocking = dtTemp.Rows[Row + 1][Cell].ToString();
                                    t.IsDocking = (IsDocking == "" ? "N" : IsDocking);

                                    int DateTime,Quantity;
                                    bool resultDateTime = int.TryParse(strDateTime, out DateTime);
                                    bool resultQuantity = int.TryParse(strQuantity, out Quantity);
                                    if (resultDateTime)
                                    {
                                        t.DateTime = Month + string.Format("{0:00}", DateTime);
                                    }                                    

                                    if (resultQuantity)
                                    {
                                        t.Quantity = Quantity;
                                    }

                                    if (resultDateTime == false || resultQuantity == false)
                                    {
                                        t.Remark = strDateTime +"/"+strQuantity  +"-->DateTime/Quantity 不能转化成数字格式";
                                        TableData.Add(t);
                                    }
                                    else
                                    {
                                        db.Capacity_Plan_Save(t, user.UserID);
                                    } 
                                    if(Cell < TotalCell)  Cell = Cell+1;
                                    iCell = Cell;
                                }*/
                                    //RowSucc = RowSucc + 1;
                            }

                            //ProcessRate = (Row / Rowcount) * 100;

                        }


                    }
                    catch(Exception ex)
                    {
                        Message = ex.ToString() +"处理Excel出现异常，请联系QMS!Row="+iRow.ToString()+",CELL="+ iCell.ToString();
                    }
               
            }



            var jsondata = new { result = Message, tableData = TableData, rowMsg = "成功上传(" + RowSucc.ToString() + ")行,失败了(" + (TableData.Count).ToString() + ")行---失败原因请见 [Capacity_Plan详细信息 Remark 栏位]" };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
#endregion capacity_Plan


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

        public JsonResult CheckRight(string PU, string Type,string Auth)
        {
            string Message;
            string DBKey = PU + "_" + Type + "_PubDb";

           

            Message = "";

            if (CheckUserSession())
            {
                try
                {

                    TUserDB db = new TUserDB (DBKey);
                    if (db.CheckAuth(user.UserID, Auth))
                    {
                        Message = "OK";
                    }
                    else
                    {
                        Message = "您在" + DBKey + " DB中没有对应的权限(" + Auth + "),请联系SF或者QMS添加";
                    }

                }

                catch
                {
                    Message = "连接数据库失败，请联系QMS处理--》DBKey：" + DBKey;
                }

            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }

            var jsondata = new { result = Message  };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
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
    }
}
