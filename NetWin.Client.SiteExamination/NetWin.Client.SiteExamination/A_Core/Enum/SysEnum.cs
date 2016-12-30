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
        /// 大于等于目标统计值
        /// </summary>
        GreaterOrEqualByAims=100,

        /// <summary>
        /// 小于目标统计值
        /// </summary>
        LessThan = 200,

        /// <summary>
        /// 大于等于目标值与僚机值比例
        /// </summary>
        GreaterOrEqualByScale = 300,

        /// <summary>
        /// 大于等于目标值与僚机值比例
        /// </summary>
        LessThanByScale = 400,
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
}
