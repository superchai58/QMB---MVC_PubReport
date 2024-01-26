using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_PubReport.Models.QA;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.Public;
using MVC_PubReport.Models.Files;

namespace MVC_PubReport.Controllers
{
    public class QAController : Controller
    {
        private TUser user = new TUser();
        public ActionResult Index()
        {
            return View();
        }


        #region QITS_CompanyCode
        public ActionResult QITS_CompanyCode()
        {
            if (CheckUserSession())
            {
                GetUserRole();

            }

            return View();
        }

        public ActionResult QITS_CompanyCodeViewAll()
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_CompanyCode> TableData = new List<TQITS_CompanyCode>();
    
             
            TableData = db.QITS_CompanyCode_ViewAll();            
            Message = "OK";

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult QITS_CompanyCodeView(string CompanyCode)
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_CompanyCode> TableData = new List<TQITS_CompanyCode>();

            TableData = db.QITS_CompanyCode_View(CompanyCode) ;
            Message = "OK";

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }

        
        public ActionResult QITS_CompanyCodeSave(string CompanyCode, string CompanyName, string CompayLocal)
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_CompanyCode> TableData = new List<TQITS_CompanyCode>();
            TQITS_CompanyCode t = new TQITS_CompanyCode();
            t.CompanyCode = CompanyCode;
            t.CompanyName = CompanyName;
            t.CompayLocal = CompayLocal;

