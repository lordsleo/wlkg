//
//文件名：    ExcelExport.cs
//功能描述：  Excel枚举类型
//创建时间：  2015/04/28
//作者：      sh
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace wlkg.Common
{
    /// <summary>
    /// Excel单元格字体下划线方式
    /// </summary>
    public enum ExcelUnderlineStyle
    {
        /// <summary>
        /// 无下划线
        /// </summary>
        NoUnderline = -4142,
        /// <summary>
        /// 双线
        /// </summary>
        DoubleLine = -4119,
        /// <summary>
        /// 双线充满全格
        /// </summary>
        DoubleLineFull = 5,
        /// <summary>
        /// 单线
        /// </summary>
        NoSingeLine = 2,
        /// <summary>
        /// 单线充满全格
        /// </summary>
        SingeLineFull = 4
       
    }

    /// <summary>
    /// Excel单元格填充方式
    /// </summary>
    public enum Pattern
    {
        Automatic = -4105,
        Checker = 9,
        CrissCross = 16,
        Down = -4121,
        Gray16 = 17,
        Gray25 = -4124,
        Gray50 = -4125,
        Gray75 = -4126,
        Gray8 = 18,
        Grid = 15,
        Horizontal = -4128,
        LightDown = 13,
        LightHorizontal = 11,
        LightUp = 14,
        LightVertical = 12,
        None = -4142,
        SemiGray75 = 10,
        Solid = 1,
        Up = -4162,
        Vertical = -4166
    }

    /// <summary>
    /// Excel单元格边框枚举
    /// </summary>
    public enum ExcelBordersEdge
    {
        /// <summary>
        /// 主对角线从
        /// </summary>
        DiagonalDown = Excel.XlBordersIndex.xlDiagonalDown,
        /// <summary>
        /// 辅对角线
        /// </summary>
        DiagonUp = Excel.XlBordersIndex.xlDiagonalUp,
        /// <summary>
        ///底边框
        /// </summary>
        EdgeBottom = Excel.XlBordersIndex.xlEdgeBottom,
        /// <summary>
        /// 左边框
        /// </summary>
        EdgeLeft = Excel.XlBordersIndex.xlEdgeLeft,
        /// <summary>
        /// 右边框
        /// </summary>
        EdgeRight = Excel.XlBordersIndex.xlEdgeRight,
        /// <summary>
        /// 顶边框
        /// </summary>
        EdgeTop = Excel.XlBordersIndex.xlEdgeTop,
        /// <summary>
        /// 边框内水平横线
        /// </summary>
        InsideHorizontal = Excel.XlBordersIndex.xlInsideHorizontal,
        /// <summary>
        /// 边框内垂直竖线
        /// </summary>
        InsideVertical = Excel.XlBordersIndex.xlInsideVertical,
         /// <summary>
        ///  无边框（所有范围）
        /// </summary>       
        LineStyleNone = Excel.XlLineStyle.xlLineStyleNone
    }

    /// <summary>
    /// Excel单元格边框的线条的粗细枚举
    /// </summary>
    public enum ExcelBorderWeight
    {
        /// <summary>
        /// 极细的线条
        /// </summary>
        Hairline = Excel.XlBorderWeight.xlHairline,
        /// <summary>
        /// 中等的线条
        /// </summary>
        Medium = Excel.XlBorderWeight.xlMedium,
        /// <summary>
        /// 粗线条
        /// </summary>
        Thick = Excel.XlBorderWeight.xlThick,
        /// <summary>
        /// 细线条
        /// </summary>
        Thin = Excel.XlBorderWeight.xlThin
    }

    /// <summary>
    /// Excel边框线条的枚举
    /// </summary>
    public enum ExcelBordersLineStyle
    {
        /// <summary>
        /// 没有线条
        /// </summary>
        StyleNone = Excel.XlLineStyle.xlLineStyleNone,
        /// <summary>
        /// 连续的细线
        /// </summary>
        Continious = Excel.XlLineStyle.xlContinuous,
        /// <summary>
        /// 点状线
        /// </summary>
        Dot = Excel.XlLineStyle.xlDot,
        /// <summary>
        /// 双条线
        /// </summary>
        Double = Excel.XlLineStyle.xlDouble,
        /// <summary>
        /// 虚线
        /// </summary>
        Dash = Excel.XlLineStyle.xlDash,
        /// <summary>
        /// 点划线
        /// </summary>
        DashDot = Excel.XlLineStyle.xlDashDot,
        /// <summary>
        /// 双点划线
        /// </summary>
        DashDotDot = Excel.XlLineStyle.xlDashDotDot,
        /// <summary>
        /// 斜冲点
        /// </summary>
        SlantDashDot = Excel.XlLineStyle.xlSlantDashDot
    }

    /// <summary>
    /// Excel单元格的竖直方法对齐枚举
    /// </summary>
    public enum ExcelVerticalAlignment
    {
        /// <summary>
        /// 居中
        /// </summary>
        Center = Excel.Constants.xlCenter,
        /// <summary>
        /// 靠上
        /// </summary>
        Top = Excel.Constants.xlTop,
        /// <summary>
        /// 靠下
        /// </summary>
        Bottom = Excel.Constants.xlBottom,
        /// <summary>
        /// 两端对齐
        /// </summary>
        Justify = Excel.Constants.xlJustify,
        /// <summary>
        /// 分散对齐
        /// </summary>
        Distributed = Excel.Constants.xlDistributed
    };
 
    /// <summary>
    /// Excel 水平方向对齐枚举
    /// </summary>
    public enum ExcelHorizontalAlignment
    {
        /// <summary>
        ///常规
        /// </summary>
        General = Excel.Constants.xlGeneral,
        /// <summary>
        /// 靠左
        /// </summary>
        Left = Excel.Constants.xlLeft,
        /// <summary>
        /// 居中
        /// </summary>
        Center = Excel.Constants.xlCenter,
        /// <summary>
        /// 靠右
        /// </summary>
        Right = Excel.Constants.xlRight,
        /// <summary>
        /// 填充
        /// </summary>
        Fill = Excel.Constants.xlFill,
        /// <summary>
        /// 两端对齐
        /// </summary>
        Justify = Excel.Constants.xlJustify,
        /// <summary>
        /// 跨列居中
        /// </summary>
        CenterAcrossSelection = Excel.Constants.xlCenterAcrossSelection,
        /// <summary>
        /// 分散对齐
        /// </summary>
        Distributed = Excel.Constants.xlDistributed
    }

    /// <summary>
    /// Excel单元格文字方向
    /// </summary>
    public enum ExcelTextOrientation
    {
        /// <summary>
        /// 垂直
        /// </summary>
        Vertical = Excel.XlOrientation.xlVertical,
        /// <summary>
        /// 水平
        /// </summary>
        Horizontal = Excel.XlOrientation.xlHorizontal,
        /// <summary>
        /// 朝下
        /// </summary>
        Downward = Excel.XlOrientation.xlDownward,
        /// <summary>
        /// 朝上
        /// </summary>
        Upward = Excel.XlOrientation.xlUpward
    }
   
    /// <summary>
    /// 排序的玫举
    /// </summary>
    public enum ExcelSortOrder
    {
        /// <summary>
        /// 升序
        /// </summary>
        Ascending = Excel.XlSortOrder.xlAscending,
        /// <summary>
        /// 降序
        /// </summary>
        Descending = Excel.XlSortOrder.xlDescending,
    }
}
