using ShenNius.Share.Models.Entity.Common;
using SqlSugar;

namespace ShenNius.Share.Models.Entity.Shop
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("shop_appuser")]
    public partial class AppUser : BaseTenantEntity
    {
        /// <summary>
        /// Desc:微信openid(唯一标示)
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string OpenId { get; set; }

        /// <summary>
        /// Desc:微信昵称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string NickName { get; set; }

        /// <summary>
        /// Desc:微信头像
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Desc:性别
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public byte Gender { get; set; }

        /// <summary>
        /// Desc:国家
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Country { get; set; }

        /// <summary>
        /// Desc:省份
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Province { get; set; }

        /// <summary>
        /// Desc:城市
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string City { get; set; }

        /// <summary>
        /// Desc:默认收货地址
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int AddressId { get; set; }
    }
}
