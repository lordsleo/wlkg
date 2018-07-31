//
//文件名：    GetFreightBuinessDetail.aspx.cs
//功能描述：  获取货运业务明细数据
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
    public partial class GetFreightBuinessDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //起始时间
            var startTime = Request.Params["StartTime"];
            //终止时间
            var endTime = Request.Params["EndTime"];
            //行ID
            var id = Request.Params["ID"];
            //startTime = "2015-01-01";
            //endTime = "2015-07-09";
            //id = "1";
            int rowNum = Convert.ToInt16(id) -  1;

            Dictionary<string, string> info = new Dictionary<string, string>();
            try
            {
                if (startTime == null || endTime == null || id == null)
                {
                    info.Add("参数StartTime，EndTime，ID不能为nul！", "举例：http://218.92.115.55/wlkg/Service/Supervise/Business/GetFreightBuinessDetail.aspx?StartTime=2015-01-01&EndTime=2015-07-09&ID=1");
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
                    info.Add("IsGet", "NO");
                    info.Add("Message", "ID错误！");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string[] nameArray = { "抵锚", "靠泊", "报关", "海关放行", "报检", "国检放行", "卸船开始","卸船结束",  
                                       "离港", "完船", "发运结束", "进出口", "内外贸", "船名", "航次", "提单号", "客户名称","货名",  
                                       "部门", "操作员", "时间"};
                //排序字符串
                string order = string.Empty;
                order = nameArray[0] + "+" + nameArray[1] + "+" + nameArray[2] + "+" + nameArray[3] + "+" + nameArray[4] + "+" + nameArray[5] + "+" + nameArray[6] + "+" +
                        nameArray[7] + "+" + nameArray[8] + "+" + nameArray[9] + "+" + nameArray[10] + "+" + nameArray[11] + "+" + nameArray[12] + "+" +
                        nameArray[13] + "+" + nameArray[14] + "+" + nameArray[15] + "+" + nameArray[16] + "+" + nameArray[17] + "+" + nameArray[18] + "+" +
                        nameArray[19] + "+" + nameArray[20];

                const string FINISH = "已完成";
                const string WAIT = "待办";
                info.Add(nameArray[0], dt.Rows[rowNum]["ArrivedAnchorTime"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[1], dt.Rows[rowNum]["Berthingtime"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[2], dt.Rows[rowNum]["CustomClearanceTime"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[3], dt.Rows[rowNum]["CustomFinalClearanceTime"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[4], dt.Rows[rowNum]["CIQInspectionTime"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[5], dt.Rows[rowNum]["CIQInspectionReleaseTime"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[6], dt.Rows[rowNum]["UnloadingTime_B"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[7], dt.Rows[rowNum]["UnloadingTime_E"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[8], dt.Rows[rowNum]["EtaSailingTime"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[9], dt.Rows[rowNum]["FinshVesselTime"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[10], dt.Rows[rowNum]["DeliveryEndTime"].ToString() == "1" ? FINISH : WAIT);
                info.Add(nameArray[11], dt.Rows[rowNum]["IEMark"].ToString());
                info.Add(nameArray[12], dt.Rows[rowNum]["TradeType"].ToString());
                info.Add(nameArray[13], dt.Rows[rowNum]["ChiVessel"].ToString());
                info.Add(nameArray[14], dt.Rows[rowNum]["Voyage"].ToString());
                info.Add(nameArray[15], dt.Rows[rowNum]["Blno"].ToString());
                info.Add(nameArray[16], dt.Rows[rowNum]["ClientNameS"].ToString());
                info.Add(nameArray[17], dt.Rows[rowNum]["CargoName"].ToString());
                info.Add(nameArray[18], dt.Rows[rowNum]["DeptName"].ToString());
                info.Add(nameArray[19], dt.Rows[rowNum]["Operator"].ToString());
                info.Add(nameArray[20], dt.Rows[rowNum]["DTime"].ToString());
                info.Add("Order", order);

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