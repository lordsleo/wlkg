//
//文件名：    GetShippingFinanceDetail.aspx.cs
//功能描述：  获取船务财务明细数据
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

namespace wlkg.Service.Supervise.Finance
{
    public partial class GetShippingFinanceDetail1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //起始时间
            var startTime = Request.Params["StartTime"];
            //终止时间
            var endTime = Request.Params["EndTime"];
            //行ID
            var id = Request.Params["ID"];
            //startTime = "2015-06-11";
            //endTime = "2015-07-09";
            //id = "1";
            int rowNum = Convert.ToInt16(id) - 1;

            Dictionary<string, string> info = new Dictionary<string, string>();
            try
            {
                if (startTime == null || endTime == null || id == null)
                {
                    info.Add("参数StartTime，EndTime，ID不能为nul！", "举例：http://218.92.115.55/wlkg/Service/Supervise/Finance/GetShippingFinanceDetail.aspx?StartTime=2015-01-01&EndTime=2015-07-09&ID=1");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                startTime = startTime + " 00:00:00";
                endTime = endTime + " 00:00:00";

                GetData.AuthHeader head = new GetData.AuthHeader();
                head.UserName = "HDXT";
                head.PassWord = "FeW*2k6t";

                GetData.WebServiceSoapClient fileDeal = new GetData.WebServiceSoapClient();
                var dt = fileDeal.GetPayReceiveBillByTime_CWGS(head, startTime, endTime);
                if (dt.Rows.Count == 0)
                {
                    info.Add("IsGet", "NO");
                    info.Add("Message", "暂无数据！");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                info.Add("进出口", dt.Rows[rowNum]["IEMark"].ToString());
                info.Add("内外贸", dt.Rows[rowNum]["TradeType"].ToString());
                info.Add("船名", dt.Rows[rowNum]["ChiVessel"].ToString());
                info.Add("航次", dt.Rows[rowNum]["Voyage"].ToString());
                info.Add("提单号", dt.Rows[rowNum]["Blno"].ToString());
                info.Add("客户名称", dt.Rows[rowNum]["ClientNameS"].ToString());
                info.Add("箱型箱量", dt.Rows[rowNum]["CtnNum"].ToString());
                info.Add("应付人民币总额", dt.Rows[rowNum]["PayRMBAmounts"].ToString());
                info.Add("应收人民币总额", dt.Rows[rowNum]["ReceiveRMBAmounts"].ToString());
                info.Add("部门", dt.Rows[rowNum]["DTime"].ToString());
                info.Add("时间", dt.Rows[rowNum]["DeptName"].ToString());

                Json = JsonConvert.SerializeObject(info);
            }
            catch (Exception ex)
            {
                info.Add("IsGet", "NO");
                info.Add("Message", "服务器异常：" + ex.Message);
                Json = JsonConvert.SerializeObject(info);
            }
        }
        protected string Json;
    }
}