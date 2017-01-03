using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.A_Core.Enum;

namespace NetWin.Client.SiteExamination.C_Module.PumpModules
{
    public class PumpInitParam
    {
        /// <summary>
        /// 具体哪项检查项目的id
        /// </summary>
        public int DetailId { set; get; }

        /// <summary>
        /// 体检评分
        /// </summary>
        public int Score { set; get; }

        /// <summary>
        /// 计算类型
        /// </summary>
        public ComputeTypeEnum ComputeType { set; get; }

        /// <summary>
        /// 抓取类型
        /// </summary>
        public SpiderTypeEnum SpiderType { set; get; }

        /// <summary>
        /// 目标匹配文本
        /// </summary>
        public string AimsContainText { set; get; }

        /// <summary>
        /// 僚机匹配文本
        /// </summary>
        public string WingManContainText { set; get; }

        /// <summary>
        /// 判断类型
        /// </summary>
        public JudgeTypeEnum JudgeType { set; get; }

        /// <summary>
        /// 判断线数值
        /// </summary>
        public decimal JudgeNumber { set; get; }

        /// <summary>
        /// 匹配结果信息
        /// </summary>
        internal string MatchMessage { set; get; }
    }
}
