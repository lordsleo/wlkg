//
//文件名：    ServiceMethod.cs
//功能描述：  服务方法通用类
//创建时间：  2015/06/11
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Leo;

namespace wlkg.Service
{
    public class ServiceMethod
    {
        private string[] paramSet = null;
        private Dictionary<string, Array> info = null;

        public ServiceMethod(string[] paramSet = null)
        {
            info = new Dictionary<string, Array>();
            if (paramSet != null)
            {
                paramSet = new string[paramSet.Length];
                this.paramSet = paramSet;
            }
        }

        /// <summary>
        /// 今日资金存储
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetTodayFundStock() 
        {
            string[] dateSet = { "" };
            string[] fundSet = { "" };
            string[] fundPercentlSet = { "" };

            string sql =
                    string.Format("select bcompany,round((baccount/1000000),2) baccount,to_char(bdate,'MM-DD') as bdate from TB_FI_BANKSAVINGS t where t.bcodecompany=0179 and bdate>trunc(sysdate)-11 order by bdate");
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            if (dt0.Rows.Count != 0)
            {
                dateSet = new string[dt0.Rows.Count];
                fundSet = new string[dt0.Rows.Count];
                fundPercentlSet = new string[dt0.Rows.Count];
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    dateSet[iRow] = dt0.Rows[iRow]["bdate"].ToString();
                    fundSet[iRow] = dt0.Rows[iRow]["baccount"].ToString();
                }
            }

            sql =
                string.Format("select bcompany,round((baccount/1000000),2) baccount,to_char(bdate,'MM-DD') as bdate from TB_FI_BANKSAVINGS t where t.bcodecompany=0179 and bdate>trunc(sysdate)-101 order by bdate");
            var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            if (dt1.Rows.Count != 0)
            {
                for (int iRow = dt1.Rows.Count; iRow > dt1.Rows.Count - dt0.Rows.Count; iRow--)
                {
                    //var aa = Convert.ToDouble(dt2.Rows[iRow]["baccount"]);
                    //var bb = Convert.ToDouble(dt2.Rows[iRow - 1]["baccount"]);
                    double fundInterval;
                    if (dt1.Rows.Count == 1)
                    {
                        fundInterval = 100.0;
                    }
                    else
                    {
                        fundInterval = (Convert.ToDouble(dt1.Rows[iRow - 1]["baccount"]) - Convert.ToDouble(dt1.Rows[iRow - 2]["baccount"])) / Convert.ToDouble(dt1.Rows[iRow - 2]["baccount"]) * 100.0;
                    }

                    string fundPercent;
                    fundPercent = (fundInterval < 0) ? string.Format("{0:F}%", fundInterval) : string.Format("+{0:F}%", fundInterval);
                    fundPercentlSet[dt0.Rows.Count - (dt1.Rows.Count - iRow + 1)] = fundPercent;
                }
            }

            info.Add("Date", dateSet);
            info.Add("Fund", fundSet);
            info.Add("FundPercent", fundPercentlSet);
            return info;
        }

        /// <summary>
        /// 今日快报
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetTodayReport()
        {
            string[] reportSet = new string[6];
            //今日仓储进库量
            string sql =
                    string.Format("select round(decode(sum(weight), NULL, 0, sum(weight))) as inweight from storageharbor.vw_hs_goodsbill_tallybill_b_i where code_company = '6377' and signdate = (SELECT MAX(storageharbor.vw_hs_goodsbill_tallybill_b_i.signdate) FROM storageharbor.vw_hs_goodsbill_tallybill_b_i)");
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            if (dt0.Rows.Count == 0)
            {
                reportSet[0] = string.Format("今日仓储进库量（{0}）吨", string.Empty);
            }
            else
            {
                reportSet[0] = string.Format("今日仓储进库量（{0}）吨", dt0.Rows[0]["inweight"].ToString());
            }
            
            //今日仓储出库量
            sql =
                string.Format("select round(decode(sum(weight), NULL, 0, sum(weight))) as outweight from storageharbor.vw_hs_goodsbill_tallybill_b_o where code_company = '6377' and signdate = (SELECT MAX(storageharbor.vw_hs_goodsbill_tallybill_b_o.signdate) FROM storageharbor.vw_hs_goodsbill_tallybill_b_o)");
            var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);      
            if (dt1.Rows.Count == 0)
            {
                reportSet[1] = string.Format("今日仓储出库量（{0}）吨", string.Empty);
            }
            else
            {
                reportSet[1] = string.Format("今日仓储出库量（{0}）吨", dt1.Rows[0]["outweight"].ToString());
            }

