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
    public class ExaminationItemDetailConfigRepository : RepositoryBase
    {
        /// <summary>
        /// 获取体检项目详细配置
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ExaminationItemDetailConfig> Get()
        {
            List<ExaminationItemDetailConfig> examinationItemDetailConfigs = new List<ExaminationItemDetailConfig>();
            try
            {
                examinationItemDetailConfigs = SqLiteConnection.Query<ExaminationItemDetailConfig>("select * from ExaminationItemDetailConfig where IsEnable=1").ToList();
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception);
            }
            return examinationItemDetailConfigs;
        }
    }
}
