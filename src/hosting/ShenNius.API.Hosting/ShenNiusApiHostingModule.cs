using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModuleCore.AppModule.Impl;
using ModuleCore.Attribute;
using ModuleCore.Context;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Sys.API;
using ShenNius.ModuleCore.Extensions;
using ShenNius.Share.Infrastructure.Extension;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using ShenNius.Cms.API;
using ShenNius.Blog.API;

namespace ShenNius.API.Hosting
{
    [DependsOn(
        
        typeof(ShenNiusCmsApiModule),
        typeof(ShenNiusSysApiModule),
        typeof(ShenNiusBlogApiModule)
        )]
    public class ShenNiusApiHostingModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            // 跨域配置
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            context.Services.AddAuthorizationSetup(context.Configuration);

            var mvcBuilder = context.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
                //全局多租户
                //options.Filters.Add(typeof(MultiTenantAttribute));
                //配置路由以减号分割
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            });


            mvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            //把控制器当成服务 进行拦截
            mvcBuilder.AddControllersAsServices();
            // 路由配置
            context.Services.AddRouting(options =>
            {
                // 设置URL为小写
                options.LowercaseUrls = true;
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
            context.Services.AddSwaggerSetup();
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

                    var result = new ApiResult(
                        msg: string.Join(",", errors.Select(e => string.Format("{0}", e)).ToList()),
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
            NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            NLog.LogManager.Configuration.Variables["connectionString"] = context.Configuration["ConnectionStrings:MySql"];
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  //避免日志中的中文输出乱码
            // 环境变量，开发环境
            if (env.IsDevelopment())
            {
                // 生成异常页面
                app.UseDeveloperExceptionPage();
            }
            // 使用HSTS的中间件，该中间件添加了严格传输安全头
            app.UseHsts();
            // 转发将标头代理到当前请求，配合 Nginx 使用，获取用户真实IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // 跨域
            app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            // 异常处理中间件
            //app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseSwaggerMiddle();
            // HTTP => HTTPS
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            // 路由映射
            app.UseEndpoints(endpoints =>
            {
                //全局路由配置
                endpoints.MapControllerRoute(
                 name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
