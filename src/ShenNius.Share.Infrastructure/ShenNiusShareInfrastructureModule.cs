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
using ShenNius.Share.Infrastructure.Hubs;
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

            context.Services.AddSignalR();
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            //我这个放到了 Mvc 管道下边，注意顺序
            app.UseSignalR(routes =>
            {
                //这里要说下，为啥地址要写 /api/xxx 
                //因为我前后端分离了，而且使用的是代理模式，所以如果你不用/api/xxx的这个规则的话，会出现跨域问题，毕竟这个不是我的controller的路由，而且自己定义的路由
                routes.MapHub<ChatHub>("/api/chatHub");
            });
            app.UseMiniProfiler();
            app.UseSwaggerMiddle();

            NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            NLog.LogManager.Configuration.Variables["connectionString"] = context.Configuration["ConnectionStrings:MySql"];
        }
    }
}
