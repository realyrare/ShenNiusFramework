using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.Share.Infrastructure.Caches;
using ShenNius.Share.Infrastructure.Extensions;
using System;
using System.Threading.Tasks;
/*************************************
* 类名：CacheDeleteInterceptorAttribute
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/25 19:41:50
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Attributes
{
    public class CacheRemoveInterceptorAttribute : AbstractInterceptorAttribute
    {
        private readonly Type[] _types;
        private readonly string[] _methods;
        /// <summary>
        /// 自定义的缓存key
        /// </summary>
        private readonly string _cacheKey;

        /// <summary>
        /// 需传入相同数量的Types跟Methods，同样位置的Type跟Method会组合成一个缓存key，进行删除
        /// </summary>
        /// <param name="Types">传入要删除缓存的类</param>
        /// <param name="Methods">传入要删除缓存的方法名称，必须与Types数组对应</param>
        public CacheRemoveInterceptorAttribute(Type[] types, string[] methodsName)
        {
            if (types.Length != methodsName.Length)
            {
                throw new FriendlyException("Types必须跟methodsName数量一致");
            }
            _types = types;
            _methods = methodsName;
        }
        public CacheRemoveInterceptorAttribute(string cacheKey)
        {
            _cacheKey = cacheKey;
        }
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var cache = context.ServiceProvider.GetService<ICacheHelper>();
            await next(context);
            //缓存删除方式：1、类名和方法名结合删除；2、自定义的缓存key删除 
            if (_types.Length > 0 && _methods.Length > 0)
            {
                for (int i = 0; i < _types.Length; i++)
                {
                    var type = _types[i];
                    var method = _methods[i];
                    string key = $"{type.FullName}:{method}";
                    cache.Remove(key);
                }
            }
            if (!string.IsNullOrEmpty(_cacheKey))
            {
                cache.Remove(_cacheKey);
            }
        }
    }
}