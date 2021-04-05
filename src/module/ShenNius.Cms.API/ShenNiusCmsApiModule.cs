using ModuleCore.AppModule.Impl;
using ModuleCore.Attribute;
using ModuleCore.Context;
using ShenNius.Share.Infrastructure;
using ShenNius.Share.Domain;

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
