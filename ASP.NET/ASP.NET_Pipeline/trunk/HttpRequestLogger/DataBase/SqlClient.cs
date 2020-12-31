using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;

namespace HttpRequestLogger.DataBase
{
    /// <summary>
    /// SQL客户端
    /// </summary>
    internal class SqlClient : IDisposable
    {
        /// <summary>
        /// 类型SQL缓存，提高查询， 批量新增、更新、删除的性能
        /// </summary>
        private static TypeSqlTableCache TypeSqlTableCache = new TypeSqlTableCache(null, null);

        /// <summary>
        /// 数据库连接
        /// </summary>
        public string ConnectionString { get; private set; }
        /// <summary>
        /// 构造一个SQL客户端
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlClient(string connectionString)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new Exception("connectionString 不能为空！");
            ConnectionString = connectionString;
        }
        /// <summary>
        /// 创建数据连接
        /// </summary>
        /// <returns></returns>
        public SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        /// <summary>
        /// 创建读取锁数据库事务
        /// </summary>
        /// <param name="transactionName"></param>
        /// <returns></returns>
        public SqlTransaction CreateReadCommittedSqlTransaction(string transactionName = null)
        {
            var conn = CreateSqlConnection();
            conn.Open();
            if (transactionName != null && transactionName.Length > 0)
                return conn.BeginTransaction(IsolationLevel.ReadCommitted, transactionName);
            return conn.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual List<T> Execute<T>(string sql) where T : class
        {
            List<T> list = new List<T>();
            var dataSet = ExecuteDateSet(sql);
            if (dataSet == null || dataSet.Tables.Count == 0) return list;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(RowToObject<T>(row));
            }
            return list;
        }
        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdParms">查询参数</param>
        /// <returns></returns>
        public virtual List<T> Execute<T>(string sql, params SqlParameter[] cmdParms) where T : class
        {
            List<T> list = new List<T>();
            var dataSet = ExecuteDateSet(sql, cmdParms);
            if (dataSet == null || dataSet.Tables.Count == 0) return list;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(RowToObject<T>(row));
            }
            return list;
        }

        /// <summary>
        /// 获取类型数据库表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual TypeSQLTable GetSqlTable<T>() where T : class
        {
            return TypeSqlTableCache.GetSQLTable(typeof(T));
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual List<T> GetEntities<T>() where T : class
        {
            var sqlTable = TypeSqlTableCache.GetSQLTable(typeof(T));
            List<T> list = new List<T>();
            var dataSet = ExecuteDateSet(sqlTable.GetSelectSql());
            if (dataSet == null || dataSet.Tables.Count == 0) return list;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                list.Add(RowToObject<T>(row));
            }
            return list;
        }

        /// <summary>
        /// 数据项转成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public virtual T RowToObject<T>(DataRow row) where T : class
        {
            T t = default(T);
            if (row == null) return t;

            Type type = typeof(T);
            t = type.Assembly.CreateInstance(type.FullName) as T;

            foreach (var property in type.GetRuntimeProperties())
            {
                var columnAtt = property.GetCustomAttribute<ColumnAttribute>();
                string columnName = property.Name;

                if (columnAtt != null) columnName = columnAtt.Name;

                object value = GetTypeDefaultValue(property.PropertyType);
                if (row.Table.Columns.Contains(columnName))
                {
                    if (!(row[columnName] is DBNull)) value = row[columnName];
                }
                else if (row.Table.Columns.Contains(columnName.ToUpper()))
                {
                    if (!(row[columnName.ToUpper()] is DBNull)) value = row[columnName.ToUpper()];
                }
                property.SetValue(t, value);
            }

            return t;
        }

        private object GetTypeDefaultValue(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>DataSet</returns>
        public virtual DataSet ExecuteDateSet(String sql)
        {
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            SqlDataAdapter daGetData = new SqlDataAdapter(cmd);
            DataSet dsReturnData = new DataSet();
            try
            {
                daGetData.Fill(dsReturnData);
                return dsReturnData;
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
                conn.Dispose();
            }
        }

        public virtual DataSet ExecuteDateSet(string sql, params SqlParameter[] cmdParms)
        {
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, sql, cmdParms);
                SqlDataAdapter daGetData = new SqlDataAdapter(cmd);
                DataSet dsReturnData = new DataSet();
                daGetData.Fill(dsReturnData);
                return dsReturnData;

            }
            finally
            {
                if (cmd.Parameters != null)
                    cmd.Parameters.Clear();
                conn.Close();
                cmd.Dispose();
                conn.Dispose();
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <returns></returns>
        public virtual bool ExecSql(String strSql)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                con.Open();
                cmd.CommandText = strSql;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (System.Data.SqlClient.SqlException E)
            {

                throw new Exception(E.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public virtual bool ExecSql(string strSql, params SqlParameter[] cmdParms)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                con.Open();
                cmd.CommandText = strSql;
                cmd.Parameters.AddRange(cmdParms);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException E)
            {
                throw new Exception(E.Message);
            }
            finally
            {
                if (cmd.Parameters != null)
                    cmd.Parameters.Clear();
                con.Close();
            }
        }

        /// <summary>
        /// 执行SQL语句，实现事务
        /// </summary>
        /// <param name="transaction">事务名称</param>
        /// <param name="SQLString">执行的SQL语句</param>
        /// <param name="cmdParms">SQL参数</param>
        /// <returns>受影响的行数</returns>
        public virtual int ExecuteSql(String transaction, String SQLString, params SqlParameter[] cmdParms)
        {
            int rowsCount = -1;
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            conn.Open();
            SqlTransaction Sqltrans = conn.BeginTransaction(IsolationLevel.ReadCommitted, transaction);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, Sqltrans, SQLString, cmdParms);
                rowsCount = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                Sqltrans.Commit();
            }
            catch (SqlException ex)
            {
                Sqltrans.Rollback(transaction);
                throw ex;
            }
            finally
            {
                if (cmd.Parameters != null)
                    cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
            return rowsCount;
        }
        /// <summary>
        /// 执行SQL语句，实现事务
        /// </summary>
        /// <param name="transaction">事务名称</param>
        /// <param name="executeSqls">待执行的SQL语句集合</param>
        /// <returns>受影响的行数</returns>
        public virtual int ExecuteSql(string transaction, IEnumerable<KeyValuePair<string, IEnumerable<SqlParameter>>> executeSqls)
        {
            int rowsCount = -1;
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlTransaction Sqltrans = conn.BeginTransaction(IsolationLevel.ReadCommitted, transaction);
            try
            {
                foreach (var item in executeSqls)
                {
                    SqlCommand cmd = new SqlCommand();
                    PrepareCommand(cmd, conn, Sqltrans, item.Key, item.Value.ToArray());
                    var i = cmd.ExecuteNonQuery();
                    if (rowsCount < 0)
                        rowsCount = i;
                    else
                        rowsCount += i;
                }
                Sqltrans.Commit();
            }
            catch (SqlException ex)
            {
                Sqltrans.Rollback(transaction);
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return rowsCount;
        }
        /// <summary>
        /// 执行SQL语句，实现事务
        /// </summary>
        /// <param name="executeSqls">待执行的SQL语句集合</param>
        /// <returns>受影响的行数</returns>
        public virtual int ExecuteSql(IEnumerable<KeyValuePair<string, IEnumerable<SqlParameter>>> executeSqls)
        {
            return ExecuteSql("ExecuteSqls", executeSqls);
        }

        /// <summary>
        /// 执行SQL语句，不含事务
        /// </summary>
        /// <param name="SQLString">执行的SQL语句</param>
        /// <param name="cmdParms">SQL参数</param>
        /// <returns></returns>
        public virtual int ExecuteSql(String SQLString, params SqlParameter[] cmdParms)
        {
            int rowsCount = -1;
            SqlConnection conn = CreateSqlConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, SQLString, cmdParms);
                rowsCount = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            finally
            {
                if (cmd.Parameters != null)
                    cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
            return rowsCount;

        }

        /// <summary>
        /// 根据SQL参数重复执行SQL语句
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="cmdParameters">SQL参数</param>
        /// <returns>受影响的行数</returns>
        public virtual int ExecuteSql(String transaction, String SQLString, params IEnumerable<SqlParameter>[] cmdParameters)
        {
            int rowsCount = -1;
            SqlTransaction Sqltrans = CreateReadCommittedSqlTransaction(transaction);
            SqlConnection conn = Sqltrans.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = SQLString;
            cmd.Transaction = Sqltrans;
            cmd.CommandType = CommandType.Text;//cmdType;

            try
            {
                if (cmdParameters != null)
                {
                    foreach (IEnumerable<SqlParameter> parameters in cmdParameters)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                            if (rowsCount < 0)
                                rowsCount = i;
                            else
                                rowsCount += i;
                        cmd.Parameters.Clear();
                    }
                }

                Sqltrans.Commit();
            }
            catch (SqlException ex)
            {
                rowsCount = -1;
                Sqltrans.Rollback(transaction);
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rowsCount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sqltrans"></param>
        /// <param name="SQLString"></param>
        /// <param name="cmdParameters"></param>
        /// <returns></returns>
        public virtual int ExecuteSql(SqlTransaction Sqltrans, String SQLString, params IEnumerable<SqlParameter>[] cmdParameters)
        {
            int rowsCount = -1;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Sqltrans.Connection;
            cmd.CommandText = SQLString;
            cmd.Transaction = Sqltrans;
            cmd.CommandType = CommandType.Text;//cmdType;

            if (cmdParameters != null)
            {
                foreach (IEnumerable<SqlParameter> parameters in cmdParameters)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                        if (rowsCount < 0)
                            rowsCount = i;
                        else
                            rowsCount += i;
                    cmd.Parameters.Clear();
                }
            }

            return rowsCount;
        }

        /// <summary>
        /// 批量新增实体实现事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int InsertEntities<T>(IEnumerable<T> entities) where T : class
        {
            var sqlTable = TypeSqlTableCache.GetSQLTable(typeof(T));

            IList<IEnumerable<SqlParameter>> sqlParameter = new List<IEnumerable<SqlParameter>>();

            foreach (var entity in entities)
            {
                sqlParameter.Add(sqlTable.GetSqlParameters(entity));
            }

            return ExecuteSql("InsertEntities", sqlTable.GetInsertSql(), sqlParameter.ToArray());
        }

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sqltrans"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int InsertEntities<T>(SqlTransaction Sqltrans, IEnumerable<T> entities) where T : class
        {
            var sqlTable = TypeSqlTableCache.GetSQLTable(typeof(T));

            IList<IEnumerable<SqlParameter>> sqlParameter = new List<IEnumerable<SqlParameter>>();

            foreach (var entity in entities)
            {
                sqlParameter.Add(sqlTable.GetSqlParameters(entity));
            }

            return ExecuteSql(Sqltrans, sqlTable.GetInsertSql(), sqlParameter.ToArray());
        }

        /// <summary>
        /// 批量新增实体 实现事务
        /// </summary>
        /// <param name="objects">实体集合</param>
        /// <returns></returns>
        public virtual int InsertEntities(IEnumerable<object> objects)
        {
            return InsertEntities("InsertEntities", objects);
        }
        /// <summary>
        /// 批量新增实体 实现事务
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="objects">实体集合</param>
        /// <returns></returns>
        public virtual int InsertEntities(string transaction, IEnumerable<object> objects)
        {
            var sqlList = new List<KeyValuePair<string, IEnumerable<SqlParameter>>>();
            foreach (var item in objects)
            {
                var sqlTable = TypeSqlTableCache.GetSQLTable(item.GetType());
                sqlList.Add(new KeyValuePair<string, IEnumerable<SqlParameter>>(sqlTable.GetInsertSql(), sqlTable.GetSqlParameters(item)));
            }
            return ExecuteSql(transaction, sqlList);
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int UpdateEntities<T>(IEnumerable<T> entities) where T : class
        {
            var sqlTable = TypeSqlTableCache.GetSQLTable(typeof(T));

            IList<IEnumerable<SqlParameter>> sqlParameter = new List<IEnumerable<SqlParameter>>();

            foreach (var entity in entities)
            {
                sqlParameter.Add(sqlTable.GetSqlParameters(entity));
            }

            return ExecuteSql("UpdateEntities", sqlTable.GetUpdateSql(), sqlParameter.ToArray());
        }
        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sqltrans"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int UpdateEntities<T>(SqlTransaction Sqltrans, IEnumerable<T> entities) where T : class
        {
            var sqlTable = TypeSqlTableCache.GetSQLTable(typeof(T));

            IList<IEnumerable<SqlParameter>> sqlParameter = new List<IEnumerable<SqlParameter>>();

            foreach (var entity in entities)
            {
                sqlParameter.Add(sqlTable.GetSqlParameters(entity));
            }

            return ExecuteSql(Sqltrans, sqlTable.GetUpdateSql(), sqlParameter.ToArray());
        }

        /// <summary>
        /// 批量更新实体 实现事务
        /// </summary>
        /// <param name="objects">实体集合</param>
        /// <returns></returns>
        public virtual int UpdateEntities(IEnumerable<object> objects)
        {
            return InsertEntities("UpdateEntities", objects);
        }

        /// <summary>
        /// 批量新增实体 实现事务
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="objects">实体集合</param>
        /// <returns></returns>
        public virtual int UpdateEntities(string transaction, IEnumerable<object> objects)
        {
            var sqlList = new List<KeyValuePair<string, IEnumerable<SqlParameter>>>();
            foreach (var item in objects)
            {
                var sqlTable = TypeSqlTableCache.GetSQLTable(item.GetType());
                sqlList.Add(new KeyValuePair<string, IEnumerable<SqlParameter>>(sqlTable.GetUpdateSql(), sqlTable.GetSqlParameters(item)));
            }
            return ExecuteSql(transaction, sqlList);
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int DeleteEntities<T>(IEnumerable<T> entities) where T : class
        {
            var sqlTable = TypeSqlTableCache.GetSQLTable(typeof(T));

            IList<IEnumerable<SqlParameter>> sqlParameter = new List<IEnumerable<SqlParameter>>();

            foreach (var entity in entities)
            {
                sqlParameter.Add(sqlTable.GetSqlParameters(entity));
            }

            return ExecuteSql("DeleteEntities", sqlTable.GetDeleteSql(), sqlParameter.ToArray());
        }
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sqltrans"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int DeleteEntities<T>(SqlTransaction Sqltrans, IEnumerable<T> entities) where T : class
        {
            var sqlTable = TypeSqlTableCache.GetSQLTable(typeof(T));

            IList<IEnumerable<SqlParameter>> sqlParameter = new List<IEnumerable<SqlParameter>>();

            foreach (var entity in entities)
            {
                sqlParameter.Add(sqlTable.GetSqlParameters(entity));
            }

            return ExecuteSql(Sqltrans, sqlTable.GetDeleteSql(), sqlParameter.ToArray());
        }

        /// <summary>
        /// 批量删除实体 实现事务
        /// </summary>
        /// <param name="objects">实体集合</param>
        /// <returns></returns>
        public virtual int DeleteEntities(IEnumerable<object> objects)
        {
            return InsertEntities("DeleteEntities", objects);
        }

        /// <summary>
        /// 批量删除实体 实现事务
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="objects">实体集合</param>
        /// <returns></returns>
        public virtual int DeleteEntities(string transaction, IEnumerable<object> objects)
        {
            var sqlList = new List<KeyValuePair<string, IEnumerable<SqlParameter>>>();
            foreach (var item in objects)
            {
                var sqlTable = TypeSqlTableCache.GetSQLTable(item.GetType());
                sqlList.Add(new KeyValuePair<string, IEnumerable<SqlParameter>>(sqlTable.GetDeleteSql(), sqlTable.GetSqlParameters(item)));
            }
            return ExecuteSql(transaction, sqlList);
        }

        /// <summary>
        /// 多数据库执行SQL语句，保持事务一致性
        /// </summary>
        /// <param name="dataBaseSqlExecuteEntities"></param>
        /// <returns></returns>
        public static int MultiDatabaseExecuteSql(IEnumerable<DataBaseSqlExecuteEntity> dataBaseSqlExecuteEntities)
        {
            int rowsCount = -1;

            Queue<DataBaseSqlExecuteEntity> queuq = new Queue<DataBaseSqlExecuteEntity>();
            foreach (var item in dataBaseSqlExecuteEntities)
            {
                try
                {
                    int value = item.ExecuteSql();
                    queuq.Enqueue(item); // 执行成功，将SQL语句执行实体添加到队列中
                    if (value > 0)
                    {
                        if (rowsCount < 0) rowsCount = value;
                        else rowsCount += value;
                    }
                }
                catch (Exception ex)
                {
                    item.CloseSqlConnection();
                    // 执行失败，回滚队列中所有SQL语句执行实体的事务
                    foreach (var it in queuq)
                    {
                        it.Sqltrans.Rollback(it.SqlTransactionName);
                        it.CloseSqlConnection();
                    }
                    throw ex;
                }
            }
            // 执行成功，提交队列中所有SQL语句执行实体的事务
            foreach (var it in queuq)
            {
                it.Sqltrans.Commit();
                it.CloseSqlConnection();
            }

            return rowsCount;
        }

        /// <summary>
        /// 组装命令
        /// </summary>
        /// <param name="cmd">表示要对 SQL Server 数据库执行的一个 Transact-SQL 语句或存储过程</param>
        /// <param name="conn">表示一个到 SQL Server 数据库的打开的连接。此类不能被继承。</param>
        /// <param name="trans">表示要在 SQL Server 数据库中生成的 Transact-SQL 事务。此类不能被继承。</param>
        /// <param name="cmdText">执行的SQL语句</param>
        /// <param name="cmdParms">SQL参数</param>
        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, String cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }

        public void Dispose()
        {
            //TypeSqlTableCache.Dispose();
            //TypeSqlTableCache = null;
        }
    }

    /// <summary>
    /// 数据SQL语句执行实体
    /// </summary>
    internal class DataBaseSqlExecuteEntity
    {
        /// <summary>
        /// 初始化一个 数据SQL语句执行实体
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParams"></param>
        public DataBaseSqlExecuteEntity(String connectionString,String cmdText,IEnumerable<SqlParameter> cmdParams)
        {
            ConnectionString = connectionString;
            CmdText = cmdText;
            CmdParams = cmdParams;
        }
        /// <summary>
        /// 数据连接配置
        /// </summary>
        public String ConnectionString { get; private set; }
        /// <summary>
        /// 执行脚本
        /// </summary>
        public String CmdText { get; private set; }
        /// <summary>
        /// 脚本参数
        /// </summary>
        public IEnumerable<SqlParameter> CmdParams { get; private set; }
        /// <summary>
        /// 事务名称
        /// </summary>
        public String SqlTransactionName { get; set; } = "TransactionName";
        /// <summary>
        /// 事务
        /// </summary>
        public SqlTransaction Sqltrans { get; private set; }
        public SqlConnection Conn { get; set; } = null;

        Boolean connIsOpen = false;

        /// <summary>
        /// 根据SQL参数重复执行SQL语句
        /// </summary>
        /// <returns>受影响的行数</returns>
        public virtual int ExecuteSql()
        {
            int rowsCount = -1;
            Conn = new SqlConnection(this.ConnectionString);
            Conn.Open();
            connIsOpen = true;
            Sqltrans = Conn.BeginTransaction(IsolationLevel.ReadCommitted, SqlTransactionName);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Conn;
            cmd.CommandText = CmdText;
            cmd.Transaction = Sqltrans;
            cmd.CommandType = CommandType.Text;//cmdType;

            if (CmdParams != null)
            {
                cmd.Parameters.AddRange(CmdParams.ToArray());
                rowsCount = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            //Sqltrans.Commit();
            return rowsCount;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseSqlConnection()
        {
            if (connIsOpen)
                Conn.Close();
        }
    }

    /// <summary>
    /// 类型SQL表缓存
    /// </summary>
    internal class TypeSqlTableCache : IDisposable
    {
        Dictionary<TypeSQLTable, DateTime> Cache = new Dictionary<TypeSQLTable, DateTime>();

        static object lock_obj = new object();

        Timer timer = new Timer();
        public TimeSpan? Lifecycle { get; private set; }
        public TimeSpan? ClearFrequency { get; private set; }
        /// <summary>
        /// 构造一个类型SQL数据表缓存
        /// </summary>
        /// <param name="lifecycle">缓存生命周期</param>
        /// <param name="clearFrequency">清除评率</param>
        public TypeSqlTableCache(TimeSpan? lifecycle, TimeSpan? clearFrequency)
        {
            Lifecycle = lifecycle;
            ClearFrequency = clearFrequency;

            if (!ClearFrequency.HasValue) return;

            timer.Interval = ClearFrequency.Value.Milliseconds;
            timer.Elapsed += new ElapsedEventHandler((sender, e) =>
            {
                Clear();
            });
            timer.Start();
        }
        /// <summary>
        /// 获取类型数据库表
        /// 线程安全的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public TypeSQLTable GetSQLTable(Type type)
        {
            var value = Cache.Keys.FirstOrDefault(p => p.Type.Equals(type));
            if (value == null)
            {
                lock (lock_obj)
                {
                    value = Cache.Keys.FirstOrDefault(p => p.Type.Equals(type));
                    if (value == null)
                    {
                        value = new TypeSQLTable(type);
                        Cache.Add(value, DateTime.Now);
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// 清除过期缓存
        /// 线程安全的
        /// </summary>
        private void Clear()
        {
            if (!Lifecycle.HasValue) return;
            if (Cache == null || Cache.Count == 0) return;
            var date = DateTime.Now;
            lock (lock_obj)
            {
                foreach (var key in Cache.Keys)
                {
                    if (Cache[key].Add(Lifecycle.Value) < date)
                        Cache.Remove(key);
                }
            }
        }

        public void Dispose()
        {
            timer.Dispose();
            Cache.Clear();
            timer = null;
            Cache = null;
        }
    }

    /// <summary>
    /// 类型数据库表
    /// </summary>
    internal class TypeSQLTable
    {
        List<TypeTableColumn> _typeTableColumns;
        String _tableName, _selectSqlHasWhere, _selectSq, _insertSql,_updateSql,_deleteSql;

        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; private set; }
        /// <summary>
        /// 表名称
        /// </summary>
        public String TableName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_tableName))
                    _tableName = GetTypeTableName();
                return _tableName;
            }
        }
        /// <summary>
        /// 获取类型数据库表字段
        /// </summary>
        public List<TypeTableColumn> TypeTableColumns
        {
            get {
                if (_typeTableColumns == null)
                    _typeTableColumns = GetTypeTableColumns().ToList();
                return _typeTableColumns;
            }
        }

        public TypeSQLTable(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// 获取类型表名称
        /// </summary>
        /// <returns></returns>
        private String GetTypeTableName()
        {
            var tableAtt = Type.GetCustomAttribute<TableAttribute>();
            if (tableAtt != null) return tableAtt.Name;
            return Type.Name;
        }
        /// <summary>
        /// 获取类型表字段
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TypeTableColumn> GetTypeTableColumns()
        {
            foreach (var properyInfo in Type.GetProperties())
            {
                yield return new TypeTableColumn(properyInfo);
            }
        }

        /// <summary>
        /// 获取SQL参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public IEnumerable<SqlParameter> GetSqlParameters<T>(T value) where T : class
        {
            if (!Type.Equals(value.GetType()))
                throw new Exception($"类型不正确，需要【{Type.FullName}】类型,传入的是【{value.GetType()}】类型");
            foreach (var column in GetTypeTableColumns())
            {
                object propertyValue = column.PropertyInfo.GetValue(value);
                if (propertyValue == null)
                    propertyValue = DBNull.Value;
                if(propertyValue.GetType().Equals(typeof(DateTime))&&((DateTime)propertyValue)==default(DateTime))
                    propertyValue = DBNull.Value;

                yield return new SqlParameter()
                {
                    SqlDbType = column.SqlDbType,
                    ParameterName=column.SqlParamaterName,
                    Value = propertyValue,
                };
            }
        }

        /// <summary>
        /// 获取 select Sql
        /// </summary>
        /// <returns></returns>
        public String GetSelectSqlHasWhere()
        {
            if (String.IsNullOrWhiteSpace(_selectSqlHasWhere))
            {
                StringBuilder sbSql = new StringBuilder(" SELECT ");
                Boolean isFrist = true;
                var columns = TypeTableColumns;
                foreach (var column in columns)
                {
                    if (isFrist)
                        sbSql.Append($" [{column.ColumnName}] ");
                    else
                        sbSql.Append($" ,[{column.ColumnName}] ");
                }
                sbSql.Append($" FROM [{TableName}]");
                isFrist = true;
                foreach (var column in TypeTableColumns.Where(p => p.IsKey))
                {
                    if (isFrist)
                        sbSql.Append($" WHERE [{column.ColumnName}] = {column.SqlParamaterName} ");
                    else
                        sbSql.Append($" AND [{column.ColumnName}] = {column.SqlParamaterName} ");
                }

                _selectSqlHasWhere = sbSql.ToString();
            }

            return _selectSqlHasWhere;
        }
        
        /// <summary>
        /// 获取 select Sql
        /// </summary>
        /// <returns></returns>
        public String GetSelectSql()
        {
            if (String.IsNullOrWhiteSpace(_selectSq))
            {
                StringBuilder sbSql = new StringBuilder(" SELECT ");
                Boolean isFrist = true;
                var columns = TypeTableColumns;
                foreach (var column in columns)
                {
                    if (isFrist)
                        sbSql.Append($" [{column.ColumnName}] ");
                    else
                        sbSql.Append($" ,[{column.ColumnName}] ");
                    isFrist = false;
                }
                sbSql.Append($" FROM [{TableName}]");
                _selectSq = sbSql.ToString();
            }

            return _selectSq;
        }
        /// <summary>
        /// 获取 insert Sql
        /// </summary>
        /// <returns></returns>
        public String GetInsertSql()
        {
            if (String.IsNullOrWhiteSpace(_insertSql))
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append($" INSERT [{TableName}] ( ");
                var columns = TypeTableColumns.Where(p => !p.IsDataBaseGrenerate).ToList();
                bool isFirst = true;
                foreach (var column in columns)
                {
                    if (isFirst)
                        sbSql.Append($" [{column.ColumnName}] ");
                    else
                        sbSql.Append($" ,[{column.ColumnName}] ");

                    isFirst = false;
                }
                sbSql.Append($" ) VALUES ( ");
                isFirst = true;
                foreach (var column in columns)
                {
                    if (isFirst)
                        sbSql.Append($" {column.SqlParamaterName} ");
                    else
                        sbSql.Append($" ,{column.SqlParamaterName} ");

                    isFirst = false;
                }
                sbSql.Append($" ) ");

                _insertSql = sbSql.ToString();
            }
            return _insertSql;
        }
        /// <summary>
        ///  获取 update Sql
        /// </summary>
        /// <returns></returns>
        public String GetUpdateSql()
        {
            if (String.IsNullOrWhiteSpace(_updateSql))
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append($"UPDATE [{TableName}] SET ");

                // 非Key字段SQL拼接
                Boolean isFirst = true;
                foreach (var column in TypeTableColumns.Where(p => !p.IsKey && !p.IsDataBaseGrenerate))
                {
                    if (!isFirst)
                        sbSql.Append($" , [{column.ColumnName}] = {column.SqlParamaterName} ");
                    else
                        sbSql.Append($" [{column.ColumnName}] = {column.SqlParamaterName} ");
                    isFirst = false;
                }

                // Key字段SQL拼接
                isFirst = true;
                foreach (var column in TypeTableColumns.Where(p => p.IsKey))
                {
                    if (isFirst)
                        sbSql.Append($" WHERE [{column.ColumnName}] = {column.SqlParamaterName} ");
                    else
                        sbSql.Append($" AND [{column.ColumnName}] = {column.SqlParamaterName} ");
                    isFirst = false;
                }

                _updateSql = sbSql.ToString();
            }
            return _updateSql;
        }
        /// <summary>
        /// 获取 delete SQL
        /// </summary>
        /// <returns></returns>
        public String GetDeleteSql()
        {
            if (String.IsNullOrWhiteSpace(_deleteSql))
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append($"DELETE [{TableName}] ");

                // Key字段SQL拼接
                Boolean isFirst = true;
                foreach (var column in TypeTableColumns.Where(p => p.IsKey))
                {
                    if (isFirst)
                        sbSql.Append($" WHERE [{column.ColumnName}] = {column.SqlParamaterName} ");
                    else
                        sbSql.Append($" AND [{column.ColumnName}] = {column.SqlParamaterName} ");
                    isFirst = false;
                }

                _deleteSql = sbSql.ToString();
            }
            return _deleteSql;
        }
        /// <summary>
        /// 比较实体类型相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Type)) return false;

            var type = obj as Type;
            return Type.FullName.Equals(type.FullName);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
    /// <summary>
    /// 类型数据库表字段
    /// </summary>
    internal class TypeTableColumn
    {
        String _columnName, _paramaterName;
        Boolean? _isKey, _isDataBaseGrenerate;
        SqlDbType? _sqlDbType;

        /// <summary>
        /// 类型属性
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public String ColumnName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_columnName))
                    _columnName = GetColumnName();
                return _columnName;
            }
        }
        /// <summary>
        /// SQL参数名称
        /// </summary>
        public String SqlParamaterName
        {
            get
            {
                if (String.IsNullOrEmpty(_paramaterName))
                    _paramaterName = $"@{ColumnName}";
                return _paramaterName;
            }
        }
        /// <summary>
        /// 获取 SqlParameter 中的字段和属性的 SQL Server 特定的数据类型
        /// </summary>
        public SqlDbType SqlDbType
        {
            get {

                if (!_sqlDbType.HasValue)
                    _sqlDbType = GetSqlDbType();
                return _sqlDbType.Value;
            }
        }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public Boolean IsKey {
            get
            {
                if (!_isKey.HasValue)
                    _isKey = GetIsKeyColumn();
                return _isKey.Value;
            }
        }
        /// <summary>
        /// 是否是数据创建值字段
        /// </summary>
        public Boolean IsDataBaseGrenerate
        {
            get
            {
                if (!_isDataBaseGrenerate.HasValue)
                    _isDataBaseGrenerate = GetIsDataBaseGrenerate();
                return _isDataBaseGrenerate.Value;
            }
        }

        public TypeTableColumn(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        private String GetColumnName()
        {
            var columnAtt = PropertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (columnAtt != null) return columnAtt.Name;
            return PropertyInfo.Name;
        }

        private Boolean GetIsKeyColumn()
        {
            var keyAtt = PropertyInfo.GetCustomAttribute<KeyAttribute>();
            return keyAtt != null;
        }

        private Boolean GetIsDataBaseGrenerate()
        {
            var att = PropertyInfo.GetCustomAttribute<DatabaseGeneratedAttribute>();
            return att != null;
        }

        private SqlDbType GetSqlDbType()
        {
            var type = PropertyInfo.PropertyType;
            if (type.GenericTypeArguments!=null&& type.GenericTypeArguments.Length>0)
            {
                return GetSqlDbType(type.GenericTypeArguments[0]);
            }
            return GetSqlDbType(PropertyInfo.PropertyType);
        }

        private SqlDbType GetSqlDbType(Type type) {
            if (type.Equals(typeof(string)))
                return SqlDbType.VarChar;

            if (type.Equals(typeof(short)))
                return SqlDbType.SmallInt;

            if (type.Equals(typeof(int))
                || type.Equals(typeof(long)))
                return SqlDbType.BigInt;

            if (type.Equals(typeof(bool)))
                return SqlDbType.Bit;

            if (type.Equals(typeof(DateTime)))
                return SqlDbType.DateTime;

            if (type.IsEnum)
                return SqlDbType.Int;

            if (type.Equals(typeof(DateTimeOffset)))
                return SqlDbType.DateTimeOffset;

            if (type.Equals(typeof(decimal))||type.Equals(typeof(double)))
                return SqlDbType.Decimal;

            if (type.Equals(typeof(float)))
                return SqlDbType.Float;

            if (type.Equals(typeof(Guid)))
                return SqlDbType.UniqueIdentifier;

            throw new Exception($"未实现类型【{type}】");
        }
    }
}
