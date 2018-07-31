//
//文件名：    ServiceWLKGg.aspx.cs
//功能描述：  物流控股服务接口
//创建时间：  2015/06/11
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

namespace wlkg.Service
{
    public partial class ServiceWLKG : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var serviceNum = Request.Params["ServiceNum"];
            var param0 = Request.Params["Param1"];
            var param1 = Request.Params["Param2"];
            var param2 = Request.Params["Param3"];
            var param3 = Request.Params["Param4"];
            var param4 = Request.Params["Param5"];
            string[] paramSet = new string[5];
            paramSet[0] = param0;
            paramSet[1] = param1;
            paramSet[2] = param2;
            paramSet[3] = param3;
            paramSet[4] = param4;

            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                if (serviceNum == null)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "获取今日资金存储";
                    info.Add("ServiceNum=0", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "获取今日快报";
                    info.Add("ServiceNum=1", arry1);
                    string[] arry2 = new string[1];
                    arry2[0] = "获取收入、成本月度情况";
                    info.Add("ServiceNum=2", arry2);
                    string[] arry3 = new string[1];
                    arry3[0] = "公司流动资产、负债情况";
                    info.Add("ServiceNum=3", arry3);
                    string[] arry4 = new string[1];
                    arry4[0] = "利润月度情况";
                    info.Add("ServiceNum=4", arry4);
                    string[] arry5 = new string[1];
                    arry5[0] = "本年集装箱代理量趋势分析";
                    info.Add("ServiceNum=5", arry5);
                    string[] arry6 = new string[1];
                    arry6[0] = "本年散杂货代理量趋势分析";
                    info.Add("ServiceNum=6", arry6);
                    string[] arry7 = new string[1];
                    arry7[0] = "本年船舶代理量趋势分析";
                    info.Add("ServiceNum=7", arry7);
                    string[] arry8 = new string[1];
                    arry8[0] = "今日保税进出量";
                    info.Add("ServiceNum=8", arry8);
                    string[] arry9 = new string[1];
                    arry9[0] = "今日贸易情况";
                    info.Add("ServiceNum=9", arry9);
                    string[] arry10 = new string[1];
                    arry10[0] = "今日仓储进库量";
                    info.Add("ServiceNum=10", arry10);
                    string[] arry11 = new string[1];
                    arry11[0] = "今日发运业务情况";
                    info.Add("ServiceNum=11", arry11);
                    string[] arry12 = new string[1];
                    arry12[0] = "今日B保公司业务情况";
                    info.Add("ServiceNum=12", arry12);
                    string[] arry13 = new string[1];
                    arry13[0] = "各公司年度盈利情况";
                    info.Add("ServiceNum=13", arry13);
                }
                else
                {
                    info = ServiceTool.getService(Convert.ToInt16(serviceNum), paramSet);
                }
     
                Json = JsonConvert.SerializeObject(info);
            }
            catch (Exception ex)
            {
                string[] arry = new string[1];
                arry[0] = "服务器异常：" + ex.Message;
                info.Add("Error", arry);
                Json = JsonConvert.SerializeObject(info);
            }  
        }
        protected string Json;
    }
}