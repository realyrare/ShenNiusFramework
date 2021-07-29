using ModuleCore.AppModule.Impl;
using ModuleCore.Attribute;
using ShenNius.Cms.API;
using ShenNius.Sys.API;

namespace ShenNius.Mvc.Admin
{
    [DependsOn(
          typeof(ShenNiusCmsApiModule),
          typeof(ShenNiusSysApiModule)
          )]
    public class ShenNiusMvcAdminModule : AppModule
    {
    }
}
