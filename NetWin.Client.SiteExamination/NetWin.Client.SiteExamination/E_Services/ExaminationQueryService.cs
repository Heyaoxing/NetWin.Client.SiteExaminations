using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetWin.Client.SiteExamination.A_Core.Model;
using NetWin.Client.SiteExamination.B_Common;
using NetWin.Client.SiteExamination.D_Data.Dto;
using NetWin.Client.SiteExamination.D_Data.Entities;
using NetWin.Client.SiteExamination.D_Data.Repository;

namespace NetWin.Client.SiteExamination.E_Services
{
    /// <summary>
    /// 体检结果查询类
    /// </summary>
    public class ExaminationQueryService
    {
        /// <summary>
        /// 查询需要体检的体检项
        /// </summary>
        /// <returns></returns>
        public static List<ExaminationItemDto> GetExaminationItem()
        {
            List<ExaminationItemDto> examinationItemDto = new List<ExaminationItemDto>();
            var examinationItem = ExaminationItemConfigRepository.Get();
            var examinationDetail = ExaminationItemDetailConfigRepository.Get();

            foreach (var item in examinationItem)
            {
                ExaminationItemDto dto = new ExaminationItemDto();
                dto.Name = item.Name;
                dto.ItemId = item.ItemId;
                dto.ExaminationItemDetail = (from a in examinationDetail
                                             where a.ItemId == item.ItemId
                                             select new ExaminationItemDetailDto()
                                            {
                                                DetailId = a.DetailId,
                                                Name = a.Name,
                                                Score = a.Score,
                                                Require = a.Require,
                                                Suggest = a.Suggest
                                            }).ToList();
                examinationItemDto.Add(dto);
            }
            return examinationItemDto;
        }

        /// <summary>
        /// 获取资源检查记录
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static SiteExaminationInfo GetSiteExaminationInfo(int siteId)
        {
            return SiteExaminationInfoRepository.Get(siteId);
        }

        /// <summary>
        /// 查询体检结果
        /// </summary>
        /// <param name="detailId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static ReslutModel<ItemDetailResultDto> GetDetailResult(int siteId, int detailId)
        {
            ReslutModel<ItemDetailResultDto> reslutModel = new ReslutModel<ItemDetailResultDto>();
            reslutModel.Result = false;
            try
            {
                var detailResult = SiteExaminationDetailInfoRepository.GetDetailResult(siteId, detailId);
                reslutModel.Data = detailResult;
                if (detailResult != null)
                    reslutModel.Result = true;
            }
            catch (Exception exception)
            {
                LogHelper.Error("查询体检结果异常:" + exception.Message);
            }
            return reslutModel;
        }

        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public static List<ExaminationHistoryDto> GetHistories(string siteUrl)
        {
            try
            {
                return SiteExaminationInfoRepository.GetHistories(siteUrl);
            }
            catch
            {
                // ignored
            }
            return new List<ExaminationHistoryDto>();
        }


