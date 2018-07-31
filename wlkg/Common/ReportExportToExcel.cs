//
//文件名：    ReportExcelToExcel.cs
//功能描述：  报表导出Excel通用类
//创建时间：  2015/05/04
//作者：      sh
//修改时间：  暂无
//修改描述：  暂无
//

using Microsoft.Office.Interop.Excel;

namespace wlkg.Common
{
    /// <summary>
    /// Excel单元格设置值关键字
    /// </summary>
    public struct _ExcelCellValue
    {
       private int rowNum;//行
       private int colNum;//列
       private string strValue;//值

       public int RowNum
       {
         get { return rowNum; }
         set { rowNum = value; }
       }
       public int ColNum
       {
         get { return colNum; }
         set { colNum = value; }
       }
       public string StrValue
       {
         get { return this.strValue; }
         set { this.strValue = value; }
       }

       public _ExcelCellValue(int rowNum, int colNum, string strValue)
       {
           this.rowNum = rowNum;
           this.colNum = colNum;
           this.strValue = strValue;
       }
    };

    /// <summary>
    /// Excel单元格边框设置关键字
    /// </summary>
    public struct _ExcelCellsBorder
    {
        private int startRowNum;//起始行
        private int startColNum;//起始列
        private int endRowNum;//结束行
        private int endColNum;//结束列
        private ExcelBordersEdge bordersEdge;//单元格边框线
        private ExcelBorderWeight bordersWeight;//边框线粗细

        public int StartRowNum
        {
            get { return startRowNum; }
            set { startRowNum = value; }
        }
        public int StartColNum
        {
            get { return startColNum; }
            set { startColNum = value; }
        }
        public int EndRowNum
        {
            get { return endRowNum; }
            set { endRowNum = value; }
        }
        public int EndColNum
        {
            get { return endColNum; }
            set { endColNum = value; }
        }
        public ExcelBordersEdge BordersEdge
        {
            get { return bordersEdge; }
            set { bordersEdge = value; }
        }
        public ExcelBorderWeight BordersWeight
        {
            get { return bordersWeight; }
            set { bordersWeight = value; }
        }

        public _ExcelCellsBorder(int startRowNum, int startColNum, int endRowNum, int endColNum, ExcelBordersEdge bordersEdge, ExcelBorderWeight bordersWeight)
        {
            this.startRowNum = startRowNum;
            this.startColNum = startColNum;
            this.endRowNum = endRowNum;
            this.endColNum = endColNum;
            this.bordersEdge = bordersEdge;
            this.bordersWeight = bordersWeight;
        }
    }

    /// <summary>
    /// Excel单元格合并关键字
    /// </summary>
    public struct _ExcelCellsMerge
    {
        int startRowNum;//起始行
        int startColNum;//起始列
        int endRowNum;//结束行
        int endColNum;//结束列
        ExcelHorizontalAlignment horizontalAlignment;//水平对齐方式

        public int StartRowNum
        {
            get { return startRowNum; }
            set { startRowNum = value; }
        }
        public int StartColNum
        {
            get { return startColNum; }
            set { startColNum = value; }
        }
        public int EndRowNum
        {
            get { return endRowNum; }
            set { endRowNum = value; }
        }
        public int EndColNum
        {
            get { return endColNum; }
            set { endColNum = value; }
        }
        public ExcelHorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set { horizontalAlignment = value; }
        }

