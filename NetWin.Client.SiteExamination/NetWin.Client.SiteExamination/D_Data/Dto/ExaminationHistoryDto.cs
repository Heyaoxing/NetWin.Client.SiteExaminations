using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWin.Client.SiteExamination.D_Data.Dto
{
    public class ExaminationHistoryDto
    {
        public int SiteId { set; get; }
        public string SiteUrl { set; get; }
        public DateTime CompletedOn { set; get; }
        public string CompletedOnString
        {
            set { }
            get { return CompletedOn.ToString("yyyy年MM月dd日 HH:mm:ss"); }
        }
    }
}
