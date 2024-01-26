using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MVC_PubReport.Models.Files;
using MVC_PubReport.Models.Public;

namespace MVC_PubReport.Models.IE
{

   
    public class TIEDB
    {

        private DB db;
 
        public TIEDB()
        {
            db = new DB("PubReportMain");
        }

        public bool BuildPubConnection(string Key)
        {
            try
            {
                db = new DB(Key);
              
                return true;
            }

            catch

            {
                return false;
            }
           
        
        }

        #region Efficiency
        public List<string> Efficiency_GetMode(string WorkDate)
        {
            List<string> list = new List<string>();
            string sql = "";
            DataTable dt = new DataTable();

            sql = "select distinct BU+Type as Mode from Efficiency_DailyDetail where WorkDate='" + WorkDate + "'";
            dt = db.Execute(sql).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["Mode"].ToString());
           
            }
                return list;
        
        }
        public List<TEfficiency> Efficiency_Query(TEfficiency eff)
        {
            List<TEfficiency> list = new List<TEfficiency>();
            DataTable dt = new DataTable();

            string sql = "";
            sql = "select BU+Type as Mode,WorkDate,Line,Shift,WKTM  ,EXCTM  ,DOTM  ,OutPut  ,OutPutHour  ,InsertDateTime " +
                     " from Efficiency_DailyDetail where 1 = 1"  ; 
            if (eff.WorkDate != "")
            {
                sql = sql + " and WorkDate ='" + eff.WorkDate + "' ";            
            }
            if (eff.Mode != "")
            {
                sql = sql + " and BU+Type ='" + eff.Mode + "' ";  
            }
            if (eff.Line != "")
            {
                sql = sql + " and line ='" + eff.Line + "' ";  
            }
            if (eff.Shift != "")
            {
                sql = sql + " and Shift ='" + eff.Shift + "' ";
            }
            sql = sql + " order by BU,Type,WorkDate,Line,Shift";
 
            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TEfficiency>(dt);
            return list;
        }
        #endregion Efficiency
        #region TIE_FQALineMapping
        public List<TIE_FQALineMapping> IE_FQALineMapping_GetData(TIE_FQALineMapping fqaLine)
        {
            List<TIE_FQALineMapping> list = new List<TIE_FQALineMapping>();
            DataTable dt = new DataTable();

            string sql = "";

            sql = "SELECT ROW_NUMBER() OVER(ORDER BY Depart_No) AS ID,FQA_Line,FQA_Shift,FQA_Line_No,Depart_No,Mode,Line,Shift,Line_No,UserID,TransDateTime FROM dbo.IE_FQALineMapping WHERE 1=1";

            if (fqaLine.FQA_Line_No != "")
            {
                sql = sql + "and FQA_Line_No='" + fqaLine.FQA_Line_No + "'";

            }
            if (fqaLine.Depart_No != "")
            {
                sql = sql + " and Depart_No='" + fqaLine.Depart_No + "'";
            }
            if (fqaLine.Mode != "")
            {
                sql = sql + " and Mode='" + fqaLine.Mode + "'";
            }
            if (fqaLine.Line_No != "")
            {
                sql = sql + "and Line_No='" + fqaLine.Line_No + "'";
            }
            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TIE_FQALineMapping>(dt);
            return list;
        }

        public List<TIE_FQALineMapping> IE_FQALineMapping_SaveData(TIE_FQALineMapping fqaLine, string opUser)
        {
            List<TIE_FQALineMapping> list = new List<TIE_FQALineMapping>();
            string sql = "";

            list = IE_FQALineMapping_GetData(fqaLine);
            if (list.Count == 0)
            {
                sql = "INSERT INTO dbo.IE_FQALineMapping(FQA_Line, FQA_Shift, FQA_Line_No, Depart_No, Mode, Line, Shift, Line_No, UserID, TransDateTime) "+
                        " VALUES('" + fqaLine.FQA_Line + "','" + fqaLine.FQA_Shift + "','" + fqaLine.FQA_Line_No + "','" + fqaLine.Depart_No + "','" + fqaLine.Mode +
                            "','" + fqaLine.Line + "','" + fqaLine.Shift + "','" + fqaLine.Line_No + "','" + opUser + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
            }
            else
            {
                sql = "UPDATE IE_FQALineMapping SET FQA_Line='"+fqaLine.FQA_Line+"', FQA_Shift='"+fqaLine.FQA_Shift+"',Line='"+fqaLine.Line+"',Shift='"+fqaLine.Shift+"',UserID='"+opUser+"',TransDateTime=dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS') "+
                        " WHERE FQA_Line_No ='"+fqaLine.Line_No+"' AND Depart_No ='"+fqaLine.Depart_No+"' AND Mode='"+fqaLine.Mode+"' AND Line_No='"+fqaLine.Line_No+"' ";

            }
            db.Execute(sql);
            list = IE_FQALineMapping_GetData(fqaLine);
            return list;
        }
        public void IE_FQALineMapping_Delete(TIE_FQALineMapping fqaLine, string opUser)
        {
            string sql = "";
            sql = "delete from IE_FQALineMapping " +
                 " WHERE FQA_Line_No ='" + fqaLine.FQA_Line_No + "' AND Depart_No ='" + fqaLine.Depart_No + "' AND Mode='" + fqaLine.Mode + "' AND Line_No='" + fqaLine.Line_No + "' ";


            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report", "4", opUser, "", sql);

        }
        #endregion TIE_FQALineMapping
        #region IE_BaseFactor
        public void IE_BaseFactor_Delete(TIE_BaseFactor baseFactory, string opUser)
        {
            string sql = "";
            sql = "delete from IE_BaseFactor  where  mode='"+baseFactory.Mode+ "' and line='"+baseFactory.Line+"' and " +
                      "Section='"+baseFactory.Section+ "' and Stage='"+baseFactory.Stage+ "'";
            
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report", "4", opUser, "", sql);
        
        }
        public List<TIE_BaseFactor> IE_BaseFactor_GetData(TIE_BaseFactor baseFactory)
        {
            List<TIE_BaseFactor> list = new List<TIE_BaseFactor>();
            DataTable dt = new DataTable();

            string sql = "";

            sql = "select ROW_NUMBER()OVER (ORDER BY Mode) AS ID, Mode,Line,Section,Stage,Base,Factor,CountTimes,TransDateTime,UserName from IE_BaseFactor where 1=1";
 
            if (baseFactory.Mode != "")
            {
                sql = sql + "and mode='" + baseFactory.Mode + "'";
            
            }
            if (baseFactory.Line != "")
            {
                sql = sql + " and line='" + baseFactory.Line + "'";
            }
            if (baseFactory.Section != "")
            {
                sql = sql + " and Section='" + baseFactory.Section + "'";
            }
            if (baseFactory.Stage != "")
            {
                sql = sql + "and Stage='" + baseFactory.Stage + "'";
            }
            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TIE_BaseFactor>(dt);
            return list;
        
        }
     
        public List<TIE_BaseFactor> IE_BaseFactor_SavaData(TIE_BaseFactor  baseFactory, string opUser)
        {
            List<TIE_BaseFactor> list = new List<TIE_BaseFactor>();
            string sql = "";

            list = IE_BaseFactor_GetData(baseFactory);
            if (list.Count == 0)
            {
                sql = "insert into IE_BaseFactor(Mode, line,Section, Stage, Base, Factor,CountTimes, TransDateTime, UserName)" +
                         " values ('" + baseFactory.Mode + "','" + baseFactory.Line + "','" + baseFactory.Section + "','" + baseFactory.Stage + "'," +
                        "" + baseFactory.Base + "," + baseFactory.Factor + ",'" + baseFactory.CountTimes + "',dbo.formatdate(getdate(),'YYYYMMDDHHNNSS'),'" + opUser + "')";
            }
            else
            {
                sql = "update IE_BaseFactor set Base=" +baseFactory.Base+ ",Factor=" +baseFactory.Factor+ ",CountTimes='" +baseFactory.CountTimes+ "',TransDateTime=dbo.formatdate(getdate(),'YYYYMMDDHHNNSS'),UserName='" +opUser+ "' " +
                     "where  mode='"+baseFactory.Mode+ "' and line='"+baseFactory.Line+"' and " +
                      "Section='"+baseFactory.Section+ "' and Stage='"+baseFactory.Stage+ "'";

            }
            db.Execute(sql);
            list = IE_BaseFactor_GetData(baseFactory);
            return list;
        }

        #endregion IE_BaseFactor
        #region IE_InputQty_TestWO
        public List<string> IE_InputQty_TestWO_GetPU(string TransDate)
            {
                string sql = "";
                DataTable dt = new DataTable();
                List<string> PUList = new List<string>();

                sql = "SELECT DISTINCT Mode AS Mode FROM IE_InputQty_TestWO A  " +
                        "WHERE TransDate='" + TransDate + "' ORDER BY Mode";
                dt = db.Execute(sql).Tables[0];
                int dtCount = dt.Rows.Count;

                for (int i = 0; i < dtCount; i++)
                {
                    PUList.Add(dt.Rows[i]["Mode"].ToString());

                }
                return PUList;

            }

            public List<TIE_InputQty_TestWO> IE_InputQty_TestWO_Query(TIE_InputQty_TestWO product)
            {
                List<TIE_InputQty_TestWO> InputList = new List<TIE_InputQty_TestWO>();
                DataTable dt = new DataTable();

                string sql = "";
                sql = "SELECT Mode,TransDate,Line,Shift,Model,WO,Stage,QTY,SourcePU,InsertDateTime,PN FROM dbo.IE_InputQty_TestWO WHERE 1 = 1";

                if (product.TransDate != "")
                {
                    sql = sql + " AND TransDate ='" + product.TransDate + "'";
                }
                if (product.Mode != "")
                {
                    sql = sql + "AND Mode='" + product.Mode + "'";

                }
                if (product.Line != "")
                {
                    sql = sql + "AND Line='" + product.Line + "'";

                }
                if (product.Shift != "")
                {
                    sql = sql + "AND Shift='" + product.Shift + "'";

                }
                if (product.Model != "")
                {
                    sql = sql + "AND Model='" + product.Model + "'";

                }
                if (product.WO != "")
                {
                    sql = sql + "AND WO='" + product.WO + "'";

                }

                dt = db.Execute(sql).Tables[0];
                InputList = DataTableTool.ToList<TIE_InputQty_TestWO>(dt);
                return InputList;
            }
            #endregion IE_InputQty_TestWO  
        #region IE_InputQty
        public List<string> IE_InputQty_GetPU(string TransDate)
        {
            string sql = "";
            DataTable dt = new DataTable();
            List<string> PUList = new List<string>();

            sql = "SELECT DISTINCT Mode AS Mode FROM IE_InputQty A  " +
                    "WHERE TransDate='" + TransDate + "' ORDER BY Mode";
            dt = db.Execute(sql).Tables[0];
            int dtCount = dt.Rows.Count;

            for (int i = 0; i < dtCount; i++)
            {
                PUList.Add(dt.Rows[i]["Mode"].ToString());

            }
            return PUList;

        }

        public List<TIE_InputQty> IE_InputQty_Query(TIE_InputQty product)
        {
            List<TIE_InputQty> InputList = new List<TIE_InputQty>();
            DataTable dt = new DataTable();

            string sql = "";
            sql = "SELECT Mode,TransDate,Line,Shift,Model,WO,QTY,SourcePU,InsertDateTime,PN FROM dbo.IE_InputQty WHERE 1 = 1";

            if (product.TransDate != "")
            {
                /*--Spite text--*/
                string[] textSplit = product.TransDate.Split(';');

                if (textSplit.Length >= 2)
                {
                    sql = sql + " AND TransDate BETWEEN '" + textSplit[0].ToString().Trim() + "' AND '" + textSplit[1].ToString().Trim() + "' ";
                }
                else
                {
                    sql = sql + " AND TransDate = '" + product.TransDate + "' ";
                }
            }
            if (product.Mode != "")
            {
                sql = sql + "AND Mode='" + product.Mode + "'";

            }
            if (product.Line != "")
            {
                sql = sql + "AND Line='" + product.Line + "'";

            }
            if (product.Shift != "")
            {
                sql = sql + "AND Shift='" + product.Shift + "'";

            }
            if (product.Model != "")
            {
                sql = sql + "AND Model='" + product.Model + "'";

            }
            if (product.WO != "")
            {
                sql = sql + "AND WO='" + product.WO + "'";

            }
            sql = sql + " Order by Mode, TransDate";

            dt = db.Execute(sql).Tables[0];
            InputList = DataTableTool.ToList<TIE_InputQty>(dt);
            return InputList;
        }
        #endregion IE_InputQty
        #region IE_DailyProductQty
        public List<string> IE_DailyProductQty_GetPU(string TransDate)
        {
            string sql = "";
            DataTable dt = new DataTable();
            List<string> PUList = new List<string>();

            /*--Spite text--*/
            string[] textSplit = TransDate.Split(';');            

            if (TransDate == "")
            {
                sql = "SELECT DISTINCT Mode AS Mode FROM IE_DailyProductQty A  ";
                    
            }
            else
            {
                if (textSplit.Length >= 2)
                {
                    sql = "SELECT DISTINCT Mode AS Mode FROM IE_DailyProductQty A  " +
                       "WHERE TransDate BETWEEN '" + textSplit[0].ToString().Trim() + "' AND '" + textSplit[1].ToString().Trim() + "' ORDER BY Mode";
                }
                else
                {
                    sql = "SELECT DISTINCT Mode AS Mode FROM IE_DailyProductQty A  " +
                       "WHERE TransDate = '" + TransDate + "' ORDER BY Mode";
                }
                
            }
           
            dt = db.Execute(sql).Tables[0];
            int dtCount = dt.Rows.Count;

            for (int i = 0; i < dtCount; i++)
            {
                PUList.Add(dt.Rows[i]["Mode"].ToString());

            }
            return PUList;
        
        }
        public List<TIE_DailyProductQty> IE_DailyProductQty_Query(TIE_DailyProductQty product)
        {
            List<TIE_DailyProductQty> ProductList = new List<TIE_DailyProductQty>();
            DataTable dt = new DataTable();

            string sql = "";
            sql = "SELECT Mode,TransDate,Line,Shift,Model,WO,QTY,SourcePU,InsertDateTime,PN FROM dbo.IE_DailyProductQty WHERE 1 = 1";

            if (product.TransDate !="")
            {
                /*--Spite text--*/
                string[] textSplit = product.TransDate.Split(';');

                if (textSplit.Length >= 2)
                {
                    sql = sql + " AND TransDate BETWEEN '" + textSplit[0].ToString().Trim() + "' AND '" + textSplit[1].ToString().Trim() + "' ";
                }
                else
                {
                    sql = sql + " AND TransDate = '" + product.TransDate + "' ";
                }
                //sql = sql +" AND TransDate ='"+product.TransDate+"'";
            }
            if (product.Mode != "")
            {
                sql = sql +"AND Mode='"+product.Mode+"'";
            
            }
            if (product.Line != "")
            {
                sql = sql +"AND Line='"+product.Line+"'";
            
            }
            if (product.Shift != "")
            {
                sql = sql +"AND Shift='"+product.Shift+"'";
            
            }
            if (product.Model != "")
            {
                sql = sql +"AND Model='"+product.Model+"'";
            
            }
            if (product.WO != "")
            {
                sql = sql +"AND WO='"+product.WO+"'";
            
            }
            sql = sql + " Order by Mode, TransDate";

            dt = db.Execute(sql).Tables[0];
            ProductList = DataTableTool.ToList<TIE_DailyProductQty>(dt);
            return ProductList;
        
        }
        #endregion IE_DailyProductQty
        #region IE_DepartMent
        public List<string> IE_DepartMent_GetDepartNO()
        {
            List<string> list = new List<string>();
            DataTable dt = new DataTable();

            string sql = "SELECT DISTINCT Depart_No FROM dbo.IE_DepartMent ORDER BY Depart_No ";
            dt = db.Execute(sql).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["Depart_No"].ToString());
            
            }

            return list;
        }

        public List<string> IE_DepartMent_GetMode(string Depart_No)
        {
            List<string> list = new List<string>();
            DataTable dt = new DataTable();

            string sql = "SELECT DISTINCT Mode FROM dbo.IE_DepartMent  where Depart_No='" + Depart_No + "' ORDER BY Mode  ";
            dt = db.Execute(sql).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["Mode"].ToString());

            }

            return list;
        }
        public void IE_DepartMent_Delete(TIE_DepartMent iE_DepartMent, string opUser)
        {
            string sql = "";
            sql = "DELETE   IE_DepartMent WHERE Line ='" + iE_DepartMent.Line + "' AND Shift='" + iE_DepartMent.Shift + "' AND Depart_No='" + iE_DepartMent.Depart_No + "' AND Mode='" + iE_DepartMent.Mode + "'";
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report", "4", opUser, "", sql);
        }

        public List<TIE_DepartMent> IE_DepartMent_SavaData(TIE_DepartMent iE_DepartMent, string opUser)
        {
            List<TIE_DepartMent> list = new List<TIE_DepartMent>();
            string sql = "";

            list = IE_DepartMent_GetData(iE_DepartMent);
            if (list.Count == 0)
            {
                sql = "INSERT INTO dbo.IE_DepartMent( Line, Shift, Line_No, Depart_No, Mode,UserID,TransDateTime )" +
                            "VALUES('" + iE_DepartMent.Line + "','" + iE_DepartMent.Shift + "','" + iE_DepartMent.Line_No + "','" + iE_DepartMent.Depart_No + "','" + iE_DepartMent.Mode + "','" + opUser + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'))";
               
            }
            else
            {
                sql = "UPDATE IE_DepartMent SET Line_No='" + iE_DepartMent.Line_No + "',UserID='" + opUser + "',TransDateTime=dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS')  WHERE Line ='" + iE_DepartMent.Line + "' AND Shift='" + iE_DepartMent.Shift + "' AND Depart_No='" + iE_DepartMent.Depart_No + "' AND Mode='" + iE_DepartMent.Mode + "'";
            
            }
            db.Execute(sql);
            list = IE_DepartMent_GetData(iE_DepartMent);
            return list;
        }

        public List<TIE_DepartMent> IE_DepartMent_GetData(TIE_DepartMent iE_DepartMent)
        {
            List<TIE_DepartMent> list = new List<TIE_DepartMent>();
            DataTable dt = new DataTable();

            string sql = "SELECT row_number() over (order by Line),Line,	Shift,	Line_No	,Depart_No	,Mode,UserID,TransDateTime FROM dbo.IE_DepartMent where 1=1 ";
            if (iE_DepartMent.Line != "")
            {
                sql = sql + " and line = '" + iE_DepartMent.Line + "'";
            
            }
            if (iE_DepartMent.Shift != "")
            {
                sql = sql + " and Shift = '" + iE_DepartMent.Shift + "'";

            }
            //if (iE_DepartMent.Line_No != "")
            //{
            //    sql = sql + " and Line_No = '" + iE_DepartMent.Line_No + "'";

            //}
            if (iE_DepartMent.Depart_No != "")
            {
                sql = sql + " and Depart_No = '" + iE_DepartMent.Depart_No + "'";

            }
            if (iE_DepartMent.Mode != "")
            {
                sql = sql + " and Mode = '" + iE_DepartMent.Mode + "'";

            }

            sql = sql + " ORDER BY Line";
            dt = db.Execute(sql).Tables[0];
            list = DataTableTool.ToList<TIE_DepartMent>(dt);
            
            return list;
        }
        #endregion IE_DepartMent

        #region IE_STD_ManHour_NextMonth 
        public List<TIE_STD_ManHour_NextMonth> IE_STD_ManHour_NextMonth_GetSaveData1(TIE_STD_ManHour_NextMonth iE_STD_ManHour, string opUser)
        {
            List<TIE_STD_ManHour_NextMonth> list = new List<TIE_STD_ManHour_NextMonth>();

            string sql = "";

            list = TIE_STD_ManHour_NextMonth_GetData("", "", iE_STD_ManHour.Mode, iE_STD_ManHour.Line, iE_STD_ManHour.Model);
            if (list.Count > 0)
            {
                sql = "update IE_STD_ManHour_NextMonth set factory = '" + iE_STD_ManHour.Factory + "', manhour='" + iE_STD_ManHour.ManHour + "',transdatetime=dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),Online_Man='" + iE_STD_ManHour.Online_Man + "'," +
                       "Offline_Man='" + iE_STD_ManHour.Offline_Man + "',Share_Rate='" + iE_STD_ManHour.Share_Rate + "',cycletime='" + iE_STD_ManHour.CycleTime + "'," +
                       "username='" + opUser + "',Remarks=N'" + iE_STD_ManHour.Remarks + "',BU='" + iE_STD_ManHour.BU + "'" +
               " where  model='" + iE_STD_ManHour.Model + "' and line='" + iE_STD_ManHour.Line + "' and mode='" + iE_STD_ManHour.Mode + "'";

            }
            else
            {
                sql = "insert into IE_STD_ManHour_NextMonth (factory,bu,model,line,manhour,mode,Online_Man,Offline_Man,Share_Rate,CycleTime,Remarks,transdatetime,username) values" +
                        "('" + iE_STD_ManHour.Factory + "','" + iE_STD_ManHour.BU + "','" + iE_STD_ManHour.Model + "','" + iE_STD_ManHour.Line + "','" + iE_STD_ManHour.ManHour + "'," +
                        "'" + iE_STD_ManHour.Mode + "','" + iE_STD_ManHour.Online_Man + "','" + iE_STD_ManHour.Offline_Man + "','" + iE_STD_ManHour.Share_Rate + "','" + iE_STD_ManHour.CycleTime + "',N'" + iE_STD_ManHour.Remarks + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),'" + opUser + "')";
            }
            db.Execute(sql);

            list = TIE_STD_ManHour_NextMonth_GetData(iE_STD_ManHour.Factory, iE_STD_ManHour.BU, iE_STD_ManHour.Mode, iE_STD_ManHour.Line, iE_STD_ManHour.Model);
            return list;
        }
        public List<TIE_STD_ManHour_NextMonth> IE_STD_ManHour_NextMonth_GetSaveData2(TIE_STD_ManHour_NextMonth iE_STD_ManHour, string opUser)
        {
            List<TIE_STD_ManHour_NextMonth> list = new List<TIE_STD_ManHour_NextMonth>();

            string sql = "";

            list = TIE_STD_ManHour_NextMonth_GetData("", "", iE_STD_ManHour.Mode, iE_STD_ManHour.Line, iE_STD_ManHour.Model);
            if (list.Count > 0)
            {
                sql = "update IE_STD_ManHour_NextMonth set factory = '" + iE_STD_ManHour.Factory + "', manhour='" + iE_STD_ManHour.ManHour + "',transdatetime=dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),Online_Man='" + iE_STD_ManHour.Online_Man + "'," +
                       "Offline_Man='" + iE_STD_ManHour.Offline_Man + "',Share_Rate='" + iE_STD_ManHour.Share_Rate + "',cycletime='" + iE_STD_ManHour.CycleTime + "'," +
                       "username='" + opUser + "',Remarks=N'" + iE_STD_ManHour.Remarks + "',BU='" + iE_STD_ManHour.BU + "'" +
               " where  model='" + iE_STD_ManHour.Model + "' and line='" + iE_STD_ManHour.Line + "' and mode='" + iE_STD_ManHour.Mode + "'";

            }
            else
            {
                sql = "insert into IE_STD_ManHour_NextMonth (factory,bu,model,line,manhour,mode,Online_Man,Offline_Man,Share_Rate,CycleTime,Remarks,transdatetime,username) values" +
                        "('" + iE_STD_ManHour.Factory + "','" + iE_STD_ManHour.BU + "','" + iE_STD_ManHour.Model + "','" + iE_STD_ManHour.Line + "','" + iE_STD_ManHour.ManHour + "'," +
                        "'" + iE_STD_ManHour.Mode + "','" + iE_STD_ManHour.Online_Man + "','" + iE_STD_ManHour.Offline_Man + "','" + iE_STD_ManHour.Share_Rate + "','" + iE_STD_ManHour.CycleTime + "',N'" + iE_STD_ManHour.Remarks + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),'" + opUser + "')";
            }
            db.Execute(sql);

            list = TIE_STD_ManHour_NextMonth_GetData(iE_STD_ManHour.Factory, iE_STD_ManHour.BU, iE_STD_ManHour.Mode, iE_STD_ManHour.Line, iE_STD_ManHour.Model);
            return list;
        }
        public void IE_STD_ManHour_NextMonth_Detele(TIE_STD_ManHour_NextMonth iE_STD_ManHour, string opUser)
        {

            string sql = "";
            sql = "delete from IE_STD_ManHour_NextMonth where  model = '" + iE_STD_ManHour.Model + "' " +
                        "and line = '" + iE_STD_ManHour.Line + "' and mode = '" + iE_STD_ManHour.Mode + "'";
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report", "4", opUser, "", sql);


        }
        public List<TIE_STD_ManHour_NextMonth> TIE_STD_ManHour_NextMonth_GetData(string factory, string pu, string mode, string line, string model)
        {
            List<TIE_STD_ManHour_NextMonth> list = new List<TIE_STD_ManHour_NextMonth>();
            DataTable dt = null;

            string sql = "SELECT row_number() over (order by Model) as ID,  Factory, Mode, BU, Line, Model, ManHour, UserName, Online_Man, Offline_Man, Share_Man, CycleTime, Share_Rate, STD_Manpower, Output_8Hrs, Output_12Hrs, Balance_Efficiency, Issue_Date, Remarks ,TransDateTime" +
                            " FROM dbo.IE_STD_ManHour_NextMonth with(nolock) WHERE 1 = 1  ";
            if (factory != "")
            {
                sql = sql + "AND Factory = '" + factory + "'";

            }
            if (pu != "")
            {
                sql = sql + " AND BU='" + pu + "' ";
            }
            if (mode != "")
            {
                sql = sql + " AND Mode ='" + mode + "'";

            }
            if (line != "")
            {
                sql = sql + " AND Line='" + line + "'";

            }
            if (model != "")
            {
                sql = sql + " AND Model='" + model + "'";

            }
            sql = sql + "  ORDER BY TransDateTime DESC ";
            dt = db.Execute(sql).Tables[0];

            list = DataTableTool.ToList<TIE_STD_ManHour_NextMonth>(dt);

            return list;

        }
        public List<string> TIE_STD_ManHour_NextMonth_GetLine(string factory, string pu, string mode)
        {
            List<string> LineList = new List<string>();
            DataTable dt = null;

            string sql = "SELECT DISTINCT Line  FROM dbo.IE_STD_ManHour_NextMonth with(nolock) WHERE Factory='" + factory +
                            "' AND BU='" + pu + "' AND Mode='" + mode + "' ORDER BY Line";
            dt = db.Execute(sql).Tables[0];
            int dtCount = dt.Rows.Count;

            for (int i = 0; i < dtCount; i++)
            {
                LineList.Add(dt.Rows[i]["Line"].ToString());

            }
            db.CloseDB();
            return LineList;
        }
        public List<string> TIE_STD_ManHour_NextMonth_GetMode(string factory, string pu)
        {
            List<string> ModeList = new List<string>();
            DataTable dt = null;

            string sql = "SELECT DISTINCT Mode  FROM dbo.IE_STD_ManHour_NextMonth with(nolock) WHERE Factory='" + factory +
                            "' AND BU='" + pu + "' ORDER BY Mode";
            dt = db.Execute(sql).Tables[0];
            int dtCount = dt.Rows.Count;

            for (int i = 0; i < dtCount; i++)
            {
                ModeList.Add(dt.Rows[i]["Mode"].ToString());

            }
            db.CloseDB();
            return ModeList;
        }
        public List<string> TIE_STD_ManHour_NextMonth_GetPU(string factory)
        {
            List<string> PUList = new List<string>();
            DataTable dt = null;

            string sql = "";

            sql = "SELECT DISTINCT BU  FROM dbo.IE_STD_ManHour_NextMonth with(nolock) WHERE Factory='" + factory + "' ORDER BY BU";

            dt = db.Execute(sql).Tables[0];
            int dtCount = dt.Rows.Count;

            for (int i = 0; i < dtCount; i++)
            {
                PUList.Add(dt.Rows[i]["BU"].ToString());

            }
            db.CloseDB();
            return PUList;
        }
        public List<string> TIE_STD_ManHour_NextMonth_GetFactory()
        {
            List<string> FactoryList = new List<string>();
            DataTable dt = null;
            string sql = "select distinct Factory from IE_STD_ManHour_NextMonth with(nolock) order by Factory";
            dt = db.Execute(sql).Tables[0];
            int dtCount = dt.Rows.Count;
            for (int i = 0; i < dtCount; i++)
            {
                FactoryList.Add(dt.Rows[i]["Factory"].ToString());

            }
            db.CloseDB();
            return FactoryList;
        }
  
  
        #endregion IE_STD_ManHour_NextMonth
        #region IE_STD_ManHour
        public List<TIE_STD_ManHour> IE_STD_ManHour_GetSaveData1(TIE_STD_ManHour iE_STD_ManHour,string opUser)
        {
            List<TIE_STD_ManHour> list = new List<TIE_STD_ManHour>();
          
            string sql = "";

            list = TIE_STD_ManHour_GetData("", "",iE_STD_ManHour.Mode,iE_STD_ManHour.Line,iE_STD_ManHour.Model);
            if (list.Count > 0)
            {
                 sql = "update IE_STD_ManHour set factory = '"+ iE_STD_ManHour.Factory+"', manhour='" + iE_STD_ManHour.ManHour + "',transdatetime=dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),Online_Man='" + iE_STD_ManHour.Online_Man + "'," +
                        "Offline_Man='" + iE_STD_ManHour.Offline_Man + "',Share_Rate='" + iE_STD_ManHour.Share_Rate + "',cycletime='" + iE_STD_ManHour.CycleTime + "'," +
                        "username='" + opUser + "',Remarks=N'" + iE_STD_ManHour.Remarks + "',BU='" + iE_STD_ManHour.BU + "'" +
                " where  model='" + iE_STD_ManHour.Model + "' and line='" + iE_STD_ManHour.Line + "' and mode='" + iE_STD_ManHour.Mode + "'";

            }
            else
            {
                sql = "insert into IE_STD_ManHour (factory,bu,model,line,manhour,mode,Online_Man,Offline_Man,Share_Rate,CycleTime,Remarks,transdatetime,username) values" +
                        "('" + iE_STD_ManHour.Factory + "','" + iE_STD_ManHour.BU + "','" + iE_STD_ManHour.Model + "','" + iE_STD_ManHour.Line + "','" + iE_STD_ManHour.ManHour + "'," +
                        "'" + iE_STD_ManHour.Mode + "','" + iE_STD_ManHour.Online_Man + "','" + iE_STD_ManHour.Offline_Man + "','" + iE_STD_ManHour.Share_Rate + "','" + iE_STD_ManHour.CycleTime + "',N'" + iE_STD_ManHour.Remarks + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),'" + opUser + "')";
            }
            db.Execute(sql);

            list = TIE_STD_ManHour_GetData(iE_STD_ManHour.Factory, iE_STD_ManHour.BU, iE_STD_ManHour.Mode, iE_STD_ManHour.Line, iE_STD_ManHour.Model);
            return list;
        }


        public void IE_STD_ManHour_GetSaveData2(TIE_STD_ManHour iE_STD_ManHour, string opUser)
        {
            List<TIE_STD_ManHour> list = new List<TIE_STD_ManHour>();

            string sql = "";

            list = TIE_STD_ManHour_GetData("", "", iE_STD_ManHour.Mode, iE_STD_ManHour.Line, iE_STD_ManHour.Model);
            if (list.Count > 0)
            {
                sql = "update IE_STD_ManHour set Issue_Date = '" + iE_STD_ManHour.Issue_Date + "', Balance_Efficiency = '" + iE_STD_ManHour.Balance_Efficiency +
                    "',  Output_12Hrs = '" + iE_STD_ManHour.Output_12Hrs + "', Output_8Hrs = '" + iE_STD_ManHour.Output_8Hrs + "', STD_Manpower ='" + iE_STD_ManHour.STD_Manpower + 
                        "', Share_Man = '" + iE_STD_ManHour.Share_Man + "', factory = '" + iE_STD_ManHour.Factory + 
                        "',manhour='" + iE_STD_ManHour.ManHour + "',transdatetime=dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),Online_Man='" + iE_STD_ManHour.Online_Man + "'," +
                        "Offline_Man='" + iE_STD_ManHour.Offline_Man + "',Share_Rate='" + iE_STD_ManHour.Share_Rate + "',cycletime='" + iE_STD_ManHour.CycleTime + "'," +
                        "username='" + opUser + "',Remarks=N'" + iE_STD_ManHour.Remarks + "',BU='" + iE_STD_ManHour.BU + "'" +
                " where  model='" + iE_STD_ManHour.Model + "' and line='" + iE_STD_ManHour.Line + "' and mode='" + iE_STD_ManHour.Mode + "'";
            }
            else
            {
                sql = "insert into IE_STD_ManHour (factory,bu,model,line,manhour,mode,Online_Man,Offline_Man,Share_Rate,CycleTime,Remarks,transdatetime,username,STD_Manpower,Output_8Hrs,Output_12Hrs,Balance_Efficiency,Issue_Date,Share_Man) values" +
                        "('" + iE_STD_ManHour.Factory + "','" + iE_STD_ManHour.BU + "','" + iE_STD_ManHour.Model + "','" + iE_STD_ManHour.Line + "','" + iE_STD_ManHour.ManHour + "'," +
                        "'" + iE_STD_ManHour.Mode + "','" + iE_STD_ManHour.Online_Man + "','" + iE_STD_ManHour.Offline_Man + "','" + iE_STD_ManHour.Share_Rate + "','" + iE_STD_ManHour.CycleTime + 
                        "',N'" + iE_STD_ManHour.Remarks +"',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),'" + opUser + 
                        "',   '"+iE_STD_ManHour.STD_Manpower+"','"+iE_STD_ManHour.Output_8Hrs+"','"+iE_STD_ManHour.Output_12Hrs+"','"+iE_STD_ManHour.Balance_Efficiency+"','"+iE_STD_ManHour.Issue_Date+"','"+iE_STD_ManHour.Share_Man+"')" ;
            }
            db.Execute(sql);

            //list = TIE_STD_ManHour_GetData(iE_STD_ManHour.Factory, iE_STD_ManHour.BU, iE_STD_ManHour.Mode, iE_STD_ManHour.Line, iE_STD_ManHour.Model);
            
        }


        public void IE_STD_ManHour_Detele(TIE_STD_ManHour iE_STD_ManHour, string opUser)
        { 

            string sql = "";
            sql = "delete from IE_STD_ManHour where  model = '" + iE_STD_ManHour.Model + "' " +
                        "and line = '" + iE_STD_ManHour.Line + "' and mode = '" + iE_STD_ManHour.Mode + "'"; 
            db.Execute(sql);
            db.SaveQMSLog("MVC_PUB_Report", "4", opUser, "", sql);

             
        }

        public List<TIE_STD_ManHour> TIE_STD_ManHour_GetData(string factory, string pu, string mode, string line, string model)
        {
            List<TIE_STD_ManHour> list = new List<TIE_STD_ManHour>();
            DataTable dt = null;

            string sql = "SELECT row_number() over (order by Model) as ID,  Factory, Mode, BU, Line, Model, ManHour, UserName, Online_Man, Offline_Man, Share_Man, CycleTime, Share_Rate, STD_Manpower, Output_8Hrs, Output_12Hrs, Balance_Efficiency, Issue_Date, Remarks ,TransDateTime" +
                            " FROM dbo.IE_STD_ManHour with(nolock) WHERE 1 = 1  ";
            if (factory != "")
            {
                sql = sql + "AND Factory = '" + factory + "'";
            
            }
            if (pu != "")
            {
                sql = sql + " AND BU='" + pu + "' ";
            }
            if (mode != "")
            {
                sql = sql + " AND Mode ='" + mode + "'";
            
            }
            if (line != "")
            {
                sql = sql + " AND Line='"+ line +"'";

            }
            if (model != "")
            {
                sql = sql + " AND Model='" + model + "'";

            }
            sql = sql + "  ORDER BY TransDateTime DESC ";
            dt = db.Execute(sql).Tables[0];

            list = DataTableTool.ToList<TIE_STD_ManHour>(dt);

            return list;
        
        }
         
        public List<string> TIE_STD_ManHour_GetLine(string factory, string pu,string mode)
        {
            List<string> LineList = new List<string>();
            DataTable dt = null;

            string sql = "SELECT DISTINCT Line  FROM dbo.IE_STD_ManHour with(nolock) WHERE Factory='" + factory +
                            "' AND BU='" + pu + "' AND Mode='" + mode + "' ORDER BY Line";
            dt = db.Execute(sql).Tables[0];
            int dtCount = dt.Rows.Count;

            for (int i = 0; i < dtCount; i++)
            {
                LineList.Add(dt.Rows[i]["Line"].ToString());

            }
            db.CloseDB();
            return LineList;
        }


        public List<string> TIE_STD_ManHour_GetMode(string factory, string pu)
        {
            List<string> ModeList = new List<string>();
            DataTable dt = null;

            string sql = "SELECT DISTINCT Mode  FROM dbo.IE_STD_ManHour with(nolock) WHERE Factory='" + factory +
                            "' AND BU='" + pu + "' ORDER BY Mode";
            dt = db.Execute(sql).Tables[0];
            int dtCount = dt.Rows.Count;

            for (int i = 0; i < dtCount; i++)
            {
                ModeList.Add(dt.Rows[i]["Mode"].ToString());

            }
            db.CloseDB();
            return ModeList;
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="type">1 By Factory 查询，2By Model 查询</param>
        /// <returns></returns>
        public List<string> TIE_STD_ManHour_GetPU(string factory)
        {
            List<string> PUList = new List<string>();
            DataTable dt = null;

            string sql = "";

            sql = "SELECT DISTINCT BU  FROM dbo.IE_STD_ManHour with(nolock) WHERE Factory='" + factory + "' ORDER BY BU";
           
            dt = db.Execute(sql).Tables[0];
            int dtCount = dt.Rows.Count;

            for (int i = 0; i < dtCount; i++)
            {
                PUList.Add(dt.Rows[i]["BU"].ToString());

            }
            db.CloseDB();
            return PUList;
        }
        
        public List<string> TIE_STD_ManHour_GetFactory()
        {
            List<string> FactoryList = new List<string>();
            DataTable dt = null;
            string sql = "select distinct Factory from IE_STD_ManHour with(nolock) order by Factory";
            dt = db.Execute(sql).Tables[0];
            int dtCount = dt.Rows.Count;
            for (int i = 0; i < dtCount; i++)
            {
                FactoryList.Add(dt.Rows[i]["Factory"].ToString());

            }
            db.CloseDB();
            return FactoryList;
        }


        #endregion IE_STD_ManHour
        #region IE_ModelStage
 
        /// <summary>
        /// 页面加载时查Mode资料，以便加载页面提供给User选择
        /// </summary>
        /// <returns></returns>
        public List<string> IE_ModelStage_GetMode()
        {
            List<string> ModeList = new List<string>();
            DataTable dt = null;

            string sql = "EXEC [MVC_PUBReport_IE_ModelStage] 1";
            dt = db.Execute(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ModeList.Add(dt.Rows[i]["Mode"].ToString());

            }
            db.CloseDB();
            return ModeList;

        }

        

        /// <summary>
        /// 处理资料，Type = 2 表示save update ；3表示查询资料，4表示删除资料
        /// </summary>
        /// <param name="IE_ModelStage"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public List<TIE_ModelStage> IE_ModelStage_ByOpType(TIE_ModelStage IE_ModelStage, int opType, string UserID)
        // public string IE_ModelStage_ByOpType(TIE_ModelStage IE_ModelStage, int Type, string UserID)
        {
            DataTable dt = null;

            //SqlParameter[] paraList = {
            //                                db.CreateInParam("@OpType",SqlDbType.VarChar,1,opType),
            //                                db.CreateInParam("@UserName",SqlDbType.VarChar,20,UserID),
            //                                db.CreateInParam("@Mode",SqlDbType.VarChar,30,IE_ModelStage.Mode),
            //                                db.CreateInParam("@Model",SqlDbType.VarChar,20,IE_ModelStage.Model),
            //                                db.CreateInParam("@Section",SqlDbType.VarChar,10,IE_ModelStage.Section),
            //                                db.CreateInParam("@Stage",SqlDbType.VarChar,20,IE_ModelStage.Stage)
            //                            };

            //dt = db.Execute("MVC_PUBReport_IE_ModelStage", paraList).Tables[0];
            string strsql = "exec MVC_PUBReport_IE_ModelStage '" + opType + "','" + UserID + "','" + IE_ModelStage.Mode + "','" + IE_ModelStage.Model + "','" + IE_ModelStage.Section + "','" + IE_ModelStage.Stage + "'";
            dt = db.Execute(strsql).Tables[0];
            List<TIE_ModelStage> list = new List<TIE_ModelStage>();

            if (opType == 4)
            {
                string sqlLog = "MVC_PUBReport_IE_ModelStage " + IE_ModelStage.Tostring(IE_ModelStage);
                db.SaveQMSLog("MVC_PUB_Report", opType.ToString(), UserID, "", sqlLog);
                return list;
            }
            //for (int i = 0; i < rowCount; i++)
            //{
            //    TIE_ModelStage IE_ModelStageTemp = new TIE_ModelStage();
            //    IE_ModelStageTemp.ID = dt.Rows[i]["ID"].ToString();
            //    IE_ModelStageTemp.Mode = dt.Rows[i]["Mode"].ToString();
            //    IE_ModelStageTemp.Model = dt.Rows[i]["Model"].ToString();
            //    IE_ModelStageTemp.Section = dt.Rows[i]["Section"].ToString();

            //    IE_ModelStageTemp.Stage = dt.Rows[i]["Stage"].ToString();
            //    IE_ModelStageTemp.UserName = dt.Rows[i]["UserName"].ToString();
            //    IE_ModelStageTemp.TransDateTime = dt.Rows[i]["TransDateTime"].ToString();

            //    list.Add(IE_ModelStageTemp);
            //}
            list = DataTableTool.ToList<TIE_ModelStage>(dt);
            db.CloseDB();

            return list;
        }

        #endregion IE_ModelStage
        #region IE_Line_Shift
        public List<TIE_Line_Shift> IE_Line_Shift_View(TIE_Line_Shift ie)
        {
            List<TIE_Line_Shift> list = new List<TIE_Line_Shift>();
            DataTable dt = new DataTable();
       
            string sql = "";
 
           
            sql = "select PU,Line,ShiftNo,Trans_DateTime,UserID from IE_Line_Shift where 1 = 1";

            //if (String.IsNullOrEmpty(ie.PU) == false)
            //{
            //    sql = sql + " AND PU = '" + ie.PU + "'";
            //}

            if (String.IsNullOrEmpty( ie.Line) == false  )
            {
                sql = sql + " AND Line = '" + ie.Line + "'";
            }
            sql = sql + "ORDER BY LINE,SHIFTNO";
            dt = db.Execute(sql).Tables[0];//sf db.Tables[0];

            if (dt.Rows.Count == 0)
            {
                dt = db.Execute(sql).Tables[0];
            }
            list = DataTableTool.ToList<TIE_Line_Shift>(dt);
             

            return list;
        
        }

        public void IE_Line_Shift_Delete( TIE_Line_Shift ie, string opUser)
        {
         
            string sql = "";         

            sql = "Delete from IE_Line_Shift Where   Line='" + ie.Line + "'";

            //sfdb.Execute(sql);//sf db
            db.Execute(sql); //5.13

            //db.SaveQMSLog("MVC_PUB_Report", "4", opUser, "", sql);

        }

        public void IE_Line_Shift_Save( TIE_Line_Shift ie, string opUser)
        {
            List<TIE_Line_Shift> list = new List<TIE_Line_Shift>();
            string sql = "";

            list = IE_Line_Shift_View( ie);

            if (list.Count > 0)
            {

                IE_Line_Shift_Delete( ie, opUser);
            }

            sql = " Insert Into IE_Line_Shift (PU,Line,ShiftNO,Trans_DateTime,UserID)" +
                      " Values('" + ie.PU + "','" + ie.Line + "','" + ie.ShiftNo + "',dbo.FormatDate(GETDATE(),'YYYYMMDDHHNNSS'),'" + opUser + "')";

            db.Execute(sql);

            //sfdb.Execute(sql);//sf db
        
        }
        #endregion IE_Line_Shift

        #region IE_ModelCheck
        public List<TIE_ModelCheck> IE_ModelCheck_View(TIE_ModelCheck ie)
        {
            List<TIE_ModelCheck> list = new List<TIE_ModelCheck>();
            DataTable dt = new DataTable();

            string sql = "";


            sql = "exec [IE_ModelCheck] '"+ie.TransDate+"','"+ie.Mode+"','FormQuery'";

             
            dt = db.Execute(sql).Tables[0];//sf db.Tables[0];

            if (dt.Rows.Count == 0)
            {
                dt = db.Execute(sql).Tables[0];
            }
            list = DataTableTool.ToList<TIE_ModelCheck>(dt);


            return list;

        }
        #endregion IE_ModelCheck

    }
}