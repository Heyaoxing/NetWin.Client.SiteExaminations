using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using NetWin.Client.SiteExamination.B_Common;
using NetWin.Client.SiteExamination.D_Data.Base;
using NetWin.Client.SiteExamination.D_Data.Dto;
using NetWin.Client.SiteExamination.D_Data.Entities;

namespace NetWin.Client.SiteExamination.D_Data.Repository
{
    public class SiteExaminationInfoRepository : RepositoryBase
    {
        /// <summary>
        /// 插入资源检查记录,返回id值
        /// </summary>
        /// <param name="siteExaminationInfo">资源检查记录对象</param>
        /// <returns>id值</returns>
        public static int Add(SiteExaminationInfo siteExamination)
        {
            string sql = string.Format("insert into SiteExaminationInfo(UserId,SiteUrl,CreatedOn,IsCompleted,CertificateNum)values({0},'{1}','{2}',{3},'{4}');select last_insert_rowid();", siteExamination.UserId, siteExamination.SiteUrl, siteExamination.CreatedOn, siteExamination.IsCompleted ? 1 : 0, siteExamination.CertificateNum);
            var result = Shove.Database.SQLite.ExecuteScalar(SqLiteConnection, sql);
            int id = 0;
            if (result != null)
                id = Shove._Convert.StrToInt(result.ToString(), 0);
            return id;
        }

        /// <summary>
        /// 完成体检更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isCompleted"></param>
        public static void Complete(int id, bool isCompleted)
        {
            string sql = string.Format("update SiteExaminationInfo  set Score=(Score-(select Sum(b.Score) as Score from SiteExaminationDetailInfo as a join ExaminationItemDetailConfig as b on a.DetailId=b.DetailId where a.IsPass=0 and a.SiteId={2})),IsCompleted={0},CompletedOn='{1}' where SiteId={2};", isCompleted ? 1 : 0, DateTime.Now, id);
            Shove.Database.SQLite.ExecuteNonQuery(SqLiteConnection, sql);
        }

        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public static List<ExaminationHistoryDto> GetHistories(string siteUrl)
        {
            List<ExaminationHistoryDto> historyDtos = new List<ExaminationHistoryDto>();

            string sql = string.Empty;
            if (string.IsNullOrEmpty(siteUrl))
            {
                sql = "select SiteId,SiteUrl,CompletedOn from SiteExaminationInfo where IsCompleted=1  ORDER BY CompletedOn DESC LIMIT 10;";
            }
            else
            {
                sql = string.Format("select SiteId,SiteUrl,CompletedOn from SiteExaminationInfo where IsCompleted=1 and SiteUrl like '%{0}%' ORDER BY CompletedOn DESC LIMIT 10;", siteUrl.Trim());
            }

            DataTable dt = Shove.Database.SQLite.Select(SqLiteConnection, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                historyDtos = DataTableHelper.GetEntities<ExaminationHistoryDto>(dt);
            }
            return historyDtos;
        }

        /// <summary>
        /// 获取资源检查记录
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static SiteExaminationInfo Get(int siteId)
        {
            SiteExaminationInfo siteExamination = new SiteExaminationInfo();
            string sql = string.Format("select * from SiteExaminationInfo where siteId={0}", siteId);
            DataTable dt = Shove.Database.SQLite.Select(SqLiteConnection, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                siteExamination = DataTableHelper.GetEntity<SiteExaminationInfo>(dt);
            }
            return siteExamination;
        }

        /// <summary>
        /// 获取体检报告
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static List<ExaminationReportDto> GetExaminationReport(int siteId)
        {
            List<ExaminationReportDto> list = new List<ExaminationReportDto>();
            string sql = string.Format(@"SELECT
	                          b.SiteUrl,
                              b.Score as SiteScore,
                              b.CompletedOn,
                              b.CertificateNum,
                              a.IsPass,
                              a.Score as DetailScore,
                              a.Result,
                              a.Position,
                              c.Name as DetailName,
                              c.Require,
                              c.Suggest,
                              c.Department,
                              d.Name as ItemName,
                              d.ItemId
                            FROM
	                            SiteExaminationDetailInfo AS a
                            JOIN SiteExaminationInfo AS b ON a.SiteId = b.SiteId
                            JOIN ExaminationItemDetailConfig AS c ON c.DetailId = a.DetailId
                            join ExaminationItemConfig as d ON c.ItemId=d.ItemId
                            where b.IsCompleted=1 and b.SiteId={0}", siteId);
            DataTable dt = Shove.Database.SQLite.Select(SqLiteConnection, sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                list = DataTableHelper.GetEntities<ExaminationReportDto>(dt);
            }
            return list;
        }
    }
}
