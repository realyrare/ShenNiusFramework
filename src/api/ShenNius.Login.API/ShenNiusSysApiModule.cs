using Microsoft.Extensions.DependencyInjection;
using ModuleCore.AppModule.Impl;
using ModuleCore.Attribute;
using ModuleCore.Context;
using ShenNius.Share.Infrastructure.JsonWebToken.Model;
using ShenNius.Share.Service;

namespace ShenNius.Sys.API
{
    [DependsOn(typeof(ShenNiusShareServiceModule)
       )]
    public  class ShenNiusSysApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            //JwtSetting jwtSetting = new JwtSetting();
            //context.Configuration.Bind("JwtSetting", jwtSetting);
            context.Services.Configure<JwtSetting>(context.Configuration.GetSection("JwtSetting"));
            context.Services.AddMemoryCache();
            context.Services.AddHttpContextAccessor();
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
