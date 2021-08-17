using ShenNius.ModuleCore;
using ShenNius.Share.Domain;
using ShenNius.Share.Infrastructure;

namespace ShenNius.Shop.API
{
    [DependsOn(typeof(ShenNiusShareDomainModule),
         typeof(ShenNiusShareInfrastructureModule)
      )]
    public class ShenNiusShopApiModule:AppModule
    {
    }
}
