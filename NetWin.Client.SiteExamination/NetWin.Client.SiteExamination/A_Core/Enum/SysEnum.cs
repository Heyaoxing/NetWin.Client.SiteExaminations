using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.C_Module.SpiderModules;

namespace NetWin.Client.SiteExamination.A_Core.Enum
{
    /// <summary>
    /// 统计类型
    /// </summary>
    public enum StatisticsEnum
    {
        /// <summary>
        /// 累计(默认值)
        /// </summary>
        Grandtotal = 0,

        /// <summary>
        /// 取最大值
        /// </summary>
        Max = 1,

        /// <summary>
        /// 取最小值
        /// </summary>
        Min = 2
    }

    /// <summary>
    /// 匹配类型
    /// </summary>
    public enum SpiderTypeEnum
    {
        /// <summary>
        /// 内部抓取
        /// </summary>
        InSpider=1,
        /// <summary>
        /// 外部抓取
        /// </summary>
        OutSpider=2
    }

    /// <summary>
    /// 判断类型
    /// </summary>
    public enum JudgeTypeEnum
    {
        /// <summary>
        /// 小于等于目标统计值
        /// </summary>
        LessThanOrEqualByAims = 100,

        /// <summary>
        /// 大于目标统计值
        /// </summary>
         Greater= 200,

        /// <summary>
        /// 小于等于目标值与僚机值比例
        /// </summary>
       LessThanEqualByScale = 300,

        /// <summary>
        /// 大于目标值与僚机值比例
        /// </summary>
       GreaterOrByScale = 400,
    }

    /// <summary>
    /// 计算类型
    /// </summary>
    public enum ComputeTypeEnum
    {
        /// <summary>
        /// 过程计算
        /// </summary>
        Section=1,

        /// <summary>
        /// 全局计算
        /// </summary>
        Global=2
    }

   

    public enum LogType
    {
        /// <summary>
        /// 插入型
        /// </summary>
        Info,
        /// <summary>
        /// 更新型
        /// </summary>
        Warning,
        /// <summary>
        /// 所有
        /// </summary>
        Error,
        All
    }

    /// <summary>
    /// 返回结果时机
    /// </summary>
    public enum MomentType
    {
        Error=100,
        Normal=200
    }
}
