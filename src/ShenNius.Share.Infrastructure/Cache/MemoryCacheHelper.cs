using Microsoft.Extensions.Caching.Memory;
using ShenNius.Share.Infrastructure.Extension;
using System;

/*************************************
* 类名：MemoryCacheHelper
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/25 18:03:11
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Cache
{
    public class MemoryCacheHelper : ICacheHelper
    {
        private readonly IMemoryCache _cache;
        public MemoryCacheHelper(IMemoryCache cache)
        {
            _cache = cache;
        }
        public bool Exists(string key)
        {
            return _cache.TryGetValue(key, out _);
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Set<T>(string key, T value)
        {
            if (value == null)
            {
                throw new FriendlyException("value 为空!");
            }
            _cache.Set(key, value);
        }
        public void Set<T>(string key, T value, TimeSpan timeSpan)
        {
            if (value == null)
            {
                throw new FriendlyException("value 为空!");
            }
            _cache.Set(key, value, timeSpan);
        }
    }
}