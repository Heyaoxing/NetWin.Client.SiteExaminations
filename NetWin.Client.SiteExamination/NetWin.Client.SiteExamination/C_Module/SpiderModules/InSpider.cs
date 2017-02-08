using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using NetWin.Client.SiteExamination.A_Core.Config;
using NetWin.Client.SiteExamination.A_Core.Model;
using NetWin.Client.SiteExamination.B_Common;

namespace NetWin.Client.SiteExamination.C_Module.SpiderModules
{
    /// <summary>
    /// 内部采集器
    /// </summary>
    public class InSpider : ISpider, IDisposable
    {
        private static readonly object _obj = new object();
        public InSpider(string seedSiteUrl)
        {
            SeedSiteUrl = seedSiteUrl;
            _concurrentQueue = new ConcurrentQueue<InSite>();
            allSite = new ConcurrentDictionary();
        }

        public string SeedSiteUrl { get; set; }
        public ResponseMessage SpiderSite(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            try
            {
                int timeout = SysConfig.RequestSiteTimeOut;
                if (url == SeedSiteUrl)
                {
                    timeout = 30;
                }

                ResponseMessage responseMessage = HttpHelper.RequestSite(url, timeout);
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
            if (response == null)
                return null;


            InSite inSite = new InSite(SeedSiteUrl, url, response.InnerHtml, response.LastModified, response.Size, depth, response.StatusCode);
            return inSite;
        }



        /// <summary>
        /// 存放已经抓取过的网址
        /// </summary>
        // private ConcurrentBag<string> spidered;
        //存放所有链接
        private ConcurrentDictionary allSite;
        //是否停止抓取,根据资源是否爬去完成,或者是否达到系统设置的限制(SysConfig中设置)
        private bool _ct = false;

        public bool CT
        {
            get
            {
                return _ct;
            }
        }

        public void Cancel()
        {
            _ct = true;

        }

        //内部资源对象队列,封装好的资源对象存放此处
        private ConcurrentQueue<InSite> _concurrentQueue;

        public ConcurrentQueue<InSite> CurrentQueue
        {
            get { return _concurrentQueue; }
        }

        private Thread spiderThread;

         EventWaitHandle wh = new AutoResetEvent(false);
        /// <summary>
        /// 当前正在运行的抓取网页线程总数
        /// </summary>
        static volatile int current = 0;

        /// <summary>
        /// 取消线程
        /// </summary>
        public void Abort()
        {
            if (spiderThread != null)
            {
                spiderThread.Interrupt();
                spiderThread.Abort();
            }
        }

        /// <summary>
        /// 开始抓取
        /// </summary>
        /// <param name="url"></param>
        public void SpiderRun()
        {
            spiderThread = new Thread(() =>
            {
                ConcurrentBag<string> spidered = new ConcurrentBag<string>();
                try
                {
                    allSite.Add(SeedSiteUrl, 0); //种子地址添加进去
                    List<string> except = EnumerableHelper.Except<string>(allSite.Keys, spidered.List);

                    int success = 0;
                    for (var i = except.Count; i > 0 ; i = (except.Count / SysConfig.SpiderBatch) == 0 ? except.Count % SysConfig.SpiderBatch == 0 ? 0 : except.Count % SysConfig.SpiderBatch : SysConfig.SpiderBatch)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            lock (_obj)
                            {
                                current++;
                            }

                            string url = except[j];
                            spidered.Add(url);
                            LogHelper.Info("抓取页:" + url);
                            ThreadPool.QueueUserWorkItem(WorkItem, url);
                        }
                        LogHelper.Info("线程数:" + current);
                        if (current > 0)
                        {
                            LogHelper.Info("等待通知");
                            wh.WaitOne(); // 等待通知
                            LogHelper.Info("等待结束");
                        }

                        //判断是否达到系统设置的爬取数量限制
                        if (spidered.Count() >= SysConfig.LinkAmountLimit && SysConfig.LinkAmountLimit != -1)
                        {
                            LogHelper.Info("达到系统设置的爬取数量限制");
                            _ct = true;
                            break;
                        }

                        except = EnumerableHelper.Except<string>(allSite.Keys, spidered.List);
                        LogHelper.Info("except:" + except.Count);
                        if (except.Count == 0)
                        {
                            LogHelper.Info("网页数据抓取完成");
                            _ct = true;
                            break;
                        }
                    }
                    except.Clear();
                    LogHelper.Info("完成内站抓取,共需要爬取的数量为" + allSite.Count + ",共处理内站数量为:" + spidered.Count());
                }
                catch (Exception exception)
                {
                    _ct = true;
                    LogHelper.Error("抓取异常:" + exception.Message);
                    LogHelper.Info("完成内站抓取,共需要爬取的数量为" + allSite.Count + ",共处理内站数量为:" + spidered.Count());
                }
            });

            spiderThread.IsBackground = true;
            spiderThread.Name = "网页抓取器线程";
            spiderThread.Start();
        }

        private void WorkItem(object obj)
        {
            try
            {
                int depth = 0;
                var url = (string)obj;
                allSite.TryGetValue(url, out depth);
                var spiderResult = GetSite(url, depth);
                if (spiderResult != null)
                {
                    _concurrentQueue.Enqueue(spiderResult);
                    foreach (var item in spiderResult.InsideLinks)
                    {
                        if (!allSite.ContainsKey(item)  && !item.EndsWith(".css") &&
                            !item.EndsWith(".js") && !item.EndsWith(".jpg") && !item.EndsWith(".jpeg") &&
                            !item.EndsWith(".gif") && !item.EndsWith(".png") && !item.EndsWith(".ico"))
                        {
                            allSite.Add(item, depth + 1);
                        }
                    }
                    LogHelper.Info(url + "的内链数:" + spiderResult.InsideLinks.Count);
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error("并发抓取时异常:" + exception.Message);
            }

            lock (_obj)
            {
                current--;
                if (current <= 0)
                {
                    wh.Set(); // 唤醒
                    LogHelper.Info("执行了唤醒");
                }
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void Dispose()
        {
            if (spiderThread != null)
            {
                spiderThread.Interrupt();
                spiderThread.Abort();
            }
            _ct = true;
            allSite.Clear();
        }
    }
}
