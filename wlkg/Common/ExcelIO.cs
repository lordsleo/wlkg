//
//文件名：    ExcelExport.cs
//功能描述：  导入导出Excel通用类
//创建时间：  2015/04/24
//作者：      sh
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Data;

namespace wlkg.Common
{
    public class ExcelIO
    {
        #region Constructor
        public ExcelIO()
        {
            mStatus = IsExistExcel() ? 1 : 0;
            //CreatExcel();    
        }
        #endregion

        #region Fields
        //
        //Excel应用程序
        //
        private Excel.Application mXLApp;
        //
        //Excel状态
        //
        private int mStatus = 0;
        //
        //Excel工作本集
        //
        private Excel.Workbooks mWorkBooks = null;
        //
        //Excel工作本,默认只有一个
        //
        private Excel.Workbook mWorkBook = null;
        //
        //Excel工作表集
        //
        private Excel.Worksheets mWorkSheets = null;
        //
        //Excel当前工作表
        //
        private Excel.Worksheet mCurWorkSheet = null;
        //
        //单元范围
        //
        private Excel.Range mRange = null;
        //
        //实例化参数对象
        //
        private Object mOMissing = System.Reflection.Missing.Value;

        #endregion

        #region Methods

        #region 私有成员

        #region 判断是否安装Excel
        /// <summary>
        /// 创建excel实例，病判断是否安装Excel
        /// </summary>
        /// <returns>结果</returns>
        private bool IsExistExcel()
        {
            mXLApp = new Excel.Application();
            if (mXLApp == null)
            {
                MessageBox.Show("无法创建Excel对象，可能您的电脑未安装Excel!");
                return false;
            }

            return true;
        }
        #endregion

        #region 获取列名
        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="num">列号</param>
        /// <returns></returns>
        private string GetColumnName(int num)
        {
            int h, l;
            h = num / 26;
            l = num % 26;
            if (l == 0)
            {
                h -= 1;
                l = 26;
            }
            string s = GetLetter(h) + GetLetter(l);
            return s;
        }
        #endregion

        #region 获取字母
        /// <summary>
        /// 获取字母
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns></returns>
        private string GetLetter(int number)
        {
            switch (number)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "D";
                case 5:
                    return "E";
                case 6:
                    return "F";
                case 7:
                    return "G";
                case 8:
                    return "H";
                case 9:
                    return "I";
                case 10:
                    return "J";
                case 11:
                    return "K";
                case 12:
                    return "L";
                case 13:
                    return "M";
                case 14:
                    return "N";
                case 15:
                    return "O";
                case 16:
                    return "P";
                case 17:
                    return "Q";
                case 18:
                    return "R";
                case 19:
                    return "S";
                case 20:
                    return "T";
                case 21:
                    return "U";
                case 22:
                    return "V";
                case 23:
                    return "W";
                case 24:
                    return "X";
                case 25:
                    return "Y";
                case 26:
                    return "Z";
                default:
                    return "";
            }
        }
        #endregion
        #endregion

