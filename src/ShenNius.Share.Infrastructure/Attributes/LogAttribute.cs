using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NLog;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Models.Enums;
using ShenNius.Share.Models.Enums.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        object logIgnore = null;
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            logIgnore = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(LogIgnoreAttribute), false).FirstOrDefault();
            if (logIgnore != null)
            {
                return;
            }
            ActionArguments = JsonConvert.SerializeObject(context.ActionArguments);
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
            base.OnActionExecuting(context);
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (logIgnore != null)
            {
                return;
            }
            base.OnActionExecuted(context);
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
                Dictionary<string, string> dic = new Dictionary<string, string>
                 {
                { "POST", LogEnum.Add.GetEnumText() },
                { "PUT", LogEnum.Update.GetEnumText() },
                { "DELETE", LogEnum.Delete.GetEnumText() },
                { "GET", LogEnum.Read.GetEnumText() },
                };
                foreach (var item in dic)
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
        }
    }

    /// <summary>
    /// 【忽略全局日志】，有这个特型标签的action基本上有自己私有定制的log记录，全局审计日志可以忽略
    /// </summary>
    public class LogIgnoreAttribute : Attribute
    {

    }
}
