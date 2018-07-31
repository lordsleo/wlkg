//
//文件名：    DeclareReported2.aspx.cs
//功能描述：  报关
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
    public partial class DeclareReported2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var message = Request.Params["message"];
                //var message = "111" + " " + "1542" + " " + "2014-11-27 16:15" + " " + "红豆爱上大殴打后搜Ida" + " " + "11111d";
                var str = message.ToString().Split(' ');
                var userId = str[0];
                var declareId = str[1];
                var Date = str[2];
                var Time = str[3];
                var remark = str[4];
                var declareNum = str[5];
                if (remark.ToString() == "")
                {
                    remark = "-";
                }

                var str1 = Date.ToString().Split('-');
                var year = str1[0];
                var month = str1[1];
                var day = str1[2];
                var str2 = Time.ToString().Split(':');
                var hour = str2[0];
                var minute = str2[1];

                var inspectTime = string.Format("{0}{1}{2}{3}{4}{5}{6}", year.ToString(), month.ToString(), day.ToString(), hour.ToString(), minute.ToString(), 0, 0);

                string sql =
                        string.Format("select t.status from lbts.APPLY_CUSTOMS_BASE t where t.CUSTOMS_ID='{0}'", declareId);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);

                string[,] ls = new string[1, 1];
                string[,] ary = new string[1, 1];
                while (dt.Rows.Count != 0)
                {
                    ary[0, 0] = dt.Rows[0]["STATUS"].ToString();
                }
                if (ary[0, 0] == "3")
                {
                    ls[0, 0] = "default";
                    Json = JsonConvert.SerializeObject(ls);
                    return;
                }

                sql =
                       string.Format("update lbts.APPLY_CUSTOMS_BASE a set a.STATUS='3',a.PERMIT_DATE='{0}',a.COSTOMS_NO='{1}',a.APPLIER='{2}',a.DESCRIBE='{3}' where  a.CUSTOMS_ID='{4}'", inspectTime, declareNum, userId, remark, declareId);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                sql =
                       string.Format("select * from lbts.APPLY_CUSTOMS_BASE t where t.APPLIER = '{0}' and t.PERMIT_DATE = '{1}' and t.CUSTOMS_ID = '{2}' and t.DESCRIBE='{3}' and t.COSTOMS_NO='{4}'", userId, inspectTime, declareId, remark, declareNum);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathLbts).ExecuteTable(sql);
                if (dt.Rows.Count != 0)
                { //存在
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
                LogTool.WriteLog(typeof(DeclareReported2), ex);
            }
        }
        protected string Json;
    }
}