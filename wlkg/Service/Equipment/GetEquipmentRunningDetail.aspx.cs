//
//文件名：    GetEquipmentRunningDetail.aspx.cs
//功能描述：  获取设备运行详细记录
//创建时间：  2015/07/14
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

namespace wlkg.Service.Equipment
{
    public partial class GetEquipmentRunningDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //id
            var id = Request.Params["ID"];

            Dictionary<string, string> info = new Dictionary<string, string>();
            try
            {
                if (id == null)
                {
                    info.Add("参数ID不能为nul！", "举例：http://218.92.115.55/wlkg/Service/Equipment/GetEquipmentRunningDetail.aspx?ID=837294");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string sql =
                    string.Format("select * from eqmanager.view_equt_runday where run_id='{0}'", id);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);

                if (dt.Rows.Count == 0)
                {
                    info.Add("IsGet", "NO");
                    info.Add("Message", "ID错误！");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                info.Add("使用部门", Convert.ToString(dt.Rows[0]["department"]));
                info.Add("车票号", Convert.ToString(dt.Rows[0]["vehicle_sign"]));
                info.Add("机械编号", Convert.ToString(dt.Rows[0]["code_equt"]));
                info.Add("机械名称", Convert.ToString(dt.Rows[0]["equt"]));
                info.Add("作业日期", Convert.ToString(dt.Rows[0]["bill_day"]));
                info.Add("作业时长", Convert.ToString(dt.Rows[0]["run_hour"]));
                info.Add("起运吨", Convert.ToString(dt.Rows[0]["carry_ton"]));
                info.Add("货种", Convert.ToString(dt.Rows[0]["cargo"]));

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