//
//文件名：    GetEquipmentRecordDetail.aspx.cs
//功能描述：  获取设备台账详细记录
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
    public partial class GetEquipmentRecordDetail : System.Web.UI.Page
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
                    info.Add("参数ID不能为nul！", "举例：http://218.92.115.55/wlkg/Service/Equipment/GetEquipmentRecordDetail.aspx?ID=AAAVBRAA6AAAAA0AA3");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string sql =
                    string.Format("select * from eqmanager.view_equipment where rowidv='{0}'", id);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);

                if (dt.Rows.Count == 0)
                {
                    info.Add("IsGet", "NO");
                    info.Add("Message", "ID错误！");
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                info.Add("设备编号", Convert.ToString(dt.Rows[0]["code_equt"]));
                info.Add("车牌号", Convert.ToString(dt.Rows[0]["vehicle_sign"]));
                info.Add("设备名称", Convert.ToString(dt.Rows[0]["equt"]));
                info.Add("设备型号", Convert.ToString(dt.Rows[0]["equt_type"]));
                info.Add("所属分公司", Convert.ToString(dt.Rows[0]["subcompany"]));
                info.Add("使用部门", Convert.ToString(dt.Rows[0]["department"]));
                info.Add("设备类别", Convert.ToString(dt.Rows[0]["equtsort"]));
                info.Add("设备状态", Convert.ToString(dt.Rows[0]["equtstate"]));
                info.Add("设备状况", Convert.ToString(dt.Rows[0]["equt_station"]));
                info.Add("生产性", Convert.ToString(dt.Rows[0]["is_produce"]));
                info.Add("使用日期", Convert.ToString(dt.Rows[0]["startuse_time"]));
                info.Add("入账日期", Convert.ToString(dt.Rows[0]["make_date"]));
                info.Add("报废日期", Convert.ToString(dt.Rows[0]["not_use_date"]));
                info.Add("制造单位", Convert.ToString(dt.Rows[0]["make_company"]));
                info.Add("制造日期", Convert.ToString(dt.Rows[0]["out_factory_date"]));
                info.Add("备注", Convert.ToString(dt.Rows[0]["remark"]));
                info.Add("资产编号", Convert.ToString(dt.Rows[0]["asset_no"]));
                info.Add("股份资产编号", Convert.ToString(dt.Rows[0]["asset_no_gf"]));
                info.Add("原值（元）", Convert.ToString(dt.Rows[0]["original_value"]));
                info.Add("使用年限", Convert.ToString(dt.Rows[0]["use_year"]));
                info.Add("年折旧率（%）", Convert.ToString(dt.Rows[0]["discount"]));
                info.Add("残值（元））", Convert.ToString(dt.Rows[0]["left_value"]));
                info.Add("净值（元）", Convert.ToString(dt.Rows[0]["jingzhi"]));




                //info.Add("出厂编号", Convert.ToString(dt.Rows[0]["leave_factory_no"]));
                //info.Add("产地", Convert.ToString(dt.Rows[0]["code_produce_area"]));
                //info.Add("额定负荷（吨）", Convert.ToString(dt.Rows[0]["mark_power"]));
                //info.Add("发动机功率（kw）", Convert.ToString(dt.Rows[0]["total_power"]));
                //info.Add("电机功率（kw）", Convert.ToString(dt.Rows[0]["e_motor_power"]));
                //info.Add("整机编号", Convert.ToString(dt.Rows[0]["mech_no"]));
                //info.Add("发动机编号", Convert.ToString(dt.Rows[0]["engine_no"]));
                //info.Add("外形尺寸", Convert.ToString(dt.Rows[0]["shape_size"]));
                //info.Add("自重（吨）", Convert.ToString(dt.Rows[0]["weight"]));
                //info.Add("功能说明", Convert.ToString(dt.Rows[0]["function"]));
                //info.Add("负责人", Convert.ToString(dt.Rows[0]["burden_man"]));
                //info.Add("机械名称", Convert.ToString(dt.Rows[0]["machine_classify"]));
                //info.Add("机械型号", Convert.ToString(dt.Rows[0]["standard_equt_type"]));

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