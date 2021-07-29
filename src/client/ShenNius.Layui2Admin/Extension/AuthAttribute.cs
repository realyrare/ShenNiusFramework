using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShenNius.Layui.Admin.Model;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ShenNius.Layui.Admin.Extension
{
    /// <summary>
    /// 前端权限验证
    /// </summary>
    public class AuthAttribute : ResultFilterAttribute
    {
        private readonly string _module;
        private readonly string _action;

        public AuthAttribute(string module, string action)
        {
            _module = module;
            _action = action;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                ReturnResult(context, "很抱歉,您未登录！", StatusCodes.Status401Unauthorized);
                return;
            }
            IMemoryCache memoryCache = context.HttpContext.RequestServices.GetRequiredService(typeof(IMemoryCache)) as IMemoryCache;
            var userId = context.HttpContext.User.Claims.FirstOrDefault(d => d.Type == ClaimTypes.Sid).Value;
            //从缓存获得权限

            var list = memoryCache.Get<List<MenuAuthOutput>>($"frontAuthMenu:{userId}");
            if (list == null || list.Count <= 0)
            {
                ReturnResult(context, "不好意思，您没有该按钮操作权限，请联系系统管理员！", StatusCodes.Status403Forbidden);
                return;
            }
            var model = list.FirstOrDefault(d => d.NameCode == _module);
            if (model == null)
            {
                ReturnResult(context, "不好意思，您没有该按钮操作权限", StatusCodes.Status403Forbidden);
                return;
            }
            if (string.IsNullOrEmpty(_action))
            {                
                return;
            }          
            if (!string.IsNullOrEmpty(model.BtnCodeName))
            {
                var arryBtn = model.BtnCodeName.Split(',');
                if (arryBtn.Length > 0)
                {
                    if (arryBtn.FirstOrDefault(d => d == _action) == null)
                    {
                        ReturnResult(context, "不好意思，您没有该按钮操作权限", StatusCodes.Status403Forbidden);
                        return;
                    }
                }
            }
        }
        private static void ReturnResult(ResultExecutingContext context, string msg, int statusCodes)
        {
            context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            var result = new ApiResult() { StatusCode = 403, Msg = msg, Success = false };
            context.Result = new JsonResult(result, setting);
            //context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(res));
            //context.Result = new EmptyResult();
        }
    }
}
