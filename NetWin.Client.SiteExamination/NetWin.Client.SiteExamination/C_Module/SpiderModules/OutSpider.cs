using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Text;
using NetWin.Client.SiteExamination.A_Core.Config;
using NetWin.Client.SiteExamination.B_Common;

namespace NetWin.Client.SiteExamination.C_Module.SpiderModules
{
    /// <summary>
    /// 外部采集器
    /// </summary>
    public class OutSpider : ISpider
    {
        /// <summary>
        /// 查询域名年龄、到期时间、网站页面压缩、ip地址
        /// </summary>
        private string SeoUrl = "http://seo.chinaz.com/{0}";

        /// <summary>
        /// 查询反链数量
        /// </summary>
        private string OutUrl = "https://www.baidu.com/s?wd=domain%3A{0}";

        public OutSpider(string seedSiteUrl)
        {
            SeedSiteUrl = seedSiteUrl;
        }
        public string SeedSiteUrl { get; set; }

        public ResponseMessage SpiderSite(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            try
            {
                ResponseMessage responseMessage = HttpHelper.RequestSite(url, 10);
                return responseMessage;
            }
            catch (Exception exception)
            {
                LogHelper.Error("网址:" + url + ",获取外链网页内容异常：" + exception.Message + exception.StackTrace);
            }
            return null;
        }


