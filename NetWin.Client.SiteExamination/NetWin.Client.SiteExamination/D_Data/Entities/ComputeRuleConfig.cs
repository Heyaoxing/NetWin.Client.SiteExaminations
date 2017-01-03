using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.B_Common.Interface;

namespace NetWin.Client.SiteExamination.D_Data.Entities
{
    public class ComputeRuleConfig : IEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int RuleId { set; get; }
        /// <summary>
        /// 计算类型
        /// </summary>
        public int ComputeType { set; get; }
        /// <summary>
        /// 抓取类型
        /// </summary>
        public int SpiderType { set; get; }
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
        public int JudgeType { set; get; }
        /// <summary>
        /// 判断值
        /// </summary>
        public decimal JudgeNumber { set; get; }
    }
}
