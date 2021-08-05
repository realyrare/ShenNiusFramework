using NLog;
using System;

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
        public LogHelper(string name) : this(LogManager.GetLogger(name))
        {

        }

        public static LogHelper Default { get; private set; }
        static LogHelper()
        {
            Default = new LogHelper(LogManager.GetCurrentClassLogger());
        }

        public void Process(string userName, string Logger, string msg, LogLevel logLevel, Exception exception=null)
        {
            LogEventInfo lei = new LogEventInfo();
            lei.Properties["UserName"] = userName;
            lei.Properties["Logger"] = Logger;
            lei.Level = logLevel;
            lei.Message = msg;
            lei.Exception = exception;
            _logger.Log(lei);
        }

        public void ProcessError(int statusCode, string msg)
        {
            LogEventInfo lei = new LogEventInfo();
            lei.Properties["Logger"] = Convert.ToString(statusCode);
            lei.Level = LogLevel.Error;
            lei.Message = msg;
            _logger.Log(lei);
        }

     



    }
}
