//
//文件名：    GetBS_BusinessCondictionReport.aspx.cs
//功能描述：  获取保税部经营情况报表
//创建时间：  2015/08/31
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data.OracleClient;
using Leo;
using DataTable = System.Data.DataTable;
using wlkg.Common;
using Microsoft.Office.Interop.Excel;

namespace wlkg.Service.ReportFile
{
    public partial class GetBS_BusinessCondictionReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var codeUser = Request.Params["CodeUser"];
            //起始时间
            var startTime = Request.Params["StartTime"];
            //终止时间
            var endTime = Request.Params["EndTime"];

            //codeUser = "121731";
            //startTime = "2015-08-09";
            //endTime = "2015-09-09";

            Dictionary<string, string> info = new Dictionary<string, string>();
            try
            {
                if (codeUser == null || startTime == null || endTime == null)
                {
                    info.Add("参数CodeUser，StartTime，EndTime不能为nul！", "举例：http://218.92.115.55/wlkg/Service/ReportFile/GetBS_BusinessCondictionReport.aspx?CodeUser=121355&StartTime=2014-06-01&EndTime=2015-07-09");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string functionName = "pg_BusinessConditionReport.GenBusinessConditionReport";             
                OracleParameter[] p = 
                {
                    new OracleParameter("codeuser", OracleType.VarChar),
                    new OracleParameter("startdate", OracleType.VarChar),
                    new OracleParameter("enddate", OracleType.VarChar),
                    new OracleParameter("dele_type", OracleType.VarChar),
                };
                p[0].Value = codeUser;
                p[1].Value = Convert.ToDateTime(startTime).ToString("yyyyMMdd");
                p[2].Value = Convert.ToDateTime(endTime).ToString("yyyyMMdd");
                p[3].Value = "1";
                p[0].Direction = System.Data.ParameterDirection.Input;
                p[1].Direction = System.Data.ParameterDirection.Input;
                p[2].Direction = System.Data.ParameterDirection.Input;
                p[3].Direction = System.Data.ParameterDirection.Input;
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteStoredProcedure(functionName, p);

                string fileName = "保税部经营情况表" + codeUser + p[1].Value + p[2].Value;
                string serverFileName = "保税部经营情况表" + codeUser;
                string serverFilePath = "http://218.92.115.55/UploadFile/wlkg/report/";
                //导出Excel
                ToExcel(startTime, endTime, serverFileName, dt);

                info.Add("IsGet", "Yes");
                info.Add("ReportName", fileName + ".xls");
                info.Add("ReportUrl", serverFilePath + serverFileName + ".xls");
                info.Add("CreateTime", Convert.ToString(DateTime.Now));
                Json = JsonConvert.SerializeObject(info);
            }
            catch (Exception ex)
            {
                info.Add("IsGet", "No");
                info.Add("Message", "服务器异常：" + ex.Message);
                Json = JsonConvert.SerializeObject(info);
            }
        }
        protected string Json;

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        private static void ToExcel(string startTime, string endTime, string fileName, DataTable dt)
        {
            ReportExportToExcel xlExport = new ReportExportToExcel();
            string filePath = "E:\\Publish\\UploadFile\\wlkg\\report\\";
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            string saveFilePath = string.Format("{0}{1}.xls", filePath, fileName); 
            try
            {
                xlExport.CreatExcelSheets();
                xlExport.SetCurActiveSheet();            
                //输出保税部经营情况表头到Excel
                _WriteHeader(startTime, endTime, xlExport);
                //输出保税部经营情况表数据到Excel
                _WriteBody(dt, xlExport);
                //保存
                xlExport.save(saveFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //释放
                xlExport.Dispose();
            }
        }


        /// <summary>
        /// 输出表头
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">终止时间</param>
        /// <param name="xlExport">xlExport对象</param>
        private static void _WriteHeader(string startTime, string endTime, ReportExportToExcel xlExport)
        {
            //插入文字
            _ExcelCellValue[] excelCellValue = new _ExcelCellValue[]
            {
                new _ExcelCellValue(1, 3, "保税部" + startTime + "至" + endTime + "经营情况统计"),
                new _ExcelCellValue(2, 1, "项目"),
                new _ExcelCellValue(2, 2, "船名"),
                new _ExcelCellValue(2, 3, "货主"),
                new _ExcelCellValue(2, 4, "货名"),
                new _ExcelCellValue(2, 5, "数量（吨）/TEU"),
            };

            //插入文字
            xlExport.SetCellSetValue(excelCellValue, 1, 1, 2, 5);
            //设置列宽(第一列到最后一列)
            xlExport.SetColumnWidth(1, 5, 20);
            //设置行高
            xlExport.SetRowHeight(1, 2, 14);
            //设置（行1，列1，行2，列5）居中
            Range range = xlExport.GetRange(1, 1, 2, 5);
            xlExport.SetCellsAlignment(range);
            //设置表头边框
            xlExport.SetAllCellsBorderExclusiveRL(2, 1, 2, 5, ExcelBorderWeight.Thin);
        }

        /// <summary>
        /// 输出表数据
        /// </summary>
        /// <param name="dt">DataTable对称</param>
        /// <param name="xlExport">ReportExportToExcel对象</param>
        private static void _WriteBody(DataTable dt, ReportExportToExcel xlExport)
        {
            int headerRows = 2;

            //插入数据集报表数据
            xlExport.DataTableToExcel(dt, 1, 1, dt.Rows.Count, 5, headerRows + 1, 1);
            //设置数报表数据单元格为文本形式
            Range range = xlExport.GetRange(headerRows + 1, 1, dt.Rows.Count + headerRows + 1, 5);
            xlExport.SetTextForm(range);
            //设置行高
            xlExport.SetRowHeight(headerRows + 1, headerRows + dt.Rows.Count + 1, 14);
            //设置数据集报表数据对齐方式（行3，列1，行末，列5）
            range = xlExport.GetRange(headerRows + 1, 1, dt.Rows.Count + headerRows, 5);
            xlExport.SetCellsAlignment(range, ExcelHorizontalAlignment.Center);
            //设置报表数据边框
            xlExport.SetAllCellsBorderExclusiveRLTB(headerRows + 1, 1, dt.Rows.Count + headerRows, 5, ExcelBorderWeight.Thin);
        }
    }
}