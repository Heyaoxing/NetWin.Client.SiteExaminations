using System;
using System.Collections.Generic;

using System.Runtime.Serialization;
using System.Text;
using HtmlAgilityPack;
using NetWin.Client.SiteExamination.B_Common;

namespace NetWin.Client.SiteExamination.C_Module.PumpModules
{
    /// <summary>
    /// 过程计算类
    /// </summary>
    internal class SectionModule : IComputeRule
    {


        /// <summary>
        /// 目标数值统计方法
        /// </summary>
        /// <param name="site"></param>
        internal override void ComputeCount(InSite site)
        {
            if (site == null)
                return;

            switch (AimsContainText)
            {

                case "keywords":
                    AimsCount = site.KeyWords.Count;
                    break;
                case "description":
                    AimsCount = site.Description.Length;
                    break;
                case "css":
                    AimsCount = site.StyleCount;
                    break;
                case "js":
                    AimsCount = site.JsCount;
                    break;
                case "iframe":
                    AimsCount = site.IframeCount;
                    if (AimsCount > 0)
                    {
                        AimsContent = "出现iframe标签";
                    }
                    else
                    {
                        AimsContent = "未检查出iframe标签";
                        SourceUrl = string.Empty;
                    }
                    break;
                case "flash":
                    AimsCount = site.FlashCount;
                    if (AimsCount > 0)
                    {
                        AimsContent = "出现flash";
                    }
                    else
                    {
                        AimsContent = "未检查出flash";
                        SourceUrl = string.Empty;
                    }
                    break;
                case "title":
                    AimsCount = site.Title.Length;
                    break;
                case "strong":
                    AimsCount = site.StrongCount;
                    if (AimsCount == 0)
                    {
                        AimsContent = "未使用strong标签";
                        SourceUrl = "全站";
                    }
                    else
                    {
                        AimsContent = "使用了strong标签";
                        SourceUrl = site.CurrentUrlUrl;
                    }
                    break;
                case "jswithinbody":
                    var body = RegexHelper.GetContentByDom(site.InnerHtml, "body");
                    AimsCount = RegexHelper.MatchCount(body, @"<script");
                    if (AimsCount == 0)
                    {
                        AimsContent = "未检测到js放于body内";
                        SourceUrl = string.Empty;
                    }
                    else
                    {
                        AimsContent = "存在js放于body内";
                    }
                    break;
                case "nulllink":
                    AimsCount = site.EmptyLinkCount;
                    if (AimsCount > 0)
                    {
                        AimsContent = "存在空链接";
                    }
                    else
                    {
                        AimsContent = "未检查出空链接";
                        SourceUrl = string.Empty;
                    }
                    break;
                case "size":
                    AimsCount = site.HtmlSize;
                    break;
                case "desccontainsword":
                    var keys = new List<string>();

                    if (site.StatusCode == 404 || site.StatusCode == 502|| site.StatusCode ==0)
                    {
                        AimsCount = 5;
                        AimsContent = string.Empty;
                        break;
                    }

                    foreach (var keyValuePair in Keywords)
                    {
                        if (site.Description.Contains(keyValuePair.Key))
                        {
                            keys.Add(keyValuePair.Key);
                        }
                    }
                    AimsCount = keys.Count;
                    if (AimsCount > 5)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (var i = 0; i < 5; i++)
                        {
                            sb.Append(keys[i]);
                            if (i != 4)
                                sb.Append(",");
                        }

                        AimsContent = sb.ToString();
                    }
                    else
                    {
                        AimsContent = "仅出现:" + string.Join(",", keys.ToArray());
                    }
                    break;
                case "contentimg":
                    List<string> imgs = RegexHelper.GetImgs(site.InnerHtmlFilter);
                    AimsCount = 0;
                    foreach (var item in imgs)
                    {
                        string alt = RegexHelper.GetTagAttrValue(item, "img", "alt");
                        string title = RegexHelper.GetTagAttrValue(item, "img", "title");
                        if (!string.IsNullOrEmpty(alt) && !string.IsNullOrEmpty(title))
                        {
                            if (Keywords.ContainsKey(alt) && Keywords.ContainsKey(title))
                            {
                                AimsCount = 1;
                                AimsContent = string.Format("alt:{0},title:{1}", alt, title);
                                break;
                            }
                            else
                            {
                                AimsContent = "alt和title属性值不为网页关键词";
                            }
                        }
                        else
                        {
                            AimsContent = "alt和title属性不能为空";
                        }


                        if (string.IsNullOrEmpty(alt))
                        {
                            AimsContent = "alt属性值不能为空";
                        }
                        else if (string.IsNullOrEmpty(title))
                        {
                            AimsContent = "title属性值不能为空";
                        }
                        else if (Keywords.ContainsKey(alt))
                        {
                            AimsContent = "alt属性值不为网页关键词";
                        }
                        else if (Keywords.ContainsKey(title))
                        {
                            AimsContent = "title属性值不为网页关键词";
                        }
                        else
                        {
                            AimsCount = 1;
                            AimsContent = string.Format("alt:{0},title:{1}", alt, title);
                        }
                    }
                    break;

                case "anchorlink":
                    // 用于比较锚链接和锚文本
                    List<Anchor> anchors = new List<Anchor>();
                    AimsCount = 0;
                    AimsContent = "锚文本链接唯一";
                    SourceUrl = string.Empty;
                    var a = RegexHelper.GetDoms(site.InnerHtmlFilter, "a");
                    foreach (var item in a)
                    {
                        var href = RegexHelper.GetTagAttrValue(item, "a", "href");
                        var text = RegexHelper.GetContentByDom(item, "a");
                        if (!string.IsNullOrEmpty(href))
                        {
                            var textCount = anchors.FindAll(p => p.Text == text).Count;
                            var hrefCount = anchors.FindAll(p => p.Link == href).Count;
                            anchors.Add(new Anchor()
                            {
                                Text = text,
                                Link = href
                            });

                            if (textCount > 0 || hrefCount > 0)
                            {
                                AimsCount = 1;
                                AimsContent = "锚文本链接不唯一";
                                SourceUrl = site.CurrentUrlUrl;
                                break;
                            }
                        }
                    }
                    break;

                case "imgdescript":
                    AimsCount = 0;
                    AimsContent = "图片周围存在描述";
                    SourceUrl = string.Empty;
                    try
                    {
                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(site.InnerHtml);

                        //去掉注释、样式、和js代码:
                        var scripts = htmlDocument.DocumentNode.Descendants("script");
                        foreach (var item in scripts)
                        {
                            if (!item.Closed)
                                item.Remove();
                        }
                        var styles = htmlDocument.DocumentNode.Descendants("style");
                        foreach (var item in styles)
                        {
                            if (!item.Closed)
                                item.Remove();
                        }

                        var node = htmlDocument.DocumentNode.SelectNodes("//img");
                        if (node != null)
                        {
                            foreach (var item in node)
                            {
                                var parent = item.ParentNode.ParentNode.ParentNode;
                                var text = RegexHelper.ReplaceHtmlTag(parent.InnerText);
                                if (text != null)
                                {
                                    text = text.Trim().Replace("\n", "").Replace("\t", "").Replace("\r", "");
                                }
                                if (string.IsNullOrEmpty(text))
                                {
                                    AimsCount = 1;
                                    AimsContent = "图片周围不存在描述";
                                    SourceUrl = site.CurrentUrlUrl;
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        LogHelper.Error("检查图片周围存在描述异常:" + exception.Message);
                    }
                    break;
            }
        }
    }


    class Anchor
    {
        public string Text { set; get; }
        public string Link { set; get; }
    }
}
