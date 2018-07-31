//
//文件名：    GetEquipmentRecord.aspx.cs
//功能描述：  获取设备台账记录
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
    public partial class GetEquipmentRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //用户部门编码
            var codeDepartment = Request.Params["CodeDepartment"];
            //选择部门编码
            var selectCodeDepartment = Request.Params["SelectCodeDepartment"];
            //查询页数
            var pages = Request.Params["Pages"];

            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                if (codeDepartment == null || selectCodeDepartment == null || pages == null)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "举例：http://218.92.115.55/wlkg/Service/Equipment/GetEquipmentRecord.aspx?CodeDepartment=6369&SelectCodeDepartment=&Pages=1";
                    info.Add("参数CodeDepartment，SelectCodeDepartment，Pages不能为nul！", arry0);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                //用户权限校验
                string[] codeDepartmentSet = new string[17]
                { "6373", //船务公司
                  "6377", //仓储公司
                  "6372", //货运公司
                  "6374", //贸易公司
                  "6375", //保税公司
                  "6395", //郁港保税服务公司
                  "6397", //陆桥公共保税仓库
                  "6376", //物流配送中心
                  "6378", //对外投资管理公司
                  "FF00ED5555FA60BAE043A864016A60BA", //B保公司

                  "6349", //江苏连云港港物流控股有限公司
                  "6369", //事业部
                  "6395", //综合部 
                  "6370", //经营管理部
                  "6371", //党群工作部
                  "FCA1F460DA5ED2E0E043A8640169D2E0", //公司领导
                  "0E25A7042BCA1324E053A86401691324"  //风险管控部
                };
                for (int iDep = 0; iDep < codeDepartmentSet.Length; iDep++)
                {
                    if (codeDepartment == codeDepartmentSet[iDep])
                    {
                        break;
                    }
                    if (iDep == codeDepartmentSet.Length)
                    {
                        string[] arry0 = new string[1];
                        arry0[0] = "NO";
                        info.Add("IsGet", arry0);
                        string[] arry1 = new string[1];
                        arry1[0] = "此用户无权限查看！";
                        info.Add("Message", arry1);
                        Json = JsonConvert.SerializeObject(info);
                        return;
                    }
                }

                if ((codeDepartment != "6349") && (codeDepartment != "6369") && (codeDepartment != "6395") && (codeDepartment != "6370") && (codeDepartment != "6371") && (codeDepartment != "codeDepartment") && (codeDepartment != "A7042BCA1324E053A86401691324"))
                {
                    if (codeDepartment != selectCodeDepartment)
                    {
                                            string[] arry0 = new string[1];
                    arry0[0] = "NO";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "此用户无权限查看！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                    }
                }

                int minRow = (Convert.ToInt16(pages) - 1) * 15 + 1;
                int maxRow = Convert.ToInt16(pages) * 15;

                string sql =
                        string.Format("select count(rowidv) as total from eqmanager.view_equipment where code_company='6349'and code_department like '%{0}%' ", selectCodeDepartment);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "NO";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无数据！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                sql =
                    string.Format(
                        "select rowidv,code_equt,vehicle_sign,equt,equt_type,subcompany from (select * from (select rowidv,code_equt,vehicle_sign,equt,equt_type,subcompany from eqmanager.view_equipment where code_company='6349'and code_department like '%{0}%' order by code_equt asc) where rownum<={1} order by code_equt desc) where rownum<={2}",
                        selectCodeDepartment, Convert.ToInt32(dt.Rows[0]["total"]) - minRow + 1, maxRow - minRow + 1);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "NO";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无更多数据！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string[,] ary = new string[dt.Rows.Count, 6];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    ary[iRow, 0] = Convert.ToString(dt.Rows[iRow]["rowidv"]);
                    ary[iRow, 1] = Convert.ToString(dt.Rows[iRow]["code_equt"]);
                    ary[iRow, 2] = Convert.ToString(dt.Rows[iRow]["vehicle_sign"]);
                    ary[iRow, 3] = Convert.ToString(dt.Rows[iRow]["equt"]);
                    ary[iRow, 4] = Convert.ToString(dt.Rows[iRow]["equt_type"]);;
                    ary[iRow, 5] = Convert.ToString(dt.Rows[iRow]["subcompany"]); ;
                }

                string[] arry2 = new string[1];
                arry2[0] = "Yes";
                info.Add("IsGet", arry2);
                info.Add("EquipmentRecord", ary);
                Json = JsonConvert.SerializeObject(info);
            }
            catch (Exception ex)
            {
                string[] arry0 = new string[1];
                arry0[0] = "NO";
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