            //今日仓储场存量
            sql =
                string.Format("select round(weight,2) as weight,signdate from (select sum(t.weight) weight,t.signdate from storageharbor.TB_HS_STOCKDORMANTHISTORY t where to_number(to_char(signdate,'yyyy'))>=2014 group by t.signdate  order by signdate desc) where rownum=1");
            var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);        
            if (dt2.Rows.Count == 0)
            {
                reportSet[2] = string.Format("今日仓储场存量（{0}）吨", string.Empty);
            }
            else
            {
                reportSet[2] = string.Format("今日仓储场存量（{0}）吨", dt2.Rows[0]["weight"].ToString());
            }

            //散杂货代理量
            sql =
                string.Format("select sum(amount) as sum from BUSINESS_INFO t where code_type='代理量' and t.actualtime>=sysdate -7");
            var dt3 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            if (dt3.Rows.Count == 0)
            {
                reportSet[3] = string.Format("本周 散杂货代理量（{0}）吨", string.Empty);
            }
            else
            {
                reportSet[3] = string.Format("本周 散杂货代理量（{0}）吨", dt3.Rows[0]["sum"].ToString());
            }
            
            //集装箱代理量
            sql =
                string.Format("select sum(amount) as sum from BUSINESS_INFO t where code_type='集装箱' and t.actualtime>=sysdate -7");
            var dt4 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);        
            if (dt4.Rows.Count == 0)
            {
                reportSet[4] = string.Format("本周 集装箱代理量（{0}）TUE", string.Empty);
            }
            else
            {
                reportSet[4] = string.Format("本周 集装箱代理量（{0}）TUE", dt4.Rows[0]["sum"].ToString());
            }

            //船舶代理量
            sql =
                string.Format("select sum(amount) as sum from BUSINESS_INFO t where code_type='船舶' and t.actualtime>=sysdate -7");
            var dt5 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            if (dt5.Rows.Count == 0)
            {
                reportSet[5] = string.Format("本周 船舶代理量（{0}）艘次", string.Empty);
            }
            else
            {
                reportSet[5] = string.Format("本周 船舶代理量（{0}）艘次", dt5.Rows[0]["sum"].ToString());
            }     

            info.Add("TodayReport", reportSet);
            return info;
        }

        /// <summary>
        /// 收入、成本月度情况
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetMonthlyIncomeAndCost()
        {
            string[] dateSet = { "" };
            string[] incomeSet = { "" };
            string[] costSet = { "" };
            string[] lastIncomeSet = { "" };
            string[] lastCostSet = { "" };

            string sql =
                    string.Format("select * from (select substr(t.fitem,-2,2) as name ,round(fcurrentvalue/1000000,2) as value,t.fyear year ,t.fperiod as month from middb.T_WL_LRB t where   fitem like '%营业收入%'  or fitem like '%营业成本%'  and t.fcompanyname='江苏连云港港物流控股有限公司'  order by fyear||lpad(fperiod,2,0)) where year=to_char(sysdate,'yyyy') and name='收入'");
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMiddb).ExecuteTable(sql);
            int count = dt.Rows.Count;
            if (count != 0)
            {
                dateSet = new string[count];
                incomeSet = new string[count];
                costSet = new string[count];
                lastIncomeSet = new string[count];
                lastCostSet = new string[count];
                for (int iRow = 0; iRow < count; iRow++)
                {
                    dateSet[iRow] = dt.Rows[iRow]["month"].ToString();
                    incomeSet[iRow] = dt.Rows[iRow]["value"].ToString();
                }

                sql =
                    string.Format("select * from (select substr(t.fitem,-2,2) as name ,round(fcurrentvalue/1000000,2) as value,t.fyear year ,t.fperiod as month from middb.T_WL_LRB t where   fitem like '%营业收入%'  or fitem like '%营业成本%'  and t.fcompanyname='江苏连云港港物流控股有限公司'  order by fyear||lpad(fperiod,2,0)) where year=to_char(sysdate,'yyyy') and name='成本'");
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMiddb).ExecuteTable(sql);
                for (int iRow = 0; iRow < count; iRow++)
                {
                    costSet[iRow] = dt.Rows[iRow]["value"].ToString();
                }

                sql =
                    string.Format("select * from (select substr(t.fitem,-2,2) as name ,round(fcurrentvalue/1000000,2) as value,t.fyear year ,t.fperiod as month from middb.T_WL_LRB t where   fitem like '%营业收入%'  or fitem like '%营业成本%'  and t.fcompanyname='江苏连云港港物流控股有限公司'  order by fyear||lpad(fperiod,2,0)) where year=to_char(sysdate,'yyyy')-1 and name='收入'");
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMiddb).ExecuteTable(sql);
                for (int iRow = 0; iRow < count; iRow++)
                {
                    lastIncomeSet[iRow] = dt.Rows[iRow]["value"].ToString();
                }

                sql =
                    string.Format("select * from (select substr(t.fitem,-2,2) as name ,round(fcurrentvalue/1000000,2) as value,t.fyear year ,t.fperiod as month from middb.T_WL_LRB t where   fitem like '%营业收入%'  or fitem like '%营业成本%'  and t.fcompanyname='江苏连云港港物流控股有限公司'  order by fyear||lpad(fperiod,2,0)) where year=to_char(sysdate,'yyyy')-1 and name='成本'");
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMiddb).ExecuteTable(sql);
                for (int iRow = 0; iRow < count; iRow++)
                {
                    lastCostSet[iRow] = dt.Rows[iRow]["value"].ToString();
                }
            }

            info.Add("Month", dateSet);
            info.Add("Income", incomeSet);
            info.Add("Cost", costSet);
            info.Add("LastIncome", lastIncomeSet);
            info.Add("LastCost", lastCostSet);
            return info;
        }

        /// <summary>
        /// 公司流动资产、负债情况
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetLiquidAssetsAndDebt()
        {
            string[] dateSet = { "" };
            string[] assetSet = { "" };
            string[] debtSet = { "" };
            string[] lastassetSet = { "" };
            string[] lastdebtSet = { "" };

            string sql =
                    string.Format("select * from (select substr(trim(t.fitem),3,2) as name,round(fcurrentvalue/1000000,2) as value,t.fyear as year,t.fperiod as month,t.fcompanyname from middb.T_WL_ZCFZB t where t.fseqno=16 or t.fseqno=57  and t.fcompanyname='江苏连云港港物流控股有限公司' order by fyear||lpad(fperiod,2,0)) where YEAR=to_char(sysdate,'yyyy') and name='资产'");
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMiddb).ExecuteTable(sql);
            int count = dt.Rows.Count;
            if (count != 0)
            {
                dateSet = new string[count];
                assetSet = new string[count];
                debtSet = new string[count];
                lastassetSet = new string[count];
                lastdebtSet = new string[count];

                for (int iRow = 0; iRow < count; iRow++)
                {
                    dateSet[iRow] = dt.Rows[iRow]["month"].ToString();
                    assetSet[iRow] = dt.Rows[iRow]["value"].ToString();
                }

                sql =
                    string.Format("select * from (select substr(trim(t.fitem),3,2) as name,round(fcurrentvalue/1000000,2) as value,t.fyear as year,t.fperiod as month,t.fcompanyname from middb.T_WL_ZCFZB t where t.fseqno=16 or t.fseqno=57  and t.fcompanyname='江苏连云港港物流控股有限公司' order by fyear||lpad(fperiod,2,0)) where YEAR=to_char(sysdate,'yyyy') and name='负债'");
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMiddb).ExecuteTable(sql);
                for (int iRow = 0; iRow < count; iRow++)
                {
                    debtSet[iRow] = dt.Rows[iRow]["value"].ToString();
                }

                sql =
                    string.Format("select * from (select substr(trim(t.fitem),3,2) as name,round(fcurrentvalue/1000000,2) as value,t.fyear as year,t.fperiod as month,t.fcompanyname from middb.T_WL_ZCFZB t where t.fseqno=16 or t.fseqno=57  and t.fcompanyname='江苏连云港港物流控股有限公司' order by fyear||lpad(fperiod,2,0)) where YEAR=to_char(sysdate,'yyyy')-1 and name='资产'");
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMiddb).ExecuteTable(sql);
                for (int iRow = 0; iRow < count; iRow++)
                {
                    lastassetSet[iRow] = dt.Rows[iRow]["value"].ToString();
                }

                sql =
                    string.Format("select * from (select substr(trim(t.fitem),3,2) as name,round(fcurrentvalue/1000000,2) as value,t.fyear as year,t.fperiod as month,t.fcompanyname from middb.T_WL_ZCFZB t where t.fseqno=16 or t.fseqno=57  and t.fcompanyname='江苏连云港港物流控股有限公司' order by fyear||lpad(fperiod,2,0)) where YEAR=to_char(sysdate,'yyyy')-1 and name='负债'");
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMiddb).ExecuteTable(sql);
                for (int iRow = 0; iRow < count; iRow++)
                {
                    lastdebtSet[iRow] = dt.Rows[iRow]["value"].ToString();
                }
            }

            info.Add("Month", dateSet);
            info.Add("Asset", assetSet);
            info.Add("Debt", debtSet);
            info.Add("LastAsset", lastassetSet);
            info.Add("LastDebt", lastdebtSet);
            return info;
        }

        /// <summary>
        /// 利润月度情况
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetMonthlyProfit()
        {
            string[] dateSet = { "" };
            string[] profitSet = { "" };
            string[] planSet = { "" };
            string[] lastProfitSet = { "" };
            string[] cashRateSet = { "" };

            string sql =
                    string.Format("select '利润' as name,fitem,round(fcurrentvalue/1000000,2) as value,t.fyear as year,t.fperiod as month from middb.T_WL_LRB t where fitem like '%四、净利润%'  and FYEAR=to_char(sysdate,'yyyy') and t.fcompanyname='江苏连云港港物流控股有限公司' order by fyear||lpad(fperiod,2,0)");
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMiddb).ExecuteTable(sql);
            if (dt0.Rows.Count != 0)
            {
                dateSet = new string[dt0.Rows.Count];
                profitSet = new string[dt0.Rows.Count];
                planSet = new string[dt0.Rows.Count];
                lastProfitSet = new string[dt0.Rows.Count];
                cashRateSet = new string[dt0.Rows.Count];
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    dateSet[iRow] = dt0.Rows[iRow]["month"].ToString();
                    profitSet[iRow] = dt0.Rows[iRow]["value"].ToString();
                }

                sql =
                    string.Format("select '利润' as name,fitem,round(fcurrentvalue/1000000,2) as value,t.fyear as year,t.fperiod as month from middb.T_WL_LRB t where fitem like '%四、净利润%'  and FYEAR=to_char(sysdate,'yyyy')-1 and t.fcompanyname='江苏连云港港物流控股有限公司' order by fyear||lpad(fperiod,2,0)");
                var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMiddb).ExecuteTable(sql);
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    lastProfitSet[iRow] = dt2.Rows[iRow]["value"].ToString();
                }

                sql =
                    string.Format("select t.planvalue,t.planitem,t.plan_year,t.plan_months,t.company  from wl.plan_data t where CODETYPE='03' and company like'物流控股%' and t.planitem='利润总额' and t.plan_year=to_char(sysdate,'yyyy') order by t.plan_months asc");
                var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                //int count = dt1.Rows.Count > 0 ? dt0.Rows.Count : 0;
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    planSet[iRow] = iRow < dt1.Rows.Count ? dt1.Rows[iRow]["planvalue"].ToString() : "0.0";
                }

                for (int iRow = dt0.Rows.Count - 1; iRow >= 0; iRow--)
                {
                    double cashPercent;
                    if (planSet[iRow] == null || planSet[iRow] == "" || planSet[iRow] == "0.0")
                    {
                        cashPercent = 100.0;
                    }
                    else
                    {
                        cashPercent = Convert.ToDouble(profitSet[iRow]) / Convert.ToDouble(planSet[iRow]) * 100.0;
                    }
                    string cashRate;
                    cashRate = (cashPercent < 0) ? string.Format("{0:F}%", cashPercent) : string.Format("+{0:F}%", cashPercent);
                    cashRateSet[iRow] = cashRate;
                }
            }

            info.Add("Month", dateSet);
            info.Add("LastProfi", lastProfitSet);
            info.Add("Plan", planSet);
            info.Add("Profit", profitSet);                  
            info.Add("CashRate", cashRateSet);
            return info;
        }

        /// <summary>
        /// 本年集装箱代理量趋势分析
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetCurYearContainer()
        {
            string[] dateSet = { "" };
            string[] amountSet = { "" };
            string[] lastAmountSet = { "" };
            string[] amountPercentlSet  = { "" };

            string sql =
                    string.Format("select sum(AMOUNT) AS AMOUNT,CODE_TYPE,B_MONTH from BIZ_JZX t where b_year=to_char(sysdate,'yyyy') GROUP BY CODE_TYPE,B_MONTH order by b_month");
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            if (dt0.Rows.Count != 0)
            {
                dateSet = new string[dt0.Rows.Count];
                amountSet = new string[dt0.Rows.Count];
                lastAmountSet = new string[dt0.Rows.Count];
                amountPercentlSet = new string[dt0.Rows.Count];
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    dateSet[iRow] = dt0.Rows[iRow]["B_MONTH"].ToString();
                    amountSet[iRow] = dt0.Rows[iRow]["AMOUNT"].ToString();
                }

                sql =
                    string.Format("select sum(AMOUNT) AS AMOUNT,CODE_TYPE,B_MONTH from BIZ_JZX t where b_year=to_char(sysdate,'yyyy')-1 GROUP BY CODE_TYPE,B_MONTH order by b_month");
                var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    lastAmountSet[iRow] = dt1.Rows[iRow]["AMOUNT"].ToString();
                }

                for (int iRow = dt0.Rows.Count - 1; iRow >= 0; iRow--)
                {
                    //var aa = Convert.ToDouble(dt0.Rows[iRow]["AMOUNT"]);
                    //var bb = Convert.ToDouble(dt1.Rows[iRow]["AMOUNT"]);
                    double amountInterval;
                    if (lastAmountSet[iRow] == null || lastAmountSet[iRow] == "" || lastAmountSet[iRow] == "0")
                    {
                        amountInterval = 100.0;
                    }
                    else
                    {
                        amountInterval = (Convert.ToDouble(amountSet[iRow]) - Convert.ToDouble(lastAmountSet[iRow])) / Convert.ToDouble(lastAmountSet[iRow]) * 100.0;
                    }

                    string amountPercent;
                    amountPercent = (amountInterval < 0) ? string.Format("{0:F}%", amountInterval) : string.Format("+{0:F}%", amountInterval);
                    amountPercentlSet[iRow] = amountPercent;
                }
            }

            info.Add("Date", dateSet);
            info.Add("Amount", amountSet);
            info.Add("LastAmount", lastAmountSet);
            info.Add("AmountPercent", amountPercentlSet);
            return info;
        }

        /// <summary>
        /// 本年散杂货代理量趋势分析
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetCurYearBulkandGeneralCargo()
        {
            string[] dateSet = { "" };
            string[] amountSet = { "" };
            string[] lastAmountSet = { "" };
            string[] amountPercentlSet = { "" };

            string sql =
                    string.Format("select round(sum(AMOUNT)/10000,2) AS AMOUNT,CODE_TYPE,B_MONTH from BIZ_DLL t where b_year=to_char(sysdate,'yyyy')  GROUP BY CODE_TYPE,B_MONTH  order by b_month");
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            if (dt0.Rows.Count != 0)
            {
                dateSet = new string[dt0.Rows.Count];
                amountSet = new string[dt0.Rows.Count];
                lastAmountSet = new string[dt0.Rows.Count];
                amountPercentlSet = new string[dt0.Rows.Count];
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    dateSet[iRow] = dt0.Rows[iRow]["B_MONTH"].ToString();
                    amountSet[iRow] = dt0.Rows[iRow]["AMOUNT"].ToString();
                }

                sql =
                    string.Format("select round(sum(AMOUNT)/10000,2) AS AMOUNT,CODE_TYPE,B_MONTH from BIZ_DLL t where b_year=to_char(sysdate,'yyyy')-1  GROUP BY CODE_TYPE,B_MONTH  order by b_month");
                var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    lastAmountSet[iRow] = dt1.Rows[iRow]["AMOUNT"].ToString();
                }

                for (int iRow = dt0.Rows.Count - 1; iRow >= 0; iRow--)
                {
                    //var aa = Convert.ToDouble(dt0.Rows[iRow]["AMOUNT"]);
                    //var bb = Convert.ToDouble(dt1.Rows[iRow]["AMOUNT"]);
                    double amountInterval;
                    if (lastAmountSet[iRow] == null || lastAmountSet[iRow] == "" || lastAmountSet[iRow] == "0")
                    {
                        amountInterval = 100.0;
                    }
                    else
                    {
                        amountInterval = (Convert.ToDouble(amountSet[iRow]) - Convert.ToDouble(lastAmountSet[iRow])) / Convert.ToDouble(lastAmountSet[iRow]) * 100.0;
                    }

                    string amountPercent;
                    amountPercent = (amountInterval < 0) ? string.Format("{0:F}%", amountInterval) : string.Format("+{0:F}%", amountInterval);
                    amountPercentlSet[iRow] = amountPercent;
                }
            }

            info.Add("Date", dateSet);
            info.Add("Amount", amountSet);
            info.Add("LastAmount", lastAmountSet);
            info.Add("AmountPercent", amountPercentlSet);
            return info;
        }

        /// <summary>
        /// 本年船舶代理量趋势分析
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetCurYearShip()
        {
            string[] dateSet = { "" };
            string[] amountSet = { "" };
            string[] lastAmountSet = { "" };
            string[] amountPercentlSet = { "" };

            string sql =
                    string.Format("select sum(AMOUNT) as AMOUNT,CODE_TYPE,B_MONTH from BIZ_CB t  where b_year=to_char(sysdate,'yyyy')  GROUP BY CODE_TYPE,B_MONTH  order by b_month");
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            if (dt0.Rows.Count != 0)
            {
                dateSet = new string[dt0.Rows.Count];
                amountSet = new string[dt0.Rows.Count];
                lastAmountSet = new string[dt0.Rows.Count];
                amountPercentlSet = new string[dt0.Rows.Count];

                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    dateSet[iRow] = dt0.Rows[iRow]["B_MONTH"].ToString();
                    amountSet[iRow] = dt0.Rows[iRow]["AMOUNT"].ToString();
                }

                sql =
                    string.Format("select sum(AMOUNT) as AMOUNT,CODE_TYPE,B_MONTH from BIZ_CB t  where b_year=to_char(sysdate,'yyyy')-1  GROUP BY CODE_TYPE,B_MONTH  order by b_month");
                var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    lastAmountSet[iRow] = dt1.Rows[iRow]["AMOUNT"].ToString();
                }

                for (int iRow = dt0.Rows.Count - 1; iRow >= 0; iRow--)
                {
                    //var aa = Convert.ToDouble(dt0.Rows[iRow]["AMOUNT"]);
                    //var bb = Convert.ToDouble(dt1.Rows[iRow]["AMOUNT"]);
                    double amountInterval;
                    if (lastAmountSet[iRow] == null || lastAmountSet[iRow] == "" || lastAmountSet[iRow] == "0")
                    {
                        amountInterval = 100.0;
                    }
                    else
                    {
                        amountInterval = (Convert.ToDouble(amountSet[iRow]) - Convert.ToDouble(lastAmountSet[iRow])) / Convert.ToDouble(lastAmountSet[iRow]) * 100.0;
                    }
                    string amountPercent;
                    amountPercent = (amountInterval < 0) ? string.Format("{0:F}%", amountInterval) : string.Format("+{0:F}%", amountInterval);
                    amountPercentlSet[iRow] = amountPercent;
                }
            }

            info.Add("Date", dateSet);
            info.Add("Amount", amountSet);
            info.Add("LastAmount", lastAmountSet);
            info.Add("AmountPercent", amountPercentlSet);
            return info;
        }

        /// <summary>
        /// 今日保税进出量
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetTodayBondInAndOut()
        {
            string[] dataSet = new string[16];
            string time = Convert.ToString(Convert.ToInt16(DateTime.Now.Year) - 1) + "-12-21";
            //陆桥非落地进库量
            string sql =
                    string.Format("select decode(sum(c.INOUT_COUNT), null, 0, sum(c.INOUT_COUNT)) as lqfldj from lbts.vw_lq_fldb_in c where signdate=(SELECT MAX(lbts.vw_lq_fldb_in.signdate) FROM lbts.vw_lq_fldb_in)");
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[0] = dt0.Rows.Count == 0 ? "" : dt0.Rows[0]["lqfldj"].ToString();
            //陆桥非落地年累计进库量
            sql =
                string.Format("select decode(sum(t.inout_count),'','0',sum(t.inout_count))  as IN_YEAR from lbts.VW_LQ_YEAR_FLDB_IN t where t.iport_dept='6397' and t.inout_date between '{0}' and 'substr(sysdate,1,8)'", time);
            var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[1] = dt1.Rows.Count == 0 ? "" : dt1.Rows[0]["IN_YEAR"].ToString();
            //陆桥非落地出库量
            sql =
                string.Format("select decode(sum(c.INOUT_COUNT), null, 0, sum(c.INOUT_COUNT)) as lqfldc from  lbts.vw_lq_fldb_out c where signdate = (SELECT MAX(lbts.vw_lq_fldb_out.signdate) FROM lbts.vw_lq_fldb_out)");
            var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[2] = dt2.Rows.Count == 0 ? "" : dt2.Rows[0]["lqfldc"].ToString();
            //陆桥非落地年累计出库量
            sql =
                string.Format("select decode(sum(t.inout_count),'','0',sum(t.inout_count))  as IN_YEAR from lbts.VW_LQ_YEAR_FLDB_OUT t where t.iport_dept='6397' and t.inout_date between '{0}' and 'substr(sysdate,1,8)'", time);
            var dt3 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[3] = dt3.Rows.Count == 0 ? "" : dt3.Rows[0]["IN_YEAR"].ToString();

            //陆桥落地进库量
            sql =
                string.Format("select decode(sum(b.gross), null, 0, sum(b.gross)) as lablqldj from lbts.vw_lq_ldb_in b where  signdate = (SELECT MAX(lbts.vw_lq_ldb_in.signdate) FROM lbts.vw_lq_ldb_in)");
            var dt4 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[4] = dt4.Rows.Count == 0 ? "" : dt4.Rows[0]["lablqldj"].ToString();
            //陆桥落地年累计进库量
            sql =
                string.Format("select decode(sum(t.gross),'','0',sum(t.gross))as net from lbts.VIEW_LQ_YEAR_LDB_IN t where t.iport_dept='6397' and t.apply_customs_date between '{0}' and 'substr(sysdate,1,8)'", time);
            var dt5 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[5] = dt5.Rows.Count == 0 ? "" : dt5.Rows[0]["net"].ToString();
            //陆桥落地出库量
            sql =
                string.Format("select decode(sum(b.gross), null, 0, sum(b.gross)) as lablqldc from lbts.vw_lq_ldb_out b where signdate = (SELECT MAX(lbts.vw_lq_ldb_out.signdate) FROM lbts.vw_lq_ldb_out)");
            var dt6 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[6] = dt6.Rows.Count == 0 ? "" : dt6.Rows[0]["lablqldc"].ToString();
            //陆桥落地年累计出库量
            sql =
                string.Format("select decode(sum(t.gross),'','0',sum(t.gross))as net from lbts.VIEW_LQ_YEAR_LDB_OUT t where t.iport_dept='6397' and t.apply_customs_date between '{0}' and 'substr(sysdate,1,8)'", time);
            var dt7 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[7] = dt7.Rows.Count == 0 ? "" : dt7.Rows[0]["net"].ToString();

            //郁港非落地进库量
            sql =
                string.Format("select decode(sum(c.INOUT_COUNT), null, 0, sum(c.INOUT_COUNT)) as labygfldj from  lbts.vw_yg_fldb_in c where   signdate = (SELECT MAX(lbts.vw_yg_fldb_in.signdate) FROM lbts.vw_yg_fldb_in)");
            var dt8 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[8] = dt8.Rows.Count == 0 ? "" : dt8.Rows[0]["labygfldj"].ToString();
            //郁港非落地年累计进库量
            sql =
                string.Format("select decode(sum(t.inout_count),'','0',sum(t.inout_count))  as IN_YEAR from lbts.VW_LQ_YEAR_FLDB_IN t where t.iport_dept='6396' and t.inout_date between '{0}' and 'substr(sysdate,1,8)'", time);
            var dt9 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[9] = dt9.Rows.Count == 0 ? "" : dt9.Rows[0]["IN_YEAR"].ToString();
            //郁港非落地出库量
            sql =
                string.Format("select decode(sum(c.INOUT_COUNT), null, 0, sum(c.INOUT_COUNT)) as labygfldc from  lbts.vw_yg_fldb_out c where   signdate = (SELECT MAX(lbts.vw_yg_fldb_out.signdate) FROM lbts.vw_yg_fldb_out)");
            var dt10 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[10] = dt10.Rows.Count == 0 ? "" : dt10.Rows[0]["labygfldc"].ToString();
            //郁港非落地年累计出库量
            sql =
                string.Format("select decode(sum(t.inout_count),'','0',sum(t.inout_count))  as IN_YEAR from lbts.VW_LQ_YEAR_FLDB_OUT t where t.iport_dept='6396' and t.inout_date between '{0}' and 'substr(sysdate,1,8)'", time);
            var dt11 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[11] = dt11.Rows.Count == 0 ? "" : dt11.Rows[0]["IN_YEAR"].ToString();

            //郁港落地进库量
            sql =
                string.Format("select decode(sum(b.gross), null, 0, sum(b.gross)) as labygldj from lbts.vw_yg_ldb_in b where  signdate = (SELECT MAX(lbts.vw_yg_ldb_in.signdate) FROM lbts.vw_yg_ldb_in)");
            var dt12 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[12] = dt12.Rows.Count == 0 ? "" : dt12.Rows[0]["labygldj"].ToString();
            //郁港落地年累计进库量
            sql =
                string.Format("select decode(sum(t.gross),'','0',sum(t.gross))as net from lbts.VIEW_LQ_YEAR_LDB_IN t where t.iport_dept='6396' and t.apply_customs_date between '{0}' and 'substr(sysdate,1,8)'", time);
            var dt13 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[13] = dt13.Rows.Count == 0 ? "" : dt13.Rows[0]["net"].ToString();
            //郁港落地出库量
            sql =
                string.Format("select decode(sum(b.gross), null, 0, sum(b.gross)) as labygldc from lbts.vw_yg_ldb_out b where signdate = (SELECT MAX(lbts.vw_yg_ldb_out.signdate) FROM lbts.vw_yg_ldb_out)");
            var dt14 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);;
            dataSet[14] = dt14.Rows.Count == 0 ? "" : dt14.Rows[0]["labygldc"].ToString();
            //郁港落地年累计出库量
            sql =
                string.Format("select decode(sum(t.gross),'','0',sum(t.gross))as net from lbts.VIEW_LQ_YEAR_LDB_OUT t where t.iport_dept='6396' and t.apply_customs_date between '{0}' and 'substr(sysdate,1,8)'", time);
            var dt15 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[15] = dt15.Rows.Count == 0 ? "" : dt15.Rows[0]["net"].ToString();

            info.Add("TodayBondInAndOut", dataSet);
            return info;
        }

        /// <summary>
        /// 今日贸易情况
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetTodayTrade()
        {
            string[] dataSet = new string[4];
            //今日购进量
            string sql =
                        "select  '购进量'as MYtype,round(sum(AMOUNT_IN),2) as amount from wl.biz_info_view a where a.code_type ='贸易量' and a.actualtime  ";
                    sql +=
                        "in(select * from (select actualtime from wl.biz_info_view b where b.code_type ='贸易量'and b.actualtime<=trunc(sysdate)  order by trunc(actualtime) desc) where rownum=1) ";
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathBases).ExecuteTable(sql);
            dataSet[0] = dt0.Rows.Count == 0 ? "" : dt0.Rows[0]["AMOUNT"].ToString();
            //今日销售量
                    sql =
                         "select  '销售量'as MYtype,round(sum(AMOUNT_OUT),2) as amount from wl.biz_info_view a where a.code_type ='贸易量' and a.actualtime  ";
                    sql +=
                        "in(select * from (select actualtime from wl.biz_info_view b where b.code_type ='贸易量'and b.actualtime<=trunc(sysdate)  order by trunc(actualtime) desc) where rownum=1) ";
            var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathBases).ExecuteTable(sql);
            dataSet[1] = dt1.Rows.Count == 0 ? "" : dt1.Rows[0]["AMOUNT"].ToString();
            //今日销售量
            //散杂货港存量
                    sql =
                        "select round(sum(STOCK_end),2) as sum from(select rownum,t.CARGO,t.CREATETIME,t.STOCK_end,    ";
                    sql +=
                        "row_number() over (partition by t.CARGO order by CREATETIME desc) row_number    ";
                    sql +=
                        "from wl.biz_info_view t where t.code_type ='贸易量' and t.STOCK_end>0 and cargo not in('GPS设备','原木','板材'))where row_number=1";
            var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[2] = dt2.Rows.Count == 0 ? "" : dt2.Rows[0]["sum"].ToString();
            //木材货港存量
                    sql =
                        "select round(sum(STOCK_end),2) as sum from(select rownum,t.CARGO,t.CREATETIME,t.STOCK_end,    ";
                    sql +=
                        "row_number() over (partition by t.CARGO order by CREATETIME desc) row_number    ";
                    sql +=
                        "from wl.biz_info_view t where t.code_type ='贸易量' and t.STOCK_end>0 and cargo in('原木','板材'))where row_number=1";
            var dt3 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[3] = dt3.Rows.Count == 0 ? "" : dt3.Rows[0]["sum"].ToString();

            info.Add("TodayTrade", dataSet);
            return info;
        }

        /// <summary>
        /// 今日仓储进库量
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetTodayStorageInAndOut()
        {
            string[] dataSet = new string[5];
            //今日仓储进库量
            string sql =
                    string.Format("select round(decode(sum(weight), NULL, 0, sum(weight))) as inweight from storageharbor.vw_hs_goodsbill_tallybill_b_i where code_company = '6377' and signdate = (SELECT MAX(storageharbor.vw_hs_goodsbill_tallybill_b_i.signdate) FROM storageharbor.vw_hs_goodsbill_tallybill_b_i)");
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathBases).ExecuteTable(sql);
            dataSet[0] = dt0.Rows.Count == 0 ? "" : dt0.Rows[0]["inweight"].ToString();
            //仓储公司年累计进库量
            sql =
                string.Format("select round(decode(sum(weight), NULL, 0, sum(weight))) as sum from storageharbor.vw_hs_goodsbill_tallybill_b where code_company = '6377'and (code_opstype = '1' or code_opstype = '3') and  extract(year from signdate) =extract(year from  sysdate)");
            var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathBases).ExecuteTable(sql);
            dataSet[1] = dt1.Rows.Count == 0 ? "" : dt1.Rows[0]["sum"].ToString();
            //今日仓储出库量
            sql =
                string.Format("select round(decode(sum(weight), NULL, 0, sum(weight))) as outweight from storageharbor.vw_hs_goodsbill_tallybill_b_o where code_company = '6377' and signdate = (SELECT MAX(storageharbor.vw_hs_goodsbill_tallybill_b_o.signdate) FROM storageharbor.vw_hs_goodsbill_tallybill_b_o)");
            var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathBases).ExecuteTable(sql);
            dataSet[2] = dt2.Rows.Count == 0 ? "" : dt2.Rows[0]["outweight"].ToString();
            //仓储公司年累计出库量
            sql =
                string.Format("Select round(decode(sum(weight), NULL, 0, sum(weight))) as sum from storageharbor.vw_hs_goodsbill_tallybill_b where code_company = '6377'and (code_opstype = '2' or code_opstype = '3') and  extract(year from signdate) =extract(year from sysdate)");
            var dt3 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathBases).ExecuteTable(sql);
            dataSet[3] = dt3.Rows.Count == 0 ? "" : dt3.Rows[0]["sum"].ToString();
            //今日仓储场存量
            sql =
                string.Format("select round(weight,2) as weight,signdate from (select sum(t.weight) weight,t.signdate from storageharbor.TB_HS_STOCKDORMANTHISTORY t where to_number(to_char(signdate,'yyyy'))>=2014 group by t.signdate  order by signdate desc) where rownum=1");
            var dt4 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathBases).ExecuteTable(sql);
            dataSet[4] = dt4.Rows.Count == 0 ? "" : dt4.Rows[0]["weight"].ToString();

            info.Add("TodayStorageInAndOut", dataSet);
            return info;
        }

        /// <summary>
        /// 今日发运业务情况
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetTodayDeliverBusiness()
        {
            string[] dataSet = new string[4];
            //今日火车发运量
            string sql = 
                        "select a.code_type,a.measure,SUM(a.AMOUNT) AS AMOUNT from wl.business_info a where a.code_type ='发运量'and a.despatch='火车' and a.actualtime  ";
                   sql += 
                       "in(select * from (select actualtime from wl.business_info b where b.code_type ='发运量'and b.despatch='火车'and b.actualtime<=trunc(sysdate)  order by trunc(actualtime) desc) where rownum=1) ";
                   sql += 
                       "group by a.code_type,a.measure";
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathBases).ExecuteTable(sql);
            dataSet[0] = dt0.Rows.Count == 0 ? "" : dt0.Rows[0]["AMOUNT"].ToString();
            //今日汽车发运量
                   sql = 
                        "select a.code_type,a.measure,SUM(a.AMOUNT) AS AMOUNT from wl.business_info a where a.code_type ='发运量'and a.despatch='汽车' and a.actualtime ";
                   sql +=
                       "in(select * from (select actualtime from wl.business_info b where b.code_type ='发运量'and b.despatch='汽车'and b.actualtime<=trunc(sysdate)  order by trunc(actualtime) desc) where rownum=1)";
                   sql += 
                       "group by a.code_type,a.measure";
            var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathBases).ExecuteTable(sql);
            dataSet[1] = dt1.Rows.Count == 0 ? "" : dt1.Rows[0]["AMOUNT"].ToString();
            //年累计发运量吨
                   sql = 
                        string.Format("select round(sum(t.AMOUNT),2) as amount  from biz_info_view t where t.code_type='发运量' and t.measure='吨' and t.b_year=to_char(sysdate,'yyyy')");
                   var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[2] = dt2.Rows.Count == 0 ? "" : dt2.Rows[0]["AMOUNT"].ToString();
            //年累计发运量立方米
                   sql = 
                        string.Format("select round(sum(t.AMOUNT),2) as amount  from biz_info_view t where t.code_type='发运量' and t.measure='立方米'  and t.b_year=to_char(sysdate,'yyyy')");
                   var dt3 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[3] = dt3.Rows.Count == 0 ? "" : dt3.Rows[0]["AMOUNT"].ToString();

            info.Add("TodayStorageInAndOut", dataSet);
            return info;
        }

        /// <summary>
        /// 今日B保公司业务情况
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetTodayBBondCompanyBusiness()
        {
            string[] dataSet = new string[4];
            //入住企业
            string sql =
                    string.Format("select count(*) as count from dbzone.tb_br_client");
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[0] = dt0.Rows.Count == 0 ? "" : dt0.Rows[0]["count"].ToString();
            //入库量
            sql =
                string.Format("select round(decode(sum(weight), NULL, 0, sum(weight))) as inweight from dbzone.vw_hs_goodsbill_tallybill_b_i where code_company = '701' and signdate = (SELECT MAX(dbzone.vw_hs_goodsbill_tallybill_b_i.signdate) FROM dbzone.vw_hs_goodsbill_tallybill_b_i)");
            var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[1] = dt1.Rows.Count == 0 ? "" : dt1.Rows[0]["inweight"].ToString();
            //出库量
            sql =
                string.Format("select round(decode(sum(weight), NULL, 0, sum(weight))) as outweight from dbzone.vw_hs_goodsbill_tallybill_b_o where code_company = '701' and signdate = (SELECT MAX(dbzone.vw_hs_goodsbill_tallybill_b_o.signdate) FROM dbzone.vw_hs_goodsbill_tallybill_b_o)");
            var dt2 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[2] = dt2.Rows.Count == 0 ? "" : dt2.Rows[0]["outweight"].ToString();
            //港存量
            sql =
                string.Format("select round(weight,2) as weight,signdate from (select sum(t.weight) weight,t.signdate from dbzone.TB_HS_STOCKDORMANTHISTORY t where to_number(to_char(signdate,'yyyy'))>=2014 group by t.signdate  order by signdate desc) where rownum=1");
            var dt3 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            dataSet[3] = dt3.Rows.Count == 0 ? "" : dt3.Rows[0]["weight"].ToString();

            info.Add("TodayStorageInAndOut", dataSet);
            return info;
        }

        /// <summary>
        /// 各公司年度盈利情况
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Array> GetCompanyAnnualProfit()
        {
            string[] companySet = { "0" };
            string[] dateSet = { "0" };

            string sql =
                        "select '去年累计金额' as aT, ycompany,yyear, round(sum(yaccount),2) as SJ from (select YITEM,yaccount,YCOMPANY,YYEAR,ymonth from TB_FI_YINGSHOU  WHERE yitem='本月营收金额'and yyear= to_char(sysdate, 'yyyy' )-1) group by ycompany,yyear union ";
                   sql +=
                        "select '今年累计金额' as aT, ycompany,yyear, round(sum(yaccount),2) as SJ from (select YITEM,yaccount,YCOMPANY,YYEAR,ymonth from TB_FI_YINGSHOU  WHERE yitem='本月营收金额'and yyear= to_char(sysdate, 'yyyy' )) group by ycompany,yyear union ";
                   sql +=
                        "select '今年计划数'  as AT,ycompany,yyear ,round(yaccount,2) as SJ from TB_FI_YINGSHOU WHERE yitem='今年计划数' and yyear= to_char(sysdate, 'yyyy' )";
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWl).ExecuteTable(sql);
            if (dt0.Rows.Count != 0)
            {
                companySet = new string[dt0.Rows.Count];
                dateSet = new string[dt0.Rows.Count];
                for (int iRow = 0; iRow < dt0.Rows.Count; iRow++)
                {
                    companySet[iRow] = dt0.Rows[iRow]["ycompany"].ToString();
                    dateSet[iRow] = dt0.Rows[iRow]["SJ"].ToString();
                }
            }

            info.Add("Company", companySet);
            info.Add("AnnualProfit", dateSet);
            return info;
        }
   }
}