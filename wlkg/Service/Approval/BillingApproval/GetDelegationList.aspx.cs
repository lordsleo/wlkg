//
//文件名：    GetDelegationList.aspx.cs
//功能描述：  获取委托信息列表
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
    public partial class GetDelegationList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //用户ID
            var userCode = Request.Params["UserCode"];

            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                if (userCode == null)
                {
                    string[] arry = new string[1];
                    arry[0] = "举例：http://218.92.115.55/wlkg/Service/Approval/BillingApproval/GetDelegationList.aspx?UserCode=1";
                    info.Add("参数UserCode不能为null！", arry);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string sql =
                    string.Format(
                        @"SELECT  id, shipname, voyage, blno, cargo_name, blno_count, clientshort, fanghuo, remark, iport_dept 
                          FROM   view_all_delegation_eas
                          WHERE  EXISTS(SELECT * FROM ACCESS_CONTROL WHERE CODE_USER='{0}' AND VIEW_ALL_DELEGATION_EAS.IPORT_DEPT=ACCESS_CONTROL.CODE_DEPARTMENT) 
                          AND    IPORT_DEPT in (select code_department FROM ACCESS_CONTROL WHERE CODE_USER='{0}' )",
                          userCode);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "No";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无委托信息！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string[,] arrys = new string[dt.Rows.Count, 10];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    arrys[iRow, 0] = Convert.ToString(dt.Rows[iRow]["id"]);
                    arrys[iRow, 1] = Convert.ToString(dt.Rows[iRow]["shipname"]);
                    arrys[iRow, 2] = Convert.ToString(dt.Rows[iRow]["voyage"]);
                    arrys[iRow, 3] = Convert.ToString(dt.Rows[iRow]["blno"]);
                    arrys[iRow, 4] = Convert.ToString(dt.Rows[iRow]["cargo_name"]);
                    arrys[iRow, 5] = Convert.ToString(dt.Rows[iRow]["blno_count"]);
                    arrys[iRow, 6] = Convert.ToString(dt.Rows[iRow]["clientshort"]);
                    arrys[iRow, 7] = Convert.ToString(dt.Rows[iRow]["fanghuo"]);
                    arrys[iRow, 8] = Convert.ToString(dt.Rows[iRow]["remark"]);
                    arrys[iRow, 9] = Convert.ToString(dt.Rows[iRow]["iport_dept"]);
                }

                string[] arry2 = new string[1];
                arry2[0] = "Yes";
                info.Add("IsGet", arry2);
                info.Add("DelegationList", arrys);
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