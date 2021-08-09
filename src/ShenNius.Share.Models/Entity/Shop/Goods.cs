using ShenNius.Share.Models.Entity.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类名：Goods
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/9 17:51:01
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Entity.Shop
{
    [SugarTable("shop_Goods")]
    public class Goods : BaseTenantEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        /// <summary>
        /// 商品规格
        /// </summary>
        public int SpecType { get; set; }
        /// <summary>
        /// 库存计算方式
        /// </summary>
        public int DeductStockType { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 初始销量
        /// </summary>
        public int SalesInitial { get; set; }
        /// <summary>
        /// 实际销量
        /// </summary>
        public int SalesActual { get; set; }
        /// <summary>
        /// 配送模板id
        /// </summary>

        public int DeliveryId { get; set; }
        /* status 为商品上架和下架状态，和删除是一样的状态*/ 

    }
}