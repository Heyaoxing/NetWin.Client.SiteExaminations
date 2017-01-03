using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.A_Core.Enum;

namespace NetWin.Client.SiteExamination.C_Module.PumpModules
{
    public abstract class IComputeRule : ComputeRuleParam
    {
        internal IComputeRule()
        {
            AimsCount = 0;
            WingManCount = 0;
        }
        /// <summary>
        /// 计算方法
        /// </summary>
        internal bool ComputeMethod(InSite site)
        {
            bool result = false;
            ComputeCount(site);
            switch (JudgeType)
            {
                case JudgeTypeEnum.GreaterOrEqualByAims:
                    result = JudgeNumber <= AimsCount;
                    break;
                case JudgeTypeEnum.LessThan:
                    result = JudgeNumber > AimsCount;
                    break;
                case JudgeTypeEnum.GreaterOrEqualByScale:
                    if (WingManCount == 0)
                    {
                        result = JudgeNumber <= 0;
                    }
                    else
                    {
                        result = JudgeNumber <= (AimsCount / WingManCount);
                    }
                    break;
                case JudgeTypeEnum.LessThanByScale:
                    if (WingManCount == 0)
                    {
                        result = JudgeNumber > 0;
                    }
                    else
                    {
                        result = JudgeNumber > (AimsCount / WingManCount);
                    }
                    break;
            }
            return result;
        }
        /// <summary>
        /// 目标数值统计方法
        /// </summary>
        /// <param name="site"></param>
        internal abstract void ComputeCount(InSite site);
    }
}
