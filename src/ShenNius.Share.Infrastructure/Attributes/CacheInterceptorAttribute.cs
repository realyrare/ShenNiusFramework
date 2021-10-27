using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShenNius.Share.Infrastructure.Caches;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
/*************************************
* 类名：CacheInterceptor
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/25 19:18:42
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Attributes
{
    public class CacheInterceptorAttribute : AbstractInterceptorAttribute
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> TypeofTaskResultMethod = new ConcurrentDictionary<Type, MethodInfo>();
        readonly int _expireSecond;
        readonly string _cacheKey;

        #region 拦截处理
        /// <summary>
        /// 过期时间，单位：分
        /// </summary>
        /// <param name="expireMin"></param>
        public CacheInterceptorAttribute(string cacheKey = null, int expireMin = -1)
        {
            _expireSecond = expireMin * 60;
            _cacheKey = cacheKey;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                string key = string.Empty;
                //自定义的缓存key不存在，再获取类名+方法名或类名+方法名+参数名的组合式key
                if (!string.IsNullOrEmpty(_cacheKey))
                {
                    key = _cacheKey;
                }
                else
                {
                    key = GetKey(context.ServiceMethod, context.Parameters);
                }

                var returnType = GetReturnType(context);
                var cache = context.ServiceProvider.GetService<ICacheHelper>();
                object result = null;
                if (cache.Exists(key))
                {
                    var strResult = cache.Get(key);
                    result = strResult;
                }

                if (result != null)
                {
                    context.ReturnValue = ResultFactory(result, returnType, context.IsAsync());
                }
                else
                {
                    result = await RunAndGetReturn(context, next);
                    if (_expireSecond > 0)
                    {
                        cache.Set(key, result, TimeSpan.FromMinutes(_expireSecond));
                    }
                    else
                    {
                        cache.Set(key, result);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        private static string GetKey(MethodInfo method, object[] parameters)
        {
            return GetKey(method.DeclaringType.Name, method.Name, parameters);
        }
        private static string GetKey(string className, string methodName, object[] parameters)
        {
            var paramConcat = parameters.Length == 0 ? string.Empty : ":" + JsonConvert.SerializeObject(parameters);
            return $"{className}:{methodName}{paramConcat}";
        }


        /// <summary>
        /// 获取被拦截方法返回值类型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Type GetReturnType(AspectContext context)
        {
            return context.IsAsync()
                ? context.ServiceMethod.ReturnType.GetGenericArguments().First()
                : context.ServiceMethod.ReturnType;
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

        /// <summary>
        /// 处理拦截器返回结果
        /// </summary>
        /// <param name="result"></param>
        /// <param name="returnType"></param>
        /// <param name="isAsync"></param>
        /// <returns></returns>
        private object ResultFactory(object result, Type returnType, bool isAsync)
        {
            return !isAsync
                ? result
                : TypeofTaskResultMethod
                    .GetOrAdd(returnType, t => typeof(Task)
                    .GetMethods()
                    .First(p => p.Name == "FromResult" && p.ContainsGenericParameters)
                    .MakeGenericMethod(returnType))
                    .Invoke(null, new object[] { result });
        }
        #endregion

    }
}