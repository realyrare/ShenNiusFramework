using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

namespace ShenNius.Share.Models.Entity.Shop
{
    [SugarTable("shop_cart")]
   public class Cart : BaseTenantEntity
    {
        public int GoodsNum { get; set; }
        public int AppUserId { get; set; }
        public int GoodsId { get; set; }
        public string  SpecSkuId { get; set; }
    }
}
