using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;
using System;

/*************************************
* 类名：ShenNiusShareCachesModule
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/9/29 14:11:51
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Caches
{
    public class ShenNiusShareCachesModule: AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            //redis和cache配置
            var redisEnable =Convert.ToBoolean(context.Configuration["Redis:Enable"]);
           var connection= context.Configuration["Redis:Connection"];
            var instanceName = context.Configuration["Redis:InstanceName"];
            var database =Convert.ToInt32(context.Configuration["Redis:Database"]);

            if (!string.IsNullOrEmpty(connection) && redisEnable&&!string.IsNullOrEmpty(instanceName))
            {
                var options = new RedisCacheOptions
                {
                    InstanceName = instanceName,
                    Configuration = connection
                };
                var redis = new RedisCacheHelper(options, database);
                context.Services.AddSingleton(redis);
                context.Services.AddSingleton<ICacheHelper>(redis);
            }
            else
            {
                context.Services.AddMemoryCache();
                context.Services.AddScoped<ICacheHelper, MemoryCacheHelper>();
            }
        }
    }
}