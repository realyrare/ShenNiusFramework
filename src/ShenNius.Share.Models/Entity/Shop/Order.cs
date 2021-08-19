using System;
using ShenNius.Share.Models.Entity.Common;
using ShenNius.Share.Models.Enums.Extension;
using ShenNius.Share.Models.Enums.Shop;
using SqlSugar;

namespace ShenNius.Share.Models.Entity.Shop
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("shop_order")]
    public partial class Order: BaseTenantEntity
    {          

           /// <summary>
           /// Desc:订单号
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string OrderNo {get;set;}

           /// <summary>
           /// Desc:商品总金额
           /// Default:0.00
           /// Nullable:False
           /// </summary>           
           public decimal TotalPrice {get;set;}

           /// <summary>
           /// Desc:订单实付款金额
           /// Default:0.00
           /// Nullable:False
           /// </summary>           
           public decimal PayPrice {get;set;}

        /// <summary>
        /// Desc:付款状态(10未付款 20已付款)
        /// Default:10
        /// Nullable:False
        /// </summary> 
        public int PayStatus { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string PayStatusText { get {
                string name = "";
                if (PayStatus == PayStatusEnum.WaitForPay.GetValue<int>())
                {
                    name = PayStatusEnum.WaitForPay.GetEnumText();
                }
                if (PayStatus == PayStatusEnum.Paid.GetValue<int>())
                {
                    name = PayStatusEnum.Paid.GetEnumText();
                }
                return name;
            } }

           /// <summary>
           /// Desc:付款时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime PayTime {get;set;}

           /// <summary>
           /// Desc:运费金额
           /// Default:0.00
           /// Nullable:False
           /// </summary>           
           public decimal ExpressPrice {get;set;}

           /// <summary>
           /// Desc:物流公司
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ExpressCompany {get;set;}

           /// <summary>
           /// Desc:物流单号
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ExpressNo {get;set;}

           /// <summary>
           /// Desc:发货状态(10未发货 20已发货)
           /// Default:10
           /// Nullable:False
           /// </summary>           
           public int DeliveryStatus {get;set;}
        [SugarColumn(IsIgnore = true)]
        public string DeliveryStatusText
        {
            get
            {
                string name = "";
                if (DeliveryStatus == DeliveryStatusEnum.WaitForSending.GetValue<int>())
                {
                    name = DeliveryStatusEnum.WaitForSending.GetEnumText();
                }
                if (DeliveryStatus == DeliveryStatusEnum.Sended.GetValue<int>())
                {
                    name = DeliveryStatusEnum.Sended.GetEnumText();
                }
                return name;
            }
        }

        /// <summary>
        /// Desc:发货时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime DeliveryTime {get;set;}

           /// <summary>
           /// Desc:收货状态(10未收货 20已收货)
           /// Default:10
           /// Nullable:False
           /// </summary>           
           public int ReceiptStatus {get;set;}
        /// <summary>
        ///   收获时间
        /// </summary>
        public DateTime ReceiptTime { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string ReceiptStatusText
        {
            get
            {
                string name = "";
                if (ReceiptStatus == ReceiptStatusEnum.WaitForReceiving.GetValue<int>())
                {
                    name = ReceiptStatusEnum.WaitForReceiving.GetEnumText();
                }
                if (ReceiptStatus == ReceiptStatusEnum.Received.GetValue<int>())
                {
                    name = ReceiptStatusEnum.Received.GetEnumText();
                }
                return name;
            }
        }

        /// <summary>
        /// Desc:收货时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime ReceipTime {get;set;}

           /// <summary>
           /// Desc:订单状态(10进行中 20取消 21待取消 30已完成)
           /// Default:10
           /// Nullable:False
           /// </summary>           
           public int OrderStatus {get;set;}

        [SugarColumn(IsIgnore = true)]
        public string OrderStatusText
        {
            get
            {
                string name = "";
                if (ReceiptStatus == OrderStatusEnum.NewOrder.GetValue<int>())
                {
                    name = OrderStatusEnum.NewOrder.GetEnumText();
                }
                if (ReceiptStatus == OrderStatusEnum.Canceled.GetValue<int>())
                {
                    name = OrderStatusEnum.Canceled.GetEnumText();
                }
                if (ReceiptStatus == OrderStatusEnum.Completed.GetValue<int>())
                {
                    name = OrderStatusEnum.Completed.GetEnumText();
                }
                return name;
            }
        }

        /// <summary>
        /// Desc:微信支付交易号
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string TransactionId {get;set;}

           /// <summary>
           /// Desc:用户id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int AppUserId {get;set;}

        /// <summary>
        /// 所有商品的总价
        /// </summary>
        public decimal AllTotalPrice { get; set; }
        /// <summary>
        /// 最终实际支付的费用
        /// </summary>
        public decimal AllPayPrice { get; set; }
    }
}
