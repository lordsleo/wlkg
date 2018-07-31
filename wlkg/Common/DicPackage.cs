//
//文件名：    DicPackage.aspx.cs
//功能描述：  交换数据包（返回值）
//创建时间：  2015/07/09
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wlkg.Common
{
    public class DicPackage
    {
        //消息
        private string message;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="message">消息</param>
        public DicPackage(string message = null)
        {
            this.message = message;
        }

        /// <summary>
        /// 成功字典交换数据包
        /// </summary>
        /// <param name="data">数据集</param>
        /// <returns>Dictionary对象</returns>
        public Dictionary<string, object> TrueDic(object data = null)
        {
            Dictionary<string, object> info = new Dictionary<string,object>();
            info.Add("IsSuccess", "Yes");
            if (data != null)
            {
                info.Add("Data", data);
            }
            info.Add("Message", this.message);
            return info;
        }

        /// <summary>
        /// 失败字典交换数据包
        /// </summary>
        /// <returns>Dictionary对象</returns>
        public Dictionary<string, object> FalseDic()
        {
            Dictionary<string, object> info = new Dictionary<string, object>();
            info.Add("IsSuccess", "No");
            info.Add("Message", this.message);
            return info;
        }

    }
}