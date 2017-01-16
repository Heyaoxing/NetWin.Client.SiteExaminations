using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using NetWin.Client.SiteExamination.B_Common;
using NetWin.Client.SiteExamination.D_Data.Repository;
using NetWin.Client.SiteExamination.E_Services;


namespace NetWin.Client.UnitTest
{
    class Program
    {


        static void Main(string[] args)
        {
            try
            {
                var repost = RegexHelper.CheckURLByString("www.zhihu.com/question/54779059/answer/141149088");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine("结束");
            Console.Read();
        }

        //static void TestGet()
        //{
        //    IExaminationQuery query = new ExaminationQuery();
        //    var item = query.GetExaminationItem();
        //    Console.WriteLine(item.Count);
        //}


        ///// <summary>
        ///// 体检资源登录检查
        ///// 不通过初始检查的,则不往下执行,也不存入数据库
        ///// </summary>
        ///// <param name="siteUrl"></param>
        ///// <returns></returns>
        //static ReslutModel CheckSite(string siteUrl)
        //{
        //    ReslutModel reslutModel = new ReslutModel();
        //    reslutModel.Result = true;
        //    try
        //    {
        //        var responseMessage = HttpHelper.RequestSite(siteUrl);
        //        var links = RegexHelper.GetValidLinks(responseMessage.InnerHtml).Where(p => p.Contains(RegexHelper.GetDomainName(siteUrl)));
        //        if (!links.Any())
        //        {
        //            reslutModel.Result = false;
        //            reslutModel.Message = "资源需要登录";
        //            return reslutModel;
        //        }

        //        //存放已经抓取过的网址
        //        ConcurrentBag<string> spidered = new ConcurrentBag<string>();
        //        Parallel.ForEach(links.Take(SysConfig.SpiderBatch), p =>
        //        {
        //            var response = HttpHelper.RequestSite(p);
        //            spidered.Add(response.ResponseUrls);
        //        });

        //        var distinct = spidered.Count - spidered.Distinct().Count();

        //        if (spidered.Count != 0)
        //        {
        //            double percent = (double)distinct / spidered.Count;
        //            if (percent >= 0.6)
        //            {
        //                reslutModel.Result = false;
        //                reslutModel.Message = "资源需要登录";
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        LogHelper.Error("体检资源登录检查异常:" + exception.Message);
        //    }
        //    return reslutModel;
        //}

        //static void TestConnection()
        //{
        //    string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        //    if (File.Exists(directory))
        //    {
        //        Directory.CreateDirectory(directory);
        //    }

        //    string dataName = "Shove_SiteExamination.db";
        //    string dataFile = Path.Combine(directory, dataName);


        //    if (!File.Exists(dataFile))
        //    {
        //        SQLiteConnection.CreateFile(dataFile);
        //    }

        //    SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
        //    sb.DataSource = dataFile;
        //    using (SQLiteConnection con = new SQLiteConnection(sb.ToString()))
        //    {
        //        con.Execute("insert into ExaminationItemDetailConfig(DetailId,ItemId,RuleId,Name,Score,Require,IsEnable,Suggest)values(1,1,1,'dsa',123,'dsca',1,'sss')");
        //    }
        //    Console.WriteLine("创建成功");
        //}
    }
}
