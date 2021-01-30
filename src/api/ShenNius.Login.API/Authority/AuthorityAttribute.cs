using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShenNius.Share.Infrastructure.ApiResponse;
using System;
using System.Collections.Generic;
using System.Text;

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
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new ApiResult("很抱歉,您未登录", StatusCodes.Status401Unauthorized));               
                return;
            }

            //if (currentUser.AuthorityMenu.TrueForAll(e => e.UniName?.Trim() + e.BtnName?.Trim() != name))
            //{
            //    context.Result = new JsonResult(new ApiResultModel()
            //    {
            //        StatusCode = System.Net.HttpStatusCode.Forbidden,
            //        Msg = "当前用户无权限"
            //    });
            //    return;
            //}
        }
    }
}
