using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;
using ShenNius.ModuleCore.Extensions;
using ShenNius.Share.Infrastructure.Caches;
using ShenNius.Share.Infrastructure.Configurations;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Infrastructure.TimedTask;
using ShenNius.Share.Models.Configs;
using System;

namespace ShenNius.Share.Infrastructure
{
    public class ShenNiusShareInfrastructureModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            if (AppSettings.Jwt.Value)
            {
                context.Services.AddSwaggerSetup();
                //注入MiniProfiler
                context.Services.AddMiniProfiler(options =>
                    options.RouteBasePath = "/profiler"
               );
                context.Services.AddAuthorizationSetup(context.Configuration);
            }
           // context.Services.AddHostedService<TimedBackgroundService>();
            //context.Services.AddCap(x =>
            //{
            //    x.UseRabbitMQ(z =>
            //    {
            //        z.HostName = context.Configuration["RabbitMQ:HostName"];
            //        z.UserName = context.Configuration["RabbitMQ:UserName"];
            //        z.Port = Convert.ToInt32(context.Configuration["RabbitMQ:Port"]);
            //        z.Password = context.Configuration["RabbitMQ:Password"];
            //    });
            //});

            //健康检查服务
            context.Services.AddHealthChecks();
            context.Services.ConfigureDynamicProxy(o =>
            {
                //添加AOP的配置
            });

            //redis和cache配置
            var redisConfiguration = context.Configuration.GetSection("Redis");
            context.Services.Configure<RedisOption>(redisConfiguration);
            RedisOption redisOption = redisConfiguration.Get<RedisOption>();
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
            //是否启用本地文件上传 选择性注入
            var enableLocalFile =Convert.ToBoolean(context.Configuration["LocalFile:IsEnable"]);
            if (enableLocalFile)
            {
                context.Services.AddScoped<IUploadHelper,LocalFile>();
            }
            else {
                //七牛云配置信息读取
                context.Services.Configure<QiNiuOss>(context.Configuration.GetSection("QiNiuOss"));              
                context.Services.AddScoped<IUploadHelper,QiniuCloud>();
            }
           
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            if (AppSettings.Jwt.Value)
            {
                app.UseMiniProfiler();
                app.UseSwaggerMiddle();
            }
            //加入健康检查中间件
            app.UseHealthChecks("/health");
            NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            NLog.LogManager.Configuration.Variables["connectionString"] = context.Configuration["ConnectionStrings:MySql"];
        }
    }
}
