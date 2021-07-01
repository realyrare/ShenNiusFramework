using AspectCore.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuleCore.AppModule.Impl;
using ModuleCore.Context;
using ShenNius.ModuleCore.Extensions;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Infrastructure.ImgUpload;

namespace ShenNius.Share.Infrastructure
{
    public class ShenNiusShareInfrastructureModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerSetup();
            context.Services.AddAuthorizationSetup(context.Configuration);

            //注入MiniProfiler
            context.Services.AddMiniProfiler(options =>
                options.RouteBasePath = "/profiler"
           );

            //
            context.Services.ConfigureDynamicProxy(o =>
            {
                //添加AOP的配置
            });

            //redis和cache配置
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

            //七牛云配置信息读取
            context.Services.Configure<QiNiuOssModel>(context.Configuration.GetSection("QiNiuOss"));
            context.Services.AddScoped<QiniuCloud>();

            //注入MediatR
            //https://www.cnblogs.com/sheng-jie/p/10280336.html
            context.Services.AddMediatR(typeof(ShenNiusShareInfrastructureModule));
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            
            app.UseMiniProfiler();
            app.UseSwaggerMiddle();

            NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            NLog.LogManager.Configuration.Variables["connectionString"] = context.Configuration["ConnectionStrings:MySql"];
        }
    }
}
