using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuleCore.AppModule.Impl;
using ModuleCore.Context;
using ShenNius.ModuleCore.Extensions;
using ShenNius.Share.Infrastructure.Cache;
using ShenNius.Share.Infrastructure.Configurations;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Infrastructure.ImgUpload;

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
            context.Services.AddCap(x =>
            {
                x.UseRabbitMQ(z =>
                {
                    z.HostName = context.Configuration["RabbitMQ:HostName"];
                    z.UserName = context.Configuration["RabbitMQ:UserName"];
                    z.Port = System.Convert.ToInt32(context.Configuration["RabbitMQ:Port"]);
                    z.Password = context.Configuration["RabbitMQ:Password"];
                });
            });

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

            //七牛云配置信息读取
            context.Services.Configure<QiNiuOssModel>(context.Configuration.GetSection("QiNiuOss"));
            context.Services.AddScoped<QiniuCloud>();

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
