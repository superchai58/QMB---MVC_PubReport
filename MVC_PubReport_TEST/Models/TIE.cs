using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_PubReport.Models.IE
{
    public class TIE
    {
    }

    public class TIE_ModelStage
    {
        public int ID { get; set; }
        public string Mode { get; set; }

        public string Model { get; set; }

        public string Section { get; set; }

        public string Stage { get; set; } 

        public string UserName { get; set; }
        public string TransDateTime { get; set; }

        public string Tostring(TIE_ModelStage model)
        {
            string str = "Mode=" + model.Mode + ",Model=" + model.Model + ",Section=" + model.Section + ",Stage=" + model.Stage;
            return str;
        }
    }

    public class TIE_STD_ManHour {
        public Int64 ID{get;set;}
        public string  Factory{ get; set; }  
        public string  Mode{ get; set; } 
        public string BU{ get; set; } 
        public string Line{ get; set; } 
        public string Model{ get; set; } 
        public double ManHour{ get; set; } 
        public string TransDateTime{ get; set; }
        public string UserName { get; set; }
        public double Online_Man { get; set; }
        public double Offline_Man { get; set; }
        public double Share_Man { get; set; }
        public double CycleTime { get; set; }
        public double Share_Rate { get; set; }
        public double STD_Manpower { get; set; }
        public Int64  Output_8Hrs { get; set; }
        public Int64 Output_12Hrs { get; set; }
        public string Balance_Efficiency { get; set; }
        public string Issue_Date { get; set; }
        public string Remarks { get; set; }
    
    }
    public class TIE_STD_ManHour_NextMonth
    {
        public Int64 ID { get; set; }
        public string Factory { get; set; }
        public string Mode { get; set; }
        public string BU { get; set; }
        public string Line { get; set; }
        public string Model { get; set; }
        public double ManHour { get; set; }
        public string TransDateTime { get; set; }
        public string UserName { get; set; }
        public double Online_Man { get; set; }
        public double Offline_Man { get; set; }
        public double Share_Man { get; set; }
        public double CycleTime { get; set; }
        public double Share_Rate { get; set; }
        public double STD_Manpower { get; set; }
        public Int64 Output_8Hrs { get; set; }
        public Int64 Output_12Hrs { get; set; }
        public string Balance_Efficiency { get; set; }
        public string Issue_Date { get; set; }
        public string Remarks { get; set; }

    }
    public class TIE_DepartMent
    {
        public string Line { get; set; }
        public string Shift { get; set; }
        public string Line_No{ get; set; }
        public string Depart_No{ get; set; }
        public string Mode { get; set; }
        public string UserID{ get; set; }
        public string TransDateTime { get; set; }
        public Int64 ID { get; set; }
    }

    public class TIE_DailyProductQty
    { 
        public string Mode { get; set; }
        public string TransDate { get; set; }
        public string Line { get; set; }
        public string Shift{ get; set; }
        public string Model{ get; set; }
        public string WO{ get; set; }
        public Int64  QTY{get;set;}
        public string SourcePU{get;set;}	
        public string InsertDateTime{ get; set; }
        public string PN { get; set; }

    
    }

    public class TIE_InputQty { 
        public string Mode{ get; set; }
        public string TransDate{ get; set; }
        public string Line{ get; set; }
        public string Shift{ get; set; }
        public string Model{ get; set; }
        public string WO{ get; set; }
        public Int64 QTY{ get; set; }
        public string SourcePU{ get; set; }
        public string InsertDateTime{ get; set; }
        public string PN { get; set; }
    
    }

    public class TIE_InputQty_TestWO
    {
        public string Mode { get; set; }
        public string TransDate { get; set; }
        public string Line { get; set; }
        public string Shift { get; set; }
        public string Model { get; set; }
        public string WO { get; set; }
        public Int64 QTY { get; set; }
        public string SourcePU { get; set; }
        public string InsertDateTime { get; set; }
        public string PN { get; set; }
        public string Stage { get; set; }
    
    }


    public class TIE_BaseFactor {     
        public string Mode{ get; set; }
        public string Line{ get; set; }
        public string Section{ get; set; }
        public string Stage{ get; set; }
        public double Base{ get; set; }
        public double Factor{ get; set; }
        public Int64 CountTimes { get; set; }
        public Int64 ID { get; set; }
        public string UserName { get; set; }
        public string TransDateTime { get; set; }
    }

    public class TIE_FQALineMapping
    { 
        public string  FQA_Line{ get; set; }
        public string FQA_Shift{ get; set; }
        public string FQA_Line_No{ get; set; }
        public string Depart_No{ get; set; }
        public string Mode{ get; set; }
        public string Line{ get; set; }
        public string Shift{ get; set; }
        public string Line_No{ get; set; }
        public string UserID{ get; set; }
        public string TransDateTime { get; set; }
        public Int64 ID { get; set; }
    
    }

    public class TEfficiency
    {
        public string Mode { get; set; }
        public string WorkDate { get; set; }
        public string Line { get; set; }
        public string Shift { get; set; }
        public decimal WKTM { get; set; } //实勤工时
        public decimal ExCTM { get; set; } //异常工时
        public decimal DOTM { get; set; } //实作工时
        public Int64 OutPut { get; set; }// 产出数量
        public decimal OutPutHour { get; set; }// 产出工时
        public string InsertDateTime { get; set; }
    
    }

    public class TIE_Line_Shift
    { 
        public string PU{ get; set; }
        public string Line{ get; set; }
        public string ShiftNo{ get; set; }
        public string Trans_DateTime{ get; set; }
        public string UserID { get; set; }
    
    }


    public class TIE_ModelCheck
    {
        public string Mode { get; set; }
        public string TransDate { get; set; }
        public string Line { get; set; }
        public string Shift { get; set; }
        public string Model { get; set; }
        public string ModelBu { get; set; }
        public string QTY { get; set; }
        
    }

}