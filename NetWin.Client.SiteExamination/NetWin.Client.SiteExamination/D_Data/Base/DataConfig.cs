using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetWin.Client.SiteExamination.D_Data.Base
{
    /// <summary>
    /// 数据库配置类
    /// </summary>
    public class DataConfig
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        protected  const  string DataName = "Shove_SiteExamination.db";
        /// <summary>
        /// 数据库文件所在目录
        /// </summary>
        protected static readonly string DataFileDirectory = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 数据库文件所在完整路径
        /// </summary>
        protected static readonly string DataFilePath = Path.Combine(DataFileDirectory, DataName);
    }
}
