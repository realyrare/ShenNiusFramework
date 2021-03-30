﻿using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Models.Entity.Common;

/*************************************
* 类名：MultiTenant
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/30 17:17:23
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
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
       // private string[] addUpdateMethods = new string[] { "add", "update" };
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = actionDescriptor.ActionName.ToLower();
            ICacheHelper cache = context.HttpContext.RequestServices.GetRequiredService(typeof(ICacheHelper)) as ICacheHelper;
            var siteId = cache.Get<Site>(KeyHelper.Cms.CurrentSite)?.Id;
            //如果是增加和修改方法  根据角色类和角色添加 标志id 、添加系统资源标志
            //if (addUpdateMethods.Any(o => actionName.Contains(o)))
            //{
                foreach (var parameter in actionDescriptor.Parameters)
                {
                    var parameterName = parameter.Name;//获取Action方法中参数的名字
                    var parameterType = parameter.ParameterType;//获取Action方法中参数的类型
                    //if (!typeof(int).IsAssignableFrom(parameterType))//如果不是ID类型
                    //{
                    //    continue;
                    //}
                    //自动添加租户id
                    if (typeof(GlobalSiteInput).IsAssignableFrom(parameterType))
                    {
                        var model = context.ActionArguments[parameterName] as GlobalSiteInput;
                        if (siteId != 0)
                        {
                            model.SiteId = siteId.Value;
                        }
                    }
                }
            //}
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}