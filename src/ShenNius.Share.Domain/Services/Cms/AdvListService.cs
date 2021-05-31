using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Models.Entity.Cms;

/*************************************
* 类 名： AdvListService
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/16 18:10:18
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Cms
{
    public interface IAdvListService : IBaseServer<AdvList>
    {
       
    }
    public class AdvListService : BaseServer<AdvList>, IAdvListService
    {
      
    }
}