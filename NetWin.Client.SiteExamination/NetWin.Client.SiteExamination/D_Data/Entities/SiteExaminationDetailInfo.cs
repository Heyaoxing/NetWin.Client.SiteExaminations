using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.B_Common.Interface;

namespace NetWin.Client.SiteExamination.D_Data.Entities
{
    public class SiteExaminationDetailInfo : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int DetailInfoId { set; get; }
        /// <summary>
        /// 体检项目详细配置表对应id
        /// </summary>
        public int DetailId { set; get; }

        /// <summary>
        /// 资源检查记录表对应id
        /// </summary>
        public int SiteId { set; get; }

        /// <summary>
        /// 是否通过检查
        /// </summary>
        public bool IsPass { set; get; }
        /// <summary>
        /// 结论
        /// </summary>
        public string Result { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { set; get; }
        /// <summary>
        /// 体检评分
        /// </summary>
        public int Score { set; get; }
    }
}
