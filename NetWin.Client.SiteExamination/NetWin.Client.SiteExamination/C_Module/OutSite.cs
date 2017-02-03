using System;
using System.Collections.Generic;

using System.Text;

namespace NetWin.Client.SiteExamination.C_Module
{
    /// <summary>
    /// 外部资源对象
    /// </summary>
    public class OutSite
    {
        public OutSite(string url)
        {
            Url = url;
            Keywords=new List<string>();
        }

        /// <summary>
        /// 网址
        /// </summary>
        public readonly string Url;

        /// <summary>
        /// 域名年龄,单位是月
        /// </summary>
        public int DomainAge { set; get; }

        /// <summary>
        /// 网站是否备案
        /// </summary>
        public bool IsRecord { set; get; }

        /// <summary>
        /// 网站响应时间,单位秒
        /// </summary>
        public int ResponseTime { set; get; }

        /// <summary>
        /// 压缩比
        /// </summary>
        public decimal  Compress { set; get; }

        /// <summary>
        /// 是否启用压缩
        /// </summary>
        public bool IsCompress { set; get; }

        /// <summary>
        /// 到期时间单位月
        /// </summary>
        public int ExpireDate { set; get; }


        /// <summary>
        /// 网站服务状态码
        /// </summary>
        public int StatusCode { set; get; }

        /// <summary>
        /// 网站IP及所属地
        /// </summary>
        public string DomainAddress { set; get; }

        /// <summary>
        /// 域名所属地是否是大陆ip
        /// </summary>
        public bool IsChinaWithDomain { set; get; }

        /// <summary>
        /// 是否存在robots.txt
        /// </summary>
        public bool ExistRobots { set; get; }

        /// <summary>
        /// 是否存在Sitemap.xml
        /// </summary>
        public bool ExistSitemap { set; get; }

        /// <summary>
        /// 关键词集合
        /// </summary>
        public List<string> Keywords { set; get; } 

        /// <summary>
        /// 首选域
        /// </summary>
        public string PreferredDomain { set; get; }

        /// <summary>
        /// 是否做了首选域处理
        /// </summary>
        public bool  IsPreferredDomain { set; get; }

        /// <summary>
        /// 首页logo图片中是否包含关键词
        /// </summary>
        public bool IsLogoContainsKeyWord { set; get; }

        /// <summary>
        /// 首页logo图片中包含的关键词
        /// </summary>
        public string LogoAltAndTitle { set; get; }

        /// <summary>
        /// 外链数(反链)
        /// </summary>
        public int  OutLinkCount { set; get; }

    }
}
