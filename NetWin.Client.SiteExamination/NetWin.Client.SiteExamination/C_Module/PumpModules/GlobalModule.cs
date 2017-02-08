using System;
using System.Collections.Generic;

using System.Text;
using NetWin.Client.SiteExamination.A_Core.Enum;
using NetWin.Client.SiteExamination.B_Common;
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
                    if (site.CurrentUrlUrl.Contains("="))
                        break;


                    WingManCount += 1;

                    if (!RegexHelper.CheckLevel(site.CurrentUrlUrl))
                    {
                        AimsCount += 1;
                    }

                    if (WingManCount != 0)
                    {
                        double percent = Convert.ToDouble(AimsCount) / Convert.ToDouble(WingManCount);
                        string result = string.Format("{0:0.00%}", percent);
                        AimsContent = "总体URL层级3（含）以内占比" + result;
                    }
                    break;
                case "insidelinkcount":
                    //foreach (var item in site.InsideLinks)
                    //{
                    //    if (!InsideLinks.Contains(item))
                    //    {
                    //        InsideLinks.Add(item);
                    //    }
                    //}
                    if (site.SiteUrl != SourceUrl && site.StatusCode != 502 && site.StatusCode != 0)
                        AimsCount += 1;
                    break;

                case "nullsite":

                    SourceUrl = string.Empty;
                    if (string.IsNullOrEmpty(site.InnerText) || site.StatusCode == 404)
                    {
                        AimsCount = AimsCount + 1;
                    }

                    if (site.StatusCode != 502 && site.StatusCode != 0)
                    {
                        WingManCount++;
                    }

                    if (AimsCount != 0)
                    {
                        double percent = Convert.ToDouble(AimsCount) / Convert.ToDouble(WingManCount);
                        string result = string.Format("{0:0.00%}", percent);
                        AimsContent = "垃圾页面占比:" + result;
                    }
                    else
                    {
                        AimsContent = "垃圾页面占比:0";
                    }


                    break;

                case "dynamic":
                    SourceUrl = string.Empty;

                    AimsCount += site.IsDynamic && site.StatusCode != 404 ? 1 : 0;
                    if (AimsCount > 0)
                    {
                        AimsContent = string.Format("存在动态链接数:{0}.原因可能如下：1.网站URL是否为静态，因为网站动态过长，不利于收录和网站排名；2.服务器未设置lastModified", AimsCount);
                    }
                    else
                    {
                        AimsContent = "未检查到动态链接";
                    }

                    break;
                case "keywordtime":
                    if (Keywords.Count > 0)
                    {
                        foreach (var item in site.KeyWords)
                        {
                            if (Keywords.ContainsKey(item))
                                Keywords[item]++;
                        }
                        AimsCount = EnumerableHelper.Min(Keywords.Values);
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
