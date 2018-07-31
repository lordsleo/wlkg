//
//文件名：    DeclareReported1.aspx.cs
//功能描述：  报关数据
//创建时间：  2015/06/18
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
    public partial class Declare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var codeDepatrment = Request.Params["CodeDepartment"];
            var shipName = Request.Params["ShipName"];
            
            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                if (codeDepatrment == null || shipName == null)
                {
                    string[] arry = new string[1];
                    arry[0] = "举例：http://218.92.115.55/wlkg/Service/Declare.aspx?CodeDepartment=6397&ShipName=丰安";
                    info.Add("参数CodeDepartment，ShipName不能为空！", arry);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }
       
                //获取系统当前时间
                DateTime curTime = DateTime.Now;
                string sql =
                        string.Format("SELECT a.CUSTOMS_ID,a.batch,a.flag,a.CLIENTSHORT,b.shipname,b.voyage,b.blno,b.blno_count,b.cargo_name FROM lbts.VIEW_APPLY_CUSTOMS_BASE a,view_delegation b where a.delegation_id=b.id and b.iport_dept='{0}' and b.shipname like '%{1}%'  and (b.input_date like'%{2}%' or b.input_date like'%{3}%') order by a.customs_id desc", codeDepatrment.ToString(), shipName.ToString(), curTime.Year - 1, curTime.Year);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);

                string[,] ary = new string[dt.Rows.Count, 9];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    ary[iRow, 0] = Convert.ToString(dt.Rows[iRow]["BATCH"]);
                    ary[iRow, 1] = Convert.ToString(dt.Rows[iRow]["FLAG"]);
                    ary[iRow, 2] = Convert.ToString(dt.Rows[iRow]["CLIENTSHORT"]);
                    ary[iRow, 3] = Convert.ToString(dt.Rows[iRow]["SHIPNAME"]);
                    ary[iRow, 4] = Convert.ToString(dt.Rows[iRow]["VOYAGE"]);
                    ary[iRow, 5] = Convert.ToString(dt.Rows[iRow]["BLNO"]);
                    ary[iRow, 6] = Convert.ToString(dt.Rows[iRow]["BLNO_COUNT"]);
                    ary[iRow, 7] = Convert.ToString(dt.Rows[iRow]["CARGO_NAME"]);
                    ary[iRow, 8] = Convert.ToString(dt.Rows[iRow]["CUSTOMS_ID"]);
                }

                info.Add("Declare", ary);
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