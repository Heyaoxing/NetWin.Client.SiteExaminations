﻿using System;
using System.Collections.Generic;

using System.Text;
using NetWin.Client.SiteExamination.A_Core.Enum;
using NetWin.Client.SiteExamination.B_Common;

namespace NetWin.Client.SiteExamination.C_Module.PumpModules
{
    /// <summary>
    /// 内部数据处理泵组件
    /// </summary>
    public class InPumpModule
    {
        /// <summary>
        /// 全局计算类
        /// </summary>
        private List<IComputeRule> globalModule = new List<IComputeRule>();

        /// <summary>
        /// 过程计算类
        /// </summary>
        private List<IComputeRule> sectionModule = new List<IComputeRule>();


        /// <summary>
        /// 初始化装载组件
        /// </summary>
        public InPumpModule(List<PumpInitParam> param, List<string> keyWords)
        {
            if (param == null)
            {
                LogHelper.Error("初始化装载内部数据处理泵组件组件失败,组件参数为空");
                return;
            }

            foreach (var item in param)
            {
                if (item.ComputeType == (int)ComputeTypeEnum.Global)
                {
                    IComputeRule global = new GlobalModule();
                    global = LoadComputeRule(global, item, keyWords);
                    globalModule.Add(global);
                }
                else if (item.ComputeType == (int)ComputeTypeEnum.Section)
                {
                    IComputeRule section = new SectionModule();
                    section = LoadComputeRule(section, item, keyWords);
                    sectionModule.Add(section);
                }
            }
        }

        /// <summary>
        /// 装载计算组件参数
        /// </summary>
        /// <param name="computeRule"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private IComputeRule LoadComputeRule(IComputeRule computeRule, PumpInitParam param, List<string> keyWords)
        {
            computeRule.DetailId = param.DetailId;
            computeRule.SpiderType = (SpiderTypeEnum)param.SpiderType;
            computeRule.AimsContainText = param.AimsContainText;
            computeRule.Score = param.Score;
            computeRule.WingManContainText = param.WingManContainText;
            computeRule.JudgeType = param.JudgeType;
            computeRule.JudgeNumber = param.JudgeNumber;
            computeRule.MatchMessage = param.MatchMessage;
            computeRule.Moment = param.Moment;
            if (keyWords != null)
                foreach (var keyword in EnumerableHelper.Distinct(keyWords))
                {
                    computeRule.Keywords.Add(keyword, 0);
                }

            return computeRule;
        }

        /// <summary>
        /// 数据处理入口
        /// </summary>
        /// <param name="site">资源对象</param>
        /// <param name="isEnd">是否是最后一个处理的资源</param>
        /// <returns></returns>
        public List<ExaminationResult> Process(InSite site, bool isEnd)
        {
            List<ExaminationResult> examinationResults = new List<ExaminationResult>();

            if (!isEnd && site == null)
            {
                return examinationResults;
            }

            #region 执行过程计算组件
            List<IComputeRule> sectionRemove = new List<IComputeRule>();
            //符合计算出结果的体检项目
            foreach (var section in sectionModule)
            {
                var resultModel = section.ComputeMethod(site);
                if (resultModel.Result)
                    if (isEnd || (section.Moment == (int)MomentType.Error && !resultModel.Data) || (section.Moment == (int)MomentType.Normal && resultModel.Data))
                    {
                        ExaminationResult examination = new ExaminationResult();
                        examination.DetailId = section.DetailId;
                        examination.IsPass = resultModel.Data;
                        examination.Score = section.Score;
                        examination.Position = section.SourceUrl;
                        examination.Result = section.MatchMessage;
                        examination.ExaminationDateTime = DateTime.Now;
                        examinationResults.Add(examination);
                        sectionRemove.Add(section);
                    }
            }
            //移除掉已经出结果的体检项目
            sectionModule.RemoveAll(p => sectionRemove.Contains(p));
            #endregion

            #region 执行全局计算组件
            List<IComputeRule> globalRemove = new List<IComputeRule>();
            //符合计算出结果的体检项目
            foreach (var global in globalModule)
            {
                var resultModel = global.ComputeMethod(site);
                if (isEnd)
                {
                    ExaminationResult examination = new ExaminationResult();
                    examination.DetailId = global.DetailId;
                    examination.IsPass = resultModel.Data;
                    examination.Score = global.Score;
                    examination.Result = global.MatchMessage;
                    examination.ExaminationDateTime = DateTime.Now;
                    examinationResults.Add(examination);
                    globalRemove.Add(global);
                }
            }
            //移除掉已经出结果的体检项目
            globalRemove.RemoveAll(p => globalRemove.Contains(p));
            #endregion

            return examinationResults;
        }
    }
}
