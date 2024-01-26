using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.ESD;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Controllers
{
    public class ESDController : Controller
    {
        private TUser user = new TUser();
        private string SystemName="ESDControl";

 

        #region ESD_User
       
        public ActionResult ESD_User()
        {
            return View();

        }

        public ActionResult ESD_User_View(string BU, string Type, string DeptCode, string Shift, string EmployeeID)
        {
            string Message = "";
            List<TESD_User> TableData = new List<TESD_User>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                ESD esd = new ESD(BU, Type);
                //string SystemName = "ESDControl";

                bool result = esd.CheckUserRight(user.UserID, SystemName);
                if (result)
                {
                    TESD_User tb = new TESD_User();
                    tb.DeptCode = DeptCode;
                    tb.EmployeeID = EmployeeID;
                    tb.Shift = Shift;

                    TableData = esd.ESD_User_View(tb);
                    Message = "OK";
                }
                else
                {
                    Message = "您没有权限访问，请联系SF/QMS在showboard的User_list中 添加[ESDControl]权限，谢谢！";
                }


            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult ESD_User_Save(string BU, string Type,  string EmployeeID)
        {

            string Message = "";
            List<TESD_User> TableData = new List<TESD_User>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                ESD esd = new ESD(BU, Type);

                TESD_User tb = new TESD_User();
                tb.DeptCode = "";
                tb.Shift = "";
                tb.EmployeeID = EmployeeID;
                tb.UserID = user.UserID;

                esd.ESD_User_Save(tb);

                TableData = esd.ESD_User_View(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult ESD_User_Delete(string BU, string Type, string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TESD_User> TableData = JsonHelper.DeserializeJsonToList<TESD_User>(jsonData);

                foreach (var item in TableData)
                {
                    ESD esd = new ESD(BU, Type);
                    item.UserID = user.UserID;
                    esd.ESD_User_Delete (item);
                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion ESD_User
        #region ESD_DeptCode
        public ActionResult ESD_DeptCode()
        {

            return View();

        }

          
        public ActionResult ESD_DeptCode_GetDepartment(string BU, string Type, string DeptCode)
        {

            string Message = "";
            List<string> TableData = new List<string>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                ESD esd = new ESD(BU, Type);

                TableData = esd.ESD_DeptCode_GetDepartment(DeptCode);
                if (TableData.Count > 0)
                {
                    Message = "OK";
                }
                else
                {
                    Message = DeptCode + "在HR中不存在";
                
                }
                
                
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }

        public ActionResult ESD_DeptCode_View(string BU, string Type)
        {
            string Message = "";
            List<TESD_DeptCode> TableData = new List<TESD_DeptCode>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                ESD esd = new ESD(BU,Type);
                

                bool result = esd.CheckUserRight(user.UserID, SystemName);
                if (result)
                {
                    TESD_DeptCode tb = new TESD_DeptCode();
                    tb.DeptCode = "";
                    tb.Department = "";

                    TableData = esd.ESD_DeptCode_View(tb);
                    Message = "OK";
                }
                else
                {
                    Message = "您没有权限访问，请联系SF/QMS 添加[ESDControl] 权限，谢谢！";
                }

               
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }


        public ActionResult ESD_DeptCode_Save(string BU, string Type, string DeptCode, string Department)
        {

            string Message = "";
            List<TESD_DeptCode> TableData = new List<TESD_DeptCode>();

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                ESD esd = new ESD(BU, Type);

                TESD_DeptCode tb = new TESD_DeptCode();
                tb.DeptCode = DeptCode;
                tb.Department = Department;
                tb.TransID = user.UserID;

                esd.ESD_DeptCode_Save(tb);
                TableData.Add(tb);
                Message = "OK";
            }
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }

        public JsonResult ESD_DeptCode_Delete(string BU, string Type,string jsonData )
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TESD_DeptCode> TableData = JsonHelper.DeserializeJsonToList<TESD_DeptCode>(jsonData);

                foreach (var item in TableData)
                {
                    ESD esd = new ESD(BU, Type);
                    item.TransID = user.UserID;
                    esd.ESD_DeptCode_Delete (item); 
                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion ESD_DeptCode



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