        #region 公有成员
        #region 显示保存文件对话框
        /// <summary>
        /// 显示保存文件对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <returns>保存路径</returns>
        public string ShowSaveFileDialog(string title, string fileName)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.DefaultExt = "xlsx";
            sfd.Title = "Export To " + title;
            sfd.FileName = fileName;
            sfd.Filter = "Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return sfd.FileName;
            }
            
            return string.Empty;
        }
        #endregion

        #region 创建Excel的Sheet
        /// <summary>
        /// 创建Excel的Sheet（传空，默认创建Sheet1）
        /// </summary>
        /// <param name="sheetNameArr">sheet名集合</param>
        public void CreatExcelSheets(string[] sheetNameArr = null)
        {
            if (mStatus == 0)
            {
                return;
            }
            mXLApp.Application.ScreenUpdating = false;
            mWorkBooks = mXLApp.Workbooks;
            mWorkBook = mWorkBooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);

            if (sheetNameArr == null)
            {
                Excel.Worksheet sheet = (Excel.Worksheet)mWorkBook.Worksheets[1];
            }
            else
            {
                for (int isheet = 2; isheet <= sheetNameArr.Length; isheet++)
                {
                    mXLApp.Sheets.Add(mOMissing, mOMissing, mOMissing, mOMissing);
                }
                for (int isheet = 1; isheet <= sheetNameArr.Length; isheet++)
                {
                    ((Excel.Worksheet)mXLApp.Sheets[isheet]).Name = sheetNameArr[isheet - 1];
                }
            }
            mXLApp.DisplayAlerts = false;
            mXLApp.Visible = false;
        }
        #endregion

        #region 设置当前活动工作表
        /// <summary>
        /// 设置当前活动工作表
        /// </summary>
        /// <returns></returns>
        public void SetCurActiveSheet(Excel.Worksheet sheet)
        {
            mCurWorkSheet = sheet;
        }

        /// <summary>
        /// 设置当前活动工作表(默认为1表)
        /// </summary>
        /// <param name="sheetIndex">sheet索引，从1开始</param>
        public void SetCurActiveSheet(int sheetIndex = 1)
        {
            Excel.Worksheet sheet = GetWorksheet(sheetIndex);
            mCurWorkSheet = sheet;
        }
        #endregion

        #region 重命名工作表
        /// <summary>
        /// 重命名工作表
        /// </summary>
        /// <param name="sheet">工作表对象</param>
        /// <param name="newName">工作表新名称</param>
        /// <returns></returns>
        public Excel.Worksheet RenameWorksheet(Excel.Worksheet sheet, string newName)
        {
            sheet.Name = newName;
            return sheet;
        }

        /// <summary>
        /// 重命名工作表
        /// </summary>
        /// <param name="oldName">原名称</param>
        /// <param name="newName">新名称</param>
        /// <returns></returns>
        public Excel.Worksheet RenameWorksheet(string oldName, string newName)
        {
            Excel.Worksheet sheet = this.GetWorksheet(oldName);
            return this.RenameWorksheet(sheet, newName);
        }
        #endregion

        #region 根据工作表名（索引）获取Excel工作表对象的引用
        /// <summary>
        /// 根据工作表名获取Excel工作表对象的引用
        /// </summary>
        /// <param name="sheetName">sheet名</param>
        /// <returns></returns>
        public Excel.Worksheet GetWorksheet(string sheetName)
        {
            return (Excel.Worksheet)mWorkBook.Sheets[sheetName];
        }

        /// <summary>
        /// 根据工作表索引获取Excel工作表对象的引用
        /// </summary>
        /// <param name="sheetindex">sheet索引，从1开始</param>
        /// <returns></returns>
        public Excel.Worksheet GetWorksheet(int sheetindex)
        {
            return (Excel.Worksheet)mWorkBook.Sheets[sheetindex];
        }
        #endregion

        #region 删除工作表
        /// <summary>
        /// 删除工作表
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        public void DeleteWorksheet(string sheetName)
        {
            if (mWorkBook.Worksheets.Count <= 1)
            {
                throw new InvalidOperationException("工作簿至少需要一个可视化的工作表！");
            }
            this.GetWorksheet(sheetName).Delete();
        }
        #endregion

        #region 删除除参数sheet指定外的其余工作表
        /// <summary>
        /// 删除除参数sheet指定外的其余工作表
        /// </summary>
        /// <param name="sheet"></param>
        public void DeleteWorksheetExcept(Excel.Worksheet sheet)
        {
            foreach (Excel.Worksheet ws in this.mWorkBook.Worksheets)
            {
                if (sheet != ws)
                {
                    ws.Delete();
                }
            }
        }
        #endregion

        #region 保存excel
        /// <summary>
        /// 保存excel
        /// </summary>
        /// <param name="saveFilePath">保存路径</param>
        /// <returns></returns>
        public bool save(string saveFilePath)
        {
            //保存文件
            if (saveFilePath == "")
            {
                return false;
            }

            int formatNum;//保存excel文件的格式
            string Version;//excel版本号
            Version = mXLApp.Version;//获取你使用的excel 的版本号

            if (Convert.ToDouble(Version) < 12)//You use Excel 97-2003
            {
                formatNum = -4143;
            }
            else
            {
                formatNum = 56;
            }

            mWorkBook.Saved = true;
            if (saveFilePath.IndexOf("xlsx") != -1)
            {
                mWorkBook.SaveCopyAs(saveFilePath);
            }
            else
            {
                mWorkBook.SaveAs(saveFilePath, formatNum);
            }

            return true;
        }
        #endregion

        #region 释放Excel对应的对象资源
        /// <summary>
        /// 释放Excel对应的对象资源
        /// </summary>
        public void Dispose()
        {
            //释放Excel对应的对象
            if (mRange != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mRange);
                mRange = null;
            }
            if (mCurWorkSheet != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mCurWorkSheet);
                mWorkSheets = null;
            }
            if (mWorkSheets != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mWorkSheets);
                mWorkSheets = null;
            }
            if (mWorkBook != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mWorkBook);
                mWorkBook = null;
            }
            if (mWorkBooks != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mWorkBooks);
                mWorkBooks = null;
            }
            mXLApp.Application.Workbooks.Close();
            mXLApp.Quit();
            if (mXLApp != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mXLApp);
                mXLApp = null;
            }
            GC.Collect();
        }
        #endregion

        #region 将DataTable导出Excel
        /// <summary>
        /// 将DataTable导出Excel
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="dtStartRowNum">数据集起始行索引，从1开始</param>
        /// <param name="dtStartColNum">数据集起始列索引，从1开始</param>
        /// <param name="dtEndRowNum">数据集结束行索引</param>
        /// <param name="dtEndColNum">数据集结束列索引</param>
        /// <param name="sheetRowIndex">工作表插入起始行，从1开始</param>
        /// <param name="sheetColIndex">工作表插入起始列，从1开始</param>
        /// <returns></returns>
        ///      
        public bool DataTableToExcel(DataTable dt, int dtStartRowNum, int dtStartColNum, int dtEndRowNum, int dtEndColNum, int sheetRowIndex, int sheetColIndex)
        {
            //判断数据集是否为null
            if (dt == null)
            {
                return false;
            }
            //判断起始结束行是否出错
            if (dtStartRowNum > dtEndRowNum || dtStartColNum > dtEndColNum)
            {
                return false;
            }

            object[,] objData = new object[dtEndRowNum - dtStartRowNum + 1, dtEndColNum - dtStartColNum + 1];
            if (dt.Rows.Count > 0)
            {
                for (int iRow = dtStartRowNum - 1; iRow < dtEndRowNum; iRow++)
                {
                    for (int iCol = dtStartColNum - 1; iCol < dtEndColNum; iCol++)
                    {
                        objData[iRow - dtStartRowNum + 1, iCol - dtStartColNum + 1] = dt.Rows[iRow][iCol];
                    }
                }
            }
            Excel.Range range = GetRange(sheetRowIndex, sheetColIndex, dt.Rows.Count + sheetRowIndex -1, sheetColIndex + dtEndColNum - dtStartColNum);
            range.Value = objData; //给Exccel中的Range整体赋值 

            return true;
        }

        /// <summary>
        /// DataTable导出Excel(根据列名导入)
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="colNameSet">列名集合</param>
        /// <param name="sheetRowIndex">工作表插入起始行，从1开始</param>
        /// <param name="sheetColIndex">工作表插入起始列，从1开始</param>
        public bool DataTableToExcel(DataTable dt, string[] colNameSet, int sheetRowIndex, int sheetColIndex)
        {
            if (dt == null)
            {
                return false;
            }

            object[,] objData = new object[dt.Rows.Count, colNameSet.Length];
            if (dt.Rows.Count > 0)
            {
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    for (int iCol = 0; iCol < colNameSet.Length; iCol++)
                    {
                        objData[iRow, iCol] = dt.Rows[iRow][colNameSet[iCol]];
                    }
                }
            }
            Excel.Range range = GetRange(sheetRowIndex, sheetColIndex, dt.Rows.Count + sheetRowIndex - 1, colNameSet.Length); 
            range.Value = objData; //给Exccel中的Range整体赋值 

            return true;
        }
        #endregion

        #region 获取指定范围的单元格（行索引为从1开始的数字，最大65536，列索引为A~Z、AA~AZ、BA~BZ...HA~HZ、IA~IV的字母及组合，也可以是1-65536数字）
        /// <summary>
        /// 获取指定范围的单元格
        /// </summary>
        /// <param name="startRowIndex">指定单元范围起始行索引，从1开始</param>
        /// <param name="startColIndex">指定单元范围起始列数字索引，从1开始</param>
        /// <param name="endRowIndex">指定单元范围结束行索引</param>
        /// <param name="endColIndex">指定单元范围结束列数字索引</param>
        /// <returns></returns>
        public Excel.Range GetRange(int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
        {
            Excel.Range range;
            //range = sheet.getmRange(sheet.Cells[startRowIndex, startColIndex], sheet.Cells[endRowIndex, endColIndex]);
            range = mCurWorkSheet.Range[mCurWorkSheet.Cells[startRowIndex, startColIndex], mCurWorkSheet.Cells[endRowIndex, endColIndex]];
            return range;
        }

        /// <summary>
        /// 获取指定范围的单元格
        /// </summary>
        /// <param name="startRowIndex">指定单元范围起始行索引，从1开始</param>
        /// <param name="strStartColIndex">指定单元范围起始列字母及组合索引</param>
        /// <param name="endRowIndex">指定单元范围结束行索引</param>
        /// <param name="strEndColIndex">指定单元范围结束列字母及组合索引</param>
        /// <returns></returns>
        public Excel.Range GetRange(int startRowIndex, string strStartColIndex, int endRowIndex, string strEndColIndex)
        {
            Excel.Range range;
            //range = worksheet.Range[worksheet.Cells[strStartColIndex + startRowIndex.ToString()], strEndColIndex + endRowIndex.ToString()];
            range = mCurWorkSheet.Range[mCurWorkSheet.Cells[startRowIndex, strStartColIndex], mCurWorkSheet.Cells[endRowIndex, strEndColIndex]];
            return range;
        }

        /// <summary>
        /// 获取指定范围的单元格
        /// </summary>
        /// <param name="RowIndex">指定单元格行索引，从1开始</param>
        /// <param name="ColIndex">指定单元格列索引，从1开始</param>
        /// <returns></returns>
        public Excel.Range GetRange(int rowIndex, int ColIndex)
        {
            return GetRange(rowIndex, ColIndex, rowIndex, ColIndex);
        }

        /// <summary>
        /// 获取指定范围的单元格
        /// </summary>
        /// <param name="rowIndex">指定单元格行索引，从1开始</param>
        /// <param name="strColIndex">指定单元格列字母及组合索引，从A开始</param>
        /// <returns></returns>
        public Excel.Range GetRange(int rowIndex, string strColIndex)
        {
            return GetRange(rowIndex, strColIndex, rowIndex, strColIndex);
        }

        #endregion

        #region  自动调整，设置自动换行以及自动调整列宽
        /// <summary>
        /// 自动调整，设置自动换行以及自动调整列宽
        /// </summary>
        /// <param name="range">Range对象</param>
        public void AutoAdjustment(Excel.Range range)
        {
            range.WrapText = true;
            range.AutoFit();
        }
        #endregion

        #region 设置单元格的值
        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="rowNum">单元格行号</param>
        /// <param name="colNum">单元格列好</param>
        /// <param name="value">单元格值</param>
        public void setCellValue(int rowNum, int colNum, object value)
        {
            mCurWorkSheet.Cells[rowNum, colNum] = value;
        }
        #endregion

        #region 合并单元格
        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="range">Range对象</param>
        /// <param name="horizontalAlignment">水平对齐方式</param>
        public void MergeCells(Excel.Range range, ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center)
        {
            range.HorizontalAlignment = horizontalAlignment;
            range.VerticalAlignment = Excel.Constants.xlCenter;
            range.WrapText = false;
            range.Orientation = 0;
            range.AddIndent = false;
            range.IndentLevel = 0;
            range.ShrinkToFit = false;
            range.MergeCells = false;
            range.Merge(mOMissing);
        }
        #endregion

        #region 在指定的行上插入一行
        /// <summary>
        /// 在指定的行上插入一行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        public void InsertRow(int rowIndex)
        {
            Excel.Range range;
            range = GetRange(rowIndex, "A");
            range.Select();
            range.EntireRow.Insert(mOMissing, mOMissing);
        }

        #endregion

        #region 在指定的列上插入一整列
        /// <summary>
        /// 在指定的列上插入一整列
        /// </summary>
        /// <param name="colIndex">列索引</param>
        public void InsertColumn(int colIndex)
        {
            Excel.Range range;
            range = GetRange(1, colIndex);
            range.Select();
            range.EntireColumn.Insert(mOMissing, mOMissing);
        }
        #endregion

        #region 删除指定行
        /// <summary>
        /// 删除指定行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        public void DeleteRow(int rowIndex)
        {
            Excel.Range range;
            range = GetRange(rowIndex, "A");
            range.Select();
            range.EntireRow.Delete(mOMissing);
        }
        #endregion

        #region 删除指定列
        /// <summary>
        /// 删除指定行
        /// </summary>
        /// <param name="colIndex">列索引</param>
        public void DeleteColumn(int colIndex)
        {
            Excel.Range range;
            range = GetRange(1, colIndex);
            range.Select();
            range.EntireColumn.Delete(mOMissing);
        }
        #endregion

        #region 设置单元格为文本形式
        /// <summary>
        /// 设置单元格为文本形式
        /// </summary>
        /// <param name="range">range对象</param>
        public void SetTextForm(Excel.Range range)
        {
            range.NumberFormatLocal = "@";
        }

        #endregion

        #region 设置行高
        /// <summary>
        /// 设置行高
        /// </summary>
        /// <param name="startRowNum">起始行</param>
        /// <param name="endRowNum">结束行</param>
        /// <param name="height">高度</param>
        public void SetRowHeight(int startRowNum, int endRowNum, int height)
        {
            Excel.Range range = (Excel.Range)mCurWorkSheet.Rows[startRowNum.ToString() + ":" + endRowNum.ToString(), System.Type.Missing];
            range.RowHeight = height;
        }
        #endregion

        #region 设置列宽
        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="startColNum">起始列</param>
        /// <param name="endColNum">结束列</param>
        /// <param name="width">宽度</param>
        public void SetColumnWidth(int startColNum, int endColNum, int width)
        {
            string strStartColumn = GetColumnName(startColNum);
            string strEndtColumn = GetColumnName(endColNum);
            Excel.Range range = (Excel.Range)mCurWorkSheet.Columns[strStartColumn + ":" + strEndtColumn, System.Type.Missing];
            range.ColumnWidth = width;
        }
        #endregion

        #region 设置单元格样式
        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="range">Range对象</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="fontName">字体名称</param>
        /// <param name="color">字体颜色</param>
        /// <param name="horizontalAlignment">水平对齐方式</param>
        public void SetRangeFormat(Excel.Range range, object fontSize, object fontName, Excel.Constants color, ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center)
        {
            range.Font.Color = color;
            range.Font.Size = fontSize;
            range.Font.Name = fontName;
            range.HorizontalAlignment = horizontalAlignment;
        }
        #endregion

        #region 设置单元格字体及大小
        /// <summary>
        /// 设置单元格字体及大小
        /// </summary>
        /// <param name="range">Range对象</param>
        /// <param name="fontName">字体名称</param>
        /// <param name="fontSize">字体大小</param>
        public void SetFontNameSize(Excel.Range range, string fontName, int fontSize)
        {
            range.Font.Name = fontName;
            range.Font.Size = fontSize;
        }
        #endregion

        #region 设置字体样式（加粗，斜体，下划线）
        /// <summary>
        /// 设置字体样式（加粗，斜体，下划线）
        /// </summary>
        /// <param name="range">Range对象</param>
        /// <param name="isBold">是否加粗</param>
        /// <param name="isItalic">是否斜</param>
        /// <param name="underlineStyle">设置是否有下划线</param>
        public void SetFontStyle(Excel.Range range, bool isBold, bool isItalic, ExcelUnderlineStyle underlineStyle)
        {
            range.Font.Bold = isBold;
            range.Font.Underline = underlineStyle;
            range.Font.Italic = isItalic;
        }
        #endregion

        #region 设置单元格背景色及填充方式
        /// <summary>
        /// 设置单元格背景色及填充方式
        /// </summary>
        /// <param name="range">Range对象</param>
        /// <param name="color"></param>
        /// <param name="pattern"></param>
        public void SetCellsBackColor(Excel.Range range, string color, Pattern pattern)
        {
            range.Interior.ColorIndex = color;
            range.Interior.Pattern = pattern;
        }
        #endregion

        #region 设置单元格对齐方式
        /// <summary>
        /// 设置单元格对齐方式
        /// </summary>
        /// <param name="range">Range对象</param>
        /// <param name="horizontalAlignment">对象方式</param>
        public void SetCellsAlignment(Excel.Range range, ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center)
        {
            range.HorizontalAlignment = horizontalAlignment;
        }
        #endregion

        #region 设置单元格文字方向
        /// <summary>
        /// 设置单元格文字方向
        /// </summary>
        /// <param name="range">Range对象</param>
        /// <param name="textOrientation">文字角度</param>
        public void SetCellsTextOrientation(Excel.Range range, ExcelTextOrientation textOrientation)
        {
            range.Orientation = textOrientation;
        }
        #endregion

        #region 设置单元格边框
        /// <summary>
        /// 设置单元格边框(只能设置单个单元格的边框)
        /// </summary>
        /// <param name="range">Range对象</param>
        /// <param name="bordersEdge">单元格边框范围</param>
        /// <param name="ExcelBordersEdge">单元格边线</param>
        /// <param name="bordersLineStyle">边框线条分隔</param>
        /// <param name="bordersWeight">边框线粗细</param>
        public void SetCellsBorder(Excel.Range range, ExcelBordersEdge bordersEdge, ExcelBordersLineStyle bordersLineStyle, ExcelBorderWeight bordersWeight)
        {
            //range.Select();
            Excel.Border border = null;

            switch (bordersEdge)
            {
                //左右顶底的线
                case ExcelBordersEdge.EdgeLeft:
                    border = range.Borders[Excel.XlBordersIndex.xlEdgeLeft];
                    break;
                case ExcelBordersEdge.EdgeRight:
                    border = range.Borders[Excel.XlBordersIndex.xlEdgeRight];
                    break;
                case ExcelBordersEdge.EdgeTop:
                    border = range.Borders[Excel.XlBordersIndex.xlEdgeTop];
                    break;
                case ExcelBordersEdge.EdgeBottom:
                    border = range.Borders[Excel.XlBordersIndex.xlEdgeBottom];
                    break;
                //对角线
                case ExcelBordersEdge.DiagonalDown:
                    border = range.Borders[Excel.XlBordersIndex.xlDiagonalDown];
                    break;
                case ExcelBordersEdge.DiagonUp:
                    border = range.Borders[Excel.XlBordersIndex.xlDiagonalUp];
                    break;
                //边框内部是横竖线(不包括边框)
                case ExcelBordersEdge.InsideHorizontal:
                    border = range.Borders[Excel.XlBordersIndex.xlInsideHorizontal];
                    break;
                case ExcelBordersEdge.InsideVertical:
                    border = range.Borders[Excel.XlBordersIndex.xlInsideVertical];
                    break;
                case ExcelBordersEdge.LineStyleNone:
                    //所先范围内所有线都没有
                    range.Borders[Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlLineStyleNone;		//xlNone
                    range.Borders[Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                    range.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                    range.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                    range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                    range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                    range.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                    range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                    break;
            }

            if (border != null)
            {
                //XlLineStyle
                Excel.XlLineStyle mXlLineStyle = Excel.XlLineStyle.xlContinuous;
                switch (bordersLineStyle)
                {
                    case ExcelBordersLineStyle.Continious:
                        mXlLineStyle = Excel.XlLineStyle.xlContinuous;
                        break;
                    case ExcelBordersLineStyle.Dash:
                        mXlLineStyle = Excel.XlLineStyle.xlDash;
                        break;
                    case ExcelBordersLineStyle.DashDot:
                        mXlLineStyle = Excel.XlLineStyle.xlDashDot;
                        break;
                    case ExcelBordersLineStyle.DashDotDot:
                        mXlLineStyle = Excel.XlLineStyle.xlDashDotDot;
                        break;
                    case ExcelBordersLineStyle.Dot:
                        mXlLineStyle = Excel.XlLineStyle.xlDot;
                        break;
                    case ExcelBordersLineStyle.Double:
                        mXlLineStyle = Excel.XlLineStyle.xlDouble;
                        break;
                    case ExcelBordersLineStyle.StyleNone:
                        mXlLineStyle = Excel.XlLineStyle.xlLineStyleNone;
                        break;
                    case ExcelBordersLineStyle.SlantDashDot:
                        mXlLineStyle = Excel.XlLineStyle.xlSlantDashDot;
                        break;
                }
                border.LineStyle = mXlLineStyle;

                //XlBorderWeight
                Excel.XlBorderWeight mXlBorderWeight = Excel.XlBorderWeight.xlThin;

                switch (bordersWeight)
                {
                    case ExcelBorderWeight.Hairline:
                        mXlBorderWeight = Excel.XlBorderWeight.xlHairline;
                        break;
                    case ExcelBorderWeight.Medium:
                        mXlBorderWeight = Excel.XlBorderWeight.xlMedium;
                        break;
                    case ExcelBorderWeight.Thick:
                        mXlBorderWeight = Excel.XlBorderWeight.xlThick;
                        break;
                    case ExcelBorderWeight.Thin:
                        mXlBorderWeight = Excel.XlBorderWeight.xlThin;
                        break;
                }
                border.Weight = mXlBorderWeight;

            }//end if
        }

        /// <summary>
        /// 设置单元格外边框(上，下，右，左边框)
        /// </summary>
        /// <param name="range">Range对象</param>
        /// <param name="bordersLineStyle">边框线条分隔</param>
        /// <param name="bordersWeight">边框线粗细</param>
        public void SetCellsOutline(Excel.Range range, ExcelBordersLineStyle bordersLineStyle, ExcelBorderWeight bordersWeight)
        {
            //Left
            SetCellsBorder(range, ExcelBordersEdge.EdgeLeft, bordersLineStyle, bordersWeight);
            //Right
            SetCellsBorder(range, ExcelBordersEdge.EdgeRight, bordersLineStyle, bordersWeight);
            //Top
            SetCellsBorder(range, ExcelBordersEdge.EdgeTop, bordersLineStyle, bordersWeight);
            //Bottom
            SetCellsBorder(range, ExcelBordersEdge.EdgeBottom, bordersLineStyle, bordersWeight);
        }
        #endregion
        #endregion
        #endregion
    }

}
