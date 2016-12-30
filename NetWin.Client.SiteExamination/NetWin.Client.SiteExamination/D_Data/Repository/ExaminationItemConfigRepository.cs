using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
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
            List<ExaminationItemConfig> examinationItemConfigs = new List<ExaminationItemConfig>();
            try
            {
                examinationItemConfigs = SqLiteConnection.Query<ExaminationItemConfig>("select * from ExaminationItemConfig where IsEnable=1").ToList();
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception);
            }
            return examinationItemConfigs;
        }
    }
}
