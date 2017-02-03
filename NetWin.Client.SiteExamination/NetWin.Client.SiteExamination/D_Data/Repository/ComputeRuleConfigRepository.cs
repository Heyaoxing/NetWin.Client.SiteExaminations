using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NetWin.Client.SiteExamination.A_Core.Enum;
using NetWin.Client.SiteExamination.B_Common;
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
            List<PumpInitParam> list = new List<PumpInitParam>();
            string sql = "select a.DetailId,a.Score,b.ComputeType,b.AimsContainText,b.WingManContainText,b.JudgeType,b.JudgeNumber,b.MatchMessage,b.Moment  from ExaminationItemDetailConfig as a join ComputeRuleConfig as b on a.RuleId=b.RuleId join ExaminationItemConfig as c on c.ItemId=a.ItemId where a.IsEnable=1 and c.IsEnable=1 and b.SpiderType=1;";

            DataTable dt = Shove.Database.SQLite.Select(SqLiteConnection, sql);
            if (dt!=null&&dt.Rows.Count > 0)
            {
                list = DataTableHelper.GetEntities<PumpInitParam>(dt);
            }

            return list;
        }

        /// <summary>
        /// 获取内部数据处理泵的初始化数据
        /// </summary>
        /// <returns></returns>
        public static List<PumpInitParam> GetOutPumpInitParams()
        {
            List<PumpInitParam> list = new List<PumpInitParam>();
            string sql = "select a.DetailId,a.Score,b.ComputeType,b.AimsContainText,b.WingManContainText,b.JudgeType,b.JudgeNumber,b.MatchMessage,b.Moment  from ExaminationItemDetailConfig as a join ComputeRuleConfig as b on a.RuleId=b.RuleId join ExaminationItemConfig as c on c.ItemId=a.ItemId where a.IsEnable=1 and c.IsEnable=1 and b.SpiderType=2;";
            DataTable dt = Shove.Database.SQLite.Select(SqLiteConnection, sql);
            if (dt!=null&&dt.Rows.Count > 0)
            {
                list = DataTableHelper.GetEntities<PumpInitParam>(dt);
            }
            return list;
        }
    }
}
