using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ShenNius.Cms.API;
using ShenNius.ModuleCore.Extensions;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Infrastructure.Hubs;
using ShenNius.Sys.API;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;
using ShenNius.Shop.API;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;
using ShenNius.Share.Infrastructure.Attributes;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using ShenNius.Share.Domain.Repository;
using System;
using AutoMapper;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Domain;
using ShenNius.Share.Infrastructure.Configurations;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using ShenNius.Share.Infrastructure.Caches;
using ShenNius.Share.Infrastructure.FileManager;

namespace ShenNius.Mvc.Admin
{
    //[DependsOn(
    //     typeof(ShenNiusShopApiModule),
    //      typeof(ShenNiusCmsApiModule),
    //      typeof(ShenNiusSysApiModule)
    //      )]
    public class ShenNiusMvcAdminModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {

            string connectionStr = context.Configuration["ConnectionStrings:MySql"];          
            DbContext._connectionStr = connectionStr;
            WebHelper.InjectAssembly(context.Services, "ShenNius.Share.Domain");
            context.Services.AddAutoMapper(typeof(AutomapperProfile));
            context.Services.AddHttpContextAccessor();
            //事务使用AOP 所以注入下。
            context.Services.AddScoped<DbContext>();
            //注入泛型BaseServer
            context.Services.AddScoped(typeof(IBaseServer<>), typeof(BaseServer<>));


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
            var enableLocalFile = Convert.ToBoolean(context.Configuration["LocalFile:IsEnable"]);
            if (enableLocalFile)
            {
                context.Services.AddScoped<IUploadHelper, LocalFile>();
            }
            else
            {
                //七牛云配置信息读取
                context.Services.Configure<QiNiuOss>(context.Configuration.GetSection("QiNiuOss"));
                context.Services.AddScoped<IUploadHelper, QiniuCloud>();
            }


            context.Services.AddDistributedMemoryCache();
            context.Services.AddSession();
            //解决 Error unprotecting the session cookie.
            context.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DataProtection"));
            // 认证
            context.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.Cookie.Name = "ShenNius.Mvc.Admin";
                o.LoginPath = new PathString("/sys/user/login");
                o.Cookie.HttpOnly = true;
            });
            context.Services.AddSignalR();
            var mvcBuilder = context.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
                options.Filters.Add(typeof(LogAttribute));
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });

            mvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        #if DEBUG
                    mvcBuilder.AddRazorRuntimeCompilation();
        #endif

            //把控制器当成服务 进行拦截
            mvcBuilder.AddControllersAsServices();
            // 路由配置
            context.Services.AddRouting(options =>
            {
                // 设置URL为小写
                // options.LowercaseUrls = true;
                // 在生成的URL后面添加斜杠
                options.AppendTrailingSlash = true;
                options.LowercaseQueryStrings = true;
            });
            // FluentValidation 统一请求参数验证          
            mvcBuilder.AddFluentValidation(options =>
            {
                var types = Assembly.Load("ShenNius.Share.Models").GetTypes()
                 .Where(e => e.Name.EndsWith("Validator"));
                foreach (var item in types)
                {
                    options.RegisterValidatorsFromAssemblyContaining(item);
                }
                options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

            // 模型验证自定义返回格式
            context.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState
                        .Values
                        .SelectMany(x => x.Errors
                            .Select(p => p.ErrorMessage))
                        .ToList();
                    // string.Join(",", errors.Select(e => string.Format("{0}", e)).ToList())； 一次性把所有未验证的消息都取出来
                    var result = new ApiResult(
                        msg: errors.FirstOrDefault(),
                        statusCode: 400
                    );
                    return new BadRequestObjectResult(result);
                };
            });

        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = ServiceProviderServiceExtensions.GetRequiredService<IWebHostEnvironment>(context.ServiceProvider);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error.html");
                app.UseHsts();
            }

            if (AppSettings.Jwt.Value)
            {
                app.UseMiniProfiler();
                app.UseSwaggerMiddle();
            }
            //加入健康检查中间件
            app.UseHealthChecks("/health");
            NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            NLog.LogManager.Configuration.Variables["connectionString"] = context.Configuration["ConnectionStrings:MySql"];


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  //避免日志中的中文输出乱码
            // 转发将标头代理到当前请求，配合 Nginx 使用，获取用户真实IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new CustomerFileExtensionContentTypeProvider()
            });
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/error.html");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // 路由映射
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapAreaControllerRoute(
                //name: "sys",
                //areaName: "sys",
                //pattern: "sys/{controller}/{action}/{id?}");

                //endpoints.MapAreaControllerRoute(
                //name: "shop",
                //areaName: "shop",
                //pattern: "shop/{controller}/{action}/{id?}");

                //endpoints.MapAreaControllerRoute(
                //name: "cms",
                //areaName: "cms",
                //pattern: "cms/{controller}/{action}/{id?}");

                endpoints.MapControllerRoute(
                name: "MyArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                //全局路由配置
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=index}/{id?}");             
                endpoints.MapHub<UserLoginNotifiHub>("userLoginNotifiHub");
            });
        }
    }
}
