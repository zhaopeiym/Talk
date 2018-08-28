using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;

namespace Talk.NPOI
{
    public static class NPOIHelper
    {
        public static IWorkbook GetWorkbook(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bool isOldThan2007 = Path.GetExtension(filePath) == ".xls";
                return GetWorkbook(stream, isOldThan2007);
            }
        }

        public static IWorkbook GetWorkbook(FileStream stream, bool isOldThan2007)
        {
            if (isOldThan2007)
                return new HSSFWorkbook(stream);
            else
                return new XSSFWorkbook(stream);
            //            return isOldThan2007 ? (IWorkbook)new HSSFWorkbook(stream) : new XSSFWorkbook(stream);

        }

        #region 读取Excel

        public static DataTable GetDataTable(string filePath, int sheetIndex)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bool isOldThan2007 = Path.GetExtension(filePath) == ".xls";
                return GetDataTable(stream, isOldThan2007, sheetIndex);
            }
        }

        /// <summary>
        /// 读取Excel到DataTable
        /// </summary>
        /// <param name="filePath">Excel路径</param>
        /// <param name="headPoint">自定义表头（坐标，从0开始计数）</param>
        /// <param name="validRow">内容开始行（从0开始计数）</param>
        /// <param name="sheetIndex">读取第几个sheet</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string filePath, List<Point> headPoint, int validRow = 1, int sheetIndex = 0)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bool isOldThan2007 = Path.GetExtension(filePath) == ".xls";
                return GetDataTable(stream, isOldThan2007, headPoint, validRow, sheetIndex);
            }
        }

        public static DataTable GetDataTable(FileStream stream, bool isOldThan2007, int sheetIndex)
        {
            IWorkbook book = GetWorkbook(stream, isOldThan2007);
            return GetDataTable(book, sheetIndex);
        }

        public static DataTable GetDataTable(FileStream stream, bool isOldThan2007, List<Point> headPoint, int validRow = 1, int sheetIndex = 0)
        {
            IWorkbook book = GetWorkbook(stream, isOldThan2007);
            return GetDataTable(book, headPoint, validRow, sheetIndex);
        }

        /// <summary>
        /// Excel 转DataTable
        /// </summary>
        /// <param name="stream">Excel文件流</param>
        /// <param name="isOldThan2007">是否低于07版本</param>
        /// <returns></returns>
        public static List<DataTable> GetDataTables(FileStream stream, bool isOldThan2007, int validRow = 1)
        {
            IWorkbook book = GetWorkbook(stream, isOldThan2007);
            var list = new List<DataTable>();
            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                list.Add(GetDataTable(book, i));
            }
            return list;
        }

        /// <summary>
        /// Excel 转DataTable集合
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static List<DataTable> GetDataTables(string filePath, int validRow = 1)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bool isOldThan2007 = Path.GetExtension(filePath) == ".xls";
                return GetDataTables(stream, isOldThan2007);
            }
        }
        #endregion

        #region 写入Excle

        /// <summary>
        /// 将DataTable读到Excle的内存流
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="isOldThan2007">是否低于2007</param>
        /// <returns></returns>
        public static MemoryStream DataTableToExcel(DataTable dataTable, bool isOldThan2007)
        {
            IWorkbook book = dataTable.ToWorkbook(isOldThan2007);
            using (MemoryStream ms = new MemoryStream())
            {
                book.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }

        /// <summary>
        /// 将DataTable写入excel文件
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="filePath"></param>
        public static void DataTableToExcel(DataTable dataTable, string filePath)
        {
            if (dataTable == null)
                throw new Exception("dataTable不能为null");
            bool isOldThan2007 = Path.GetExtension(filePath) == ".xls";
            IWorkbook book = dataTable.ToWorkbook(isOldThan2007);
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                book.Write(fs);
            }
        }

        /// <summary>
        /// 保存结果到源Excel
        /// </summary>
        /// <param name="filePath">源Excel文件路径</param>
        /// <param name="savePath">要保存的路径</param>
        /// <param name="sheetIndex">第几个Sheet</param>
        /// <param name="columnIndex">第多少列</param>
        /// <param name="rowsResult">行 对应要写的注释</param>
        public static void SaveExcel(string filePath, string savePath, int sheetIndex, int columnIndex, Dictionary<int, string> rowsResult)
        {
            var entity = new List<SheetEntity>()
            {
                new SheetEntity() {
                   SheetIndex= sheetIndex,
                   ColumnIndex = columnIndex,
                   RowsResult = rowsResult
                }
            };
            SaveExcel(filePath, savePath, entity);
        }

        /// <summary>
        /// 保存结果到源Excel
        /// </summary>
        /// <param name="filePath">源Excel文件路径</param>
        /// <param name="savePath">要保存的路径</param>
        /// <param name="sheetEntity">要记录的结果相关信息</param>
        public static void SaveExcel(string filePath, string savePath, SheetEntity sheetEntity)
        {
            SaveExcel(filePath, savePath, new List<SheetEntity>() { sheetEntity });
        }

        /// <summary>
        /// 保存结果到源Excel
        /// </summary>
        /// <param name="savePath">要保存的路径</param>
        /// <param name="stream"></param>
        /// <param name="isOldThan2007">是否低于2007的版本</param>
        /// <param name="sheetEntity">要记录的结果相关信息</param>
        public static void SaveExcel(string savePath, FileStream stream, bool isOldThan2007, SheetEntity sheetEntity)
        {

            SaveExcel(savePath, stream, isOldThan2007, new List<SheetEntity>() { sheetEntity });
        }

        /// <summary>
        /// 保存结果到源Excel
        /// </summary>
        /// <param name="filePath">源Excel文件路径</param>
        /// <param name="savePath">要保存的路径</param>        
        /// <param name="sheetEntityList">要记录的结果相关信息</param>
        public static void SaveExcel(string filePath, string savePath, List<SheetEntity> sheetEntityList)
        {
            var book = GetWorkbook(filePath);
            SaveExcel(savePath, book, sheetEntityList);
        }

        /// <summary>
        /// 保存结果到源Excel
        /// </summary>
        /// <param name="savePath">要保存的路径</param>
        /// <param name="stream"></param>
        /// <param name="isOldThan2007">是否低于2007的版本</param>   
        /// <param name="sheetEntityList">要记录的结果相关信息</param>
        public static void SaveExcel(string savePath, FileStream stream, bool isOldThan2007, List<SheetEntity> sheetEntityList)
        {
            var book = GetWorkbook(stream, isOldThan2007);
            SaveExcel(savePath, book, sheetEntityList);
        }

        #endregion

        #region private
        /// <summary>
        /// Excel转成DataTable
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="isOldThan2007">excel版本是否低于2007</param>
        /// <returns></returns>
        private static DataTable GetDataTable(IWorkbook book, int index, int validRow = 1)
        {
            DataTable table = new DataTable();
            ISheet sheet = book.GetSheetAt(index);
            IRow headerRow = sheet.GetRow(0);//读取第一行（头）
            if (headerRow == null)
                return null;
            //book.NumberOfSheets;

            //列头
            int cellCount = headerRow.LastCellNum;
            for (int i = 0; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).ToString());
                table.Columns.Add(column);
            }

            //行内容
            for (int i = validRow; i < sheet.LastRowNum - 1; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                    continue;//TODO :                
                DataRow dataRow = table.NewRow();
                //DTDO：有些行 大于  table.Columns.Count 所以不能用row.LastCellNum
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    dataRow[j] = row.GetValue(j);
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }

        /// <summary>
        ///  Excel转成DataTable
        /// </summary>
        /// <param name="book"></param>
        /// <param name="headPoint">表头定位</param>
        /// <param name="validRow">从第几行开始算有效数据</param>
        /// <param name="sheetIndex">第几个sheet</param>
        /// <returns></returns>
        private static DataTable GetDataTable(IWorkbook book, List<Point> headPoint, int validRow, int sheetIndex = 0)
        {
            DataTable table = new DataTable();
            ISheet sheet = book.GetSheetAt(sheetIndex);

            foreach (var point in headPoint)
            {
                IRow headerRow = sheet.GetRow(point.X);
                if (headerRow == null)
                    throw new Exception(point.X + "行为空");
                DataColumn column = new DataColumn(headerRow.GetCell(point.Y).ToString());
                if (column == null)
                    throw new Exception(point.X + "行的" + point.Y + "列为空");
                table.Columns.Add(column);
            }

            //行内容
            for (int i = validRow; i < sheet.LastRowNum - 1; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                    continue;//TODO :                
                DataRow dataRow = table.NewRow();
                //DTDO：有些行 大于  table.Columns.Count 所以不能用row.LastCellNum
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    dataRow[j] = row.GetValue(j);
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }

        private static void SaveExcel(string savePath, IWorkbook book, List<SheetEntity> sheetList)
        {
            foreach (var sheetEntity in sheetList)
            {
                var sheetIndex = sheetEntity.SheetIndex;
                var columnIndex = sheetEntity.ColumnIndex;
                var rowsResult = sheetEntity.RowsResult;

                ISheet sheet = book.GetSheetAt(sheetIndex);
                foreach (var rowObj in rowsResult)
                {
                    var row = sheet.GetRow(rowObj.Key);
                    if (row == null)
                        throw new Exception("没有" + rowObj.Key + "行");
                    var cell = row.GetCell(columnIndex);
                    if (cell != null)
                        cell.SetCellValue(rowObj.Value);
                    else
                        row.CreateCell(columnIndex).SetCellValue(rowObj.Value);
                }
            }
            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                book.Write(fs);
            }
        }
        #endregion

        #region 【注释】直接输入流 OutputStream 
        //直接输入流
        //public static void OutputStream(MemoryStream file)
        //{
        //    using (file)
        //    {
        //        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + _workbookName + ".xls");
        //        HttpContext.Current.Response.Charset = "UTF-8";
        //        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
        //        HttpContext.Current.Response.ContentType = "application/ms-excel";
        //        file.WriteTo(HttpContext.Current.Response.OutputStream);
        //        HttpContext.Current.Response.End();
        //    }
        //} 
        #endregion
    }


    public class SheetEntity
    {
        /// <summary>
        /// 第几个Sheet
        /// </summary>
        public int SheetIndex { get; set; }
        /// <summary>
        /// 第多少列
        /// </summary>
        public int ColumnIndex { get; set; }
        /// <summary>
        /// 行 对应要写的注释
        /// </summary>
        public Dictionary<int, string> RowsResult { get; set; }
    }

    public static class NPOIExtensions
    {
        /// <summary>
        /// 获取XSSFRow的值（全部统一转成字符串）
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetValue(this IRow row, int index)
        {
            string value = null;
            var rowCell = row.GetCell(index);
            switch (rowCell?.CellType)
            {
                case CellType.String:
                    value = rowCell.StringCellValue;
                    break;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(rowCell))
                    {
                        value = DateTime.FromOADate(rowCell.NumericCellValue).ToString();
                    }
                    else
                    {
                        value = rowCell.NumericCellValue.ToString();
                    }
                    break;
                case CellType.Boolean:
                    value = rowCell.BooleanCellValue.ToString();
                    break;
                case CellType.Error:
                    value = ErrorEval.GetText(rowCell.ErrorCellValue);
                    break;
                case CellType.Formula:
                    switch (rowCell.CachedFormulaResultType)
                    {
                        case CellType.String:
                            value = rowCell.StringCellValue;
                            break;
                        case CellType.Numeric:
                            if (DateUtil.IsCellDateFormatted(rowCell))
                            {
                                value = DateTime.FromOADate(rowCell.NumericCellValue).ToString();
                            }
                            else
                            {
                                value = rowCell.NumericCellValue.ToString();
                            }
                            break;
                        case CellType.Boolean:
                            value = rowCell.BooleanCellValue.ToString();
                            break;
                        case CellType.Error:
                            value = ErrorEval.GetText(rowCell.ErrorCellValue);
                            break;
                    }
                    break;
            }
            return value;
        }

        /// <summary>
        /// 把DataTable数据写入Workbook
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="isOldThan2007">是否低于2007</param>
        /// <returns></returns>
        public static IWorkbook ToWorkbook(this DataTable dataTable, bool isOldThan2007)
        {
            IWorkbook book = isOldThan2007 ? (IWorkbook)new HSSFWorkbook() : new XSSFWorkbook();
            ISheet sheet = book.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            foreach (DataColumn column in dataTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                IRow dataRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    dataRow.CreateCell(j).SetCellValue(row[j]?.ToString());
                }
            }

            return book;
        }
    }

    public static class DataTableExtensions
    {
        /// <summary>
        /// DataTable转换成实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static List<T> GetEntityList<T>(this DataTable table, Action<DataRow, T> action) where T : new()
        {
            var listEntity = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                var entity = new T();
                action(row, entity);
                listEntity.Add(entity);
            }
            return listEntity;
        }

        /// <summary>
        /// DataTable转换成实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static List<T> GetEntityList<T>(this List<DataTable> tableList, Action<DataRow, T> action) where T : new()
        {
            var listEntity = new List<T>();
            foreach (var table in tableList)
            {
                foreach (DataRow row in table.Rows)
                {
                    var entity = new T();
                    action(row, entity);
                    listEntity.Add(entity);
                }
            }
            return listEntity;
        }

        /// <summary>
        /// 实体集合转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listEntity"></param>
        /// <param name="headList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> listEntity, Dictionary<string, string> headList, Action<DataRow, T> action) where T : new()
        {
            var table = new DataTable();
            //设置表头
            foreach (var head in headList)
            {
                table.Columns.Add(new DataColumn(head.Value, typeof(string)));
            }
            foreach (var entity in listEntity)
            {
                var row = table.NewRow();
                action(row, entity);
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
