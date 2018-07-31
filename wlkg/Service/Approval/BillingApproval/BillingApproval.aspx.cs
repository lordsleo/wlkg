//
//文件名：    BillingApproval.aspx.cs
//功能描述：  发票审批
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
    public partial class BillingApproval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //审批人
            var isAuditName = Request.Params["IsAuditName"];
            //编码
            var TFNO = Request.Params["TFNO"];

            Dictionary<string, string> info = new Dictionary<string, string>();
            try
            {
                if (isAuditName == null || TFNO == null)
                {
                    info.Add("参数IsAuditName，TFNO不能为null！", "举例：http://218.92.115.55/wlkg/Service/Approval/BillingApproval/BillingApproval.aspx?IsAuditName=227&TFNO=123234");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                var TFNOArry = TFNO.ToString().Split(' ');
                for (int iTFNO = 0; iTFNO < TFNOArry.Length; iTFNO++)
                {
                    string sql =
                            string.Format("select * from tb_ht_tariff  where TFNO='{0}'", TFNOArry[iTFNO]);
                    var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                    if (dt.Rows.Count == 0)
                    {
                        info.Add("Approval", "No");
                        info.Add("Message", "编码（TFNO）不存在！");
                        Json = JsonConvert.SerializeObject(info);
                        return;
                    }

                    sql =
                        string.Format("UPDATE tb_ht_tariff t SET t.isaudit='2' ,t.isauditname='{0}' where TFNO='{1}'", isAuditName, TFNOArry[iTFNO]);
                    dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);

                    sql =
                        string.Format("select * from tb_ht_tariff  where TFNO='{0}'", TFNOArry[iTFNO]);
                    dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                    if (dt.Rows.Count == 0)
                    {
                        info.Add("Approval", "No");
                        info.Add("Message", "网络错误，请稍后重试！");
                        return;
                    }
                    else
                    {
                        if ((Convert.ToString(dt.Rows[0]["isaudit"]) != "2") || (Convert.ToString(dt.Rows[0]["isauditname"]) != isAuditName))
                        {
                            info.Add("Approval", "No");
                            info.Add("Message", "网络错误，请稍后重试！");
                            return;
                        }
                    }
                }

                info.Add("Approval", "Yes");
                info.Add("Message", "审批成功！");
                Json = JsonConvert.SerializeObject(info);
            }
            catch (Exception ex)
            {
                info.Add("ApprovalReturn", "No");
                info.Add("Message", "服务器异常：" + ex.Message);
                Json = JsonConvert.SerializeObject(info);
            }

        }
        protected string Json;
    }
}