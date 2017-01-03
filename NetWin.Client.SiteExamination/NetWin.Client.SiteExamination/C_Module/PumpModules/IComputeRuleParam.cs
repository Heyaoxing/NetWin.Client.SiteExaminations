﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.A_Core.Enum;

namespace NetWin.Client.SiteExamination.C_Module.PumpModules
{
    public class ComputeRuleParam
    {
        public ComputeRuleParam()
        {
            Keywords=new Dictionary<string, int>();
        }
        /// <summary>
        /// 具体哪项检查项目的id
        /// </summary>
        internal int DetailId { set; get; }

        /// <summary>
        /// 体检评分
        /// </summary>
        internal int Score { set; get; }

        /// <summary>
        /// 匹配类型
        /// </summary>
        internal SpiderTypeEnum SpiderType { set; get; }

        private string _aimsContainText = string.Empty;

        /// <summary>
        /// 目标匹配文本
        /// </summary>
        internal string AimsContainText
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_aimsContainText))
                    return _aimsContainText.ToLower();
                return string.Empty;
            }
            set { _aimsContainText = value; }
        }

        /// <summary>
        /// 僚机匹配文本
        /// </summary>
        internal string WingManContainText { set; get; }

        /// <summary>
        /// 目标统计结果值
        /// </summary>
        internal decimal AimsCount { set; get; }

        /// <summary>
        /// 统计抓取结果内容
        /// </summary>
        internal string AimsContent { set; get; }

        /// <summary>
        /// 僚机统计结果值
        /// </summary>
        internal decimal WingManCount { set; get; }

        /// <summary>
        /// 匹配结果信息
        /// </summary>
        private string _matchMessage = string.Empty;

        /// <summary>
        /// 匹配结果信息
        /// </summary>
        internal string MatchMessage
        {
            set { _matchMessage = value; }
            get
            {
                if (!string.IsNullOrWhiteSpace(_matchMessage))
                    return _matchMessage.Replace("_COUNT_", AimsCount.ToString()).Replace("_CONTENT_", AimsContent);
                return string.Empty;
            }
        }

        /// <summary>
        /// 判断类型
        /// </summary>
        internal JudgeTypeEnum JudgeType { set; get; }

        /// <summary>
        /// 判断线数值
        /// </summary>
        internal decimal JudgeNumber { set; get; }

        /// <summary>
        /// 关键词统计
        /// </summary>
        internal Dictionary<string,int> Keywords { set; get; } 
    }
}
    