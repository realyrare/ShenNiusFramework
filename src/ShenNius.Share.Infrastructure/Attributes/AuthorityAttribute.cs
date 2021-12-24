using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShenNius.Share.Infrastructure.Caches;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Input.Sys;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace ShenNius.Share.Infrastructure.Attributes
{
    /// <summary>
    ///权限验证,配置大于约定。controller名称就是权限码，action就是动作按钮名称权限码
    /// </summary>
    public class AuthorityAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// action别名，如果有别名，先验证
        /// </summary>
        public string Action { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                ReturnResult(context, "很抱歉,您未登录！", StatusCodes.Status401Unauthorized);
                return;
            }
            //当用户名为mhg时（超级管理员），不用验证权限。
#if DEBUG
            var currentName = context.HttpContext.User.Identity.Name;
            if (currentName.Equals("mhg"))
            {
                return;
            }
#endif

            var controller = context.ActionDescriptor.RouteValues["controller"].ToString();
            var action = context.ActionDescriptor.RouteValues["action"].ToString();
            var method = context.HttpContext.Request.Method;
            if (string.IsNullOrEmpty(controller) ||string.IsNullOrEmpty(action) ||string.IsNullOrEmpty(method))
            {
                ReturnResult(context, "controller and action and method is not found", StatusCodes.Status403Forbidden);
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

            //1、验证列表(前端校验)、列表权限码
            var model = list.FirstOrDefault(d => d.NameCode.Equals(controller,StringComparison.OrdinalIgnoreCase));
            if (model == null)
            {
                ReturnResult(context, "不好意思，您没有该列表操作权限", StatusCodes.Status403Forbidden);
                return;
            }
            //2、验证列表按钮
            //2.1 不验证列表权限的action 过滤掉
            if (action.Equals("GetListPages",StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            if (!string.IsNullOrEmpty(model.BtnCodeName))
            {
                var arryBtn = model.BtnCodeName.Split(',');
                if (arryBtn.Length > 0)
                {
                    if (!string.IsNullOrEmpty(Action))
                    {
                        action = Action;                        
                    }
                    if (arryBtn.FirstOrDefault(d => d == action.ToLower()) == null)
                    {
                        ReturnResult(context, "不好意思，您没有该按钮操作权限", StatusCodes.Status403Forbidden);
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

    }

}
