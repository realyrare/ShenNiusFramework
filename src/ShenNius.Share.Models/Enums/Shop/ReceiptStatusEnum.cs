using System.ComponentModel;

/*************************************
* 类名：SpecTypeEnum
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/10 19:38:04
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Enums.Shop
{
    public enum ReceiptStatusEnum
    {
        /// <summary>
        /// 待收货
        /// </summary>
        [Description("待收货")]
        WaitForReceiving = 10,
        /// <summary>
        ///已收货
        /// </summary>
        [Description("已收货")]
        Received = 20
    }

    public enum DeliveryStatusEnum
    {/// <summary>
     /// 待发货
     /// </summary>
        [Description("待发货")]
        WaitForSending = 10,
        /// <summary>
        /// 已发货
        /// </summary>
        [Description("已发货")]
        Sended = 20
    }
}