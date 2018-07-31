//
//文件名：    NewsDetail.aspx.cs
//功能描述：  详细新闻
//创建时间：  2015/07/13
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

namespace wlkg.Service.News
{
    public partial class NewsDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var message = Request.Params["message"];
                var str = message.Split('=');
                var str1 = str[1].Split(' ');
                var newsType = str1[0];
                var newsId = str1[1];

                string sql = string.Format("select topic,to_char(audittime,'yyyy-MM-dd HH24:mi:ss') as audittime,message  from News where news_type='{0}' and id = '{1}'", newsType, newsId);
                var dt = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathWl).ExecuteTable(sql);
                var arry = new Leo.Data.Table(dt).ToArray();

                Json = JsonConvert.SerializeObject(arry);
            }
            catch (Exception ex)
            {
                LogTool.WriteLog(typeof(NewsDetail), ex);
            }
        }
        protected string Json;
    }
}