using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;
using ShenNius.Share.BaseController;

namespace ShenNius.Cms.API
{
    [DependsOn(typeof(ShenNiusShareBaseControllerModule)
      )]
    public class ShenNiusCmsApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {

        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
