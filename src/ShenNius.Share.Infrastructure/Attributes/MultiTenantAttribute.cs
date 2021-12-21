using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.Share.Infrastructure.Caches;
using ShenNius.Share.Models.Entity.Common;
using ShenNius.Share.Models.Entity.Sys;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

/*************************************
* 类名：MultiTenant
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/30 17:17:23
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Attributes
{
    public class MultiTenantAttribute : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// 全局注册过滤器 ，自动为添加 更新方法赋值。也可自行手动打上特性标签
        /// </summary>
        /// <param name="context"></param>
        //private string[] methods = new string[] { "add", "modify" };
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            //s  var actionName = actionDescriptor.ActionName.ToLower();
            ICacheHelper cache = context.HttpContext.RequestServices.GetRequiredService(typeof(ICacheHelper)) as ICacheHelper;
            var currentUserId = context.HttpContext.User.Claims.FirstOrDefault(d=>d.Type== JwtRegisteredClaimNames.Sid)?.Value;
            var tenantId = cache.Get<Tenant>($"{KeyHelper.Sys.CurrentTenant}:{currentUserId}")?.Id;
          
            foreach (var parameter in actionDescriptor.Parameters)
            {
                var parameterName = parameter.Name;//获取Action方法中参数的名字
                var parameterType = parameter.ParameterType;//获取Action方法中参数的类型
                                                           
                                                            //自动添加租户id
                if (typeof(IGlobalTenant).IsAssignableFrom(parameterType))
                {
                    var model = context.ActionArguments[parameterName] as IGlobalTenant;
                    if (tenantId != null)
                    {
                        model.TenantId = tenantId.Value;
                    }
                }
            }
            //}
        }
    }
}