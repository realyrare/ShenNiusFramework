﻿using ShenNius.Share.Models.Dtos.Common;
using System;

namespace ShenNius.Share.Models.Dtos.Input.Shop
{
    public class GoodsInput : GlobalTenantInput
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

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public decimal GoodsPrice { get; set; }
        public decimal LinePrice { get; set; }
        public string GoodsNo { get; set; }
        public int StockNum { get; set; }
        public int GoodsWeight { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public int GoodsStatus { get; set; }
        public string ImgUrl { get; set; }
        /// <summary>
        /// 商品多规格
        /// </summary>
        public string SpecMany { get; set; }
    }
}