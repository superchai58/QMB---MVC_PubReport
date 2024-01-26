using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_PubReport.Models.User;
using MVC_PubReport.Models.Public;
using Newtonsoft.Json;

namespace MVC_PubReport.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
       

        public ActionResult Index()
        {
            FunctionListContext db = new FunctionListContext();
            List<TFunctionList> list = new List<TFunctionList>();
            if (Session["Pub_Report_User"] != null)
            {
                TUser user = Session["Pub_Report_User"] as TUser;
                
                list = db.View_FunctionList(2, user.UserID);
            }
            else
            {
                list = db.View_FunctionList(1, "");
            }
            ViewData.Model = list;
            //TempData["Model"] = list;
            return View( );
        }

    }
}
