using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetWin.Client.SiteExamination.B_Common
{
    public class RegexHelper
    {
        #region 验证输入字符串是否与模式字符串匹配
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }



        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回匹配结果
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static string Match(string input, string pattern)
        {
            return Regex.Match(input, pattern, RegexOptions.IgnoreCase).Value;
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回匹配结果统计数
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <returns></returns>
        public static int MatchCount(string input, string pattern)
        {
            return Regex.Matches(input, pattern, RegexOptions.IgnoreCase).Count;
        }
        #endregion


        #region 网页解析


        /// <summary>
        /// 匹配URL是否合法
        /// </summary>
        /// <param name="source">待匹配字符串</param>
        /// <returns>匹配结果true是URL反之不是URL</returns>
        public static bool CheckURLByString(string source)
        {

            Regex rg = new Regex(@"^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", RegexOptions.IgnoreCase);

            return rg.IsMatch(source);

        }

        /// <summary>
        /// 获取文本中的日期
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static DateTime GetPublishDate(string html)
        {
            // 过滤html标签，防止标签对日期提取产生影响
            string text = Regex.Replace(html, "(?is)<.*?>", "");
            Match match = Regex.Match(
                text,
                @"((\d{4}|\d{2})(\-|\/)\d{1,2}\3\d{1,2})(\s?\d{2}:\d{2})?|(\d{4}年\d{1,2}月\d{1,2}日)(\s?\d{2}:\d{2})?",
                RegexOptions.IgnoreCase);

            DateTime result = new DateTime(1900, 1, 1);
            if (match.Success)
            {
                try
                {
                    string dateStr = "";
                    for (int i = 0; i < match.Groups.Count; i++)
                    {
                        dateStr = match.Groups[i].Value;
                        if (!String.IsNullOrEmpty(dateStr))
                        {
                            break;
                        }
                    }
                    // 对中文日期的处理
                    if (dateStr.Contains("年"))
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var ch in dateStr)
                        {
                            if (ch == '年' || ch == '月')
                            {
                                sb.Append("/");
                                continue;
                            }
                            if (ch == '日')
                            {
                                sb.Append(' ');
                                continue;
                            }
                            sb.Append(ch);
                        }
                        dateStr = sb.ToString();
                    }
                    result = Convert.ToDateTime(dateStr);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                if (result.Year < 1900)
                {
                    result = new DateTime(1900, 1, 1);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取网页中的所有链接
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static List<string> GetLinks(string htmlContent)
        {
            List<string> links = new List<string>();
            try
            {
                const string pattern = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matchCollection = regex.Matches(htmlContent);
                links.AddRange(from object link in matchCollection select link.ToString().TrimEnd('\\').Trim());
            }
            catch
            {
                // ignored
            }
            return links;
        }

        /// <summary>
        /// 获取排除掉资源文件后的有效链接
        /// </summary>
        /// <returns></returns>
        public static List<string> GetValidLinks(string htmlContent)
        {
            return GetLinks(htmlContent)
                       .Except(GetStyleLinks(htmlContent))
                       .Except(GetImageLinks(htmlContent))
                       .Except(GetJsLinks(htmlContent)).Distinct().Where(p => !string.IsNullOrEmpty(p) && p != "#").ToList();
        }

        /// <summary>
        /// 获取网站标题
        /// </summary>
        /// <param name="htmlContent">网页内容</param>
        /// <returns></returns>
        public static string GetTitle(string htmlContent)
        {
            var reg = new Regex(@"<title[^>]*?>(.*?)<\/title>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return reg.Match(htmlContent).Groups[1].Value;
        }



        /// <summary>
        /// 获取网站域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDomainName(string url)
        {
            try
            {
                const string pattern = @"https?://(.*?)($|/)";
                string host = Match(url, pattern);
                if (!string.IsNullOrWhiteSpace(host))
                    host = host.TrimEnd('/');
                return host;
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }


        /// <summary>
        /// 读取网页编码
        /// </summary>
        /// <param name="htmlContent">网页内容</param>
        /// <returns></returns>
        public static string GetEncoding(string htmlContent)
        {
            try
            {
                var reg = new Regex(@"<meta.+?charset=[^\w]?([-\w]+)", RegexOptions.IgnoreCase);
                string encodingStirng = reg.Match(htmlContent).Groups[1].Value;
                if (string.IsNullOrWhiteSpace(encodingStirng) || encodingStirng == "“”")
                    encodingStirng = "utf-8";
                return encodingStirng;
            }
            catch (Exception ex)
            {
                // ignored
            }
            return "gb18030";
        }

        /// <summary>
        /// 获取网页关键字
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static string GetKeyWords(string htmlContent)
        {
            try
            {
                const string pattern = "(?<=meta[^>]*?name=\"keywords\"[^>]*?content=\").*?(?=\")";
                return Match(htmlContent, pattern);
            }
            catch (Exception)
            {
                // ignored
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取网页描述
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static string GetDescription(string htmlContent)
        {
            try
            {
                const string pattern = "(?<=meta[^>]*?name=\"description\"[^>]*?content=\").*?(?=\")";
                return Match(htmlContent, pattern);
            }
            catch (Exception)
            {
                // ignored
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取所有资源链接集合
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static List<string> GetResources(string htmlContent)
        {
            List<string> links = new List<string>();
            try
            {
                const string jsPattern = "(?<=script[^>]*?src=\").*?(?=\")";
                const string cssPattern = "(?<=link[^>]*?href=\").*?(?=\")";
                const string aPattern = "(?<=a[^>]*?href=\").*?(?=\")";
                const string imgPattern = "(?<=img[^>]*?src=\").*?(?=\")";
                MatchCollection jsMatch = Regex.Matches(htmlContent, jsPattern, RegexOptions.IgnoreCase);
                MatchCollection cssMatch = Regex.Matches(htmlContent, cssPattern, RegexOptions.IgnoreCase);
                MatchCollection imgMatch = Regex.Matches(htmlContent, imgPattern, RegexOptions.IgnoreCase);
                MatchCollection aMatch = Regex.Matches(htmlContent, aPattern, RegexOptions.IgnoreCase);
                links.AddRange(from object link in jsMatch select link.ToString().Trim());
                links.AddRange(from object link in cssMatch select link.ToString().Trim());
                links.AddRange(from object link in imgMatch select link.ToString().Trim());
                links.AddRange(from object link in aMatch select link.ToString().Trim());
            }
            catch
            {
                // ignored
            }
            return links;
        }

        /// <summary>
        /// 获取a标签中的链接,如果出现“/”开头的链接，则将域名添加上去 成为合法链接
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <param name="domainName">域名</param>
        /// <returns></returns>
        public static List<string> GetALinks(string htmlContent, string domainName)
        {
            List<string> links = new List<string>();
            try
            {
                const string aPattern = "(?<=a[^>]*?href=\").*?(?=\")";
                MatchCollection aMatch = Regex.Matches(htmlContent, aPattern, RegexOptions.IgnoreCase);
                var alinks = from object link in aMatch where !link.ToString().Contains("#") select link.ToString().Trim();
                foreach (var item in alinks)
                {
                    if (item.StartsWith("/") || item.StartsWith("\\"))
                    {
                        links.Add(new Uri(new Uri(domainName), item).ToString());
                    }
                    else if (CheckURLByString(item))
                    {
                        links.Add(item);
                    }
                }
            }
            catch
            {
                // ignored
            }
            return links;
        }

        /// <summary>
        /// 获取所有样式文件链接
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static List<string> GetStyleLinks(string htmlContent)
        {
            List<string> links = new List<string>();
            try
            {
                const string cssPattern = "(?<=link[^>]*?href=\").*?(?=\")";
                MatchCollection cssMatch = Regex.Matches(htmlContent, cssPattern, RegexOptions.IgnoreCase);
                links.AddRange(from object link in cssMatch select link.ToString().Trim());
            }
            catch
            {
                // ignored
            }
            return links;
        }

        /// <summary>
        /// 获取所有图片文件链接
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static List<string> GetImageLinks(string htmlContent)
        {
            List<string> links = new List<string>();
            try
            {
                const string aPattern = "(?<=img[^>]*?src=\").*?(?=\")";
                MatchCollection cssMatch = Regex.Matches(htmlContent, aPattern, RegexOptions.IgnoreCase);
                links.AddRange(from object link in cssMatch select link.ToString().Trim());
            }
            catch
            {
                // ignored
            }
            return links;
        }

        /// <summary>
        ///  获取所有脚本文件链接
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static List<string> GetJsLinks(string htmlContent)
        {
            List<string> links = new List<string>();
            try
            {
                const string jsPattern = "(?<=script[^>]*?src=\").*?(?=\")";
                MatchCollection cssMatch = Regex.Matches(htmlContent, jsPattern, RegexOptions.IgnoreCase);
                links.AddRange(from object link in cssMatch select link.ToString().Trim());
            }
            catch
            {
                // ignored
            }
            return links;
        }

        /// <summary>
        /// 获取所有img标签
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static List<string> GetImgs(string htmlContent)
        {
            List<string> imgs = new List<string>();
            try
            {
                const string pattern = "<img[^>]*?>";
                MatchCollection match = Regex.Matches(htmlContent, pattern, RegexOptions.IgnoreCase);
                imgs.AddRange(from object link in match select link.ToString().Trim());
            }
            catch
            {
                // ignored
            }
            return imgs;
        }

        /// <summary>
        /// 通过dom元素和元素值获得标签内文本
        /// 默认匹配第一个
        /// </summary>
        /// <param name="htmlContent">html内容</param>
        /// <param name="dom">标签</param>
        /// <param name="attribute">属性</param>
        /// <param name="attrValue">属性值</param>
        /// <param name="index">取第几个符合条件的匹配值</param>
        /// <returns></returns>
        public static string GetContentByDomAttr(string htmlContent, string dom, string attribute, string attrValue, int index = 0)
        {
            try
            {
                string pattern = string.Format("<{0}.*?{1}=\"{2}.*?\">(.*?)<\\/{0}>", dom, attribute, attrValue);
                return new Regex(pattern).Matches(htmlContent)[index].Groups[1].Value;
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取移动云logo的img标签
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static string GetLogoTag(string htmlContent)
        {
            try
            {
                string pattern = "<img[^>]*?id=\"t_logo\"[^>]*?>";
                return Match(htmlContent, pattern);
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取移动云logo的alt值
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static string GetLogoAlt(string htmlContent)
        {
            try
            {
                string pattern = "(?<=img[^>]*?id=\"t_logo\"[^>]*?alt=\").*?(?=\")";
                return Match(htmlContent, pattern);
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取指定标签的属性值
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static string GetTagAttrValue(string htmlContent, string dom, string attribute, int index = 0)
        {
            try
            {
                string pattern = string.Format("(?<={0}[^>]*?{1}=\").*?(?=\")", dom, attribute);
                return Match(htmlContent,pattern);
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取移动云logo的title值
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static string GetLogoTitle(string htmlContent)
        {
            try
            {
                string pattern = "(?<=img[^>]*?id=\"t_logo\"[^>]*?title=\").*?(?=\")";
                return Match(htmlContent, pattern);
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过标签获得标签内文本
        /// 默认匹配第一个
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <param name="dom">标签</param>
        /// <param name="index">取第几个符合条件的匹配值</param>
        /// <returns></returns>
        public static string GetContentByDom(string htmlContent, string dom, int index = 0)
        {
            try
            {
                string pattern = string.Format(@"<{0}[^>]*?>([\s\S]*?)<\/{0}>", dom);
                var result= new Regex(pattern).Matches(htmlContent)[index].Groups[1].Value;
                return result;
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }

        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(string html)
        {
            try
            {
                string strText = Regex.Replace(html, "<[^>]+>", "");
                strText = Regex.Replace(strText, "&[^;]+;", "");
                return strText;
            }
            catch (Exception exception)
            {
                LogHelper.Error("去除html标签异常："+exception.Message);
            }
            return html;
        }

        /// <summary>
        ///获取指定的所有标签
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <param name="dom"></param>
        /// <returns></returns>
        public static List<string> GetDoms(string htmlContent, string dom)
        {
            List<string> tags = new List<string>();
            try
            {
                string pattern = string.Format("<{0}[^>].*?>.*?</{0}>", dom);
                MatchCollection match = Regex.Matches(htmlContent, pattern, RegexOptions.IgnoreCase);
                tags.AddRange(from object link in match select link.ToString().Trim());
            }
            catch
            {
                // ignored
            }
            return tags;
        }

        /// <summary>
        /// 过滤html注释
        /// </summary>
        /// <returns></returns>
        public static string FilterNotes(string htmlContent)
        {
            try
            {
                return Regex.Replace(htmlContent, @"(?s)<!--.*?-->", "");
            }
            catch
            {
                // ignored
            }
            return htmlContent;
        }

        /// <summary>
        /// 过滤js脚本
        /// </summary>
        /// <returns></returns>
        public static string FilterJs(string htmlContent)
        {
            try
            {
                return Regex.Replace(htmlContent, @"(?s)<script.*?</script>", "");
            }
            catch
            {
                // ignored
            }
            return htmlContent;
        }
        #endregion
    }
}
