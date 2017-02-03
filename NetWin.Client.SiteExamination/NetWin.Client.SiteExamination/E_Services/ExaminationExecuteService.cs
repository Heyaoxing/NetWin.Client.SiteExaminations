using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NetWin.Client.SiteExamination.A_Core.Config;
using NetWin.Client.SiteExamination.A_Core.Model;
using NetWin.Client.SiteExamination.B_Common;
using NetWin.Client.SiteExamination.C_Module;
using NetWin.Client.SiteExamination.C_Module.PumpModules;
using NetWin.Client.SiteExamination.C_Module.SpiderModules;
using NetWin.Client.SiteExamination.D_Data.Entities;
using NetWin.Client.SiteExamination.D_Data.Repository;
using ThreadState = System.Threading.ThreadState;

namespace NetWin.Client.SiteExamination.E_Services
{
    /// <summary>
    /// 体检过程执行类
    /// </summary>
    public class ExaminationExecuteService
    {
        private static readonly object _obj = new object();
        /// <summary>
        /// 执行入口
        /// step1：检查服务器更新（后期）
        /// step2：检查体检资源
        /// step3：挑选出可访问的首选域
        /// step4：执行体检过程
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public ReslutModel<int> Start(long userId, string siteUrl)
        {
            ReslutModel<int> ResultModel = new ReslutModel<int>();
            ResultModel.Result = true;
            if (string.IsNullOrEmpty(siteUrl))
            {
                ResultModel.Result = false;
                ResultModel.Message = "启动失败,url为空!";
                return ResultModel;
            }

            siteUrl = RegexHelper.GetDomainName(siteUrl.Trim());

            try
            {

                //新增一条资源检查记录
                var id = SiteExaminationInfoRepository.Add(new SiteExaminationInfo()
                {
                    UserId = userId,
                    SiteUrl = siteUrl,
                    CreatedOn = DateTime.Now,
                    IsCompleted = false,
                    CertificateNum = "1234567"   //TODO 待开发 7位数
                });
                ResultModel.Data = id;
                bool isExex = Execute(id, siteUrl);
                if (!isExex)
                {
                    ResultModel.Result = false;
                    ResultModel.Message = "启动失败,当前体检正在运行";
                }
            }
            catch (Exception exception)
            {
                ResultModel.Result = false;
                ResultModel.Message = exception.Message;
            }

            return ResultModel;
        }

        /// <summary>
        /// 检查服务更新
        /// </summary>
        [Obsolete("暂未开发")]
        private void CheckServer()
        {
            throw new NotImplementedException();//TODO 待开发检查服务更新
        }

