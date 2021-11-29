using AspectCore.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
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
using System.Linq;
using System.Reflection;

namespace ShenNius.API.Hosting
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
            //����ʹ��AOP ����ע���¡�
            services.AddScoped<DbContext>();

            //ע�뷺��BaseServer
            services.AddScoped(typeof(IBaseServer<>), typeof(BaseServer<>));

            services.AddSwaggerSetup();
            //ע��MiniProfiler
            services.AddMiniProfiler(options =>
                     options.RouteBasePath = "/profiler"
            );
            services.AddAuthorizationSetup(Configuration);

            //����������
            services.AddHealthChecks();
            services.ConfigureDynamicProxy(o =>
            {
                //���AOP������
            });

            //redis��cache����
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
            //�Ƿ����ñ����ļ��ϴ� ѡ����ע��
            var enableLocalFile = Convert.ToBoolean(Configuration["LocalFile:IsEnable"]);
            if (enableLocalFile)
            {
                services.AddScoped<IUploadHelper, LocalFile>();
            }
            else
            {
                //��ţ��������Ϣ��ȡ
                services.Configure<QiNiuOss>(Configuration.GetSection("QiNiuOss"));
                services.AddScoped<IUploadHelper, QiniuCloud>();
            }


            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSignalR();
            var mvcBuilder = services.AddControllers(options =>
            {
                // options.Filters.Add(new AuthorizeFilter());
                options.Filters.Add(typeof(MvcAndApiAuthorizeFilter));
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


            //�ѿ��������ɷ��� ��������
            mvcBuilder.AddControllersAsServices();
            // ·������
            services.AddRouting(options =>
            {
                // ����URLΪСд
                // options.LowercaseUrls = true;
                // �����ɵ�URL�������б��
                options.AppendTrailingSlash = true;
                options.LowercaseQueryStrings = true;
            });
            // FluentValidation ͳһ���������֤          
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

            // ģ����֤�Զ��巵�ظ�ʽ
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState
                        .Values
                        .SelectMany(x => x.Errors
                            .Select(p => p.ErrorMessage))
                        .ToList();
                    // string.Join(",", errors.Select(e => string.Format("{0}", e)).ToList())�� һ���԰�����δ��֤����Ϣ��ȡ����
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

            app.UseMiniProfiler();
            app.UseSwaggerMiddle();
            //���뽡������м��
            app.UseHealthChecks("/health");
            NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            NLog.LogManager.Configuration.Variables["connectionString"] = Configuration["ConnectionStrings:MySql"];

            // ת������ͷ������ǰ������� Nginx ʹ�ã���ȡ�û���ʵIP
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

            // ·��ӳ��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization(); 
                endpoints.MapHub<UserLoginNotifiHub>("userLoginNotifiHub");
            });
        }
    }
}
