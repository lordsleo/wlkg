//
//文件名：    GetFreightBuiness.aspx.cs
//功能描述：  获取货运业务数据
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
    public partial class GetFreightBuiness : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //起始时间
            var startTime = Request.Params["StartTime"];
            //终止时间
            var endTime = Request.Params["EndTime"];
            //startTime = "2013-01-01";
            //endTime = "2015-07-09";

            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                if (startTime == null || endTime == null)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "举例：http://218.92.115.55/wlkg/Service/Supervise/Business/GetFreightBuiness.aspx?StartTime=2015-01-01&EndTime=2015-07-09";
                    info.Add("参数StartTime，EndTime不能为nul！", arry0);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                startTime = startTime + " 00:00:00";
                endTime = endTime + " 00:00:00";

                GetData.AuthHeader head = new GetData.AuthHeader();
                head.UserName = "HDXT";
                head.PassWord = "FeW*2k6t";

                GetData.WebServiceSoapClient fileDeal = new GetData.WebServiceSoapClient();
                var dt = fileDeal.GetYWByTime_HYGS(head, startTime, endTime);
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
                    ary[iRow, 0] = (iRow + 1).ToString();
                    ary[iRow, 1] = Convert.ToString(dt.Rows[iRow]["TradeType"]);
                    ary[iRow, 2] = Convert.ToString(dt.Rows[iRow]["ChiVessel"]);
                    ary[iRow, 3] = Convert.ToString(dt.Rows[iRow]["Voyage"]);
                    ary[iRow, 4] = Convert.ToString(dt.Rows[iRow]["Blno"]);
                    ary[iRow, 5] = Convert.ToString(dt.Rows[iRow]["ClientNameS"]);
                    ary[iRow, 6] = Convert.ToString(dt.Rows[iRow]["CargoName"]);
                }

                string[] arry2 = new string[1];
                arry2[0] = "Yes";
                info.Add("IsGet", arry2);
                info.Add("FreightBuiness", ary);
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