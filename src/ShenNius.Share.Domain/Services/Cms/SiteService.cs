using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Models.Entity.Tenant;
using ShenNius.Share.Domain.Repository;

/*************************************
* 类名：ColumnService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/11 17:15:30
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Cms
{
    public interface ISiteService : IBaseServer<Site>
    {

    }
    public class SiteService : BaseServer<Site>, ISiteService
    {
    }

   
}