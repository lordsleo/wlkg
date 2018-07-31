//
//文件名：    TodayReport.aspx.cs
//功能描述：  今日快报
//创建时间：  2015/06/15
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
using ServiceInterface.Common;

namespace wlkg.Page
{
    public partial class TodayReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string[] reportSet = new string[6];
                //今日仓储进库量
                string sql =
                        string.Format("select round(decode(sum(weight), NULL, 0, sum(weight))) as inweight from storageharbor.vw_hs_goodsbill_tallybill_b_i where code_company = '6377' and signdate = (SELECT MAX(storageharbor.vw_hs_goodsbill_tallybill_b_i.signdate) FROM storageharbor.vw_hs_goodsbill_tallybill_b_i)");
                var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                reportSet[0] = string.Format("今日仓储进库量（{0}）吨", dt0.Rows[0]["inweight"].ToString());
                //今日仓储出库量
                sql =
                    string.Format("select round(decode(sum(weight), NULL, 0, sum(weight))) as outweight from storageharbor.vw_hs_goodsbill_tallybill_b_o where code_company = '6377' and signdate = (SELECT MAX(storageharbor.vw_hs_goodsbill_tallybill_b_o.signdate) FROM storageharbor.vw_hs_goodsbill_tallybill_b_o)");
                var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                reportSet[1] = string.Format("今日仓储出库量（{0}）吨", dt1.Rows[0]["outweight"].ToString());
                //今日仓储场存量
                sql =
                    string.Format("select round(weight,2) as weight,signdate from (select sum(t.weight) weight,t.signdate from storageharbor.TB_HS_STOCKDORMANTHISTORY t where to_number(to_char(signdate,'yyyy'))>=2014 group by t.signdate  order by signdate desc) where rownum=1");
                var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                reportSet[2] = string.Format("今日仓储场存量（{0}）吨", dt2.Rows[0]["weight"].ToString());
                //散杂货代理量
                sql =
                    string.Format("select sum(amount) as sum from BUSINESS_INFO t where code_type='代理量' and t.actualtime>=sysdate -7");
                var dt3 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                reportSet[3] = string.Format("本周 散杂货代理量（{0}）吨", dt3.Rows[0]["sum"].ToString());
                //集装箱代理量
                sql =
                    string.Format("select sum(amount) as sum from BUSINESS_INFO t where code_type='集装箱' and t.actualtime>=sysdate -7");
                var dt4 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                reportSet[4] = string.Format("本周 集装箱代理量（{0}）TUE", dt4.Rows[0]["sum"].ToString());
                //船舶代理量
                sql =
                    string.Format("select sum(amount) as sum from BUSINESS_INFO t where code_type='船舶' and t.actualtime>=sysdate -7");
                var dt5 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                reportSet[5] = string.Format("本周 船舶代理量（{0}）艘次", dt5.Rows[0]["sum"].ToString());
                Json = JsonConvert.SerializeObject(reportSet);

            }
            catch (Exception ex)
            {
                LogTool.WriteLog(typeof(TodayReport), ex);
            }
        }
        protected string Json;
    }
}