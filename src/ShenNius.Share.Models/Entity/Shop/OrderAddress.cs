using System;
using SqlSugar;

namespace ShenNius.Share.Models.Entity.Shop
{
    ///<summary>
    ///订单地址表
    ///</summary>
    [SugarTable("shop_order_address")]
    public partial class OrderAddress
    {         
           /// <summary>
           /// Desc:地址id
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id {get;set;}

           /// <summary>
           /// Desc:用户id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int AppUserId {get;set;}

           /// <summary>
           /// Desc:收货人姓名
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:联系电话
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Phone {get;set;}

           /// <summary>
           /// Desc:所在省份id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public string Province {get;set;}

           /// <summary>
           /// Desc:所在城市id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public string City {get;set;}

           /// <summary>
           /// Desc:所在区id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public string Region {get;set;}

           /// <summary>
           /// Desc:详细地址
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Detail {get;set;}

           /// <summary>
           /// Desc:订单id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int OrderId {get;set;}

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
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? ModifyTime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool Status {get;set;}

    }
}
