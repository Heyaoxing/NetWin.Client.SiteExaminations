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
              ExaminationExecuteService examinationExecute=new ExaminationExecuteService();
              examinationExecute.CheckSite("http://www.python.org", true);
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
