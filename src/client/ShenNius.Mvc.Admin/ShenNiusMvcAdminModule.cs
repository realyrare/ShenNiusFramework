using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModuleCore.AppModule.Impl;
using ModuleCore.Attribute;
using ModuleCore.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ShenNius.Cms.API;
using ShenNius.ModuleCore.Extensions;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Infrastructure.Hubs;
using ShenNius.Sys.API;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShenNius.Mvc.Admin
{
    [DependsOn(
          typeof(ShenNiusCmsApiModule),
          typeof(ShenNiusSysApiModule)
          )]
    public class ShenNiusMvcAdminModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddDistributedMemoryCache();
            context.Services.AddSession();
            // 认证
            context.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.Cookie.Name = "ShenNius.Mvc.Admin";
                o.LoginPath = new PathString("/user/login");
                o.Cookie.HttpOnly = true;
            });
            context.Services.AddSignalR();
            var mvcBuilder = context.Services.AddControllersWithViews();


            mvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            mvcBuilder.AddRazorRuntimeCompilation();
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  //避免日志中的中文输出乱码
            // 转发将标头代理到当前请求，配合 Nginx 使用，获取用户真实IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/Error");
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
                //这里要说下，为啥地址要写 /api/xxx 
                //因为我前后端分离了，而且使用的是代理模式，所以如果你不用/api/xxx的这个规则的话，会出现跨域问题，毕竟这个不是我的controller的路由，而且自己定义的路由
                endpoints.MapHub<ChatHub>("/api/chatHub");
            });
        }
    }
}
