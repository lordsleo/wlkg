//
//文件名：    GetCC_BusinessManagementCenterDetail.aspx.cs
//功能描述：  获取仓储业务管理中心明细数据
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
    public partial class GetCC_BusinessManagementCenterDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.Params["ID"];
            //id = "20150908000047";

            Dictionary<string, string> info = new Dictionary<string, string>();
            try
            {
                if (id == null)
                {
                    info.Add("参数ID不能为nul！", "举例：http://218.92.115.55/wlkg/Service/Supervise/Business/GetCC_BusinessManagementCenterDetail.aspx?ID=20150908000047");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string sql =
                        string.Format("select * from storageharbor.vw_hc_goodbill_landfee where gbno='{0}'", id);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    info.Add("IsGet", "NO");
                    info.Add("Message", "ID错误！");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string[] nameArray = { "货代", "货物", "包装", "件重", "计划件数", "计划重量", "航次","提单","备注",  
                                       "出卡", "开票", "园区", "唛头"};
                //排序字符串
                string order = string.Empty;
                order = nameArray[0] + "+" + nameArray[1] + "+" + nameArray[2] + "+" + nameArray[3] + "+" + nameArray[4] + "+" + nameArray[5] + "+" + nameArray[6] + "+" +
                        nameArray[7] + "+" + nameArray[8] + "+" + nameArray[9] + "+" + nameArray[10] + "+" + nameArray[11] + "+" + nameArray[12];

                info.Add(nameArray[0], Convert.ToString(dt.Rows[0]["client"]));
                info.Add(nameArray[1], Convert.ToString(dt.Rows[0]["cargo"]));
                info.Add(nameArray[2], Convert.ToString(dt.Rows[0]["pack"]));
                info.Add(nameArray[3], Convert.ToString(dt.Rows[0]["pieceweight"]));
                info.Add(nameArray[4], Convert.ToString(dt.Rows[0]["planamount"]));
                info.Add(nameArray[5], Convert.ToString(dt.Rows[0]["planweight"]));
                info.Add(nameArray[6], Convert.ToString(dt.Rows[0]["vgdisplay"]));
                info.Add(nameArray[7], Convert.ToString(dt.Rows[0]["blno"]));
                info.Add(nameArray[8], Convert.ToString(dt.Rows[0]["remark"]));
                info.Add(nameArray[9], Convert.ToString(dt.Rows[0]["mark_sub"]) != "1" ? "" : "√");
                info.Add(nameArray[10], Convert.ToString(dt.Rows[0]["mark_makeiv"]) != "1" ? "" : "√");
                info.Add(nameArray[11], Convert.ToString(dt.Rows[0]["section"]));
                info.Add(nameArray[12], Convert.ToString(dt.Rows[0]["vgdisplay"]));
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