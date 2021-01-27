using SqlSugar;
using System;

namespace ShenNius.Share.Service.Repository
{
    public class DbContext
    {
        internal static string _connectionStr = string.Empty;
        public DbContext()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _connectionStr ?? throw new ArgumentNullException("数据库连接字符串为空"),
               
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });
            // 调式代码 用来打印SQL
#if DEBUG
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                string s = sql;
                Console.WriteLine($"当前执行的sql：\r\n{sql}");
            };
            Db.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
            {
                string s = sql;
            };
            Db.Aop.OnError = (exp) =>//执行SQL 错误事件
            {
                string s = exp.Sql;
            };
#endif
        }
        public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作
    }
}
