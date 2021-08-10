using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;
using ShenNius.ModuleCore.Extensions;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ShenNius.Swagger
{
    /// <summary>
    /// 现在没有使用  ，使用的是扩展方法。也可以当成组件来使用
    /// </summary>
    public class ShenNiusSwaggerModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"接口文档——{RuntimeInformation.FrameworkDescription}", Version = "v1",Description= "HTTP API" });
                c.OrderActionsBy(o => o.RelativePath);
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var files = Directory.GetFiles(basePath, "*.xml");
                foreach (var file in files)
                {
                    c.IncludeXmlComments(file, true);                   
                }
                var baseModelPath = AppContext.BaseDirectory;
                var xmlModelPath = Path.Combine(basePath, "ShenNius.Share.Models.xml");//这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);
               c.CustomOperationIds(apiDesc =>
                {
                    return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
                });

                // TODO:一定要返回true！
                c.DocInclusionPredicate((docName, description) =>
                {
                    return true;
                });

                ////https://github.com/domaindrivendev/Swashbuckle.AspNetCore  

                 // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //// 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();  // 很重要！这里配置安全校验，和之前的版本不一样
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",                                     
                });
               // c.AddFluentValidationRules();
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                       new OpenApiSecurityScheme{
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                       },
                       new[] { "readAccess", "writeAccess" }
                    }
                });
            });            
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Example);
                c.DefaultModelsExpandDepth(-1);

                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                c.EnableDeepLinking();
                c.EnableFilter();
                c.MaxDisplayedTags(int.MaxValue);
                c.ShowExtensions();
                c.EnableValidator();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShenNius API v1");
                c.RoutePrefix = string.Empty;
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("ShenNius.API.Hosting.index.html");
            });
        }
    }
}
