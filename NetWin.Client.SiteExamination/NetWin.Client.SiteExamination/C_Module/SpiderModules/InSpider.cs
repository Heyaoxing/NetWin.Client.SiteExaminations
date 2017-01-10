using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetWin.Client.SiteExamination.A_Core.Config;
using NetWin.Client.SiteExamination.B_Common;

namespace NetWin.Client.SiteExamination.C_Module.SpiderModules
{
    /// <summary>
    /// 内部采集器
    /// </summary>
    public class InSpider : ISpider, IDisposable
    {
        public InSpider(string seedSiteUrl)
        {
            SeedSiteUrl = seedSiteUrl;
            _concurrentQueue = new ConcurrentQueue<InSite>();
            _ct = new CancellationTokenSource();
            spidered = new ConcurrentBag<string>();
            allSite = new ConcurrentDictionary<string, int>();
        }

        public string SeedSiteUrl { get; set; }
        public ResponseMessage SpiderSite(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            try
            {
                ResponseMessage responseMessage= HttpHelper.RequestSite(url, SysConfig.RequestSiteTimeOut);
                return responseMessage;
            }
            catch (Exception exception)
            {
                LogHelper.Error("网址为:" + url + ", 获取内链网页内容异常：" + exception.Message + exception.StackTrace);
            }
            return null;
        }

        /// <summary>
        /// 爬取资源并封装为内部资源对象
        /// </summary>
        /// <param name="url"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private InSite GetSite(string url, int depth)
        {
            var response = SpiderSite(url);
            if (response==null)
                return null;
            InSite inSite = new InSite(SeedSiteUrl, url, response.InnerHtml, response.LastModified, response.Size, depth);
            return inSite;
        }



        //存放已经抓取过的网址
        private ConcurrentBag<string> spidered ;
        //存放所有链接
        private ConcurrentDictionary<string, int> allSite;
        //是否停止抓取,根据资源是否爬去完成,或者是否达到系统设置的限制(SysConfig中设置)
        private CancellationTokenSource _ct ;

        public CancellationTokenSource CT
        {
            get
            {
                return _ct;
            }
        }

        private InSpider inSpider;
        //内部资源对象队列,封装好的资源对象存放此处
        private ConcurrentQueue<InSite> _concurrentQueue;

        public ConcurrentQueue<InSite> CurrentQueue
        {
            get { return _concurrentQueue; }
        }
        /// <summary>
        /// 开始抓取
        /// </summary>
        /// <param name="url"></param>
        public void SpiderRun()
        {
            ThreadPool.QueueUserWorkItem(u =>
           {
               try
               {
                   inSpider = new InSpider(SeedSiteUrl);
                   allSite.GetOrAdd(SeedSiteUrl, 0); //种子地址添加进去
                   while (allSite.Keys.Except(spidered).Any() && !_ct.IsCancellationRequested)
                   {
                       Parallel.ForEach(allSite.OrderBy(p => p.Value).Select(p => p.Key).Except(spidered).Take(SysConfig.SpiderBatch), p =>
                       {
                           try
                           {
                               int depth = 0;
                               allSite.TryGetValue(p, out depth);
                               var spiderResult = inSpider.GetSite(p, depth);
                               if (spiderResult != null)
                               {
                                   _concurrentQueue.Enqueue(spiderResult);
                                   foreach (var item in spiderResult.InsideLinks)
                                   {
                                       if (!allSite.ContainsKey(item) || item.Contains(SeedSiteUrl) || !item.EndsWith(".css") || !item.EndsWith(".js") || !item.EndsWith(".jpg") || !item.EndsWith(".jpeg") || !item.EndsWith(".gif") || !item.EndsWith(".png"))
                                           allSite.GetOrAdd(item, depth + 1);
                                   }
                               }
                               spidered.Add(p);
                           }
                           catch (Exception exception)
                           {
                               LogHelper.Error("并发抓取时异常:" + exception.Message);
                           }

                       });

                       //判断是否达到系统设置的爬取数量限制
                       if (spidered.Count >= SysConfig.LinkAmountLimit && SysConfig.LinkAmountLimit != -1)
                       {
                           LogHelper.Info("达到系统设置的爬取数量限制");
                           _ct.Cancel();
                       }


                       //判断是否达到系统设置的爬取层级限制
                       if (allSite.Max(p => p.Value) >= SysConfig.LevelLimit && SysConfig.LevelLimit != -1)
                       {
                           LogHelper.Info("达到系统设置的爬取层级限制");
                           _ct.Cancel();
                       }

                   }
                   _ct.Cancel();
                   LogHelper.Info("完成内站抓取,共需要爬取的数量为" + allSite.Count + ",共处理内站数量为:" + spidered.Count);
               }
               catch (Exception exception)
               {
                   _ct.Cancel();
                   LogHelper.Error("抓取异常:" + exception.Message);
                   LogHelper.Info("完成内站抓取,共需要爬取的数量为" + allSite.Count + ",共处理内站数量为:" + spidered.Count);
               }
           });
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void Dispose()
        {
            if (_ct != null)
            {
                _ct.Cancel();
                _ct.Dispose();
                allSite.Clear();
            }
        }
    }
}
