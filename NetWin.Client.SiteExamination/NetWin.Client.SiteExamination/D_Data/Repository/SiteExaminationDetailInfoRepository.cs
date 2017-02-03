using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NetWin.Client.SiteExamination.B_Common;
using NetWin.Client.SiteExamination.D_Data.Base;
using NetWin.Client.SiteExamination.D_Data.Dto;
using NetWin.Client.SiteExamination.D_Data.Entities;
using Shove.Database;

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
            string sql = string.Format("insert into SiteExaminationDetailInfo(DetailId,SiteId,IsPass,Result,Score,CreatedOn,Position)values({0},{1},{2},'{3}',{4},'{5}','{6}');select last_insert_rowid();", siteExaminationDetail.DetailId, siteExaminationDetail.SiteId,siteExaminationDetail.IsPass?1:0, siteExaminationDetail.Result, siteExaminationDetail.Score, siteExaminationDetail.CreatedOn, siteExaminationDetail.Position);
            var result = SQLite.ExecuteScalar(SqLiteConnection,sql);
            int id = 0;
            if (result != null)
                id = Shove._Convert.StrToInt(result.ToString(), 0);
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
            string sql = string.Format("select a.IsPass,a.Score,a.Position,a.Result,b.Name,b.Require,b.ItemId from SiteExaminationDetailInfo as a join ExaminationItemDetailConfig as b on a.DetailId=b.DetailId where a.SiteId={0} and a.DetailId={1} ", siteId, detailId);
            DataTable dt = SQLite.Select(SqLiteConnection, sql);
            if (dt!=null&&dt.Rows.Count > 0)
            {
            return DataTableHelper.GetEntity<ItemDetailResultDto>(dt);
            }
            
                return null;
        }
    }
}
