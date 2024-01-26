using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QMSSDK;
using System.Data;
using System.Data.SqlClient;

namespace MVC_PubReport.Models
{
    public class DB : QMSSDK.Db.Static
    {
        public DB(string Key)
        {
            QMSSDK.Db.Connections.CreateCn(Key);

        }

        /// <summary>
        /// 生成存储过程参数
        /// </summary>
        /// <param name="paramName">存储过程名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="direction">参数方向</param>
        /// <param name="value">参数值</param>
        /// <returns>新的 parameter 对象</returns>
        public static SqlParameter CreateParam(string paramName, SqlDbType dbType, Int32 size, ParameterDirection direction, object value)
        {
            SqlParameter param;

            ///当参数大小为0时，不使用该参数大小值

            if (size > 0)
            {
                param = new SqlParameter(paramName, dbType, size);
            }
            else
            {
                ///当参数大小为0时，不使用该参数大小值

                param = new SqlParameter(paramName, dbType);
            }

            ///创建输出类型的参数

            param.Direction = direction;
            if (!(direction == ParameterDirection.Output && value == null))
            {
                param.Value = value;
            }

            ///返回创建的参数

            return param;
        }

        /// <summary>
        /// 传入输入参数
        /// </summary>
        /// <param name="ParamName">存储过程名称</param>
        /// <param name="DbType">参数类型</param></param>
        /// <param name="Size">参数大小</param>
        /// <param name="Value">参数值</param>
        /// <returns>新的parameter 对象</returns>
        public SqlParameter CreateInParam(string paramName, SqlDbType dbType, int size, object value)
        {
            return CreateParam(paramName, dbType, size, ParameterDirection.Input, value);
        }

        public void CloseDB()
        {
            if (QMSSDK.Db.Connections.cn.State != ConnectionState.Closed)
            { QMSSDK.Db.Connections.Close(); }
        }

        public void SaveQMSLog(string System_Name, string Event_NO,string UserName,string Info2,string desc1 )
        {
            desc1 = desc1.Replace("'", "");
            string strSQL = "Insert into QMS_Log (Process_Name,event_no,Info1,Info2,Desc1,trans_Date) values ('" + System_Name + "','" + Event_NO + "','" + UserName + "','" + Info2 + "','" + desc1 + "' , getdate() )";
            
            this.Execute(strSQL);
        }
    }
}