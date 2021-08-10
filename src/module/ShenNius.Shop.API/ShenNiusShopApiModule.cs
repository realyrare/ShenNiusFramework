using ShenNius.ModuleCore;
using ShenNius.Share.BaseController;

namespace ShenNius.Shop.API
{
    [DependsOn(typeof(ShenNiusShareBaseControllerModule)
    )]
    public class ShenNiusShopApiModule:AppModule
    {
    }
}
