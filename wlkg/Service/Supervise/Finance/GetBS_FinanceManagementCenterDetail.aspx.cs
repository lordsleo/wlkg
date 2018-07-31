//
//文件名：    GetBS_FinanceManagementCenterDetail.aspx.cs
//功能描述：  获取保税财务管理中心明细数据
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
    public partial class GetBS_FinanceManagementCenterDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //用户编码
            var codeUser = Request.Params["CodeUser"];
            //起始时间
            var startTime = Request.Params["StartTime"];
            //终止时间
            var endTime = Request.Params["EndTime"];
            //ID
            var id = Request.Params["ID"];
            //id = "LQ150914004";
            //codeUser = "121355";
            //startTime = "2015-08-11";
            //endTime = "2015-09-11";

            Dictionary<string, string> info = new Dictionary<string, string>();
            try
            {
                if (codeUser == null || startTime == null || endTime == null || id == null)
                {
                    info.Add("参数CodeUser，StartTime，EndTime，ID不能为nul！", "举例：http://218.92.115.55/wlkg/Service/Supervise/Finance/GetBS_FinanceManagementCenterDetail.aspx?CodeUser=121355&StartTime=2015-08-11&EndTime=2015-09-11&ID=LQ150914004");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string sql =
                        string.Format(@"SELECT SHIPNAME,VOYAGE,CARGO_NAME,CLIENTSHORT,BLNO_COUNT,CTNNO_COUNT,PROGRESS_NAME,EXPENSES_NAME,USERNAME,to_date(INPUT_DATE, 'yyyy-MM-dd HH24:mi :ss') as INPUT_DATE,REMARK,FANGHUO 
                                        from view_delegation where id='{0}'", 
                                        id);
                var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                if (dt0.Rows.Count == 0)
                {
                    info.Add("IsGet", "NO");
                    info.Add("Message", "ID错误！");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                startTime = Convert.ToDateTime(startTime).ToString("yyyyMMdd");
                endTime = Convert.ToDateTime(endTime).ToString("yyyyMMdd");

                string firm = "EXISTS(SELECT * FROM ACCESS_CONTROL WHERE CODE_USER='" + codeUser + "'AND VIEW_DELEGATION.IPORT_DEPT=ACCESS_CONTROL.CODE_DEPARTMENT)";
                string firmQuasi = "(SUBSTR(ORDER_DATE,1,8)>='" + startTime + "'AND SUBSTR(ORDER_DATE,1,8)<='" + endTime + "')";
               
                sql =
                    string.Format("SELECT * FROM V_ACC_ALL WHERE delegation_id IN(SELECT ID FROM VIEW_DELEGATION WHERE {0} AND {1}) ", firm, firmQuasi);
                var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                sql =
                    string.Format("select * from VW_ACC_OUTFEE t where t.work_comp_code!='0' and delegation_id IN(SELECT ID FROM VIEW_DELEGATION WHERE {0} AND {1})", firm, firm);
                var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);

                //获取应收、应付、利润
                double infeel = 0;
                double outfeel = 0;
                double profit = 0;
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (id == Convert.ToString(dt1.Rows[i]["DELEGATION_ID"]))
                    { 
                        infeel += Convert.ToDouble(dt1.Rows[i]["OVERALL_TOTAL"]);
                    }
                }
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    if (id == Convert.ToString(dt2.Rows[j]["DELEGATION_ID"]))
                    {
                        outfeel += Convert.ToDouble(dt2.Rows[j]["OVERALL_TOTAL"]);
                    }
                }
                profit = infeel - outfeel;

                //录入时间格式化
                DateTime inputDate = Convert.ToDateTime(Convert.ToString(dt0.Rows[0]["INPUT_DATE"]));
                    
                string[] nameArray = { "船名", "船次", "货物名称", "货主名称", "提单数", "箱数","业务进度","保税类型",  
                                       "录入人", "录入时间", "备注", "放货信息", "应收金额", "应付金额", "利润"};
                //排序字符串
                string order = string.Empty;
                order = nameArray[0] + "+" + nameArray[1] + "+" + nameArray[2] + "+" + nameArray[3] + "+" + nameArray[4] + "+" + nameArray[5] + "+" + nameArray[6] + "+" +
                        nameArray[7] + "+" + nameArray[8] + "+" + nameArray[9] + "+" + nameArray[10] + "+" + nameArray[11] + "+" + nameArray[12] + "+" +
                        nameArray[13] + "+" + nameArray[14];

                info.Add(nameArray[0], Convert.ToString(dt0.Rows[0]["SHIPNAME"]));
                info.Add(nameArray[1], Convert.ToString(dt0.Rows[0]["VOYAGE"]));
                info.Add(nameArray[2], Convert.ToString(dt0.Rows[0]["CARGO_NAME"]));
                info.Add(nameArray[3], Convert.ToString(dt0.Rows[0]["CLIENTSHORT"]));
                info.Add(nameArray[4], Convert.ToString(dt0.Rows[0]["BLNO_COUNT"]));
                info.Add(nameArray[5], Convert.ToString(dt0.Rows[0]["CTNNO_COUNT"]));
                info.Add(nameArray[6], Convert.ToString(dt0.Rows[0]["PROGRESS_NAME"]));
                info.Add(nameArray[7], Convert.ToString(dt0.Rows[0]["EXPENSES_NAME"]));
                info.Add(nameArray[8], Convert.ToString(dt0.Rows[0]["USERNAME"]));
                info.Add(nameArray[9], inputDate.ToString("yyyy-MM-dd"));
                info.Add(nameArray[10], Convert.ToString(dt0.Rows[0]["REMARK"]));
                info.Add(nameArray[11], Convert.ToString(dt0.Rows[0]["FANGHUO"]));
                info.Add(nameArray[12], infeel.ToString("F2"));
                info.Add(nameArray[13], outfeel.ToString("F2"));
                info.Add(nameArray[14], profit.ToString("F2"));
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