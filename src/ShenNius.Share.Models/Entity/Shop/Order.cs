using System;
using SqlSugar;

namespace ShenNius.Share.Models.Entity.Shop
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("shop_order")]
    public partial class Order
    {          
           /// <summary>
           /// Desc:订单id
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id {get;set;}

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
           public int PayStatus {get;set;}

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
           public byte DeliveryStatus {get;set;}

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
           /// Desc:小程序id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int TenantId {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreateTime {get;set;}

           /// <summary>
           /// Desc:更新时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime ModifyTime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool Status {get;set;}

    }
}
