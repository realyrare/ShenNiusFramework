using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
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

/*************************************
* 类名：MvcAndApiAuthorize
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/11/29 16:25:32
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Attributes
{/// <summary>
/// mvc和api共用的同步授权过滤器，如果要用异步的请使用AuthorizeFilter
/// </summary>
    public class MvcAndApiAuthorizeFilter : Attribute, IAuthorizationFilter
    {
        /*使用AuthorizeFilter可以提前拦截到controller和action的请求，但在asp.netcore中需要过滤掉同时请求进来的js\img等请求，
        猜测跟以前asp.net httpmodule的使用方式一样，故选择放弃继续使用ActionFilterAttribute，可以作为了解，看后期asp.net core更新发展，也可以自己过滤掉多余的请求
        */
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (IsHaveAllow(context.Filters))
            {
                return;
            }
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/sys/user/login");
                return;
               // ReturnResult(context, "很抱歉,您未登录！", StatusCodes.Status401Unauthorized);
            }
            //当用户名为mhg时（超级管理员），不用验证权限。
#if DEBUG
            var currentName = context.HttpContext.User.Identity?.Name;
            if (currentName.Equals("mhg"))
            {
                return;
            }
#endif
            ICacheHelper cache = context.HttpContext.RequestServices.GetRequiredService(typeof(ICacheHelper)) as ICacheHelper;
            var userId = context.HttpContext.User.Claims.FirstOrDefault(d => d.Type == JwtRegisteredClaimNames.Sid).Value;
            //从缓存获得权限
            var list = cache.Get<List<MenuAuthOutput>>($"{KeyHelper.User.AuthMenu}:{userId}");

            var area = context.ActionDescriptor.RouteValues["area"].ToString();
            var controller = context.ActionDescriptor.RouteValues["controller"].ToString();
            var action = context.ActionDescriptor.RouteValues["action"].ToString();


            if (list == null || list.Count <= 0)
            {
                ReturnResult(context, "不好意思，您没有该按钮操作权限，请联系系统管理员！", StatusCodes.Status403Forbidden);
                return;
            }
            var url = $"/{area}/{controller}/{action}";
            // 判断列表权限并判断列表的请求方式和数据库里面存放的是否一致
            var model = list.FirstOrDefault(d => d.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase) && d.HttpMethod.Equals("".ToLower()));
            if (model == null)
            {
                ReturnResult(context, "不好意思，您没有列表操作权限", StatusCodes.Status403Forbidden);
                return;
            }

            if (string.IsNullOrEmpty(action))
            {
                ReturnResult(context, "不好意思，您没有该按钮的操作权限", StatusCodes.Status403Forbidden);
                return;
            }
            if (!string.IsNullOrEmpty(model.BtnCodeName))
            {
                var arryBtn = model.BtnCodeName.Split(',');
                if (arryBtn.Length > 0)
                {
                    if (arryBtn.FirstOrDefault(d => d == action.ToLower()) == null)
                    {
                        ReturnResult(context, "不好意思，您没有该按钮操作权限", StatusCodes.Status403Forbidden);
                        return;
                    }
                }
            }
        }

        public static bool IsHaveAllow(IList<IFilterMetadata> filers)
        {
            for (int i = 0; i < filers.Count; i++)
            {
                if (filers[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }
            return false;

        }
        /// <summary>
        /// 判断该请求是否是ajax请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsAjaxRequest(HttpRequest request)
        {
            string header = request.Headers["X-Requested-With"];
            return "XMLHttpRequest".Equals(header);
        }
        private void ReturnResult(AuthorizationFilterContext context, string msg, int statusCodes)
        {
            context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            context.Result = new JsonResult(new ApiResult(msg, statusCodes), setting);
            return;
        }
    }
}