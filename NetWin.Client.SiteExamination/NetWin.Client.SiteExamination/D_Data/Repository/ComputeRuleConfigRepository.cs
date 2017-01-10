using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using NetWin.Client.SiteExamination.C_Module.PumpModules;
using NetWin.Client.SiteExamination.D_Data.Base;

namespace NetWin.Client.SiteExamination.D_Data.Repository
{
    public class ComputeRuleConfigRepository : RepositoryBase
    {
        /// <summary>
        /// 获取内部数据处理泵的初始化数据
        /// </summary>
        /// <returns></returns>
        public static List<PumpInitParam> GetInPumpInitParams()
        {
            string sql = "select a.DetailId,a.Score,b.ComputeType,b.AimsContainText,b.WingManContainText,b.JudgeType,b.JudgeNumber,b.MatchMessage,b.Moment  from ExaminationItemDetailConfig as a join ComputeRuleConfig as b on a.RuleId=b.RuleId join ExaminationItemConfig as c on c.ItemId=a.ItemId where a.IsEnable=1 and c.IsEnable=1 and b.SpiderType=1;";
            return SqLiteConnection.Query<PumpInitParam>(sql).ToList();
        }

        /// <summary>
        /// 获取内部数据处理泵的初始化数据
        /// </summary>
        /// <returns></returns>
        public static List<PumpInitParam> GetOutPumpInitParams()
        {
            string sql = "select a.DetailId,a.Score,b.ComputeType,b.AimsContainText,b.WingManContainText,b.JudgeType,b.JudgeNumber,b.MatchMessage,b.Moment  from ExaminationItemDetailConfig as a join ComputeRuleConfig as b on a.RuleId=b.RuleId join ExaminationItemConfig as c on c.ItemId=a.ItemId where a.IsEnable=1 and c.IsEnable=1 and b.SpiderType=2;";
            return SqLiteConnection.Query<PumpInitParam>(sql).ToList();
        }
    }
}
