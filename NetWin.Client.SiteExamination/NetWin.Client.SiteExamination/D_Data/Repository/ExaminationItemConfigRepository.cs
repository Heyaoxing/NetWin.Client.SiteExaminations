using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using NetWin.Client.SiteExamination.B_Common;
using NetWin.Client.SiteExamination.D_Data.Base;
using NetWin.Client.SiteExamination.D_Data.Entities;

namespace NetWin.Client.SiteExamination.D_Data.Repository
{
    public class ExaminationItemConfigRepository : RepositoryBase
    {
        /// <summary>
        /// 获取需要体检的项目
        /// </summary>
        public static IEnumerable<ExaminationItemConfig> Get()
        {
            List<ExaminationItemConfig> list = new List<ExaminationItemConfig>();
            try
            {
                const string sql = "select * from ExaminationItemConfig where IsEnable=1";
                DataTable dt = Shove.Database.SQLite.Select(SqLiteConnection, sql);
                if (dt!=null&&dt.Rows.Count > 0)
                {
                    list = DataTableHelper.GetEntities<ExaminationItemConfig>(dt);
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception);
            }
            return list;
        }
    }
}
