using Microsoft.Extensions.DependencyInjection;
using ModuleCore.AppModule.Impl;
using ModuleCore.Attribute;
using ModuleCore.Context;
using ShenNius.Share.Infrastructure;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Infrastructure.ImgUpload;
using ShenNius.Share.Service;

namespace ShenNius.Cms.API
{
    [DependsOn(typeof(ShenNiusShareServiceModule),
         typeof(ShenNiusShareInfrastructureModule)
      )]
    public class ShenNiusCmsApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<QiNiuOssModel>(context.Configuration.GetSection("QiNiuOss"));
            context.Services.AddScoped<QiniuCloud>();
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
