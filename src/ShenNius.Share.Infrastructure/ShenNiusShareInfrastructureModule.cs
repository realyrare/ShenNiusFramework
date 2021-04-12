﻿using AspectCore.Extensions.DependencyInjection;
using MediatR;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuleCore.AppModule.Impl;
using ModuleCore.Context;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Infrastructure.ImgUpload;

namespace ShenNius.Share.Infrastructure
{
    public class ShenNiusShareInfrastructureModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.ConfigureDynamicProxy(o => {
                //添加AOP的配置
            });
            var RedisConfiguration = context.Configuration.GetSection("Redis");
            context.Services.Configure<RedisOption>(RedisConfiguration);
            RedisOption redisOption = RedisConfiguration.Get<RedisOption>();
            if (redisOption != null && redisOption.Enable)
            {
                var options = new RedisCacheOptions
                {
                    InstanceName = redisOption.InstanceName,
                    Configuration = redisOption.Connection
                };
                var redis = new RedisCacheHelper(options, redisOption.Database);
                context.Services.AddSingleton(redis);
                context.Services.AddSingleton<ICacheHelper>(redis);
            }
            else
            {
                context.Services.AddMemoryCache();
                context.Services.AddScoped<ICacheHelper, MemoryCacheHelper>();
            }
            context.Services.Configure<QiNiuOssModel>(context.Configuration.GetSection("QiNiuOss"));
            context.Services.AddScoped<QiniuCloud>();
            //注入MediatR
            //https://www.cnblogs.com/sheng-jie/p/10280336.html
            context.Services.AddMediatR(typeof(ShenNiusShareInfrastructureModule));
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }
    }
}
