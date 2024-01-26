using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MVC_PubReport.Models.Files;
namespace MVC_PubReport.Controllers
{
    public class FileDLController : Controller
    {
        //
        // GET: /FileDL/

        public void DLFile(string fileName)
        {

            //string fileName = "aaa.txt";//客户端保存的文件名
            string filePath = Server.MapPath("~/Upload/Template/" + fileName);//路径
            FileInfo fileinfo = new FileInfo(filePath);
            Response.Clear();         //清除缓冲区流中的所有内容输出
            Response.ClearContent();  //清除缓冲区流中的所有内容输出
            Response.ClearHeaders();  //清除缓冲区流中的所有头
            Response.Buffer = true;   //该值指示是否缓冲输出，并在完成处理整个响应之后将其发送
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileinfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/unknow";  //获取或设置输出流的 HTTP MIME 类型
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312"); //获取或设置输出流的 HTTP 字符集
            Response.TransmitFile(filePath);
            Response.End(); 
        }

       

    }
}
