//
//文件名：    GetBillingList.aspx.cs
//功能描述：  获取发票信息列表
//创建时间：  2015/07/18
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

namespace wlkg.Service.Approval.BillingApproval
{
    public partial class GetBillingList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //用户ID
            var delegationId = Request.Params["DelegationId"];

            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                if (delegationId == null)
                {
                    string[] arry = new string[1];
                    arry[0] = "举例：http://218.92.115.55/wlkg/Service/Approval/BillingApproval/GetBillingList.aspx?DelegationId=1";
                    info.Add("参数DelegationId不能为null！", arry);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string sql =
                    string.Format(
                        @"select '0' AS flag , SIGNDATE, CLIENTFULL, CHARGERATE, EXPENSE_NAME, SUMMARY, QUANTITY, PRICE, TOTAL, MARK_AUDIT, AUDITORNAME, ACCID, DELEGATION_ID ,TFNO
                          from tb_ht_tariff t where t.isaudit='0' AND DELEGATION_ID ='{0}'",
                          delegationId);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "No";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无发票信息！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string[,] arrys = new string[dt.Rows.Count, 12];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    arrys[iRow, 0] = Convert.ToString(dt.Rows[iRow]["SIGNDATE"]);
                    arrys[iRow, 1] = Convert.ToString(dt.Rows[iRow]["CLIENTFULL"]);
                    arrys[iRow, 2] = Convert.ToString(dt.Rows[iRow]["CHARGERATE"]);
                    arrys[iRow, 3] = Convert.ToString(dt.Rows[iRow]["EXPENSE_NAME"]);
                    arrys[iRow, 4] = Convert.ToString(dt.Rows[iRow]["SUMMARY"]);
                    arrys[iRow, 5] = Convert.ToString(dt.Rows[iRow]["QUANTITY"]);
                    arrys[iRow, 6] = Convert.ToString(dt.Rows[iRow]["PRICE"]);
                    arrys[iRow, 7] = Convert.ToString(dt.Rows[iRow]["TOTAL"]);
                    arrys[iRow, 8] = Convert.ToString(dt.Rows[iRow]["MARK_AUDIT"]);
                    arrys[iRow, 9] = Convert.ToString(dt.Rows[iRow]["AUDITORNAME"]);
                    arrys[iRow, 9] = Convert.ToString(dt.Rows[iRow]["ACCID"]);
                    arrys[iRow, 10] = Convert.ToString(dt.Rows[iRow]["DELEGATION_ID"]);
                    arrys[iRow, 11] = Convert.ToString(dt.Rows[iRow]["TFNO"]);
                }

                string[] arry2 = new string[1];
                arry2[0] = "Yes";
                info.Add("IsGet", arry2);
                info.Add("BillingList", arrys);
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