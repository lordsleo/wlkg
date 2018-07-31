using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wlkg.Common
{
    /// <summary>
    /// 业务状态。
    /// </summary>
    public enum BusinessOperationCode
    {
        /// <summary>
        /// 完成
        /// </summary>
        Finish,
        /// <summary>
        /// 待办
        /// </summary>
        Wait
    }

    public class BusinessOperation
    {
        static Dictionary<BusinessOperationCode, string> _index;      
        static BusinessOperation()
        {
            _index = new Dictionary<BusinessOperationCode, string>();
            _index.Add(BusinessOperationCode.Finish, "已完成");
            _index.Add(BusinessOperationCode.Wait, "待办");
 
        }
        /// <summary>
        /// 获取业务操作状态描述
        /// </summary>
        /// <param name="code">业务操作编码</param>
        /// <returns>操作名称</returns>
        public static string GetName(BusinessOperationCode code)
        {
            if (_index.ContainsKey(code) == true)
                return _index[code];
            else
                return null;
        }
    }
}