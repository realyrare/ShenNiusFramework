using Microsoft.Extensions.Caching.StackExchangeRedis;
using Newtonsoft.Json;
using ShenNius.Share.Infrastructure.Extension;
using StackExchange.Redis;
using System;

/*************************************
* 类名：RedisCacheHelper
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/25 18:02:04
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Cache
{
    public class RedisCacheHelper : ICacheHelper
    {
        public IDatabase _cache;

        private readonly ConnectionMultiplexer _connection;

        private readonly string _instance;
        public RedisCacheHelper(RedisCacheOptions options, int database = 0)
        {
            _connection = ConnectionMultiplexer.Connect(options.Configuration);
            _cache = _connection.GetDatabase(database);
            _instance = options.InstanceName;
        }
        public bool Exists(string key)
        {
            return _cache.KeyExists(_instance + key);
        }

        public T Get<T>(string key)
        {
            var value= _cache.StringGet(_instance + key);
           return JsonConvert.DeserializeObject<T>(value);
        }

        public void Remove(string key)
        {
            _cache.KeyDelete(_instance + key);
        }

        public void Set<T>(string key, T value)
        {
            if (value == null)
            {
                throw new FriendlyException("value 为空!");
            }
            var values=  JsonConvert.SerializeObject(value);
            _cache.StringSet(_instance + key, values);
        }

        public void Set<T>(string key, T value, TimeSpan timeSpan)
        {
            if (value == null)
            {
                throw new FriendlyException("value 为空!");
            }
            var values = JsonConvert.SerializeObject(value);
            _cache.StringSet(_instance + key, values,timeSpan);
        }
    }
}