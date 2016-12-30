using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWin.Client.SiteExamination.D_Data.Dto
{
    /// <summary>
    /// 体检项 数据传输实体
    /// </summary>
    public class ExaminationItemDto
    {
        public ExaminationItemDto()
        {
            ExaminationItemDetail=new List<ExaminationItemDetailDto>();
        }
        /// <summary>
        /// 体检大项名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 主键
        /// </summary>
        public int ItemId { set; get; }

        public List<ExaminationItemDetailDto> ExaminationItemDetail { set; get; }
    }
}
