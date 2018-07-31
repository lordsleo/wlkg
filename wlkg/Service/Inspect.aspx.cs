//
//文件名：    DeclareReported1.aspx.cs
//功能描述：  报检数据
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
    public partial class Inspect : System.Web.UI.Page
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
                    arry[0] = "举例：http://218.92.115.55/wlkg/Service/Inspect.aspx?CodeDepartment=6397&ShipName=丰安";
                    info.Add("参数CodeDepartment，ShipName不能为空！", arry);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                //获取系统当前时间
                DateTime curTime = DateTime.Now;
                string sql =
                        string.Format("SELECT a.flag,a.INSPECTION_ID,a.CLIENTSHORT,b.shipname,b.voyage,b.blno,b.blno_count,b.cargo_name FROM LBTS.VIEW_APPLY_INSPECTION_BASE a,LBTS.view_delegation b where a.delegation_id=b.id and b.iport_dept='{0}' and b.shipname like '%{1}%'  and (b.input_date like'%{2}%' or b.input_date like'%{3}%') order by a.INSPECTION_ID desc", codeDepatrment.ToString(), shipName.ToString(), curTime.Year - 1, curTime.Year);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);

                string[,] ary = new string[dt.Rows.Count, 8];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    ary[iRow, 0] = dt.Rows[iRow]["FLAG"].ToString();
                    ary[iRow, 1] = dt.Rows[iRow]["CLIENTSHORT"].ToString(); ;
                    ary[iRow, 2] = dt.Rows[iRow]["SHIPNAME"].ToString(); ;
                    ary[iRow, 3] = dt.Rows[iRow]["VOYAGE"].ToString(); ;
                    ary[iRow, 4] = dt.Rows[iRow]["BLNO"].ToString(); ;
                    ary[iRow, 5] = dt.Rows[iRow]["BLNO_COUNT"].ToString(); ;
                    ary[iRow, 6] = dt.Rows[iRow]["CARGO_NAME"].ToString(); ;
                    ary[iRow, 7] = dt.Rows[iRow]["INSPECTION_ID"].ToString(); ;
                }

                info.Add("Inspect", ary);
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