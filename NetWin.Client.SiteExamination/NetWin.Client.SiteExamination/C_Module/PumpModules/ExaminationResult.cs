using System;
using System.Collections.Generic;

using System.Text;

namespace NetWin.Client.SiteExamination.C_Module.PumpModules
{
    /// <summary>
    /// 检查结果返回实体
    /// </summary>
    public class ExaminationResult
    {
        /// <summary>
        /// 具体哪项检查项目的id
        /// </summary>
        public int DetailId { set; get; }

        /// <summary>
        /// 是否通过检查
        /// </summary>
        public bool IsPass { set; get; }

        /// <summary>
        /// 结论
        /// </summary>
        public string Result { set; get; }

        /// <summary>
        /// 本次体检时间
        /// </summary>
        public DateTime ExaminationDateTime { set; get; }

        /// <summary>
        /// 体检评分
        /// </summary>
        public int Score { set; get; }

        /// <summary>
        /// 结果所在的url
        /// </summary>
        public string Position { set; get; }
    }
}
