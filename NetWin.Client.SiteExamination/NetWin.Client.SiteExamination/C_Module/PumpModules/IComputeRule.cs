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
            if (site == null)
                return true;

            bool result = true;

            SourceUrl = site.CurrentUrlUrl;
            try
            {
                ComputeCount(site);
                switch (JudgeType)
                {
                    case JudgeTypeEnum.LessThanOrEqualByAims:
                        result = JudgeNumber <= AimsCount;
                        break;
                    case JudgeTypeEnum.Greater:
                        result = JudgeNumber > AimsCount;
                        break;
                    case JudgeTypeEnum.LessThanEqualByScale:
                        if (WingManCount == 0)
                        {
                            result = JudgeNumber <= 0;
                        }
                        else
                        {
                            result = JudgeNumber <= (AimsCount / WingManCount);
                        }
                        break;
                    case JudgeTypeEnum.GreaterOrByScale:
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
            }
            catch
            {
                // ignored
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
