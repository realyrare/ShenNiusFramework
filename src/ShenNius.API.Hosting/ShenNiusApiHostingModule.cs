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
using ShenNius.Models.ViewModels.Response;
using ShenNius.ModuleCore.Extensions;
using ShenNius.Swagger;
using System.Linq;
using System.Reflection;

namespace ShenNius.API.Hosting
{
    [DependsOn(
        typeof(ShenNiusSwaggerModule),
        typeof(ShenNiusApiModule)
        )]
    public class ShenNiusApiHostingModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            // 跨域配置
            //context.Services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            //});
            var mvcBuilder = context.Services.AddControllers();
            // 路由配置
            context.Services.AddRouting(options =>
            {
                // 设置URL为小写
                options.LowercaseUrls = true;
                // 在生成的URL后面添加斜杠
                options.AppendTrailingSlash = true;
            });

            // FluentValidation 统一请求参数验证          
            mvcBuilder.AddFluentValidation(fv =>
            {
                var types = Assembly.Load("ShenNius.Models").GetTypes().ToList();
                    // .Where(x => x.GetCustomAttribute(typeof(ValidatorAttribute)) != null);
                    types.ForEach(x => { fv.RegisterValidatorsFromAssemblyContaining(x); });
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
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

                    var result = new ApiResult<string>()
                    {
                        StatusCode = 400,
                        Msg = errors.FirstOrDefault(),
                        Success = false
                    };
                    return new BadRequestObjectResult(result);
                };
            });

        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = ServiceProviderServiceExtensions.GetRequiredService<IWebHostEnvironment>(context.ServiceProvider);
            // 环境变量，开发环境
            if (env.IsDevelopment())
            {
                // 生成异常页面
                //app.UseDeveloperExceptionPage();
            }

            // 使用HSTS的中间件，该中间件添加了严格传输安全头
            app.UseHsts();

            // 转发将标头代理到当前请求，配合 Nginx 使用，获取用户真实IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // 路由
            app.UseRouting();

            // 跨域
            //app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            // 异常处理中间件
            //app.UseMiddleware<ExceptionHandlerMiddleware>();

            // 身份验证
            app.UseAuthentication();

            // 认证授权
            app.UseAuthorization();

            // HTTP => HTTPS
            app.UseHttpsRedirection();

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
