using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.B_Common;

namespace NetWin.Client.SiteExamination.C_Module
{
    /// <summary>
    /// 内部资源对象
    /// </summary>
    public class InSite
    {
        public InSite(string siteUrl, string currentUrl, string innerHtml, DateTime lastModified,long size, int level)
        {
            CurrentUrlUrl = currentUrl;
            InnerHtml = RegexHelper.FilterNotes(innerHtml);
            Level = level;
            SiteUrl = siteUrl;
            LastModified = lastModified;
            HtmlSize = size;
        }

        /// <summary>
        /// 当前网址
        /// </summary>
        public readonly string CurrentUrlUrl;

        /// <summary>
        /// 网页大小
        /// </summary>
        public readonly long HtmlSize;
        /// <summary>
        /// 种子地址
        /// </summary>
        public readonly string SiteUrl;

        /// <summary>
        /// 网页内容
        /// </summary>
        public readonly string InnerHtml;

        /// <summary>
        /// 最新缓存时间
        /// </summary>
        public readonly DateTime LastModified;

        /// <summary>
        /// 网页文本
        /// </summary>
        public string InnerText
        {
            get
            {
                return RegexHelper.ReplaceHtmlTag(InnerHtml);
            }
        }

        /// <summary>
        /// 当前站内第几层,起点从0层开始
        /// </summary>
        public readonly int Level;

        /// <summary>
        /// 内链数
        /// </summary>
        public int InsideLinkCount
        {
            get
            {
                var count = InsideLinks.Count;
                return count;
            }
        }

        public List<string> InsideLinks
        {
            get
            {
                return HttpLinks.Where(p => p.Contains(DomainName)).ToList();
            }
        }

        /// <summary>
        /// 外链数
        /// </summary>
        public int OutsideLinkCount
        {
            get
            {
                var count = HttpLinks.Count(p => !p.Contains(DomainName));
                return count;
            }
        }

        /// <summary>
        /// 空链接数
        /// </summary>
        public int EmptyLinkCount
        {
            get
            {
                return Links.Count(p =>string.IsNullOrWhiteSpace(p)||
                    p=="#" || p.Contains("javascript:") || p.Contains("javascript:void()") ||
                    p.Contains("javascript:void(0)"));
            }
        }

        /// <summary>
        /// 相同锚链接数,(链接和名称均相同)
        /// </summary>
        public int SameAnchorLinksCount { set; get; }

        /// <summary>
        /// 网站关键词
        /// </summary>
        public List<string> KeyWords
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return new List<string>();

                return RegexHelper.GetKeyWords(InnerHtml).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
            }
        }

        /// <summary>
        /// 网站描述
        /// </summary>
        public string Description
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return string.Empty;

                return RegexHelper.GetDescription(InnerHtml);
            }
        }

        /// <summary>
        /// 是否是垃圾页面
        /// </summary>
        public bool IsInvalid { set; get; }

        /// <summary>
        /// 图片数
        /// </summary>
        public int ImagesCount
        {
            get
            {
                return RegexHelper.GetImageLinks(InnerHtml).Count;
            }
        }

        /// <summary>
        /// 脚本文件数
        /// </summary>
        public int JsCount
        {
            get
            {
                return RegexHelper.GetJsLinks(InnerHtml).Count;
            }
        }

        /// <summary>
        /// 样式文件数
        /// </summary>
        public int StyleCount
        {
            get
            {
                return RegexHelper.GetStyleLinks(InnerHtml).Count;

            }
        }

        /// <summary>
        /// 种子地址域名
        /// </summary>
        public string DomainName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SiteUrl))
                    return string.Empty;
                return RegexHelper.GetDomainName(SiteUrl);
            }
        }

        /// <summary>
        /// 所有链接集合
        /// </summary>
        public List<string> Links
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return new List<string>();
                return RegexHelper.GetResources(this.InnerHtml).Distinct().ToList();
            }
        }


        /// <summary>
        /// 有效链接集合
        /// </summary>
        public List<string> HttpLinks
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return new List<string>();
                var httplinks = RegexHelper.GetALinks(InnerHtml, DomainName);
                return httplinks.Distinct().ToList();
            }
        }

        /// <summary>
        /// 标题
        /// 限制返回不超过200字符的标题,太长的标题是有问题的
        /// </summary>
        public string Title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return string.Empty;
                string title = RegexHelper.GetTitle(InnerHtml);
                if (!string.IsNullOrWhiteSpace(title) && title.Length > 200)
                    title = title.Substring(0, 200);
                return title;
            }
        }

        /// <summary>
        /// iframe数量
        /// </summary>
        /// <returns></returns>
        public int IframeCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return 0;
                return RegexHelper.MatchCount(InnerHtml, @"<iframe");
            }
        }

        /// <summary>
        ///  Flash数量
        /// </summary>
        /// <returns></returns>
        public int FlashCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return 0;
                return RegexHelper.GetLinks(InnerHtml).Count(p=>p.EndsWith(".swf")||p.Contains(".swf?"));
            }
        }

        /// <summary>
        /// strong数量
        /// </summary>
        /// <returns></returns>
        public int StrongCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return 0;
                return RegexHelper.MatchCount(InnerHtml, @"<strong");
            }
        }

        /// <summary>
        /// h1数量
        /// </summary>
        /// <returns></returns>
        public int H1Count
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return 0;
                return RegexHelper.MatchCount(InnerHtml, @"<h1");
            }
        }
        /// <summary>
        /// h2数量
        /// </summary>
        /// <returns></returns>
        public int H2Count
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return 0;
                return RegexHelper.MatchCount(InnerHtml, @"<h2");
            }
        }
        /// <summary>
        /// h3数量
        /// </summary>
        /// <returns></returns>
        public int H3Count
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return 0;
                return RegexHelper.MatchCount(InnerHtml, @"<h3");
            }
        }
        /// <summary>
        /// h4数量
        /// </summary>
        /// <returns></returns>
        public int H4Count
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return 0;
                return RegexHelper.MatchCount(InnerHtml, @"<h4");
            }
        }
        /// <summary>
        /// h5数量
        /// </summary>
        /// <returns></returns>
        public int H5Count
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return 0;
                return RegexHelper.MatchCount(InnerHtml, @"<h5");
            }
        }
        /// <summary>
        /// h6数量
        /// </summary>
        /// <returns></returns>
        public int H6Count
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.InnerHtml))
                    return 0;
                return RegexHelper.MatchCount(InnerHtml, @"<h6");
            }
        }


        /// <summary>
        /// 是否是动态网站
        /// </summary>
        public bool IsDynamic
        {
            get
            {
                if (DateTime.Now.AddMinutes(5) > LastModified || DateTime.Now.AddMinutes(-5) <= LastModified)
                    return true;
                else
                    return false;
            }
        }
    }
}
