using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

namespace ShenNius.Share.Models.Entity.Shop
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("shop_appuser_address")]
    public partial class AppUserAddress : BaseTenantEntity
    {         
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
           /// Desc:用户id
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int AppUserId {get;set;}


    }
}
