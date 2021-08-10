using ShenNius.Share.Infrastructure;
using ShenNius.Share.Domain;
using ShenNius.ModuleCore;

/*************************************
* 类名：ShenNiusShareBaseControllerModule
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/1 15:09:04
*┌───────────────────────────────────┐　    
*│　      版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.BaseController
{
    [DependsOn(typeof(ShenNiusShareDomainModule),
       typeof(ShenNiusShareInfrastructureModule)
    )]
    public class ShenNiusShareBaseControllerModule : AppModule
    {
    }
}