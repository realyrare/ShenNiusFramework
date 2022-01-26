using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Common
{
    /// <summary>
    /// 自定义授权策略
    /// </summary>
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        public CustomAuthorizationHandler()
        {

        }
        //https://www.cnblogs.com/wei325/p/15575141.html Asp.NetCore MVC自定义授权解决方案
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {

            bool flag = false;
            if (requirement.Name == "Policy01")
            {
                //策略1的逻辑

            }

            if (requirement.Name == "Policy02")
            {
                Console.WriteLine("进入自定义策略授权02...");
                ///策略2的逻辑
            }

            if (flag)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
