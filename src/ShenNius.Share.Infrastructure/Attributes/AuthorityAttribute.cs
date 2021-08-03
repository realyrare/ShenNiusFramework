using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Utils;
using ShenNius.Share.Models.Dtos.Input.Sys;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace ShenNius.Share.Infrastructure.Attributes
{
    /// <summary>
    ///后端API提交请求 权限验证
    /// </summary>
    public class AuthorityAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 模块别名，可配置更改
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// 权限动作
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 是否保存日志
        /// </summary>
        public bool IsLog { get; set; } = true;
        private string ActionArguments { get; set; }
        private string LogType { get; set; }
        private Stopwatch Stopwatch { get; set; }

      
        public override void OnActionExecuting(ActionExecutingContext context)
        {
          
            if (IsLog)
            {
                ActionArguments = JsonConvert.SerializeObject(context.ActionArguments);
                Stopwatch = new Stopwatch();
                Stopwatch.Start();
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                ReturnResult(context, "很抱歉,您未登录！", StatusCodes.Status401Unauthorized);
                return;
            }
            //当用户名为mhg时（超级管理员），不用验证权限。
            var currentName = context.HttpContext.User.Identity.Name;
            if (currentName.Equals("mhg"))
            {
                return;
            }
            ICacheHelper cache = context.HttpContext.RequestServices.GetRequiredService(typeof(ICacheHelper)) as ICacheHelper;
            var userId = context.HttpContext.User.Claims.FirstOrDefault(d => d.Type == JwtRegisteredClaimNames.Sid).Value;
            //从缓存获得权限

            var list = cache.Get<List<MenuAuthOutput>>($"{KeyHelper.User.AuthMenu}:{userId}");
            if (list == null || list.Count <= 0)
            {
                ReturnResult(context, "不好意思，您没有该按钮操作权限，请联系系统管理员！", StatusCodes.Status403Forbidden);
                return;
            }                    
            var model = list.FirstOrDefault(d => d.NameCode == Module.Trim().ToLower());
            if (model == null)
            {
                ReturnResult(context, "不好意思，您没有该列表操作权限", StatusCodes.Status403Forbidden);
                return;
            }
            LogType= model.Name;
            if (string.IsNullOrEmpty(Method))         
            {
                base.OnActionExecuting(context);
                return;
            }
            LogType += ":"+ Method;
            if (!string.IsNullOrEmpty(model.BtnCodeName))
            {
                var arryBtn = model.BtnCodeName.Split(',');
                if (arryBtn.Length > 0)
                {
                    if (arryBtn.FirstOrDefault(d => d == Method.ToLower()) == null)
                    {
                        ReturnResult(context,"不好意思，您没有该按钮操作权限", StatusCodes.Status403Forbidden);
                        return;
                    }
                }
            }
            base.OnActionExecuting(context);
        }
        private static void ReturnResult(ActionExecutingContext context, string msg, int statusCodes)
        {
            context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            context.Result = new JsonResult(new ApiResult(msg, statusCodes), setting);                     
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (!IsLog) return;
            Stopwatch.Stop();
            var url = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
            var method = context.HttpContext.Request.Method;

            var qs = ActionArguments;
            var userName = context.HttpContext.User.Identity.Name;
            var str = $"\n 方法： \n " +
                $"地址：{url} \n " +
                $"方式：{method} \n " +
                $"参数：{qs}\n " +
                $"耗时：{Stopwatch.Elapsed.TotalMilliseconds} 毫秒";
            LogHelper.Default.Process(userName, LogType, str,LogLevel.Trace);
        }
    }
}
