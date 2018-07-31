//
//文件名：    GetCC_BusinessManagementCenter.aspx.cs
//功能描述：  获取仓储业务管理中心数据
//创建时间：  2015/09/15
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
    public partial class GetCC_BusinessManagementCenter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var codeUser = Request.Params["CodeUser"];
            //起始时间
            var startTime = Request.Params["StartTime"];
            //终止时间
            var endTime = Request.Params["EndTime"];

            startTime = "2015-08-15";
            endTime = "2015-09-15";

            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                if (startTime == null || endTime == null)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "举例：http://218.92.115.55/wlkg/Service/Supervise/Business/GetCC_BusinessManagementCenter.aspx?StartTime=2015-08-11&EndTime=2015-09-11";
                    info.Add("参数StartTime，EndTime不能为nul！", arry0);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                startTime = Convert.ToDateTime(startTime).ToString("yyyy/MM/dd") + "00:00:00";
                endTime = Convert.ToDateTime(endTime).ToString("yyyy/MM/dd") + "00:00:00";

                string sql =
                    string.Format(@"select gbno,client,cargo,pack,pieceweight,planamount,planweight,vgdisplay,blno,remark,mark_sub,mark_makeiv 
                                    from storageharbor.vw_hc_goodbill_landfee 
                                    where  signdate>=to_date('{0}','yyyy-MM-dd hh24:mi:ss') and signdate<=to_date('{1}','yyyy-MM-dd hh24:mi:ss')  order by signdate desc",
                                    startTime, endTime);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);

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

                string[,] ary = new string[dt.Rows.Count, 7];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    ary[iRow, 0] = Convert.ToString(dt.Rows[iRow]["gbno"]);
                    ary[iRow, 1] = Convert.ToString(dt.Rows[iRow]["client"]);
                    ary[iRow, 2] = Convert.ToString(dt.Rows[iRow]["cargo"]);
                    ary[iRow, 3] = Convert.ToString(dt.Rows[iRow]["pack"]);
                    ary[iRow, 4] = Convert.ToString(dt.Rows[iRow]["pieceweight"]);
                    ary[iRow, 5] = Convert.ToString(dt.Rows[iRow]["vgdisplay"]);
                    ary[iRow, 6] = Convert.ToString(dt.Rows[iRow]["blno"]);
                }

                string[] arry2 = new string[1];
                arry2[0] = "Yes";
                info.Add("IsGet", arry2);
                info.Add("BusinessManagementCenter", ary);
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