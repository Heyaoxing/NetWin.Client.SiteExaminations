using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Threading;
using NetWin.Client.SiteExamination.A_Core.Config;
using NetWin.Client.SiteExamination.A_Core.Enum;

namespace NetWin.Client.SiteExamination.B_Common
{
    /// <summary>
    /// 记录日志
    /// </summary>
    public class LogHelper
    {

        #region 自定义变量
        /// <summary>
        /// 异常信息的队列
        /// </summary>
        private static Queue<string> qMsg = null;
        /// <summary>
        /// 文件大小最大值，单位：Mb
        /// </summary>
        private static int maxFileSize = 100;
        /// <summary>
        /// 当天创建同一类型的日志文件的个数
        /// </summary>
        private static int[] createdFileCounts = new int[(int)LogType.All];
        /// <summary>
        /// 日志文件存放路径
        /// </summary>
        private static string logFilePath = "";
        #endregion

        #region 属性
        /// <summary>
        /// 文件大小最大值，单位：Mb。小于0时则不限制
        /// </summary>
        private static int MaxFileSize
        {
            get { return maxFileSize; }
            set { maxFileSize = value; }
        }
        /// <summary>
        /// 日志文件存放路径
        /// </summary>
        private static string LogFilePath
        {
            set { logFilePath = value; }
            get
            {
                if (!String.IsNullOrEmpty(logFilePath))
                {
                    return logFilePath;
                }
                else
                {
                    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log//"+DateTime.Now.ToString("yyyy-MM-dd"));
                }
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static LogHelper()
        {
            qMsg = new Queue<string>();
            if (SysConfig.IsDebug)
            {
                SetCreatedFileCount();
                RunThread();
            }
        }
        #endregion

        #region 辅助
        /// <summary>
        /// 获取日志文件的全路径
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        private static string GetLogPath(LogType logType, bool isCreateNew)
        {
            string logPath = LogFilePath;
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
                //看成是新的一天，要将昨天的数据清空
                for (int i = 0; i < createdFileCounts.Length; i++)
                {
                    createdFileCounts[i] = 0;
                }
            }
            switch (logType)
            {
                case LogType.Info:
                    logPath = logPath + "\\" + "Info";
                    break;
                case LogType.Warning:
                    logPath = logPath + "\\" + "Warning";
                    break;
                case LogType.Error:
                    logPath = logPath + "\\" + "Error";
                    break;
                default:
                    logPath = logPath + "\\" + "All";
                    break;
            }
            if (isCreateNew)
            {
                int num = ++createdFileCounts[(int)logType];
                logPath += string.Format("({0}).log", num);
                return logPath;
            }

            logPath += ".log";
            if (!File.Exists(logPath))
            {
                FileStream fs = File.Create(logPath);
                fs.Close();
                fs.Dispose();
            }

            return logPath;
        }

        /// <summary>
        /// 运行线程
        /// </summary>
        private static void RunThread()
        {
            ThreadPool.QueueUserWorkItem(u =>
            {
                while (true)
                {
                    string tmsg = string.Empty;
                    lock (qMsg)
                    {
                        if (qMsg.Count > 0)
                            tmsg = qMsg.Dequeue();
                    }

                    //往日志文件中写错误信息                     
                    if (!String.IsNullOrEmpty(tmsg))
                    {
                        int index = tmsg.IndexOf("&&");
                        string logTypeStr = tmsg.Substring(0, index);
                        LogType logType = LogType.All;
                        if (logTypeStr == string.Format("{0}", LogType.Info))
                        {
                            logType = LogType.Info;
                        }
                        else if (logTypeStr == string.Format("{0}", LogType.Warning))
                        {
                            logType = LogType.Warning;
                        }
                        else if (logTypeStr == string.Format("{0}", LogType.Error))
                        {
                            logType = LogType.Error;
                        }

                        //记录所有日志
                        WriteLog(tmsg.Substring(index + 2));
                        //分开记录日志
                        if (logType != LogType.All)
                        {
                            WriteLog(tmsg.Substring(index + 2), logType);
                        }
                    }

                    if (qMsg.Count <= 0)
                    {
                        Thread.Sleep(1000);
                    }
                }
            });
        }

        /// <summary>
        /// 程序刚启动时 检测已创建的日志文件个数
        /// </summary>
        private static void SetCreatedFileCount()
        {
            string logPath = LogFilePath;
            if (!Directory.Exists(logPath))
            {
                for (int i = 0; i < createdFileCounts.Length; i++)
                {
                    createdFileCounts[i] = 0;
                }
            }
            else
            {
                DirectoryInfo dirInfo = new DirectoryInfo(logPath);
                FileInfo[] fileInfoes = dirInfo.GetFiles("*.log");
                foreach (FileInfo fi in fileInfoes)
                {
                    string fileName = Path.GetFileNameWithoutExtension(fi.FullName).ToLower();
                    if (fileName.Contains("(") && fileName.Contains(")"))
                    {
                        fileName = fileName.Substring(0, fileName.LastIndexOf('('));
                        switch (fileName)
                        {
                            case "info":
                                createdFileCounts[(int)LogType.Info]++;
                                break;
                            case "warning":
                                createdFileCounts[(int)LogType.Warning]++;
                                break;
                            case "error":
                                createdFileCounts[(int)LogType.Error]++;
                                break;
                            case "all":
                                createdFileCounts[(int)LogType.All]++;
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
        }
        #endregion

        #region 写日志

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strLog">日志内容</param>
        private static void WriteLog(string strLog)
        {
            WriteLogAsync(strLog, LogType.All);
        }

        /// <summary>
        /// 输出级别
        /// </summary>
        /// <param name="strLog"></param>
        public static void Info(string strLog)
        {
            WriteLogAsync(strLog, LogType.Info);
        }

        /// <summary>
        /// 输出级别
        /// </summary>
        /// <param name="strLog"></param>
        public static void Info(int strLog)
        {
            WriteLogAsync(strLog.ToString(), LogType.Info);
        }

        /// <summary>
        /// 日志警告级别
        /// </summary>
        /// <param name="strLog"></param>
        public static void Warning(string strLog)
        {
            WriteLogAsync(strLog, LogType.Warning);
        }
        /// <summary>
        /// 日志错误级别
        /// </summary>
        /// <param name="strLog"></param>
        public static void Error(string strLog)
        {
            WriteLogAsync(strLog, LogType.Error);
        }

        /// <summary>
        /// 日志错误级别
        /// </summary>
        /// <param name="strLog"></param>
        public static void Error(Exception exception)
        {
            WriteLogAsync(exception.Message, LogType.Error);
        }



        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strLog">日志内容</param>
        /// <param name="logType">日志类型</param>
        private static void WriteLog(string strLog, LogType logType)
        {

            //调试模式下才会写日志
            if (!SysConfig.IsDebug)
                return;

            if (String.IsNullOrEmpty(strLog))
            {
                return;
            }
            strLog = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + ":" + strLog + "\n";


            switch (logType)
            {
                case LogType.Info:
                case LogType.All:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(strLog);
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(strLog);
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(strLog);
                    break;
            }

             strLog = strLog.Replace("\n", "\r\n");

           

            FileStream fs = null;
            try
            {
                string logPath = GetLogPath(logType, false);
                FileInfo fileInfo = new FileInfo(logPath);
                if (MaxFileSize > 0 && fileInfo.Length > (1024 * 1024 * MaxFileSize))
                {
                    fileInfo.MoveTo(GetLogPath(logType, true));
                }
                fs = File.Open(logPath, FileMode.OpenOrCreate);
                byte[] btFile = Encoding.UTF8.GetBytes(strLog);
                //设定书写的開始位置为文件的末尾  
                fs.Position = fs.Length;
                //将待写入内容追加到文件末尾  
                fs.Write(btFile, 0, btFile.Length);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }


        }

        /// <summary>
        /// 写入错误日志队列
        /// </summary>
        /// <param name="msg">错误信息</param>
        private static void WriteLogAsync(string strLog, LogType logType)
        {
            if (!SysConfig.IsDebug)
                return;

            //将错误信息添加到队列中
            lock (qMsg)
            {
                qMsg.Enqueue(string.Format("{0}&&{1}\r\n", logType, strLog));
            }
        }
        #endregion

    }
}
