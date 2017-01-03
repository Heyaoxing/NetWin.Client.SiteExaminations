using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWin.Client.SiteExamination.B_Common;

namespace NetWin.Client.SiteExamination.C_Module.SpiderModules
{
    /// <summary>
    /// 网页采集器接口
    /// </summary>
    public interface ISpider
    {
        /// <summary>
        /// 种子地址
        /// </summary>
        string SeedSiteUrl { set; get; }

        /// <summary>
        /// 爬取资源网页内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        ResponseMessage SpiderSite(string url);
    }
}
