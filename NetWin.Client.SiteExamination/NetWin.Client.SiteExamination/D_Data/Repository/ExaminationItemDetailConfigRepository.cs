using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using NetWin.Client.SiteExamination.B_Common;
using NetWin.Client.SiteExamination.D_Data.Base;
using NetWin.Client.SiteExamination.D_Data.Entities;

namespace NetWin.Client.SiteExamination.D_Data.Repository
{
    public class ExaminationItemDetailConfigRepository : RepositoryBase
    {
        /// <summary>
        /// 获取体检项目详细配置
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ExaminationItemDetailConfig> Get()
        {
            List<ExaminationItemDetailConfig> list = new List<ExaminationItemDetailConfig>();
            try
            {
                const string sql = "select * from ExaminationItemDetailConfig where IsEnable=1";
                DataTable dt = Shove.Database.SQLite.Select(SqLiteConnection, sql);
                if (dt!=null&&dt.Rows.Count > 0)
                {
                    list = DataTableHelper.GetEntities<ExaminationItemDetailConfig>(dt);
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
