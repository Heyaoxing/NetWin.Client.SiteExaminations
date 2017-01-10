using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.A_Core.Enum;
using NetWin.Client.SiteExamination.B_Common;

namespace NetWin.Client.SiteExamination.C_Module.PumpModules
{
    /// <summary>
    /// 外部数据处理泵组件
    /// </summary>
    public class OutPumpModule
    {
        /// <summary>
        /// 外部计算类
        /// </summary>
        private List<ComputeRuleParam> outModule = new List<ComputeRuleParam>();

        /// <summary>
        /// 初始化计算数据
        /// </summary>
        /// <param name="param"></param>
        public OutPumpModule(List<PumpInitParam> param)
        {
            if (param == null)
            {
                LogHelper.Error("初始化外部数据处理泵组件装载组件失败,组件参数为空");
                return;
            }

            foreach (var item in param)
            {
                ComputeRuleParam global = new ComputeRuleParam();
                global = LoadComputeRule(global, item);
                outModule.Add(global);
            }

        }
        /// <summary>
        /// 装载计算组件参数
        /// </summary>
        /// <param name="computeRule"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private ComputeRuleParam LoadComputeRule(ComputeRuleParam computeRule, PumpInitParam param)
        {
            computeRule.DetailId = param.DetailId;
            computeRule.SpiderType = param.SpiderType;
            computeRule.Score = param.Score;
            computeRule.AimsContainText = param.AimsContainText;
            computeRule.WingManContainText = param.WingManContainText;
            computeRule.JudgeType = param.JudgeType;
            computeRule.JudgeNumber = param.JudgeNumber;
            computeRule.MatchMessage = param.MatchMessage;
            return computeRule;
        }

        /// <summary>
        /// 数据处理入口
        /// </summary>
        /// <param name="site">资源对象</param>
        /// <returns></returns>
        public List<ExaminationResult> Process(OutSite site)
        {
            List<ExaminationResult> examination = new List<ExaminationResult>();
            try
            {
                foreach (var item in outModule)
                {
                    examination.Add(ComputeMethod(site, item));
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error("外部数据处理泵组件数据处理入口异常:" + exception);
            }
            return examination;
        }

        /// <summary>
        /// 合法后缀名
        /// </summary>
        private string[] suffixs = new[] { ".com", ".net", "com.cn", ".org", ".gov.cn", ".edu", ".net.cn", ".cn" };

        private string[] areas = new[] {"北京","天津","上海","重庆","河北","山西","辽宁","吉林","黑龙江","江苏","浙江","安徽","福建","江西","山东","河南","湖北",
                                        "湖南","广东","甘肃","四川","山东","贵州","海南","云南","青海","陕西","广西","西藏","宁夏","新疆","内蒙"};
        private ComputeRuleParam ComputeCount(OutSite site, ComputeRuleParam param)
        {
            if (site == null)
                return param;
            param.SourceUrl = site.Url;
            switch (param.AimsContainText)
            {
                case "domainage":
                    param.AimsCount = site.DomainAge;
                    break;
                case "record":
                    param.AimsCount = 0;
                    param.AimsContent = "域名未备案";

                    if (site.IsRecord)
                    {
                        param.AimsCount = 1;
                        param.AimsContent = "域名已备案";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(site.DomainAddress))
                        {
                            if (areas.AsEnumerable().Count(p => site.DomainAddress.Contains(p)) > 0)
                            {
                                param.AimsCount = 1;
                                param.AimsContent = "域名已备案";
                            }
                        }
                    }

                    break;
                case "responsetime":
                    param.AimsCount = site.ResponseTime;
                    break;
                case "compress":
                    param.AimsCount = site.Compress;
                    break;
                case "expiredate":
                    param.AimsCount = site.ExpireDate;
                    break;
                case "domainsuffix":
                    param.AimsCount = 0;
                    var strings = site.Url.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (strings.Length > 1)
                    {
                        param.AimsContent = strings[strings.Length - 1];
                    }

                    foreach (var item in suffixs)
                    {
                        var endsWith = site.Url.EndsWith(item);
                        if (endsWith)
                        {
                            param.AimsCount = 1;
                            param.AimsContent = item;
                        }
                    }
                    break;
                case "statuscode":
                    param.AimsCount = site.StatusCode;
                    break;
                case "domainaddress":
                    param.AimsContent = site.DomainAddress;
                    if (!string.IsNullOrWhiteSpace(site.DomainAddress))
                    {
                        param.AimsCount = areas.AsEnumerable().Count(p => site.DomainAddress.Contains(p));
                    }
                    break;
                case "robots":
                    if (site.ExistRobots)
                    {
                        param.AimsCount = 1;
                        param.AimsContent = "存在";
                    }
                    else
                    {
                        param.AimsCount = 0;
                        param.AimsContent = "不存在";
                    }
                    break;
                case "sitemap":
                    if (site.ExistSitemap)
                    {
                        param.AimsCount = 1;
                        param.AimsContent = "存在";
                    }
                    else
                    {
                        param.AimsCount = 0;
                        param.AimsContent = "不存在";
                    }
                    break;
                case "preferred":

                    if (site.IsPreferredDomain)
                    {
                        param.AimsCount = 1;
                        param.AimsContent = "存在首选域";
                    }
                    else
                    {
                        param.AimsCount = 0;
                        param.AimsContent = "不存在首选域";
                    }
                    break;
                case "logo":
                    if (site.IsLogoContainsKeyWord)
                    {
                        param.AimsCount = 1;
                        param.AimsContent = "图片logo中的alt或title属性包含关键字:" + site.LogoAltAndTitle;
                    }
                    else
                    {
                        param.AimsCount = 0;
                        param.AimsContent = "图片logo中的alt或title属性没有包含关键字";
                    }
                    break;
            }
            return param;
        }

        /// <summary>
        /// 计算方法
        /// </summary>
        private ExaminationResult ComputeMethod(OutSite site, ComputeRuleParam param)
        {
            bool result = false;
            param = ComputeCount(site, param);
            switch (param.JudgeType)
            {
                case JudgeTypeEnum.LessThanOrEqualByAims:
                    result = param.JudgeNumber <= param.AimsCount;
                    break;
                case JudgeTypeEnum.Greater:
                    result = param.JudgeNumber > param.AimsCount;
                    break;
                case JudgeTypeEnum.LessThanEqualByScale:
                    result = param.JudgeNumber <= (param.AimsCount / param.WingManCount);
                    break;
            }

            ExaminationResult examinationResult = new ExaminationResult();
            examinationResult.DetailId = param.DetailId;
            examinationResult.IsPass = result;
            examinationResult.Score = param.Score;
            examinationResult.Result = param.MatchMessage;
            examinationResult.ExaminationDateTime = DateTime.Now;
            examinationResult.Position = param.SourceUrl;
            return examinationResult;
        }
    }
}
