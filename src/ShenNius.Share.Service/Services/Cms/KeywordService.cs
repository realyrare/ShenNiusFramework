using ShenNius.Share.Models.Entity.Cms;
using ShenNius.Share.Service.Repository;

/*************************************
* 类 名： AdvListService
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/16 18:10:18
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Service.Cms
{
    public interface IKeywordService : IBaseServer<Keyword>
    {

    }
    public class KeywordService : BaseServer<Keyword>, IKeywordService
    {
    }
}