        /// <summary>
        /// 体检资源登录检查
        /// 不通过初始检查的,则不往下执行,也不存入数据库
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="isReplace"></param>
        /// <returns></returns>
        public ReslutModel<string> CheckSite(string siteUrl, bool isReplace)
        {
            ReslutModel<string> reslutModel = new ReslutModel<string>();
            reslutModel.Result = true;
            reslutModel.Data = siteUrl;
            if (string.IsNullOrEmpty(siteUrl))
            {
                reslutModel.Result = false;
                reslutModel.Message = "启动失败,url为空!";
                return reslutModel;
            }

            if (!RegexHelper.CheckURLByString(siteUrl))
            {
                reslutModel.Result = false;
                reslutModel.Message = "启动失败,网址为不合法!";
                return reslutModel;
            }

            siteUrl = RegexHelper.GetDomainName(siteUrl);

            try
            {
                var responseMessage = HttpHelper.RequestSite(siteUrl);
                if (isReplace && responseMessage.StatusCode != 200 && responseMessage.StatusCode != 301)
                {
                    siteUrl = siteUrl.Replace("http://", "https://");
                    responseMessage = HttpHelper.RequestSite(siteUrl);
                    reslutModel.Data = siteUrl;
                }


                if (responseMessage.StatusCode == 200 || responseMessage.StatusCode == 301)
                {
                    var links = EnumerableHelper.Distinct(RegexHelper.GetALinks(responseMessage.InnerHtml, siteUrl).FindAll(p => p.Contains(RegexHelper.GetDomainName(responseMessage.ResponseUrls, true))));

                    if (links.Count == 0)
                    {
                        reslutModel.Result = false;
                        reslutModel.Message = @"对不起，您输入的网址可能存在以下情况：
                                                {0}1.该网站需要登录，搜索引擎无法识别，请更换网站推广优化
                                                {0}2.该网站属于单页面网站，不利于推广优化，请更换网站推广优化
                                                {0}3.输入的网址不适合用本网站去推广优化，请更换网址或者大批量更改该网站内链
                                                ";
                        return reslutModel;
                    }
                    spidered.Clear();

                    for (int i = 0; i < (links.Count > 5 ?5: links.Count); i++)
                    {
                        lock (_obj)
                        {
                            current++;
                        }
                        string url = links[i];
                        ThreadPool.QueueUserWorkItem(WorkItem, url);
                    }

                    wh.WaitOne();

                    var distinct = spidered.Count() - EnumerableHelper.Distinct(spidered.List).Count;
                    if (spidered.Count() != 0)
                    {
                        double percent = (double)distinct / spidered.Count();
                        if (percent >= 0.6)
                        {
                            reslutModel.Result = false;
                            reslutModel.Message = "网站需要登陆才能看到内容";
                        }
                    }
                }
                else
                {
                    reslutModel.Result = false;
                    reslutModel.Message = "网址不能访问,请检查网址拼写是否正确!";
                    return reslutModel;
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error("体检资源登录检查异常:" + exception.Message);
            }
            return reslutModel;
        }

        //存放已经抓取过的网址
        ConcurrentBag<string> spidered = new ConcurrentBag<string>();
        /// <summary>
        /// 当前正在运行的抓取网页线程总数
        /// </summary>
        static volatile int current = 0;
        static EventWaitHandle wh = new AutoResetEvent(false);

        private void WorkItem(object obj)
        {
            try
            {
                string url = (string)obj;
                var response = HttpHelper.RequestSite(url);
                lock (_obj)
                {
                    spidered.Add(response.ResponseUrls);
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error("登录检查抓取网页异常:" + exception.Message);
            }

            lock (obj)
            {
                current--;
                if (current <= 0)
                {
                    wh.Set();
                }
            }
        }

        Thread inSiteThread;
        /// <summary>
        /// 异步执行资源体检
        /// 内部链接：分批采集,并进入数据处理泵交给过程处理组件和全局处理组件计算后入库
        /// 外部链接:直接调用三方资源获取结果后入库
        /// </summary>
        /// <returns>是否启动成功</returns>
        private bool Execute(int id, string siteUrl)
        {
            canStop = false;
            bool result = false;
            if (inSiteThread != null && inSiteThread.ThreadState != ThreadState.Stopped)
                return result;

            try
            {
                ProcessParam param = new ProcessParam();
                param.Id = id;
                param.siteUrl = siteUrl;

                inSiteThread = new Thread(InSiteProcess);
                inSiteThread.IsBackground = true;
                inSiteThread.Start(param);


                result = true;
            }
            catch (Exception exception)
            {
                LogHelper.Error("执行资源体检异常:" + exception.Message);
            }
            return result;
        }

        /// <summary>
        /// 停止体检
        /// </summary>
        public ReslutModel Stop()
        {
            ReslutModel reslutModel = new ReslutModel();
            reslutModel.Result = true;
            if (inSiteThread == null)
            {
                return reslutModel;
            }
            try
            {
                canStop = true;
                inSiteThread.Interrupt();
                while (true)
                {
                    if (inSiteThread.ThreadState == ThreadState.Stopped)
                    {
                        inSiteThread.Abort();
                        LogHelper.Info("外部线程注销");
                        inSiteThread = null;
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                    LogHelper.Info("线程轮询检查");
                }

            }
            catch (Exception exception)
            {
                reslutModel.Result = false;
                reslutModel.Message = exception.Message;
                LogHelper.Error("停止体检异常:" + exception.Message);
            }
            return reslutModel;
        }


        /// <summary>
        /// 控制线程终止
        /// </summary>
        private volatile bool canStop = false;

        /// <summary>
        /// 内部资源处理
        /// step1:初始化数据处理泵
        /// step2:启动内部采集器
        /// step3:计算采集器出来的数据流
        /// step4:将结果入库
        /// <param name="siteUrl"></param>
        /// </summary>
        private void InSiteProcess(object processParam)
        {
            Stopwatch watch = new Stopwatch();//计算用时
            watch.Start();

            ProcessParam param = (ProcessParam)processParam;
            InSpider inSpider = new InSpider(param.siteUrl);
            try
            {
                var outSite = OutSiteProcess(processParam);//先进行外部处理


                #region step1
                //初始化数据处理泵
                List<PumpInitParam> pumpParam = ComputeRuleConfigRepository.GetInPumpInitParams();
                InPumpModule pumpModule = new InPumpModule(pumpParam, outSite.Keywords);

                #endregion

                #region step2

                inSpider.SpiderRun();
                //是否结束处理
                bool isEnd = false;
                while (true)
                {


                    var inSite = inSpider.CurrentQueue.Dequeue();

                    //判断是否是结束处理
                    if (inSpider.CT && inSite == null)
                    {
                        isEnd = true;
                    }




                    if (inSite == null)
                    {
                        Thread.Sleep(1000);
                    }

                    //将结果入库
                    List<ExaminationResult> examinationResults = pumpModule.Process((InSite)inSite, isEnd);
                    foreach (var item in examinationResults)
                    {
                        SiteExaminationDetailInfoRepository.Add(new SiteExaminationDetailInfo()
                        {
                            SiteId = param.Id,
                            DetailId = item.DetailId,
                            IsPass = item.IsPass,
                            Result = item.Result,
                            CreatedOn = item.ExaminationDateTime,
                            Score = item.Score,
                            Position = item.Position
                        });
                    }

                    if (canStop)
                    {
                        isEnd = true;
                        inSpider.Cancel();
                        inSpider.Abort();
                        LogHelper.Info("收到外部停止信息");
                    }

                    if (isEnd)
                    {
                        bool isCompleted = !canStop;
                        SiteExaminationInfoRepository.Complete(param.Id, isCompleted);
                        break;
                    }

                }
                #endregion
            }
            catch (Exception exception)
            {
                inSpider.Cancel();
                LogHelper.Error("内部资源处理异常:" + exception.StackTrace + exception.Message);
            }
            watch.Stop();
            LogHelper.Info("内部资源处理结束,总共用时(秒):" + watch.Elapsed.TotalSeconds);
        }

        /// <summary>
        /// 外部资源处理
        /// <param name="processParam"></param>
        /// </summary>
        private OutSite OutSiteProcess(object processParam)
        {
            LogHelper.Info("执行外部处理");
            ProcessParam param = (ProcessParam)processParam;
            try
            {
                //初始化数据处理泵
                List<PumpInitParam> pumpParam = ComputeRuleConfigRepository.GetOutPumpInitParams();
                OutPumpModule pumpModule = new OutPumpModule(pumpParam);
                OutSpider outSpider = new OutSpider(param.siteUrl);
                OutSite outSite = outSpider.GetSite();
                List<ExaminationResult> examination = pumpModule.Process(outSite);
                foreach (var item in examination)
                {
                    SiteExaminationDetailInfoRepository.Add(new SiteExaminationDetailInfo()
                    {
                        SiteId = param.Id,
                        DetailId = item.DetailId,
                        IsPass = item.IsPass,
                        Result = item.Result,
                        CreatedOn = item.ExaminationDateTime,
                        Score = item.Score,
                        Position = item.Position
                    });
                }
                return outSite;
            }
            catch (Exception exception)
            {
                LogHelper.Error("外部资源处理异常:" + exception.Message);
            }
            LogHelper.Info("执行外部处理结束");
            return new OutSite("");
        }
    }

    public class ProcessParam
    {
        public int Id { set; get; }
        public string siteUrl { set; get; }
    }
}
