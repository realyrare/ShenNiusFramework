using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Models.Entity.Sys;

/*************************************
* 类名：RecycleService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/8 18:56:27
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface IRecycleService : IBaseServer<Recycle>
    {

    }
    public class RecycleService : BaseServer<Recycle>, IRecycleService
    {
    }   
}