        /// <summary>
        /// 获取体检报告
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static void GetExaminationReport(int siteId, string filePath)
        {
            var report = SiteExaminationInfoRepository.GetExaminationReport(siteId);
            if (report == null || !report.Any())
                return;

            string content = CreateTable(report) + CreateOptimization(report);
            CreateFile(filePath, content);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        private static void CreateFile(string path, string content)
        {
            FileStream fs = null;
            try
            {
                FileInfo fileInfo = new FileInfo(path);

                fs = File.Open(path, FileMode.OpenOrCreate);
                byte[] btFile = Encoding.UTF8.GetBytes(content);
                //设定书写的開始位置为文件的末尾  
                fs.Position = fs.Length;
                //将待写入内容追加到文件末尾  
                fs.Write(btFile, 0, btFile.Length);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        /// <summary>
        /// 创建报告表格
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        private static string CreateTable(List<ExaminationReportDto> reportDto)
        {
            StringBuilder table = new StringBuilder();
            table.Append("<div style='margin:0 auto;text-align:center;height:100%'>");
            try
            {
                foreach (var item in reportDto.GroupBy(p => p.ItemId).Select(p => p.Key))
                {
                    table.AppendFormat("<p  style='font-size:20px;text-align:left;margin-top:10px;'><strong>{0}</strong></p>", reportDto.FirstOrDefault(p => p.ItemId == item).ItemName);
                    table.Append("<table cellspacing='0' cellpadding='0' style='border-left:1px solid #000;border-top:1px solid #000;'>");
                    table.Append("<tr style='text-align:center;' >");
                    table.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;border-top:1px solid #000;'>项目</td>");
                    table.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;border-top:1px solid #000;'>结果</td>");
                    table.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;border-top:1px solid #000;'>要求</td>");
                    table.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;border-top:1px solid #000;'>建议</td>");
                    table.Append("</tr>");
                    foreach (var itemDetail in reportDto.Where(p => p.ItemId == item))
                    {
                        table.Append("<tr>");
                        table.AppendFormat("<td  style='white-space:normal; display:block; width:80px;border-bottom:1px solid #000;border-right:1px solid #000;'>{0}</td>", itemDetail.DetailName);
                        if (string.IsNullOrWhiteSpace(itemDetail.Position))
                        {
                            table.AppendFormat("<td  style='white-space:normal;display:block; width:220px;border-bottom:1px solid #000;border-right:1px solid #000;'>{0}</td>", itemDetail.Result);
                        }
                        else
                        {
                            table.AppendFormat("<td  style='white-space:normal;display:block; width:220px;border-bottom:1px solid #000;border-right:1px solid #000;'>{0}</td>", "网址:" + TextHelper.InsertString(itemDetail.Position, "<br/>", 20) + "<br/>" + itemDetail.Result);
                        }
                        table.AppendFormat("<td  style='white-space:normal;display:block; width:150px;border-bottom:1px solid #000;border-right:1px solid #000;'>{0}</td>", itemDetail.Require);
                        table.AppendFormat("<td  style='white-space:normal;display:block; width:100px;text-align:center;border-bottom:1px solid #000;border-right:1px solid #000;'>{0}</td>", itemDetail.IsPass ? "" : "需优化");
                        table.Append("</tr>");
                    }
                    table.Append("</table>");
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error("创建报告表格异常:" + exception.Message);
            }
            table.Append("</div>");
            return table.ToString();
        }

        /// <summary>
        /// 创建优化方案
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        private static string CreateOptimization(List<ExaminationReportDto> reportDto)
        {
            Dictionary<int, string> tip = new Dictionary<int, string>();
            tip.Add(1, "一");
            tip.Add(2, "二");
            tip.Add(3, "三");
            tip.Add(4, "四");
            tip.Add(5, "五");
            tip.Add(6, "六");
            tip.Add(7, "七");
            tip.Add(8, "八");
            tip.Add(9, "九");
            tip.Add(10, "十");

            StringBuilder sb = new StringBuilder();
            sb.Append("<div style='margin:0 auto;text-align:center;height:100%;white-space:normal; display:block;width:490px;margin-top:20px;'>");
            sb.Append("<div  cellspacing='0' cellpadding='0' style='text-align:left;' >");
            try
            {
                sb.Append("<div style='font-size:30px; float:right;'><strong>网站体检优化方案</strong></div>");
                int bigTip = 1;
                foreach (var item in reportDto.GroupBy(p => p.ItemId).Select(p => p.Key))
                {
                    sb.AppendFormat("<div  style='font-size:25px'><strong>{0}、{1}评分</strong></div>", tip[bigTip], reportDto.FirstOrDefault(p => p.ItemId == item).ItemName);
                    bigTip++;
                    int smallTip = 1;
                    foreach (var itemDetail in reportDto.Where(p => p.ItemId == item))
                    {
                        sb.AppendFormat("<p  style='font-size:20px'><strong>{0}、{1}</strong></p>", smallTip, itemDetail.DetailName);
                        if (!string.IsNullOrWhiteSpace(itemDetail.Department))
                        {
                            sb.AppendFormat("<p><strong style='color:red;'>&nbsp;&nbsp;小科普：</strong><strong>{0}</strong></p>", itemDetail.Department);
                        }
                        sb.AppendFormat("<p><strong>&nbsp;&nbsp;我们建议：</strong>{0}</p>", itemDetail.Suggest);
                        smallTip++;
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error("创建优化方案异常:" + exception.Message);
            }
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }
    }
}
