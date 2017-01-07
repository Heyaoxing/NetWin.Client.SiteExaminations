using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.A_Core.Enum;
using NetWin.Client.SiteExamination.B_Common.Interface;

namespace NetWin.Client.SiteExamination.C_Module.PumpModules
{
    /// <summary>
    /// 全局计算类
    /// </summary>
    internal class GlobalModule : IComputeRule
    {

        /// <summary>
        /// 目标数值统计方法
        /// </summary>
        /// <param name="site"></param>
        internal override void ComputeCount(InSite site)
        {
            if (site == null)
                return;

            switch (AimsContainText)
            {

                case "h1":
                    AimsCount += site.H1Count;
                    break;
                case "h2-h6":
                    AimsCount += site.H2Count + site.H3Count + site.H4Count + site.H5Count + site.H6Count;
                    break;
                case "level":
                    if (site.Level <= 3)
                    {
                        AimsCount += 1;
                    }
                    WingManCount += 1;

                    if (WingManCount != 0)
                    {
                        double percent=Convert.ToDouble(AimsCount)/Convert.ToDouble(WingManCount);
                        string result=string.Format("{0:0.00%}",percent);
                        AimsContent = "总体URL层级3（含）以内占比" + result;
                    }

                    break;
                case "insidelinkcount":
                    AimsCount += site.InsideLinkCount;
                    break;
                case "outsidelinkcount":
                    AimsCount += site.OutsideLinkCount;
                    break;
                case "nullsite":
                     AimsCount += string.IsNullOrWhiteSpace(site.InnerText) ? 1 : 0;
                    WingManCount++;

                    if (AimsCount != 0)
                    {
                        double percent = Convert.ToDouble(AimsCount)/Convert.ToDouble(WingManCount);
                        string result = string.Format("{0:0.00%}", percent);
                        AimsContent = "垃圾页面占比:" + result;
                    }
                    else
                    {
                        AimsContent = "垃圾页面占比:0";
                    }
                    break;
               
                case "dynamic":
                    AimsCount += site.IsDynamic ? 1 : 0;
                    break;

                case "keywordtime":
                    if (Keywords.Any())
                    {
                        foreach (var item in site.KeyWords)
                        {
                            if (Keywords.ContainsKey(item))
                                Keywords[item]++;
                        }
                        AimsCount = Keywords.Min(p => p.Value);
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in Keywords)
                        {
                            sb.AppendFormat("{0}:{1} ", item.Key, item.Value);
                        }
                        AimsContent = sb.ToString();
                    }
                    else
                    {
                        AimsCount = 0;
                        AimsContent = "无关键词";
                    }

                    break;
            }
        }
    }
}
