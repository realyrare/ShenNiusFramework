using AspectCore.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ShenNius.Share.Domain;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Infrastructure.Caches;
using ShenNius.Share.Infrastructure.Common;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Infrastructure.Hubs;
using ShenNius.Share.Models.Configs;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShenNius.Mvc.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionStr = Configuration["ConnectionStrings:MySql"];
            DbContext._connectionStr = connectionStr;
            WebHelper.InjectAssembly(services, "ShenNius.Share.Domain");
            services.AddAutoMapper(typeof(AutomapperProfile));
            services.AddHttpContextAccessor();
            //事务使用AOP 所以注入下。
            services.AddScoped<DbContext>();

            //注入泛型BaseServer
            services.AddScoped(typeof(IBaseServer<>), typeof(BaseServer<>));

            //健康检查服务
            services.AddHealthChecks();
            services.ConfigureDynamicProxy(o =>
             {
                 //添加AOP的配置
             });

            //redis和cache配置
            var redisConfiguration = Configuration.GetSection("Redis");
            services.Configure<RedisOption>(redisConfiguration);
            RedisOption redisOption = redisConfiguration.Get<RedisOption>();
            if (redisOption != null && redisOption.Enable)
            {
                var options = new RedisCacheOptions
                {
                    InstanceName = redisOption.InstanceName,
                    Configuration = redisOption.Connection
                };
                var redis = new RedisCacheHelper(options, redisOption.Database);
                services.AddSingleton(redis);
                services.AddSingleton<ICacheHelper>(redis);
            }
            else
            {
                services.AddMemoryCache();
                services.AddScoped<ICacheHelper, MemoryCacheHelper>();
            }

            //是否启用本地文件上传 选择性注入
            var enableLocalFile = Convert.ToBoolean(Configuration["LocalFile:IsEnable"]);
            if (enableLocalFile)
            {
                services.AddScoped<IUploadHelper, LocalFile>();
            }
            else
            {
                //七牛云配置信息读取
                services.Configure<QiNiuOss>(Configuration.GetSection("QiNiuOss"));
                services.AddScoped<IUploadHelper, QiniuCloud>();
            }


            services.AddDistributedMemoryCache();
            services.AddSession();
            //解决 Error unprotecting the session cookie.
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DataProtection"));
            // 认证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
             {
                 o.Cookie.Name = "ShenNius.Mvc.Admin";
                 o.LoginPath = new PathString("/sys/user/login");
                 o.Cookie.HttpOnly = true;
                 o.AccessDeniedPath = new PathString("/no-control.html");//没权限跳到这个路径
             });
            services.AddSignalR();
            var mvcBuilder = services.AddControllersWithViews(options =>
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
            services.AddRouting(options =>
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
            services.Configure<ApiBehaviorOptions>(options =>
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error.html");
                app.UseHsts();
            }
            //加入健康检查中间件
            app.UseHealthChecks("/health");
            NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            NLog.LogManager.Configuration.Variables["connectionString"] = Configuration["ConnectionStrings:MySql"];

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
