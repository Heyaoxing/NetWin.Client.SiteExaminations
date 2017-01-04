using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWin.Client.SiteExamination.D_Data.Dto
{
    public class ExaminationReportDto
    {
        public string SiteUrl { set; get; }
        public string SiteScore { set; get; }
        public string CompletedOn { set; get; }
        public string CertificateNum { set; get; }
        public bool IsPass { set; get; }
        public string DetailScore { set; get; }
        public string Result { set; get; }
        public string DetailName { set; get; }
        public string Require { set; get; }
        public string Suggest { set; get; }
        public string ItemName { set; get; }
        public string ItemId { set; get; }
        public string Department { set; get; }
    }
}
