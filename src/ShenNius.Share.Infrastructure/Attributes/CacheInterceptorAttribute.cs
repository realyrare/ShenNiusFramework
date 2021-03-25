using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Newtonsoft.Json;
using ShenNius.Share.Infrastructure.Cache;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
/*************************************
* 类名：CacheInterceptor
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/25 19:18:42
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Attributes
{
    public class CacheInterceptorAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 缓存秒数
        /// </summary>
        public int ExpireSeconds { get; set; }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            //判断是否是异步方法
            bool isAsync = context.IsAsync();
            //if (context.ImplementationMethod.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
            //{
            //    isAsync = true;
            //}
            //先判断方法是否有返回值，无就不进行缓存判断
            var methodReturnType = context.GetReturnParameter().Type;
            if (methodReturnType == typeof(void) || methodReturnType == typeof(Task) || methodReturnType == typeof(ValueTask))
            {
                await next(context);
                return;
            }
            var returnType = methodReturnType;
            if (isAsync)
            {
                //取得异步返回的类型
                returnType = returnType.GenericTypeArguments.FirstOrDefault();
            }
            //获取方法参数名

            var param = context.Parameters.Length == 0 ? string.Empty : ":" + JsonConvert.SerializeObject(context.Parameters);
            //获取方法名称，也就是缓存key值
            string key = context.ImplementationMethod.DeclaringType.FullName + ":" + context.ImplementationMethod.Name + param;
            var cache = context.ServiceProvider.GetService<ICacheHelper>();


            //如果缓存有值，那就直接返回缓存值
            if (cache.Exists(key))
            {
                //反射获取缓存值，相当于cache.HashGet<>(key,param)
                var value = typeof(ICacheHelper).GetMethod(nameof(ICacheHelper.Get)).MakeGenericMethod(returnType).Invoke(cache, new[] { key, param });
                if (isAsync)
                {
                    //判断是Task还是ValueTask
                    if (methodReturnType == typeof(Task<>).MakeGenericType(returnType))
                    {
                        //反射获取Task<>类型的返回值，相当于Task.FromResult(value)
                        context.ReturnValue = typeof(Task).GetMethod(nameof(Task.FromResult)).MakeGenericMethod(returnType).Invoke(null, new[] { value });
                    }
                    else if (methodReturnType == typeof(ValueTask<>).MakeGenericType(returnType))
                    {
                        //反射构建ValueTask<>类型的返回值，相当于new ValueTask(value)
                        context.ReturnValue = Activator.CreateInstance(typeof(ValueTask<>).MakeGenericType(returnType), value);
                    }
                }
                else
                {
                    context.ReturnValue = value;
                }
                return;
            }
            await next(context);
            object returnValue;
            if (isAsync)
            {
                returnValue = await context.UnwrapAsyncReturnValue();
                //反射获取异步结果的值，相当于(context.ReturnValue as Task<>).Result
                //returnValue = typeof(Task<>).MakeGenericType(returnType).GetProperty(nameof(Task<object>.Result)).GetValue(context.ReturnValue);

            }
            else
            {
                returnValue = context.ReturnValue;
            }
            if (ExpireSeconds > 0)
            {
                cache.Set(key, returnValue, TimeSpan.FromSeconds(ExpireSeconds));
            }
            else
            {
                cache.Set(key, returnValue);
            }
        }
    }
}