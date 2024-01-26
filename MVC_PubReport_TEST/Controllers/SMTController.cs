using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.SMT;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Controllers
{
    public class SMTController : Controller
    {
        private TUser user = new TUser();


        #region PCBA_CPU_Material
        public ActionResult PCBA_CPU_Material()
        {
            return View();
        }

        public JsonResult PCBA_CPU_MaterialSave(string jsonData)
        {
            string Message = "";
            List<TPCBA_CPU_MaterialMapping> TableData = JsonHelper.DeserializeJsonToList<TPCBA_CPU_MaterialMapping>(jsonData);

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                

                foreach (var item in TableData)
                {
                    TPCBA_CPU_MaterialMapping t = new TPCBA_CPU_MaterialMapping();
                    SMT db = new SMT("PubReportMain");

                    t = item;
                    t.UserID = user.UserID;

                    db.PCBA_CPU_MaterialMappingSave(t);

                }

                Message = "OK";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult PCBA_CPU_MaterialDelete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TPCBA_CPU_MaterialMapping> TableData = JsonHelper.DeserializeJsonToList<TPCBA_CPU_MaterialMapping>(jsonData);

                foreach (var item in TableData)
                {
                    SMT db = new SMT("PubReportMain");
                    db.PCBA_CPU_MaterialMappingDelete(item, user.UserID);
                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult PCBA_CPU_MaterialQuery(string jsonData)
        {
            string Message = "";
            List<TPCBA_CPU_MaterialMapping> TableData = JsonHelper.DeserializeJsonToList<TPCBA_CPU_MaterialMapping>(jsonData);

            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            { 
             
                TPCBA_CPU_MaterialMapping t = new TPCBA_CPU_MaterialMapping();
                SMT db = new SMT("PubReportMain");

                t = TableData[0]; 
                TableData = db.PCBA_CPU_MaterialMappingQuery(t); 
                Message = "OK";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        } 
        #endregion PCBA_CPU_Material

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
