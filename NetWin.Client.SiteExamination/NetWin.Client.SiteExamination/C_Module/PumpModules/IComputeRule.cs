using System;
using System.Collections.Generic;

using System.Text;
using NetWin.Client.SiteExamination.A_Core.Enum;
using NetWin.Client.SiteExamination.A_Core.Model;

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
        internal ReslutModel<bool> ComputeMethod(InSite site)
        {
            ReslutModel<bool> reslutModel = new ReslutModel<bool>();
            reslutModel.Result = false;
            if (site != null)
            {
                SourceUrl = site.CurrentUrlUrl;
                ComputeCount(site);
            }
            bool result = true;
            try
            {
                switch (JudgeType)
                {
                    case (int)JudgeTypeEnum.LessThanOrEqualByAims:
                        result = JudgeNumber <= AimsCount;
                        break;
                    case (int)JudgeTypeEnum.Greater:
                        result = JudgeNumber > AimsCount;
                        break;
                    case (int)JudgeTypeEnum.LessThanEqualByScale:
                        if (WingManCount == 0)
                        {
                            result = JudgeNumber <= 0;
                        }
                        else
                        {
                            result = JudgeNumber <= (AimsCount / WingManCount);
                        }
                        break;
                    case (int)JudgeTypeEnum.GreaterOrByScale:
                        if (WingManCount == 0)
                        {
                            result = JudgeNumber >= 0;
                        }
                        else
                        {
                            result = JudgeNumber >= (AimsCount / WingManCount);
                        }
                        break;
                }
                reslutModel.Result = true;
            }
            catch
            {
                reslutModel.Result = false;
            }
            reslutModel.Data = result;
            return reslutModel;
        }
        /// <summary>
        /// 目标数值统计方法
        /// </summary>
        /// <param name="site"></param>
        internal abstract void ComputeCount(InSite site);
    }
}
