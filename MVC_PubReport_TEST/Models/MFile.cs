using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
 

namespace MVC_PubReport.Models.Files
{
    public class MFile
    {
        private string _filename;
        private string _filetype;
        private string _fileextension;
        private string _filerename;
        private string _uploadpath;

        public string FileName
        {
            set { _filename = value; }
            get { return _filename; }
        }

        public string FileType
        {
            set { _filetype = value; }
            get { return _filetype; }
        }


        public string FileExtension
        {
            set { _fileextension = value; }
            get { return _fileextension; }
        }

        public string FileReName
        {
            set { _filerename = value; }
            get { return _filerename; }
        }

        public string UploadPath
        {
            set { _uploadpath = value; }
            get { return _uploadpath; }
        }

 

        public static MFile UpLoadFile(HttpPostedFileBase InputFile, string strFilePath, string userName, ref string result)
        {
            string fileExtension = System.IO.Path.GetExtension(System.IO.Path.GetFileName(InputFile.FileName));
            //string fileName = strTable + "_" + userName + fileExtension;
            string fileName = userName + "_" + InputFile.FileName;
            string filePath = HttpContext.Current.Request.MapPath(strFilePath);

            filePath = filePath + fileName;
            try
            {
                InputFile.SaveAs(filePath);
            }
            catch (Exception ee)
            {
                result = "FAIL";
                throw new ApplicationException("上传失败!" + ee.Message);
            }

            MFile mfile = new MFile();
            mfile.UploadPath = filePath;
            mfile.FileReName = fileName;
            result = "OK";
            return mfile;
        }


         



     /// <summary>
     /// sheetName = "" 就读第一个sheet,isHeader = Y表示Excel里面有表头
     /// </summary>
     /// <param name="file"></param>
     /// <param name="startRow"></param>
     /// <param name="sheetName"></param>
     /// <param name="isHeader"></param>
     /// <returns></returns>
        public static DataTable ExcelToTable(string file,int startRow,string sheetName,string isHeader)
        {
            DataTable dt = new DataTable();
            IWorkbook workbook;
            string fileExt = Path.GetExtension(file).ToLower();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                if (fileExt == ".xlsx") 
                { 
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileExt == ".xls") 
                { 
                    workbook = new HSSFWorkbook(fs); 
                } 
                else 
                { 
                    workbook = null; 
                }
                if (workbook == null) 
                { 
                    return null;
                }
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheetName != "") // 可指定sheetName
                {
                    sheet = workbook.GetSheet(sheetName);
                }

                //表头  
                //IRow header = sheet.GetRow(sheet.FirstRowNum);
                IRow header = sheet.GetRow(startRow);
                //if (startRow != 1) // 指定开始行，默认开始行是第一行
                //{
                //    header = sheet.GetRow(startRow);
                //}

                List<int> columns = new List<int>();
                //Warinya Add condition for upload IE Define line Start 20230630
                //for (int i = 0; i <= header.LastCellNum; i++) //superchai add condition (<=) 20230601
                //{
                //    object obj = GetValueType(header.GetCell(i));
                //    //dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                //    if (isHeader == "N")
                //    {
                //        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                //    }
                //    else
                //        dt.Columns.Add(new DataColumn(obj.ToString()));
                //    columns.Add(i);
                //}

                //if (file.Contains("IE_DepartMent"))
                //{
                //    for (int i = 0; i < header.LastCellNum; i++) 
                //    {
                //        object obj = GetValueType(header.GetCell(i));
                //        //dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                //        if (isHeader == "N")
                //        {
                //            dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                //        }
                //        else
                //            dt.Columns.Add(new DataColumn(obj.ToString()));
                //        columns.Add(i);
                //    }
                //}
                //else
                //{
                //20230825 comment IE define use same of PMC [NT]
                for (int i = 0; i < header.LastCellNum; i++)
                {
                    object obj = GetValueType(header.GetCell(i));
                    //dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                    if (isHeader == "N")
                    {
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                    }
                    else
                        dt.Columns.Add(new DataColumn(obj.ToString()));
                    columns.Add(i);
                }
                //}
                //Warinya Add condition for upload IE Define line End 20230630

                //数据  
                for (int i = startRow + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    bool hasValue = false;
                    foreach (int j in columns)
                    {
                        dr[j] = GetValueType(sheet.GetRow(i).GetCell(j));
                        if (dr[j] != null && dr[j].ToString() != string.Empty)
                        {
                            hasValue = true;
                        }
                    }
                    if (hasValue)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            // 最后删掉传到服务器上的文件
            File.Delete(file);
            return dt;
        }

        /// <summary>
        /// Datable导出成Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file">导出路径(包括文件名与扩展名)</param>
        public static void TableToExcel(DataTable dt, string file)
        {
            IWorkbook workbook;
            string fileExt = Path.GetExtension(file).ToLower();
            if (fileExt == ".xlsx") { workbook = new XSSFWorkbook(); } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(); } else { workbook = null; }
            if (workbook == null) { return; }
            ISheet sheet = string.IsNullOrEmpty(dt.TableName) ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet(dt.TableName);

            //表头  
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            //数据  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            //转为字节数组  
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件  
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }

        /// <summary>
        /// 获取单元格类型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueType(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:  
                    return null;
                case CellType.Boolean: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:  
                    return cell.NumericCellValue;
                case CellType.String: //STRING:  
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA: 
                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");

                    }
                    else  
                    {

                        return cell.NumericCellValue.ToString();
                    }
                default:
                    return "=" + cell.CellFormula;
            }
        }
    }

    
}