        public _ExcelCellsMerge(int startRowNum, int startColNum, int endRowNum, int endColNum, ExcelHorizontalAlignment horizontalAlignment)
       {
           this.startRowNum = startRowNum;
           this.startColNum = startColNum;
           this.endRowNum = endRowNum;
           this.endColNum = endColNum;
           this.horizontalAlignment = horizontalAlignment;
       }
    }

    /// <summary>
    /// Excel单元格设置字体样式关键字
    /// </summary>
    public struct _ExcelCellsFontStyle
    {
        int startRowNum;//起始行
        int startColNum;//起始列
        int endRowNum;//结束行
        int endColNum;//结束列

        public int StartRowNum
        {
            get { return startRowNum; }
            set { startRowNum = value; }
        }
        public int StartColNum
        {
            get { return startColNum; }
            set { startColNum = value; }
        }
        public int EndRowNum
        {
            get { return endRowNum; }
            set { endRowNum = value; }
        }
        public int EndColNum
        {
            get { return endColNum; }
            set { endColNum = value; }
        }

        public _ExcelCellsFontStyle(int startRowNum, int startColNum, int endRowNum, int endColNum)
        {
            this.startRowNum = startRowNum;
            this.startColNum = startColNum;
            this.endRowNum = endRowNum;
            this.endColNum = endColNum;
        }
    }

    public class ReportExportToExcel : ExcelIO
    {
        /// <summary>
        /// 设置单元格集合值
        /// </summary>
        /// <param name="arr">_ExcelCellValue对象</param>
        /// <param name="startRowNum">起始行</param>
        /// <param name="startColNum">起始列</param>
        /// <param name="endRowNum">结束行</param>
        /// <param name="endColNum">结束列</param>
        public void SetCellSetValue(_ExcelCellValue[] arr, int startRowNum, int startColNum, int endRowNum, int endColNum)
        {
            object[,] objData = new object[endRowNum, endColNum];      
            for (int iCell = 0; iCell < arr.Length; iCell++)
            { 
                 _ExcelCellValue excelCellValue = arr[iCell];
                 objData[excelCellValue.RowNum - 1, excelCellValue.ColNum - 1] = excelCellValue.StrValue;
            }
            Range range = GetRange(startRowNum, startColNum, endRowNum, endColNum);
            range.Value = objData; //给Exccel中的Range整体赋值         
        }

        /// <summary>
        /// 设置单元格集合值
        /// </summary>
        /// <param name="arr">_ExcelCellValue数组</param>
        public void SetCellSetValue(_ExcelCellValue[] arr)
        {
            for (int iarr = 0; iarr < arr.Length; iarr++)
            {
                _ExcelCellValue excelCellValue = arr[iarr];
                setCellValue(excelCellValue.RowNum, excelCellValue.ColNum, excelCellValue.StrValue);
            }
        }
        
        /// <summary>
        /// 设置单元格边框集合的边框(默认边框线连续)
        /// </summary>
        /// <param name="arr">_ExcelCellsBorder数组</param>
        public void SetCellsBorderSetBorder(_ExcelCellsBorder[] arr)
        {
            for (int iarr = 0; iarr < arr.Length; iarr++)
            {
                _ExcelCellsBorder excelCellsBorder = arr[iarr];
                Range range = GetRange(excelCellsBorder.StartRowNum, excelCellsBorder.StartColNum, excelCellsBorder.EndRowNum, excelCellsBorder.EndColNum);
                SetCellsBorder(range, excelCellsBorder.BordersEdge, ExcelBordersLineStyle.Continious, excelCellsBorder.BordersWeight);
            }
        }

        /// <summary>
        ///  设置某范围内所有单元格边框（不包含最左，最右边框）
        /// </summary>
        /// <param name="StartRowNum">起始行</param>
        /// <param name="StartColNum">起始列</param>
        /// <param name="EndRowNum">结束行</param>
        /// <param name="EndRowNum">结束列</param>
        /// <param name="bordersWeight">边框线粗细</param>
        public void SetAllCellsBorderExclusiveRL(int StartRowNum, int StartColNum, int EndRowNum, int EndColNum, ExcelBorderWeight bordersWeight)
        {
            for (int iRow = StartRowNum; iRow <= EndRowNum + 1; iRow++)
            {
                Range range = GetRange(iRow, StartColNum, iRow, EndColNum);
                SetCellsBorder(range, ExcelBordersEdge.EdgeTop, ExcelBordersLineStyle.Continious, bordersWeight);
            }
            for (int iCol = StartColNum; iCol < EndColNum; iCol++)
            {
                Range range = GetRange(StartRowNum, iCol, EndRowNum, iCol);
                SetCellsBorder(range, ExcelBordersEdge.EdgeRight, ExcelBordersLineStyle.Continious, bordersWeight);
            }
        }

        /// <summary>
        ///  设置某范围内所有单元格边框（不包含最左，最右边框,最顶部，最底部）
        /// </summary>
        /// <param name="StartRowNum">起始行</param>
        /// <param name="StartColNum">起始列</param>
        /// <param name="EndRowNum">结束行</param>
        /// <param name="EndRowNum">结束列</param>
        /// <param name="bordersWeight">边框线粗细</param>
        public void SetAllCellsBorderExclusiveRLTB(int StartRowNum, int StartColNum, int EndRowNum, int EndColNum, ExcelBorderWeight bordersWeight)
        {
            for (int iRow = StartRowNum + 1; iRow <= EndRowNum; iRow++)
            {
                Range range = GetRange(iRow, StartColNum, iRow, EndColNum);
                SetCellsBorder(range, ExcelBordersEdge.EdgeTop, ExcelBordersLineStyle.Continious, bordersWeight);
            }
            for (int iCol = StartColNum; iCol < EndColNum; iCol++)
            {
                Range range = GetRange(StartRowNum, iCol, EndRowNum, iCol);
                SetCellsBorder(range, ExcelBordersEdge.EdgeRight, ExcelBordersLineStyle.Continious, bordersWeight);
            }
        }

        /// <summary>
        /// 设置单元格合并集合的合并
        /// </summary>
        /// <param name="arr">_ExcelCellsMerge数组</param>
        public void SetCellsMergeSetMerge(_ExcelCellsMerge[] arr)
        {
            for (int iarr = 0; iarr < arr.Length; iarr++)
            {
                _ExcelCellsMerge excelCellsMerge = arr[iarr];
                Range range = GetRange(excelCellsMerge.StartRowNum, excelCellsMerge.StartColNum, excelCellsMerge.EndRowNum, excelCellsMerge.EndColNum);
                MergeCells(range, excelCellsMerge.HorizontalAlignment);
            }
        }

        /// <summary>
        /// 设置单元格集合的字体样式
        /// </summary>
        /// <param name="arr">_ExcelCellsFontStyle数组</param>
        public void SetCellSetFontStyle(_ExcelCellsFontStyle[] arr)
        {
            for (int iarr = 0; iarr < arr.Length; iarr++)
            {
                _ExcelCellsFontStyle excelCellsFontStyle = arr[iarr];
                Range range = GetRange(excelCellsFontStyle.StartRowNum, excelCellsFontStyle.StartColNum, excelCellsFontStyle.EndRowNum, excelCellsFontStyle.EndColNum);
                SetFontStyle(range, true, false, ExcelUnderlineStyle.NoUnderline);
            }
        }
    }
}
