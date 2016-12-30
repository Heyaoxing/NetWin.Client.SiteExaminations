using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.B_Common.Interface;

namespace NetWin.Client.SiteExamination.D_Data.Entities
{
    public class ExaminationItemDetailConfig : IEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int DetailId { set; get; }
        /// <summary>
        /// 对应体检项目配置表Id
        /// </summary>
        public int ItemId { set; get; }
        /// <summary>
        /// 对应计算规则配置表Id
        /// </summary>
        public int RuleId { set; get; }
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
        /// 建议
        /// </summary>
        public string Suggest { set; get; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { set; get; }
    }
}
