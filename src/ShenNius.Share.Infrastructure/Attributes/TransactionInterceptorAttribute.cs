using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

/*************************************
* 类名：TransactionInterceptorAttribute
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/25 19:20:10
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Attributes
{
    public class TransactionInterceptorAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            //var dbContext = context.ServiceProvider.GetService<DbContext>();
            ////先判断是否已经启用了事务
            //if (dbContext.Database.CurrentTransaction == null)
            //{
            //    await dbContext.Database.BeginTransactionAsync();
            //    try
            //    {
            //        await next(context);
            //        dbContext.Database.CommitTransaction();
            //    }
            //    catch (Exception ex)
            //    {
            //        dbContext.Database.RollbackTransaction();
            //        throw ex;
            //    }
            //}
            //else
            //{
            //    await next(context);
            //}
        }
    }
}