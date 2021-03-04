using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Service.Sys;
using System;
using System.Linq;

namespace ShenNius.Sys.API.Authority
{
    /// <summary>
    /// 权限验证
    /// </summary>
    public class AuthorityAttribute : Attribute, IAuthorizationFilter
    {
        private string name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">菜单名+按钮名，如 菜单列表新增</param>
        public AuthorityAttribute(string name)
        {
            this.name = name;
        }

        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new ApiResult("很抱歉,您未登录", StatusCodes.Status401Unauthorized));               
                return;
            }
            IMenuService menuService =context.HttpContext.RequestServices.GetRequiredService(typeof(IMenuService)) as IMenuService;

            //从缓存获得权限
            var list= menuService.GetCurrentAuthMenus().Result;
            var model= list.FirstOrDefault(d => d.Name == name);
            if (model==null)
            {
               
                context.Result = new JsonResult(new ApiResult("您没有操作权限，请联系系统管理员！", StatusCodes.Status403Forbidden));
                return;
            }
            if (!string.IsNullOrEmpty(model.BtnCodeName))
            {
                var arryBtn= model.BtnCodeName.Split(',');
                if (arryBtn.Length>0)
                {
                    if (arryBtn.FirstOrDefault(d => d != name)!=null)
                    {
                        context.Result = new JsonResult(new ApiResult("不好意思，您没有该按钮操作权限", StatusCodes.Status403Forbidden));
                        return;
                    }
                }
            }
        }
    }
}
