using AspectCore.DynamicProxy;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
/*************************************
* 类名：TransactionInterceptorAttribute
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/30 16:27:18
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Repository
{
    /// <summary>
    /// AOP 事务
    /// </summary>
    public class TransactionAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            // DbContext dbContext = new DbContext();
            var dbContext = context.ServiceProvider.GetService<DbContext>();
            try
            {
                dbContext.Db.BeginTran();

                var result = await RunAndGetReturn(context, next);
                if (result is Task)
                {
                    Task.WaitAll(result as Task);
                }
                dbContext.Db.CommitTran();
            }
            catch (Exception ex)
            {
                dbContext.Db.RollbackTran();
                throw ex;
            }
        }
        /// <summary>
        /// 执行被拦截方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private async Task<object> RunAndGetReturn(AspectContext context, AspectDelegate next)
        {
            await context.Invoke(next);
            return context.IsAsync()
            ? await context.UnwrapAsyncReturnValue()
            : context.ReturnValue;
        }
    }
}