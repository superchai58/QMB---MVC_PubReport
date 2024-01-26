using System;
 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.Public;
 
 
 
 
namespace MVC_PubReport.Controllers
{
    public class UserController : Controller
    {
        private TUser user = new TUser();




        #region FunctionListManage 页面
        public ActionResult FunctionListManage()
        {
            FunctionListContext db = new FunctionListContext();
            List<TFunctionList> list = new List<TFunctionList>();

            list = db.View_FunctionList(4, "");
           
            ViewData.Model = list;
            return View();
        }

        public ActionResult FunctionListSave(int ParentID, string ParentMenuName, string MenuName, string LinkPage, int IsPublic)
        {
            
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TFunctionList menu = new TFunctionList();
                int MenuLevel = int.Parse(ParentMenuName.Split(new string[] { "||" }, StringSplitOptions.None)[1]);
                
                menu.ParentMenuId = ParentID;
                menu.MenuName = MenuName;
                menu.LinkPage = LinkPage;
                menu.IsPublic = IsPublic;

                FunctionListContext db = new FunctionListContext();
                Message = db.Save_FunctionList(menu, MenuLevel, user.UserID, 1);
 
            }
            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }

          
       public ActionResult FunctionListDelete(int ParentID)
        {
            
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                TFunctionList menu = new TFunctionList();
                 
                
                menu.ParentMenuId = ParentID;
                menu.MenuName = "";
                menu.LinkPage = "";
                menu.IsPublic = 0;

                FunctionListContext db = new FunctionListContext();
                Message = db.Save_FunctionList(menu, 0, user.UserID, 2);
 
            }
            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }


        #endregion FunctionListManage 页面
       #region 登录页面
       /// <summary>
        /// 登录页面视图显示
        /// </summary>
        /// <returns></returns>
        public ActionResult Login( )
        {

            return View();
        }
        /// <summary>
        /// 检查登录账号是否合法
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoginCheckAuth(string UserID, string PassWord)
        {
            string Message = "";

            if (CheckAuth(UserID, PassWord) == true)
            {
                TUser user = new TUser();
                user.UserID = UserID;
                user.PassWord = PassWord;
                Session["Pub_Report_User"] = user;

                Message = "OK";
            }
            else
            {
                Message = "Fail";
            }


            return Json(new
            {
                result = Message,

            }, JsonRequestBehavior.AllowGet);
        }


        private bool CheckAuth(string UserID, string PassWord)
        {

            TUserDB userDB = new TUserDB("PubReportMain");
            string Damion = "Quantacn";

            MISHR_WebService.OAService wf = new MISHR_WebService.OAService();

            if (wf.CheckDomainUserLogin(Damion, UserID, PassWord) == true)
            {

                user.UserID = UserID;
                user.PassWord = PassWord;
                return true;

            }
            else
            {
                //若没有在域账号里面 查看是否是注册账号
                //List<TUser> user = new List<TUser>();
                //user = userDB.CheckAuth(UserID, PassWord);
                List<TUser> userList = new List<TUser>();
                userList = userDB.ViewUser(UserID, PassWord, 1);
                if (userList.Count > 0)
                {
                    user = userList[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult LoginOut()
        {
            Session.Remove("Pub_Report_User");
            return RedirectToAction("Index", "Home");
        }
       #endregion 登录页面
        #region 注册用户页面

        /// <summary>
        /// 注册页面显示
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterUser()
        {

            return View();
        }
        /// <summary>
        /// 检查工号是否存在HR系统中
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult CheckUser(string UserID)
        {
            string Message = "";
            TUserDB userDB = new TUserDB("PubReportMain");
            List<TUser> userList = new List<TUser>();
            //userList = userDB.ViewUser(UserID, "", 4);
            //if (userList.Count > 0)
            MISHR_WebService.OAService wf = new MISHR_WebService.OAService();

            if (wf.CheckEmployeeIDIsExists(UserID) == true)
            {
                Message = "OK";

            }
            else
            {
                Message = "工号不存在HR系统中";
            }


            return Json(new
            {
                result = Message,

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUser(string PU, string Type, string UserID, string PassWord)
        {
            string Message = "";
            TUserDB userDB = new TUserDB("PubReportMain");
            List<TUser> userList = new List<TUser>();
            //userList = userDB.ViewUser(UserID, "", 4);
            //if (userList.Count > 0)
            MISHR_WebService.OAService wf = new MISHR_WebService.OAService();

            if (wf.CheckEmployeeIDIsExists(UserID) == true)
            {
                Message = "OK";
                userDB.RegisterUser(PU, Type, UserID, PassWord);

            }
            else
            {
                Message = "工号不存在HR系统中";
            }


            return Json(new
            {
                result = Message,

            }, JsonRequestBehavior.AllowGet);
        }
       #endregion 注册用户页面

        #region AuthManage页面
        public ActionResult AuthManage()
        {

            return View();
        }

        public ActionResult AuthManageGetData(string UserID)
        {
            FunctionListContext db = new FunctionListContext();
            List<TFunctionList> list = new List<TFunctionList>();
            TUserDB userDB = new TUserDB("PubReportMain");
            List<TUser> userList = new List<TUser>();

            string Message = "";
            MISHR_WebService.OAService wf = new MISHR_WebService.OAService();

            if (wf.CheckEmployeeIDIsExists(UserID) == true)
             {
                 Message = "OK";
                 list = db.View_FunctionList(3, UserID);
             }
            //userList = userDB.ViewUser(UserID, "", 4);
            //if (userList.Count > 0)
            //{
            //    Message = "OK";
            //    list = db.View_FunctionList(3, UserID);

            //}
            else
            {
                Message = "工号不存在HR系统中";
            } 

            var jsondata = new { result = Message, tableData = list };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }

        public ActionResult AuthManageSaveData(string UserID,string[] AuthItem)
        {
            FunctionListContext db = new FunctionListContext();
           

            
            string Message = "OK";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                db.AuthManageSaveData(UserID, AuthItem, user.UserID);
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
            
        }
        public JsonResult AuthManageDelete(string jsonData)
        {
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TFunctionList> TableData = JsonHelper.DeserializeJsonToList<TFunctionList>(jsonData);

                foreach (var item in TableData)
                {
                    
                    FunctionListContext db = new FunctionListContext();
                    db.AuthManageDelete(item, user.UserID);

                }

                Message = "OK";
            }

            var jsondata = new { result = Message };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion AuthManage页面


        #region 用户信息查看
     
        /// <summary>
        /// 查看当前登录者的个人资料
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult ViewUser(string UserID)
        {
            TUserDB userDB = new TUserDB("PubReportMain");
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TUser> userList = new List<TUser>();
                userList = userDB.ViewUser(UserID, "", 2);
                if (userList.Count < 0)
                {
                    Message = "您没有注册QMS PUB_Report 账号，所以没有您的相关信息";
                }
                else
                {
                    user = userList[0];
                }
            }
            ViewBag.Message = Message;
            ViewBag.User = user;
            return View();

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public ActionResult ChangePW(string UserID,string PassWord)
        {
            TUserDB userDB = new TUserDB("PubReportMain");
            string Message = "";
            if (CheckUserSession() == false)
            {
                Message = "用户登录已失效，请重新登录后操作";
            }
            else
            {
                List<TUser> userList = new List<TUser>();
                userList = userDB.ViewUser(UserID, PassWord, 3);
                if (userList.Count < 0)
                {
                    Message = "您没有注册QMS PUB_Report 账号，所以没有您的相关信息";
                }
                else
                {
                    user = userList[0];
                }
            }
            var jsondata = new { result = Message, tableData = user };
            JsonResult jsonResult = new JsonResult();

            jsonResult.Data = jsondata;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }
        #endregion 用户信息查看
     

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
