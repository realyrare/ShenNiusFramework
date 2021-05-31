using ShenNius.Share.Models.Entity.Common;

/*************************************
* 类名：DetailQuery
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/31 18:07:23
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Common
{
    /// <summary>
    /// 详情查询基类（不是多租户的模块可以使用）
    /// </summary>
    public class DetailQuery
    {
        public int Id { get; set; }
    }
    /// <summary>
    /// 多租户查询使用
    /// </summary>
    public class DetailTenantQuery : DetailQuery, IGlobalTenant
    {
        public int TenantId { get; set; }
    }
}