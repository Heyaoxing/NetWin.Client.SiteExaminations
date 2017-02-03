using System;
using System.Collections.Generic;

using System.Text;

namespace NetWin.Client.SiteExamination.D_Data.Dto
{
    /// <summary>
    /// 体检详细项目传输实体
    /// </summary>
    public class ExaminationItemDetailDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int DetailId { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 分值
        /// </summary>
        public int Score { set; get; }
        /// <summary>
        /// 要求
        /// </summary>
        public string Require { set; get; }
        /// <summary>
        /// 优化建议
        /// </summary>
        public string Suggest { set; get; }
    }
}
