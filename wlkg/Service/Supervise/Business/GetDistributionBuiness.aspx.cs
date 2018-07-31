//
//文件名：    GetDistributionBuiness.aspx.cs
//功能描述：  获取物流配送业务流程数据
//创建时间：  2015/07/09
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

namespace wlkg.Service.Supervise.Business
{
    public partial class GetDistributionBuiness : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                string sql =
                    string.Format("select * from V_TB_CONSIGN_COURSE order by task_no desc");
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(sql);

                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "NO";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无数据！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string[,] ary = new string[dt.Rows.Count, 8];
                const string FINISH = "已完成";
                const string WAIT = "待办";
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    ary[iRow, 0] = Convert.ToString(dt.Rows[iRow]["TASK_NO"]);
                    ary[iRow, 1] = Convert.ToString(dt.Rows[iRow]["VESSEL"]);
                    ary[iRow, 2] = Convert.ToString(dt.Rows[iRow]["VOYAGE"]);
                    ary[iRow, 3] = Convert.ToString(dt.Rows[iRow]["PLAN_MARK"]) == "1" ? FINISH : WAIT;
                    ary[iRow, 4] = Convert.ToString(dt.Rows[iRow]["TRANS_MARK"]) == "1" ? FINISH : WAIT;
                    ary[iRow, 5] = Convert.ToString(dt.Rows[iRow]["ZHIDAN_MARK"]) == "1" ? FINISH : WAIT;
                    ary[iRow, 6] = Convert.ToString(dt.Rows[iRow]["BILL_MARK"]) == "1" ? FINISH : WAIT;
                    ary[iRow, 7] = Convert.ToString(dt.Rows[iRow]["FINISH_MARK"]) == "1" ? FINISH : WAIT;
                }

                string[] arry2 = new string[1];
                arry2[0] = "Yes";
                info.Add("IsGet", arry2);
                info.Add("DistributionBuiness", ary);
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