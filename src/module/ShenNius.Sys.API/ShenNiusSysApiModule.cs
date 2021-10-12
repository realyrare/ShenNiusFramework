using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;
using ShenNius.Share.BaseController;

namespace ShenNius.Sys.API
{
    [DependsOn
        (typeof(ShenNiusShareBaseControllerModule)
     )]
    public  class ShenNiusSysApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            //JwtSetting jwtSetting = new JwtSetting();
            //context.Configuration.Bind("JwtSetting", jwtSetting);
           // context.Services.Configure<JwtSetting>(context.Configuration.GetSection("JwtSetting"));
          
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
