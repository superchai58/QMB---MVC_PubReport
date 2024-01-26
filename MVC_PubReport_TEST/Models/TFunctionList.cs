using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using System.Data;
using System.Data.SqlClient;

namespace MVC_PubReport.Models.Public
{
    public class TFunctionList
    {
        [Key]
        public int MenuID { get;set; }

        
        [Required]
        [DisplayName("菜单名称")]
        public string MenuName { get; set; }

        [Required]
        [DisplayName("菜单Link的页面")]
        public string LinkPage { get; set; }

        [Required]
        public int ParentMenuId { get; set; }

        [DisplayName("菜单是否公开")]
        public int IsPublic { get; set; } // 0 表示不是公共，1表示是公共，默认是0

        public string UserId { get; set; }
    }


    public class FunctionListContext {

        private DB db ;

        public FunctionListContext() 
        {
            db = new DB("PubReportMain");
        }

        public List<TFunctionList> View_FunctionList(int type,string UserId)
        {
            
            DataTable dt = null;
            string sql = "EXEC [MVC_PUBReport_GetMenu] " + type + ",'MVC_PUB_Report','" + UserId + "'";
            dt = db.Execute(sql).Tables[0];
            db.CloseDB();

            return DataTableTool.ToList<TFunctionList>(dt);
        }
        public void AuthManageDelete(TFunctionList func,string OpUserId)
        {

            DataTable dt = null;
            string sql = "DELETE  FROM MVC_UT_Auth WHERE System_Name = 'MVC_PUB_Report' AND  UserID = '" + func.UserId + "' AND  Active = 'Y' AND AuthItem =N'" + func.MenuName + "'";
            db.Execute(sql);
            db.CloseDB();

            
        }


        public string Save_FunctionList(TFunctionList menu, int Level, string UserId, int opType)
        {
            string result = "";
            DataSet ds = null;
            DataTable dt = null;
            string sql = "";

            if (opType == 1)
            {
                sql = "SELECT * FROM dbo.MVC_FunctionList WITH(NOLOCK) WHERE MenuName = N'" + menu.MenuName + "'";
                ds = db.Execute(sql);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        result = "节点名称[" + menu.MenuName + "]重复!";
                        return result;
                    }
                }
            }

            sql = "exec MVC_PUBReport_SaveMenu " + menu.ParentMenuId + "," + Level + ",N'" + menu.MenuName + "','" + menu.LinkPage + "'," + menu.IsPublic + ",'" + UserId + "'," + opType + "";
            //SqlParameter[] paraList = {
                                       
            //                            db.CreateInParam("@ParentID",SqlDbType.Int,10 ,menu.ParentMenuId),
            //                            db.CreateInParam("@Level",SqlDbType.Int,1,Level),
            //                            db.CreateInParam("@MenuName",SqlDbType.NVarChar,50,menu.MenuName),
            //                            db.CreateInParam("@LinkPage",SqlDbType.VarChar,50,menu.LinkPage),
            //                            db.CreateInParam("@Ispublic",SqlDbType.Int,1,menu.IsPublic),
            //                             db.CreateInParam("@UserName",SqlDbType.VarChar,20,UserId),
            //                             db.CreateInParam("@OpType",SqlDbType.VarChar,1,opType)
                                       
            //                        };

            //db.Execute("MVC_PUBReport_SaveMenu", paraList) ;
            db.Execute(sql);
            

            return result;
        
        }


 

        public void AuthManageSaveData(string UserID, string[] AuthItem, string OpUserID)
        {
            for (int i = 0;i < AuthItem.Length;i++)
            {
                string sql = "INSERT INTO MVC_UT_Auth(System_Name, AuthItem, UserID, Active, UID, TransDateTime) " +
                            "VALUES('MVC_PUB_Report',N'" + AuthItem[i] + "','" + UserID + "','Y','" + OpUserID + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
                db.Execute(sql);
                db.CloseDB();      
            }

        
        }
 
    
    }
}