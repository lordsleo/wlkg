//
//文件名：    ServiceTool.cs
//功能描述：  服务工具类,用于指定的服务
//创建时间：  2015/06/11
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wlkg.Service
{
    public class ServiceTool
    {
        /// <summary>
        /// 获取指定编号的服务
        /// </summary>
        /// <param name="serviceIndex">从零开始的服务索引</param>
        public static Dictionary<string, Array>  getService(int serviceNum, string[] paramSet)
        {
            ServiceMethod serviceMethod = new ServiceMethod();

            switch (serviceNum)
            {
                case 0:
                    // 今日资金存储
                    return serviceMethod.GetTodayFundStock();
                case 1:
                    // 今日快报
                    return serviceMethod.GetTodayReport();
                case 2:
                    // 收入、成本月度情况
                    return serviceMethod.GetMonthlyIncomeAndCost();
                case 3:
                    // 公司流动资产、负债情况
                    return serviceMethod.GetLiquidAssetsAndDebt();
                case 4:
                    // 利润月度情况
                    return serviceMethod.GetMonthlyProfit();
                case 5:
                    // 本年集装箱代理量趋势分析
                    return serviceMethod.GetCurYearContainer();
                case 6:
                    // 本年散杂货代理量趋势分析
                    return serviceMethod.GetCurYearBulkandGeneralCargo();
                case 7:
                    // 本年船舶代理量趋势分析
                    return serviceMethod.GetCurYearShip();
                case 8:
                    // 今日保税进出量
                    return serviceMethod.GetTodayBondInAndOut();
                case 9:
                    // 今日贸易情况
                    return serviceMethod.GetTodayTrade();
                case 10:
                    // 今日仓储进库量
                    return serviceMethod.GetTodayStorageInAndOut();
                case 11:
                    // 今日发运业务情况
                    return serviceMethod.GetTodayDeliverBusiness();
                case 12:
                    // 今日B保公司业务情况
                    return serviceMethod.GetTodayBBondCompanyBusiness();
                case 13:
                    // 各公司年度盈利情况
                    return serviceMethod.GetCompanyAnnualProfit();                            
                default:
                    throw new Exception("错误的对象索引");
            }
        }
    }
}