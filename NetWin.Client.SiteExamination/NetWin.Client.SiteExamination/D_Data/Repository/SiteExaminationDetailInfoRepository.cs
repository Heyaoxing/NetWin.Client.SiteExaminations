using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using NetWin.Client.SiteExamination.D_Data.Base;
using NetWin.Client.SiteExamination.D_Data.Dto;
using NetWin.Client.SiteExamination.D_Data.Entities;

namespace NetWin.Client.SiteExamination.D_Data.Repository
{
    public class SiteExaminationDetailInfoRepository : RepositoryBase
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="siteExaminationDetail"></param>
        /// <returns></returns>
        public static long Add(SiteExaminationDetailInfo siteExaminationDetail)
        {
            string sql = "insert into SiteExaminationDetailInfo(DetailId,SiteId,IsPass,Result,Score,CreatedOn,Position)values(@DetailId,@SiteId,@IsPass,@Result,@Score,@CreatedOn,@Position);select last_insert_rowid();";
            var id = SqLiteConnection.ExecuteScalar<long>(sql, siteExaminationDetail);
            return id;
        }

        /// <summary>
        /// 查询体检结果
        /// </summary>
        /// <param name="detailId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static ItemDetailResultDto GetDetailResult(int siteId, int detailId)
        {
            string sql = "select a.IsPass,a.Score,a.Position,a.Result,b.Name,b.Require,b.ItemId from SiteExaminationDetailInfo as a join ExaminationItemDetailConfig as b on a.DetailId=b.DetailId where a.SiteId=@SiteId and a.DetailId=@DetailId ";
            return SqLiteConnection.Query<ItemDetailResultDto>(sql, new { DetailId = detailId, SiteId = siteId }).FirstOrDefault();
        }
    }
}
