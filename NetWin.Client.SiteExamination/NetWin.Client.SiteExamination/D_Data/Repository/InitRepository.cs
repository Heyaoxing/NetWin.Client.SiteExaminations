using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using NetWin.Client.SiteExamination.A_Core.Model;
using NetWin.Client.SiteExamination.B_Common;
using NetWin.Client.SiteExamination.D_Data.Base;

namespace NetWin.Client.SiteExamination.D_Data.Repository
{
    /// <summary>
    /// 初始化数据库
    /// </summary>
    public class InitRepository : RepositoryBase
    {
        /// <summary>
        /// 初始化Sqlite数据库
        /// </summary>
        public static ReslutModel SqliteInit()
        {
            ReslutModel reslutModel=new ReslutModel();
            reslutModel.Result = true;
            try
            {
                if (File.Exists(DataFileDirectory))
                {
                    Directory.CreateDirectory(DataFileDirectory);
                }

                if (!File.Exists(DataFilePath))
                {
                    SQLiteConnection.CreateFile(DataFilePath);
                    //初始化创建所有表
                    SqLiteConnection.Execute(CreateTable);
                    //初始化插入所有系统数据
                    SqLiteConnection.Execute(InsertData);
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception);
                reslutModel.Result = false;
                reslutModel.Message = "初始化Sqlite数据库异常:" + exception.Message;
            }
            return reslutModel;
        }

        public static void Close()
        {
            try
            {
                SqLiteConnection.Dispose();
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception);
            }
        }
    }
}
