//
//文件名：    InspectReported1.aspx.cs
//功能描述：  报检
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

namespace wlkg.Page
{
    public partial class InspectReported1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var message = Request.Params["message"];
                //var message = "111" + " " + "1542" + " " + "2014-11-27 16:15";
                var str = message.ToString().Split(' ');
                var userId = str[0];
                var inspectionId = str[1];
                var Date = str[2];
                var Time = str[3];
                var str1 = Date.ToString().Split('-');
                var year = str1[0];
                var month = str1[1];
                var day = str1[2];
                var str2 = Time.ToString().Split(':');
                var hour = str2[0];
                var minute = str2[1];

                var inspectTime = string.Format("{0}{1}{2}{3}{4}{5}{6}", year.ToString(), month.ToString(), day.ToString(), hour.ToString(), minute.ToString(), 0, 0);

                string sql =
                    string.Format("update lbts.APPLY_INSPECTION_BASE a set a.APPLY_INSPECTION_DATE='{0}',a.APPLIER='{1}' where a.INSPECTION_ID='{2}'", inspectTime, userId, inspectionId);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                sql =
                    string.Format("select * from APPLY_INSPECTION_BASE t where t.APPLIER = '{0}' and t.APPLY_INSPECTION_DATE = '{1}' and t.INSPECTION_ID = '{2}'", userId, inspectTime, inspectionId);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);

                string[,] ls = new string[1, 1];
                if (dt.Rows.Count != 0)
                {//存在
                    ls[0, 0] = "ture";
                }
                else
                {
                    ls[0, 0] = "false";
                }

                Json = JsonConvert.SerializeObject(ls);
            }
            catch (Exception ex)
            {
                LogTool.WriteLog(typeof(InspectReported1), ex);
            }
        }
        protected string Json;
    }
}