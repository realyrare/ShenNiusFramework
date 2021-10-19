using ShenNius.Share.Infrastructure;
using ShenNius.Share.Domain;
using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;

namespace ShenNius.Cms.API
{
    [DependsOn(typeof(ShenNiusShareDomainModule),
         typeof(ShenNiusShareInfrastructureModule)
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
