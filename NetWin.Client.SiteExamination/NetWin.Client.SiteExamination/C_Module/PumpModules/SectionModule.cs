﻿using System;
using System.Collections.Generic;
using System.Linq;
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
                    break;
                case "flash":
                    AimsCount = site.FlashCount;
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
                    }
                    break;
                case "jswithinbody":
                    var body = RegexHelper.GetContentByDom(site.InnerHtml,"body");
                    AimsCount = RegexHelper.MatchCount(body, @"<script");
                    if (AimsCount == 0)
                    {
                        AimsContent = "未检测到js放于body内";
                    }
                    else
                    {
                        AimsContent = "存在js放于body内";
                    }
                    break;
                case "nulllink":
                    AimsCount += site.EmptyLinkCount;
                    break;
                case "size":
                    AimsCount = site.HtmlSize;
                    break;
                case "desccontainsword":
                    int time = Keywords.Count(item => site.Description.Contains(item.Key));
                    AimsCount = time;
                    break;
                case "contentimg":
                    List<string> imgs = RegexHelper.GetImgs(site.InnerHtml);
                    AimsCount = 0;
                    foreach (var item in imgs)
                    {
                        string alt = RegexHelper.GetTagAttrValue(item, "img", "alt");
                        string title = RegexHelper.GetTagAttrValue(item, "img", "title");
                        if (!string.IsNullOrWhiteSpace(alt) && !string.IsNullOrWhiteSpace(title))
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
                    }
                    break;

                case "anchorlink":
                    // 用于比较锚链接和锚文本
                    List<Anchor> anchors = new List<Anchor>();
                    AimsCount = 0;
                    var a = RegexHelper.GetDoms(site.InnerHtmlFilter, "a");
                    foreach (var item in a)
                    {
                        var href = RegexHelper.GetTagAttrValue(item, "a", "href");
                        var text = RegexHelper.GetContentByDom(item, "a");
                        if (!string.IsNullOrWhiteSpace(href))
                        {
                            anchors.Add(new Anchor()
                            {
                                Text = text,
                                Link = href
                            });

                            var textCount = anchors.GroupBy(p => p.Text).Count(p => p.Count() > 1);
                            var hrefCount = anchors.GroupBy(p => p.Link).Count(p => p.Count() > 1);

                            if (textCount > 0 || hrefCount > 0)
                            {
                                AimsCount = 1;
                                AimsContent = "锚文本或链接不唯一";
                                break;
                            }
                        }
                    }
                    break;

                case "imgdescript":
                    AimsCount = 0;

                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(site.InnerHtml);

                    //去掉注释、样式、和js代码:
                    foreach (var script in htmlDocument.DocumentNode.Descendants("script").ToArray())
                        script.Remove();
                    foreach (var style in htmlDocument.DocumentNode.Descendants("style").ToArray())
                        style.Remove();

                    var node = htmlDocument.DocumentNode.SelectNodes("//img");
                    if (node != null)
                    {
                        foreach (var item in node)
                        {
                            var parent = item.ParentNode.ParentNode.ParentNode;
                            var text = RegexHelper.ReplaceHtmlTag(parent.InnerText);
                            if (string.IsNullOrWhiteSpace(text))
                            {
                                AimsCount = 1;
                                AimsContent = "图片周围不存在描述";
                                break;
                            }
                        }
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
