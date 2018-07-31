//
//文件名：    DelegationApprovalReturn.aspx.cs
//功能描述：  委托审批退回
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
    public partial class DelegationApprovalReturn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //委托id
            var id = Request.Params["ID"];

            Dictionary<string, string> info = new Dictionary<string, string>();
            try
            {
                if (id == null)
                {
                    info.Add("参数ID不能为null！", "举例：http://218.92.115.55/wlkg/Service/Approval/BillingApproval/DelegationApprovalReturn.aspx?ID=633633344");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                var idArry = id.ToString().Split(' ');
                for (int iId = 0; iId < idArry.Length; iId++)
                {
                    string sql =
                            string.Format("select * from delegation  where id='{0}'", idArry[iId]);
                    var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                    if (dt.Rows.Count == 0)
                    {
                        info.Add("Approval", "No");
                        info.Add("Message", "委托ID不存在！");
                        Json = JsonConvert.SerializeObject(info);
                        return;
                    }

                    sql =
                        string.Format("UPDATE delegation t SET t.IAEASAUDIT='0',t.isaudit='3'  where t.id='{0}'", idArry[iId]);
                    dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);

                    sql =
                        string.Format("select * from delegation  where id='{0}'", idArry[iId]);
                    dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                    if (dt.Rows.Count == 0)
                    {
                        info.Add("Approval", "No");
                        info.Add("Message", "网络错误，请稍后重试！");
                        return;
                    }
                    else
                    {
                        if ((Convert.ToString(dt.Rows[0]["IAEASAUDIT"]) != "0") || (Convert.ToString(dt.Rows[0]["IAEASAUDIT"]) != "1"))
                        {
                            info.Add("Approval", "No");
                            info.Add("Message", "网络错误，请稍后重试！");
                            return;
                        }
                    }
                }

                info.Add("Approval", "Yes");
                info.Add("Message", "审批退回成功！");
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