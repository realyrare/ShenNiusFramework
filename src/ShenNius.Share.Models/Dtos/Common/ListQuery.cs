/*************************************
* 类名：ListQuery
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/31 19:13:24
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

using ShenNius.Share.Models.Entity.Common;

namespace ShenNius.Share.Models.Dtos.Common
{
    /// <summary>
    /// 列表查询基类（不是多租户的模块可以使用）
    /// </summary>
    public class ListQuery
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 15;
        public string  Key { get; set; }
    }
    /// <summary>
    /// 多租户列表查询使用
    /// </summary>
    public class ListSiteQuery : ListQuery, IGlobalSite
    {
        public int SiteId { get; set; }
    }

}