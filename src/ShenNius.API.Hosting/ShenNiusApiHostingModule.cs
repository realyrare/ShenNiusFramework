using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModuleCore.AppModule.Impl;
using ModuleCore.Attribute;
using ModuleCore.Context;
using ShenNius.ModuleCore.Extensions;
using ShenNius.Swagger;

namespace ShenNius.API.Hosting
{
    [DependsOn(
        typeof(ShenNiusApiModule),
        typeof(ShenNiusSwaggerModule)
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
            context.Services.AddControllers();
            // 路由配置
            context.Services.AddRouting(options =>
            {
                // 设置URL为小写
                options.LowercaseUrls = true;
                // 在生成的URL后面添加斜杠
                options.AppendTrailingSlash = true;
            });
           
      
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app =  context.GetApplicationBuilder();          
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
           // app.UseAuthentication();

            // 认证授权
           // app.UseAuthorization();

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
