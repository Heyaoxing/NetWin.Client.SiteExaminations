using System;
using System.Collections.Generic;

using System.Text;
using NetWin.Client.SiteExamination.B_Common.Interface;

namespace NetWin.Client.SiteExamination.D_Data.Entities
{
    public class ExaminationItemConfig : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ItemId { set; get; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { set; get; }
    }
}
