using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.PMRoom;


namespace MVC_PubReport.Controllers
{
    public class PMROOMController : Controller
    { 
        private TUser user = new TUser();

        #region GlueToSAP
        public ActionResult GlueToSAP()
        {
            return View();
        }

        public ActionResult GetLineByPU(string PU)
        {
            TPMRoom pmRoomDB = new TPMRoom("PubReportMain");
            List<string> TableData = new List<string>();
            TableData = pmRoomDB.GetLineByPU(PU);

            var jsondata = new { result = "OK", tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;  
                
        }
        public ActionResult GetCompPN(string PU)
        {
            TPMRoom pmRoomDB = new TPMRoom("PubReportMain");
            List<string> TableData = new List<string>();
            TableData = pmRoomDB.GetCompPN(PU);

            var jsondata = new { result = "OK", tableData = TableData };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;  
                
        }
        public ActionResult GlueRecord(string PU,string Line,string CompPN,string Qty)
        {
             
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TPMRoom pmRoomDB = new TPMRoom("PubReportMain");
                Message = pmRoomDB.GlueRecord(PU, Line, CompPN, Qty, user.UserID);

            }
            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;

        }
        public ActionResult GlueQuery(string DateTime)
        {
            List<TRFC_GLUEPOST_Log> list = new List<TRFC_GLUEPOST_Log>();

            TPMRoom pmRoomDB = new TPMRoom("PubReportMain");
            list = pmRoomDB.GlueQuery(DateTime);

            var jsondata = new { tableData = list };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;

        }

        
        #endregion GlueToSAP

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
