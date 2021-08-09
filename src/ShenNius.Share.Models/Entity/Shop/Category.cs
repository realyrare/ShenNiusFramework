using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

/*************************************
* 类名：Shop_Category
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/9 16:51:33
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Entity.Shop
{
    [SugarTable("shop_category")]
    public class Category: BaseTenantEntity
    {
        public int ParentId { get; set; }

        public string  IconSrc { get; set; }
        public string  Name { get; set; }

    }
}