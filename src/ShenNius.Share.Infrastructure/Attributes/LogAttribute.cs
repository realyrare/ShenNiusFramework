using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NLog;
using ShenNius.Share.Infrastructure.Common;
using System;
using System.Diagnostics;

namespace ShenNius.Share.Infrastructure.Attributes
{
    /// <summary>
    /// 审计日志
    /// </summary>
    public class LogAttribute : ActionFilterAttribute
    {        
        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType { get; set; }
        private string ActionArguments { get; set; }
        private Stopwatch Stopwatch { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
         
            ActionArguments = JsonConvert.SerializeObject(context.ActionArguments);
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
            base.OnActionExecuting(context);
        }

       
        public override void OnActionExecuted(ActionExecutedContext context)
        {                
            Stopwatch.Stop();

            var url = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
            var method = context.HttpContext.Request.Method;

            var qs = ActionArguments;
            var userName = context.HttpContext.User.Identity.Name;

            var str = $"地址：{url} \n " +
                $"方式：{method} \n " +
                $"参数：{qs}\n " +
                //$"结果：{res}\n " +
                $"耗时：{Stopwatch.Elapsed.TotalMilliseconds} 毫秒";
            if (string.IsNullOrEmpty(LogType))
            {
                foreach (var item in WebHelper.GetDicLogEnumText)
                {
                    if (method.Equals(item.Key, StringComparison.CurrentCultureIgnoreCase))
                    {
                        LogType = item.Value;
                    }
                    else
                    {
                        LogType = method;
                    }
                }
            }
            new LogHelper().Process(userName, LogType, str, LogLevel.Trace);
            base.OnActionExecuted(context);
        }
    }
}
