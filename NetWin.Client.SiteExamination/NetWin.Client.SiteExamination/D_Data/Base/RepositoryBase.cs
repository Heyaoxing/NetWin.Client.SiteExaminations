using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using NetWin.Client.SiteExamination.A_Core.Config;

namespace NetWin.Client.SiteExamination.D_Data.Base
{
    public class RepositoryBase : DataScript,IDisposable
    {
        protected static SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
        
        private static SQLiteConnection _connection;
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected static SQLiteConnection SqLiteConnection {
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
            if(_connection!=null)
                _connection.Dispose();
        }
    }
}
