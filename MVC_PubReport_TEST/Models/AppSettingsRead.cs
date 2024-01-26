using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace MVC_PubReport.Models.AppSettings
{
    public class AppSettingsRead
    {

        public static List<SelectListItem> AppSettingsReadKey(string Key,char SplitFlag)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            string strKey = ConfigurationManager.AppSettings[Key].ToString();
            /*--Chai Modify: 20220408--*/
            //if (Key == "PU")
            //{
            //    strKey += "PU1;";
            //}
            /*--END Chai Modify: 20220408--*/
            string[] arraryKey = strKey.Split(SplitFlag);


            for (int i = 0; i < arraryKey.Length; i++)
            {

                listItem.Add(new SelectListItem { Text = arraryKey[i].ToString(), Value = arraryKey[i].ToString() });



            }
            return listItem;
        
        }

        public static  List<string> AppSettingsReadKeyStr(string Key, char SplitFlag)
        {

            List<string> listItem = new List<string>();
            string strKey = ConfigurationManager.AppSettings[Key].ToString();

            string[] arraryKey = strKey.Split(SplitFlag);


            for (int i = 0; i < arraryKey.Length; i++)
            {

                listItem.Add(arraryKey[i].ToString());



            }
            return listItem;
        }
    }
}