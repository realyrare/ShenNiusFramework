using System;
using System.Linq;
using System.Text;
using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

namespace ShenNius.Share.Models.Entity.Shop
{
    ///<summary>
    ///订单商品表（不用商品id关联是因为商品会存在更新问题）
    ///</summary>
    [SugarTable("shop_order_goods")]
    public partial class OrderGoods: BaseTenantEntity
    {          
           /// <summary>
           /// Desc:商品id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int GoodsId {get;set;}

           /// <summary>
           /// Desc:商品名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string GoodsName {get;set;}

           /// <summary>
           /// Desc:商品封面图id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int ImgUrl {get;set;}

           /// <summary>
           /// Desc:库存计算方式(10下单减库存 20付款减库存)
           /// Default:20
           /// Nullable:False
           /// </summary>           
           public int DeductStockType {get;set;}

           /// <summary>
           /// Desc:规格类型(10单规格 20多规格)
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int SpecType {get;set;}

           /// <summary>
           /// Desc:商品sku标识
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string SpecSkuId {get;set;}

           /// <summary>
           /// Desc:商品规格id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int GoodsSpecId {get;set;}

           /// <summary>
           /// Desc:商品规格信息
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string GoodsAttr {get;set;}

           /// <summary>
           /// Desc:商品详情
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Content {get;set;}

           /// <summary>
           /// Desc:商品编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string GoodsNo {get;set;}

           /// <summary>
           /// Desc:商品价格
           /// Default:0.00
           /// Nullable:False
           /// </summary>           
           public decimal GoodsPrice {get;set;}

           /// <summary>
           /// Desc:商品划线价
           /// Default:0.00
           /// Nullable:False
           /// </summary>           
           public decimal LinePrice {get;set;}

           /// <summary>
           /// Desc:商品重量(Kg)
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public object GoodsWeight {get;set;}

           /// <summary>
           /// Desc:购买数量
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int TotalNum {get;set;}

           /// <summary>
           /// Desc:商品总价
           /// Default:0.00
           /// Nullable:False
           /// </summary>           
           public decimal TotalPrice {get;set;}

           /// <summary>
           /// Desc:订单id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int OrderId {get;set;}

           /// <summary>
           /// Desc:用户id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int AppUserId {get;set;}

    }
}
