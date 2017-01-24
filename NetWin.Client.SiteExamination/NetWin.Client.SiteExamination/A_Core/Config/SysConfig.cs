using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWin.Client.SiteExamination.A_Core.Config
{
    /// <summary>
    /// 系统运行配置类
    /// </summary>
    public static class SysConfig
    {
       
         
        /// <summary>
        /// 是否开启体检超时
        /// </summary>
        public static readonly bool IsTimeOut = true;

        /// <summary>
        /// 体检超时时间,单位秒
        /// 只有开启体检超时( IsTimeOut = true)时才生效
        /// </summary>
        public static readonly int TimeOut = 60;

        /// <summary>
        /// 是否为调试模式,调试模式下会写日志
        /// </summary>
#if DEBUG
        public static readonly bool IsDebug = true;
#else
        public static readonly bool IsDebug = false;
#endif

        #region 采集器设置
        /// <summary>
        /// 每次批量爬取的资源数
        /// </summary>
        public static readonly int SpiderBatch =5;

        /// <summary>
        /// 抓取层级限制,-1为无限制
        /// </summary>
        public static readonly int LevelLimit = -1;

        /// <summary>
        /// 抓取链接数量限制,-1为无限制
        /// </summary>
        public static  int LinkAmountLimit = 500;

        /// <summary>
        /// 请求网站资源超时时间,单位为秒
        /// </summary>
        public static readonly int RequestSiteTimeOut = 10;

        #endregion
    }
}
