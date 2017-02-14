using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
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
                Console.WriteLine("体检开始");
                ExaminationExecuteService execute = new ExaminationExecuteService();
                execute.Start(1, "http://www.wopow.com");

                Thread.Sleep(1000 * 20);
                execute.Stop();
                Console.WriteLine("体检停止");

                Thread.Sleep(1000 * 5);

                execute.Start(1, "http://www.wopow.com");
                Console.WriteLine("体检重新开始");


            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine("结束");
            Console.Read();
        }
    }
}
