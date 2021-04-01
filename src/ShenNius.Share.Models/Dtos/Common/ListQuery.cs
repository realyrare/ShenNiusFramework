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
    public class PageQuery
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 15;
    }
    /// <summary>
    /// 多租户列表查询使用
    /// </summary>
    public class ListSiteQuery : PageQuery, IGlobalSite
    {
        public int SiteId { get; set; }
    }
    /// <summary>
    /// 通用查询 ， 适合只有一个关键词查询的列表
    /// </summary>
    public class KeyListSiteQuery : ListSiteQuery, IGlobalSite
    {
        public string Key { get; set; }
    }
    /// <summary>
    /// 适用于不是多租户的
    /// </summary>
    public class KeyListQuery : PageQuery
    {
        public string Key { get; set; }
    }

}