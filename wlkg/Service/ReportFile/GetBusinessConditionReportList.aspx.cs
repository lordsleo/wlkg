//
//文件名：    GetBusinessConditionReportList.aspx.cs
//功能描述：  获取经营情况报表文件列表
//创建时间：  2015/07/14
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
using Leo;

namespace wlkg.Service.ReportFile
{
    public partial class GetBusinessConditionReportList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //查询页数
            var pages = Request.Params["Pages"];

            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                if (pages == null)
                {
                    string[] arry = new string[1];
                    arry[0] = "举例：http://218.92.115.55/wlkg/Service/ReportFile/GetBusinessConditionReportList.aspx?Pages=1";
                    info.Add("参数Pages不能为null！", arry);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                int minRow = (Convert.ToInt16(pages) - 1) * 15 + 1;
                int maxRow = Convert.ToInt16(pages) * 15;
                string reportName = "经营情况";
                string sql =
                    string.Format(
                            "select count(id) as total from REPORT_FILE where reportname like '%{0}%'", reportName);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "No";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无报表！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                sql =
                    string.Format(
                        "select id,reportname,reporturl,createtime from (select * from (select id,reportname,'http://218.92.115.100:81/wlbi/report/' || reportname as reporturl,createtime from REPORT_FILE  where reportname like '%{0}%' order by createtime asc) where rownum<={1} order by createtime desc) where rownum<={2}",
                         reportName, Convert.ToInt32(dt.Rows[0]["total"]) - minRow + 1, maxRow - minRow + 1);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "No";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无更多报表！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string[,] arrys = new string[dt.Rows.Count, 4];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {        
                    arrys[iRow, 0] = Convert.ToString(dt.Rows[iRow]["id"]);
                    arrys[iRow, 1] = Convert.ToString(dt.Rows[iRow]["reportname"]);
                    arrys[iRow, 2] = Convert.ToString(dt.Rows[iRow]["reporturl"]);
                    arrys[iRow, 3] = Convert.ToString(dt.Rows[iRow]["createtime"]);
                }

                string[] arry2 = new string[1];
                arry2[0] = "Yes";
                info.Add("IsGet", arry2);
                info.Add("ReportFileList", arrys);
                Json = JsonConvert.SerializeObject(info);
            }
            catch (Exception ex)
            {
                string[] arry0 = new string[1];
                arry0[0] = "No";
                info.Add("IsGet", arry0);
                string[] arry1 = new string[1];
                arry1[0] = "服务器异常：" + ex.Message;
                info.Add("Message", arry1);
                Json = JsonConvert.SerializeObject(info);
            }

        }
        protected string Json;
    }
}