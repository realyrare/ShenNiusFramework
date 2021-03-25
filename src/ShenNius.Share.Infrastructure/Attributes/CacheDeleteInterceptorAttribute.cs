using AspectCore.DynamicProxy;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
/*************************************
* 类名：CacheDeleteInterceptorAttribute
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/25 19:41:50
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Attributes
{
    public class CacheRemoveInterceptorAttribute : AbstractInterceptorAttribute
    {
        private readonly Type[] _types;
        private readonly string[] _methods;

        /// <summary>
        /// 需传入相同数量的Types跟Methods，同样位置的Type跟Method会组合成一个缓存key，进行删除
        /// </summary>
        /// <param name="Types">传入要删除缓存的类</param>
        /// <param name="Methods">传入要删除缓存的方法名称，必须与Types数组对应</param>
        public CacheRemoveInterceptorAttribute(Type[] Types, string[] Methods)
        {
            if (Types.Length != Methods.Length)
            {
                throw new FriendlyException("Types必须跟Methods数量一致");
            }
            _types = Types;
            _methods = Methods;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var cache = context.ServiceProvider.GetService<ICacheHelper>();
            await next(context);
            for (int i = 0; i < _types.Length; i++)
            {
                var type = _types[i];
                var method = _methods[i];
                string key = $"{type.FullName}:{method}";
                cache.Remove(key);
            }
        }
    }
}