//
//文件名：    GetCompanyNewsList.aspx.cs
//功能描述：  获取公司要闻新闻列表
//创建时间：  2015/07/06
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

namespace wlkg.Service.News.CompanyNews
{
    public partial class GetCompanyNewsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //用户ID
            var userId = Request.Params["UserId"];
            //查询页数
            var pages = Request.Params["Pages"];
            //标题筛选
            var filter = Request.Params["Filter"];

            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                if (pages == null || filter == null)
                {
                    string[] arry = new string[1];
                    arry[0] = "http://218.92.115.55/wlkg/Service/News/CompanyNews/GetCompanyNewsList.aspx?UserId=&Pages=1&Filter=";
                    info.Add("参数UserId,Pages，Filter不能为空！举例", arry);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                int minRow = (Convert.ToInt16(pages) - 1) * 15 + 1;
                int maxRow = Convert.ToInt16(pages) * 15;
                int msgValidDays = Convert.ToInt32(FileTool.GetWebConfigKey("NewsValidDays"));
                const string newsType = "03.01";

                string rsql =
                    string.Format(
                        "SELECT newsid, mark, source, userid FROM news_mark WHERE (source = 'mobile' and newstype='{0}' and userid='{1}')",
                         newsType, userId);
                var rdt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(rsql);

                string Strfilter = null;
                if (!string.IsNullOrEmpty(filter) && !string.IsNullOrWhiteSpace(filter))
                {
                    Strfilter = string.Format(" and TOPIC like '%{0}%' ", filter);
                }

                string sql =
                    string.Format(
                            "select count(ID) as total from News where news_type = '{0}' and audit_mark='1' {1} ", newsType, Strfilter);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "NO";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无新闻！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                sql =
                    string.Format(
                        "select id,topic,audittime,mark_new from (select * from (select id,topic,audittime,CASE WHEN audittime >= sysdate - {0} THEN '1' ELSE '0' END AS mark_new from News where NEWS_TYPE = '{1}' and audit_mark='1' {2} order by audittime asc) where rownum<={3} order by audittime desc) where rownum<={4}",
                        msgValidDays, newsType, Strfilter, Convert.ToInt32(dt.Rows[0]["total"]) - minRow + 1, maxRow - minRow + 1);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "NO";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无更多新闻！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string[,] arrys = new string[dt.Rows.Count, 6];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    arrys[iRow, 0] = Convert.ToString(dt.Rows[iRow]["id"]);
                    arrys[iRow, 1] = Convert.ToString(dt.Rows[iRow]["topic"]);
                    arrys[iRow, 2] = Convert.ToString(dt.Rows[iRow]["audittime"]);
                    arrys[iRow, 3] =
                            ("1" == Convert.ToString(dt.Rows[iRow]["mark_new"]) &&
                             rdt.Select(string.Format("newsid='{0}'", Convert.ToString(dt.Rows[iRow]["id"]))).Length == 0)
                                ? "1"
                                : "0";
                    arrys[iRow, 4] = "公司要闻";
                    arrys[iRow, 5] = newsType;
                }

                string[] arry2 = new string[1];
                arry2[0] = "Yes";
                info.Add("IsGet", arry2);
                info.Add("ConmpanyNewsList", arrys);
                Json = JsonConvert.SerializeObject(info);       
            }
            catch (Exception ex)
            {
                string[] arry0 = new string[1];
                arry0[0] = "NO";
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