        /// <summary>
        /// 爬去资源并封装为外部资源对象
        /// </summary>
        /// <returns></returns>
        public OutSite GetSite()
        {

            var domain = RegexHelper.GetDomainName(SeedSiteUrl);
            OutSite outSite = new OutSite(domain);
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();


                //状态码
                var response = HttpHelper.RequestSite(domain);
                outSite.StatusCode = response.StatusCode;
                watch.Stop();
                outSite.ResponseTime = watch.Elapsed.Milliseconds;
                //首选域
                string preferredDomain;
                if (domain.Contains("www."))
                {
                    preferredDomain = domain.Replace("www.", "");
                }
                else
                {
                    preferredDomain = domain.Replace("http://", "http://www.").Replace("https://", "https://www.");
                }
                var responsePreferred = HttpHelper.RequestSite(preferredDomain);
                if (responsePreferred.ResponseUrls == response.ResponseUrls)
                {
                    outSite.IsPreferredDomain = true;
                    outSite.PreferredDomain = response.ResponseUrls;
                }
                else
                {
                    outSite.IsPreferredDomain = false;
                }

                //关键词
                outSite.Keywords = new List<string>(RegexHelper.GetKeyWords(response.InnerHtml).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                outSite.ExistRobots = HttpHelper.GetStatusCode(domain + "/robots.txt") == 200 ? true : false;
                outSite.ExistSitemap = HttpHelper.GetStatusCode(domain + "/sitemap.xml") == 200 ? true : false;


                //logo有浮动标题和alt属性且皆为关键词
                string logoContent = RegexHelper.GetContentByDom(response.InnerHtml, "div", "t_logo");
                if (!string.IsNullOrEmpty(logoContent))
                {
                    var alt = RegexHelper.GetLogoAlt(logoContent);
                    var title = RegexHelper.GetLogoTitle(logoContent);


                    if (string.IsNullOrEmpty(alt))
                    {
                        outSite.IsLogoContainsKeyWord = false;
                        outSite.LogoAltAndTitle = "logo图片中不包含alt属性值";
                    }
                    else if (string.IsNullOrEmpty(title))
                    {
                        outSite.IsLogoContainsKeyWord = false;
                        outSite.LogoAltAndTitle = "logo图片中不包含title属性值";
                    }
                    else if (!outSite.Keywords.Contains(alt))
                    {
                        outSite.IsLogoContainsKeyWord = false;
                        outSite.LogoAltAndTitle = "logo图片中alt属性值不包含关键词";
                    }
                    else if (!outSite.Keywords.Contains(title))
                    {
                        outSite.IsLogoContainsKeyWord = false;
                        outSite.LogoAltAndTitle = "logo图片中title属性值不包含关键词";
                    }
                    else
                    {
                        outSite.IsLogoContainsKeyWord = true;
                        outSite.LogoAltAndTitle = string.Format("alt:{0},title:{1}", alt, title);
                    }


                }
                else
                {
                    outSite.IsLogoContainsKeyWord = false;
                }



                #region 查询反链

                string searchUrl = SeedSiteUrl.Replace("http://", "").Replace("https://", "");
                var html = HttpHelper.RequestSite(string.Format(OutUrl, searchUrl));
                var count = TextHelper.Truncation(html.InnerHtml, "百度为您找到相关结果约", "个").Replace(",", "");
                outSite.OutLinkCount = 0;
                if (!string.IsNullOrEmpty(count))
                {
                    try
                    {
                        outSite.OutLinkCount = Int32.Parse(count);
                    }
                    catch
                    {
                        // ignored
                    }
                }
                #endregion

                #region http://seo.chinaz.com/
                string whoisUrl = string.Format(SeoUrl, domain.Replace("http://", "").Replace("https://", ""));
                var spiderSite = SpiderSite(whoisUrl);
                if (spiderSite == null || string.IsNullOrEmpty(spiderSite.InnerHtml))
                    return outSite;



                var whoisContent = spiderSite.InnerHtml;
                if (whoisContent.Contains("获取不到Seo数据"))
                {
                    LogHelper.Warning("获取不到Seo数据");
                    return outSite;
                }


                if (whoisContent.Contains("被屏蔽的域名"))
                {
                    LogHelper.Warning("被屏蔽的域名");
                    return outSite;
                }

                //域名年龄和过期信息
                string ageMessage = RegexHelper.GetContentByDom(RegexHelper.GetContentByDomAttr(whoisContent, "div", "class", "w97-0 brn ml5 col-hint02"), "a");
                if (!string.IsNullOrEmpty(ageMessage))
                {
                    //到期时间
                    var expireDate = TextHelper.Truncation(ageMessage, "过期时间为", ")");
                    outSite.ExpireDate = (DateTime.ParseExact(expireDate, "yyyy年MM月dd日", null) - DateTime.Now).Days / 30;
                    var date = TextHelper.Truncation(ageMessage, "（");
                    LogHelper.Info("date" + date);
                    var year = Int32.Parse(string.IsNullOrEmpty(TextHelper.Truncation(date, "年")) ? "0" : TextHelper.Truncation(date, "年"));
                    LogHelper.Info("year" + year);
                    var month = Int32.Parse(string.IsNullOrEmpty(TextHelper.Truncation(date, "年", "月")) ? "0" : TextHelper.Truncation(date, "年", "月"));
                    //域名年龄
                    outSite.DomainAge = year * 12 + month;
                }
                //网页压缩比
                string compress = RegexHelper.GetContentByDomAttr(whoisContent, "div", "class", "MaLi03Row w150 brn", 3);
                if (compress.Contains("-"))
                {
                    outSite.Compress = 0;
                }
                else
                {
                    outSite.Compress = decimal.Parse(compress.Replace("%", ""));
                }
                string compressstr =RegexHelper.ReplaceHtmlTag(RegexHelper.GetContentByDomAttr(whoisContent, "div", "class", "MaLi03Row w150 brn", 0));
                if (compressstr.Contains("否"))
                {
                    outSite.IsCompress = false;
                }
                else
                {
                    outSite.IsCompress = true;
                }


                //ip所在城市地址
                outSite.DomainAddress = RegexHelper.GetContentByDom(RegexHelper.GetContentByDomAttr(whoisContent, "div", "class", "brn ipmW"), "a");
                //是否备案
                outSite.IsRecord = whoisContent.Contains("备案号");
                #endregion
            }
            catch (Exception exception)
            {
                LogHelper.Error("爬去资源并封装为外部资源对象异常:" + exception.Message);
            }
            return outSite;
        }
    }
}
