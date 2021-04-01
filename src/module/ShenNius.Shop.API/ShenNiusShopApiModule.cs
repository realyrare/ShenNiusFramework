using ModuleCore.AppModule.Impl;
using ModuleCore.Attribute;
using ShenNius.Share.BaseController;

namespace ShenNius.Shop.API
{
    [DependsOn(typeof(ShenNiusShareBaseControllerModule)
    )]
    public class ShenNiusShopApiModule:AppModule
    {
    }
}
