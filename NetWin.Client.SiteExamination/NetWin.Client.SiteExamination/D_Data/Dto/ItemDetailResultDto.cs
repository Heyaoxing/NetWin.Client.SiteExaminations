using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWin.Client.SiteExamination.D_Data.Dto
{
    public  class ItemDetailResultDto
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
        /// 结论
        /// </summary>
        public string Result { set; get; }
        /// <summary>
        /// 要求
        /// </summary>
        public string Require { set; get; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsPass { set; get; }
    }
}
