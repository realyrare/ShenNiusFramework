using System.ComponentModel;

/*************************************
* 类名：GoodsStatusEnum
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/10 19:53:53
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Enums.Shop
{
    public enum GoodsStatusEnum
    {
        /// <summary>
        /// 上架
        /// </summary>
        [Description("上架")]
        PutAway = 10,
        /// <summary>
        /// 下架
        /// </summary>
        [Description("下架")]
        SoldOut = 20,
    }
}