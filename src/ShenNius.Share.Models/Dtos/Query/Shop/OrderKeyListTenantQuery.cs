using ShenNius.Share.Models.Dtos.Common;

/*************************************
* 类名：OrderKeyListTenantQuery
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/23 14:28:04
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Query.Shop
{
    public class OrderKeyListTenantQuery : KeyListTenantQuery
    {
        /// <summary>
        /// Desc:付款状态(10未付款 20已付款)
        /// Default:10
        /// Nullable:False
        /// </summary> 
        public int PayStatus { get; set; }

        /// <summary>
        /// Desc:发货状态(10未发货 20已发货)
        /// Default:10
        /// Nullable:False
        /// </summary>           
        public int DeliveryStatus { get; set; }
        /// <summary>
        /// Desc:收货状态(10未收货 20已收货)
        /// Default:10
        /// Nullable:False
        /// </summary>           
        public int ReceiptStatus { get; set; }
        /// <summary>
        /// Desc:订单状态(10进行中 20取消 21待取消 30已完成)
        /// Default:10
        /// Nullable:False
        /// </summary>           
        public int OrderStatus { get; set; }
    }
}