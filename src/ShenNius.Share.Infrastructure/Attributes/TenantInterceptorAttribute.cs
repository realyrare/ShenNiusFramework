using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

/*************************************
* 类名：Tenant
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/30 16:58:05
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Attributes
{
    /// <summary>
    /// 准备基于aspect 实现多租户赋值（控制器和服务层都可使用）  
    /// </summary>
    public class TenantInterceptorAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
          var Parameters=  context.Parameters;
            await context.Invoke(next);
           var result= context.IsAsync() ? await context.UnwrapAsyncReturnValue(): context.ReturnValue;

        }
    }
}