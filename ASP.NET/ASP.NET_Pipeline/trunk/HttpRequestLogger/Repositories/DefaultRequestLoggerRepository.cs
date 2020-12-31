using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml;
using HttpRequestLogger.DataBase;
using HttpRequestLogger.Entities;

namespace HttpRequestLogger.Repositories
{
    /// <summary>
    /// 默认请求日志仓储
    /// </summary>
    public class DefaultRequestLoggerRepository : IRequestLoggerRepository
    {
        private static string _connectionString = string.Empty;

        private SqlClient sqlClient = null;
        private IList<KeyValuePair<string, IEnumerable<SqlParameter>>> executeSqls;

        /// <summary>
        /// 设置数据库连接地址
        /// </summary>
        /// <param name="connectionString"></param>
        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DefaultRequestLoggerRepository()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("未设置数据库连接地址");
            }
            sqlClient = new SqlClient(_connectionString);
            executeSqls = new List<KeyValuePair<string, IEnumerable<SqlParameter>>>();
        }

        public void AddRequestLogger(RequestLogger requestLogger)
        {
            var sqlTable = sqlClient.GetSqlTable<RequestLogger>();
            executeSqls.Add(new KeyValuePair<string, IEnumerable<SqlParameter>>(sqlTable.GetInsertSql(), sqlTable.GetSqlParameters(requestLogger)));
        }

        public void Dispose()
        {
            sqlClient.Dispose();
            executeSqls.Clear();
            executeSqls = null;
            executeSqls = null;
        }

        public void SaveChange()
        {
            try
            {
                sqlClient.ExecuteSql(executeSqls);
            }
            finally
            {
                executeSqls.Clear();
            }
        }

        public void UpdateRequestLogger(RequestLogger requestLogger)
        {
            var sqlTable = sqlClient.GetSqlTable<RequestLogger>();
            executeSqls.Add(new KeyValuePair<string, IEnumerable<SqlParameter>>(sqlTable.GetUpdateSql(), sqlTable.GetSqlParameters(requestLogger)));
        }
    }
}