            if (CheckUserSession() == true)
            {
                Message = "OK";
                TableData = db.QITS_CompanyCode_Save(t, user.UserID);
            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult QITS_CompanyCode_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TQITS_CompanyCode> TableData = JsonHelper.DeserializeJsonToList<TQITS_CompanyCode>(jsonData);

                foreach (var item in TableData)
                {
                    TQA db = new TQA("PubReportMS");
                    db.QITS_CompanyCode_Delete(item,user.UserID);
                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion QITS_CompanyCode


        #region User_List
        public ActionResult User_List()
        {
            List<SelectListItem> ListItem = new List<SelectListItem>();
            TQA db = new TQA("PubReportMS");
            List<string> list = db.QITS_CompanyCode_GetCompanyCode();

            foreach (var item in list)
            {
                ListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["CompanyCode"] = new SelectList(ListItem, "Value", "Text", "");

            if (CheckUserSession() == true)
            {
                GetUserRole();
            }
            
            return View();
        }

        public ActionResult User_ListViewAll()
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TUser_List> TableData = new List<TUser_List>();


            if (CheckUserSession() == true)
            {

                if (user.PU != null && user.Type != null)
                {
                   
                    TableData = db.User_List_ViewAll(user.PU, user.Type);
                    Message = "OK";

                }
                else if (GetUserRole())
                {
                    TableData = db.User_List_ViewAll(user.PU, user.Type);
                    Message = "OK";
                }
                else 
                {
                    Message = "您没有相关操作权限！请联系QMS维护ms.dbo.User_List 资料";
                }
               
                
            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            
            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult User_ListView(string UserID)
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TUser_List> TableData = new List<TUser_List>();

            TableData = db.User_List_View (UserID);
            Message = "OK";

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult User_ListViewGetLeader()
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TUser_List> TableData = new List<TUser_List>();

            if (CheckUserSession() == true)
            {
               TableData =  db.User_List_GetLeader(user.PU, user.Type);
            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult User_ListViewGetQA()
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TUser_List> TableData = new List<TUser_List>();

            if (CheckUserSession() == true)
            {
                TableData = db.User_List_GetQA(user.PU, user.Type);
            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult User_ListSave(string jsonData)
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TUser_List> t = JsonHelper.DeserializeJsonToList<TUser_List>(jsonData);
            List<TUser_List> TableData = new List<TUser_List>();

            if (CheckUserSession() == true)
            {
                t[0].PU = user.PU;
                t[0].Type = user.Type;
                // 若公司代码是QTA的，则检查员工号是否是HR里面的资料，否则不能保存
                if (t[0].CompanyCode.ToUpper() == "QTA")
                {
                    MISHR_WebService.OAService wf = new MISHR_WebService.OAService();
                  
                    if (wf.CheckEmployeeIDIsExists(t[0].UserID) == true)
                    {
                        Message = "OK"; 
                        TableData = db.User_List_Save(t[0], user.UserID);

                    }
                    else
                    {
                        Message = "工号不存在HR系统中";
                    }


                }
                else 
                {//若公司代码不是QTA代表不是广达员工，则系统生成UserID
                    Message = "OK";
                    //若UserID是空，则表示是新增的User 需系统自动生成UserID
                    if (t[0].UserID == "" || t[0].UserID == null)
                    {
                        t[0].UserID = db.User_List_GetNonUserID();
                    }
                   
                    TableData = db.User_List_Save(t[0], user.UserID);
                }

            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult User_List_Delete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TUser_List> TableData = JsonHelper.DeserializeJsonToList<TUser_List>(jsonData);

                foreach (var item in TableData)
                {
                    TQA db = new TQA("PubReportMS");
                    db.User_List_Delete(item,user.UserID);
                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion User_List

        #region QITS_ModelSetting
        public ActionResult QITS_ModelSetting()
        {
            List<SelectListItem> ListItem = new List<SelectListItem>();
            TQA db = new TQA("PubReportMS");
            List<string> list = db.QITS_CompanyCode_GetCompanyCode();

            foreach (var item in list)
            {
                ListItem.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }

            ViewData["CompanyCode"] = new SelectList(ListItem, "Value", "Text", ""); 

            if (CheckUserSession() == true)
            {
                GetUserRole();
            }

            return View();
        }

        public ActionResult QITS_ModelSettingViewAll()
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_ModelSetting> TableData = new List<TQITS_ModelSetting>();


            if (CheckUserSession() == true)
            {

                if (user.PU != null && user.Type != null)
                {

                    TableData = db.QITS_ModelSetting_ViewAll (user.PU, user.Type);
                    Message = "OK";

                }
                else if (GetUserRole())
                {
                    TableData = db.QITS_ModelSetting_ViewAll(user.PU, user.Type);
                    Message = "OK";
                }
                else
                {
                    Message = "您没有相关操作权限！请联系QMS维护ms.dbo.User_List 资料";
                }


            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult QITS_ModelSettingView(string Model)
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_ModelSetting> TableData = new List<TQITS_ModelSetting>();
         

            if (CheckUserSession())
            {
                if (GetUserRole())
                {
                    TQITS_ModelSetting t = new TQITS_ModelSetting();
                    t.PU = user.PU;
                    t.Type = user.Type;
                    t.Model = Model;

                    TableData = db.QITS_ModelSetting_View(t);
                    Message = "OK"; //

                }
                else
                {
                    Message = "您没有相关操作权限";
                }
                
            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult QITS_ModelSettingDelete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TQITS_ModelSetting> TableData = JsonHelper.DeserializeJsonToList<TQITS_ModelSetting>(jsonData);

                foreach (var item in TableData)
                {
                    TQA db = new TQA("PubReportMS");
                    db.QITS_ModelSetting_Delete(item, user.UserID);
                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public ActionResult QITS_ModelSettingSave(string jsonData)
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_ModelSetting> t = JsonHelper.DeserializeJsonToList<TQITS_ModelSetting>(jsonData);
            List<TQITS_ModelSetting> TableData = new List<TQITS_ModelSetting>();

            if (CheckUserSession() == true)
            {
                t[0].PU = user.PU;
                t[0].Type = user.Type;
                Message = "OK";
                TableData = db.QITS_ModelSetting_Save(t[0], user.UserID);
            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        #endregion QITS_ModelSetting


        #region QITS_IssueTypeSetting
        public ActionResult QITS_IssueTypeSetting()
        {
            if (CheckUserSession() == true)
            {
                GetUserRole();
            }

            return View();
        }

        public ActionResult QITS_IssueTypeSettingViewAll()
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_IssueTypeSetting> TableData = new List<TQITS_IssueTypeSetting>();


            if (CheckUserSession() == true)
            {

                if (user.PU != null && user.Type != null)
                {

                    TableData = db.QITS_IssueTypeSetting_ViewAll  (user.PU, user.Type);
                    Message = "OK";

                }
                else if (GetUserRole())
                {
                    TableData = db.QITS_IssueTypeSetting_ViewAll(user.PU, user.Type);
                    Message = "OK";
                }
                else
                {
                    Message = "您没有相关操作权限！请联系QMS维护ms.dbo.User_List 资料";
                }


            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }


        public ActionResult QITS_IssueTypeSettingSave(string IssueType)
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            TQITS_IssueTypeSetting t = new TQITS_IssueTypeSetting();

            List<TQITS_IssueTypeSetting> TableData = new List<TQITS_IssueTypeSetting>();

            if (CheckUserSession() == true)
            {
                t.PU = user.PU;
                t.Type = user.Type;
                t.IssueType = IssueType;

                TableData = db.QITS_IssueTypeSetting_Save(t, user.UserID);
                Message = "OK";
                    
            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }


        public JsonResult QITS_IssueTypeSettingDelete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TQITS_IssueTypeSetting> TableData = JsonHelper.DeserializeJsonToList<TQITS_IssueTypeSetting>(jsonData);

                foreach (var item in TableData)
                {
                    TQA db = new TQA("PubReportMS");
                    db.QITS_IssueTypeSetting_Delete  (item, user.UserID);
                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion QITS_IssueTypeSetting

        #region QITS_IssueList
        public ActionResult QITS_IssueList()
        {
           
            TQA db = new TQA("PubReportMS");
            List<TQITS_ModelSetting> ModelTableData = new List<TQITS_ModelSetting>();
            List<SelectListItem> ModelListItem = new List<SelectListItem>();

            List<TQITS_IssueTypeSetting> IssueTypeTableData = new List<TQITS_IssueTypeSetting>();
            List<SelectListItem> IssueTypeListItem = new List<SelectListItem>();

            if (CheckUserSession() == true)
            {
                //GetUserRole();
                if (user.PU != null && user.Type != null)
                {

                    ModelTableData = db.QITS_ModelSetting_ViewAll(user.PU, user.Type);
                    IssueTypeTableData = db.QITS_IssueTypeSetting_ViewAll(user.PU, user.Type);

                }
                else if (GetUserRole())
                {
                    ModelTableData = db.QITS_ModelSetting_ViewAll(user.PU, user.Type);
                    IssueTypeTableData = db.QITS_IssueTypeSetting_ViewAll(user.PU, user.Type);
                   
                } 
            foreach (var item in ModelTableData)
            {
                ModelListItem.Add(new SelectListItem { Text = item.Model.ToString(), Value = item.Model.ToString() });
            }

            ViewData["Model"] = new SelectList(ModelListItem, "Value", "Text", "");

            foreach (var item in IssueTypeTableData)
            {
                IssueTypeListItem.Add(new SelectListItem { Text = item.IssueType.ToString(), Value = item.IssueType.ToString() });
            }

            ViewData["IssueType"] = new SelectList(IssueTypeListItem, "Value", "Text", ""); 

                 

            }

            return View();
        }


        public JsonResult QITS_IssueListGetDataByModel(string Model)
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_IssueList> TableData = new List<TQITS_IssueList>();
            List<TQITS_ModelSetting> ModelSettingTableData = new List<TQITS_ModelSetting>();
            string QMOwer = "";

            if (CheckUserSession())
            {
                if (user.PU != null && user.Type != null)
                {
                    TQITS_ModelSetting t = new TQITS_ModelSetting();
                    t.PU = user.PU;
                    t.Type = user.Type;
                    t.Model = Model;

                    TableData = db.QITS_IssueListGetDataByModel(user.PU,user.Type,Model);
                    ModelSettingTableData = db.QITS_ModelSetting_View(t);
                    QMOwer = ModelSettingTableData[0].QAPIC;
                    Message = "OK";  

                }
                else
                {
                    Message = "您没有相关操作权限";
                }

            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }


            var jsondata = new { result = Message, tableData = TableData, QMOwer = QMOwer };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public JsonResult QITS_IssueListGetDataByRole()
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_IssueList> TableData = new List<TQITS_IssueList>();
            List<TQITS_ModelSetting> ModelSettingTableData = new List<TQITS_ModelSetting>();
            //string QMOwer = "";

            if (CheckUserSession())
            {
                if (user.PU != null && user.Type != null)
                {
                    TQITS_ModelSetting t = new TQITS_ModelSetting();
                    t.PU = user.PU;
                    t.Type = user.Type; 

                    TableData = db.QITS_IssueListGetDataByUserRole(user.PU, user.Type,user.UserRole,  user.UserID);
                    //ModelSettingTableData = db.QITS_ModelSetting_View(t);
                    //QMOwer = ModelSettingTableData[0].QAPIC;
                    Message = "OK";

                }
                else
                {
                    Message = "您没有相关操作权限";
                }

            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }


            var jsondata = new { result = Message, tableData = TableData  };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }

        public ActionResult QITS_IssueListSave(string jsonData)
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_IssueList> t = JsonHelper.DeserializeJsonToList<TQITS_IssueList>(jsonData);

            List<TQITS_IssueList> TableData = new List<TQITS_IssueList>();

            if (CheckUserSession() == true)
            {
                t[0].PU = user.PU;
                t[0].Type = user.Type;
                Message = "OK";
                if (t[0].IssueNO != "")
                {
                    TableData = db.QITS_IssueListUpdate(user.PU, user.Type, user.UserID, t[0]);
                }
                else
                {
                    TableData = db.QITS_IssueListSave(user.PU, user.Type, user.UserID, t[0]);
                }
                
            }
            else
            {
                Message = "用户登录已失效，请重新登录后操作";
            }

            var jsondata = new { result = Message, tableData = TableData };
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        public ActionResult QITS_IssueListDelete(string jsonData)
        {
            string Message = "";
            TQA db = new TQA("PubReportMS");
            List<TQITS_IssueList> t = JsonHelper.DeserializeJsonToList<TQITS_IssueList>(jsonData);
 

            if (CheckUserSession() == true)
            {
                t[0].PU = user.PU;
                t[0].Type = user.Type;
                Message = "OK";
                foreach (var item in t)
                {
                    db.QITS_IssueListDelete(user.UserID, item.IssueNO);
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

        #endregion QITS_IssueList
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

        private bool GetUserRole()
        {
            List<TUser_List> list = new List<TUser_List>();
            TQA db = new TQA("PubReportMS");
            list = db.User_List_View(user.UserID);
            if (list.Count > 0)
            {
                user.PU = list[0].PU;
                user.Type = list[0].Type;
                user.UserRole = list[0].UserRole;
                //TableData = db.User_List_ViewAll(user.PU, user.Type);
                //Message = "OK";
                Session["Pub_Report_User"] = user;
                return true;
            }
            else
            {

                return false;
            }
        }

    }
}
