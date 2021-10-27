using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Linq;

namespace ShenNius.Share.Infrastructure.Common
{
    /// <summary>
    /// 日志管理
    /// </summary>
    public class LogHelper
    {
        readonly Logger _logger;
        private LogHelper(Logger logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 如果调用构造函数，默认使用数据困记录日志，其他类型对照nlog.config查看
        /// </summary>
        /// <param name="name"></param>
        public LogHelper(string name = "database") : this(LogManager.GetLogger(name))
        {

        }

        public static LogHelper Default { get; private set; }
        static LogHelper()
        {
            Default = new LogHelper(LogManager.GetCurrentClassLogger());

        }
        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="Logger">业务类型</param>
        /// <param name="msg">内容</param>
        /// <param name="logLevel">log等级</param>
        /// <param name="exception">异常信息</param>
        public void Process(string userName, string Logger, string msg, LogLevel logLevel, Exception exception = null)
        {
            try
            {
                var _accessor = new HttpContextAccessor();
                string ip = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                LogEventInfo lei = new LogEventInfo();
                lei.Properties["UserName"] = userName;
                lei.Properties["Logger"] = Logger;
                lei.Level = logLevel;
                lei.Message = msg;
                lei.Exception = exception;
                //TODO
                lei.Properties["Address"] = IpParseHelper.GetAddressByIP(ip);
                _logger.Log(lei);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="Logger">业务类型</param>
        /// <param name="msg">具体内容</param>
        public void ProcessError(string Logger, string msg)
        {
            LogEventInfo lei = new LogEventInfo();
            lei.Properties["Logger"] = Logger;
            lei.Level = LogLevel.Error;
            lei.Message = msg;
            _logger.Log(lei);
        }


    }
}
