﻿using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using ShenNius.Share.Infrastructure.Common;
using System.Diagnostics;

namespace ShenNius.Share.Infrastructure.Attributes
{
    public class LogAttribute : ActionFilterAttribute
    {
        private string ActionArguments { get; set; }
        private Stopwatch Stopwatch { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ActionArguments = Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments);
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            Stopwatch.Stop();

            var url = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
            var method = context.HttpContext.Request.Method;

            var qs = ActionArguments;
            var userName = "";
            //检测是否包含'Authorization'请求头，如果不包含则直接放行
            if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                //var tokenHeader = context.HttpContext.Request.Headers["Authorization"];
                //tokenHeader = tokenHeader.ToString().Substring("Bearer ".Length).Trim();
                userName = context.HttpContext.User.Identity.Name;

            }
            var str = $"地址：{url} \n " +
                $"方式：{method} \n " +
                $"参数：{qs}\n " +
                //$"结果：{res}\n " +
                $"耗时：{Stopwatch.Elapsed.TotalMilliseconds} 毫秒";
            try
            {
                new LogHelper().Process(userName, "浏览", str, LogLevel.Trace);
            }
            catch
            {
            }
        }
    }
}
