using System;
using System.Collections.Generic;

using System.Text;
using NetWin.Client.SiteExamination.B_Common.Interface;

namespace NetWin.Client.SiteExamination.D_Data.Entities
{
    public class SiteExaminationInfo:IEntity
    {

        public int Id { set; get; }
        /// <summary>
        /// 犀牛云Id
        /// </summary>
        public long UserId { set; get; }
       
        /// <summary>
        /// 目标网址
        /// </summary>
        public string SiteUrl { set; get; }
        /// <summary>
        /// 评语
        /// </summary>
        public string Reviews { set; get; }
        /// <summary>
        /// 体检评分
        /// </summary>
        public int Score { set; get; }
        /// <summary>
        /// 体检结束时间
        /// </summary>
        public DateTime CompletedOn { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { set; get; }
        /// <summary>
        /// 是否完成体检
        /// </summary>
        public bool IsCompleted { set; get; }
        /// <summary>
        /// 认证编号
        /// </summary>
        public string CertificateNum { set; get; }
    }
}
