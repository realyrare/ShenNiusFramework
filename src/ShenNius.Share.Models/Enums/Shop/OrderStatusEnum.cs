using System.ComponentModel;

/*************************************
* 类名：OrderStatusEnum
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/10 19:59:38
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Enums.Shop
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatusEnum
    {
        /// <summary>
        /// 新订单
        /// </summary>
        [Description("新订单")]
        NewOrder = 10,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Canceled = 20,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Completed = 30
    }
}