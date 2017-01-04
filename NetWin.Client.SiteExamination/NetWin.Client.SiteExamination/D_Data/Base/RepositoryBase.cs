using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using NetWin.Client.SiteExamination.A_Core.Config;
using NetWin.Client.SiteExamination.A_Core.Model;
using NetWin.Client.SiteExamination.B_Common;

namespace NetWin.Client.SiteExamination.D_Data.Base
{
    public class RepositoryBase : DataScript, IDisposable
    {
        protected static SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();

        private static SQLiteConnection _connection;

        static RepositoryBase()
        {
            SqliteInit();
        }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected static SQLiteConnection SqLiteConnection
        {
            get
            {
                sb.DataSource = DataFilePath;
                if (_connection == null)
                {
                    _connection = new SQLiteConnection(sb.ToString());
                }

                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }

                return _connection;
            }
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }


        /// <summary>
        /// 初始化Sqlite数据库
        /// </summary>
        private static void SqliteInit()
        {
            ReslutModel reslutModel = new ReslutModel();
            reslutModel.Result = true;

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
    }
}
