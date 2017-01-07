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
    public class SiteExaminationInfoRepository : RepositoryBase
    {
        /// <summary>
        /// 插入资源检查记录,返回id值
        /// </summary>
        /// <param name="siteExaminationInfo">资源检查记录对象</param>
        /// <returns>id值</returns>
        public static int Add(SiteExaminationInfo siteExamination)
        {
            string sql = "insert into SiteExaminationInfo(UserId,SiteUrl,CreatedOn,IsCompleted,CertificateNum)values(@UserId,@SiteUrl,@CreatedOn,@IsCompleted,@CertificateNum);select last_insert_rowid();";
            var id = SqLiteConnection.ExecuteScalar<int>(sql, siteExamination);
            return id;
        }

        /// <summary>
        /// 完成体检更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isCompleted"></param>
        public static void Complete(int id, bool isCompleted)
        {
            string sql = "update SiteExaminationInfo  set Score=(Score-(select Sum(b.Score) as Score from SiteExaminationDetailInfo as a join ExaminationItemDetailConfig as b on a.DetailId=b.DetailId where a.IsPass=0 and a.SiteId=@SiteId)),IsCompleted=@IsCompleted,CompletedOn=@CompletedOn where SiteId=@SiteId;";
            SqLiteConnection.Execute(sql, new { IsCompleted = isCompleted, CompletedOn = DateTime.Now, SiteId = id });
        }

        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public static List<ExaminationHistoryDto> GetHistories(string siteUrl)
        {
            List<ExaminationHistoryDto> historyDtos = new List<ExaminationHistoryDto>();
            if (string.IsNullOrWhiteSpace(siteUrl))
            {
                historyDtos = SqLiteConnection.Query<ExaminationHistoryDto>("select SiteId,SiteUrl,CompletedOn from SiteExaminationInfo where IsCompleted=1  ORDER BY CompletedOn DESC LIMIT 10;")
                       .ToList();
            }
            else
            {
                historyDtos = SqLiteConnection.Query<ExaminationHistoryDto>("select SiteId,SiteUrl,CompletedOn from SiteExaminationInfo where IsCompleted=1 and SiteUrl like @SiteUrl ORDER BY CompletedOn DESC LIMIT 10;", new { SiteUrl = '%'+siteUrl+'%' })
                        .ToList();
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
            return SqLiteConnection.Query<SiteExaminationInfo>("select * from SiteExaminationInfo where siteId=@SiteId",
                    new { SiteId = siteId }).FirstOrDefault();
        }

        /// <summary>
        /// 获取体检报告
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static List<ExaminationReportDto> GetExaminationReport(int siteId)
        {
            string sql = @"SELECT
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
                            where b.IsCompleted=1 and b.SiteId=@SiteId";
            return SqLiteConnection.Query<ExaminationReportDto>(sql, new { SiteId = siteId }).ToList();
        }
    }
}
