using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Domain.Repository;

/*************************************
* 类名：ColumnService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:15:30
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface ITenantService : IBaseServer<Tenant>
    {

    }
    public class TenantService : BaseServer<Tenant>, ITenantService
    {
    }


}