using Newtonsoft.Json;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Entity.Shop;
using System;
using System.Collections.Generic;

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

        /// <summary>
        /// 商品状态
        /// </summary>
        public int GoodsStatus { get; set; }
        public string ImgUrl { get; set; }
        /// <summary>
        /// 商品多规格
        /// </summary>
        public string SpecMany { get; set; }
        /// <summary>
        /// 单规格
        /// </summary>
        public string SpecSingle { get; set; }
        public GoodsSpecInput GoodsSpecInput { get; set; }

        /// <summary>
        /// 但规格构建实体数据
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public GoodsSpec BuildGoodsSpec(int goodsId)
        {
            GoodsSpecInput.CreateTime = CreateTime;
            GoodsSpecInput.TenantId = TenantId;
            GoodsSpecInput.GoodsId = goodsId;
            GoodsSpecInput.SpecSkuId = null;
            return CreateGoodsSpecEntity(GoodsSpecInput);
        }

        private GoodsSpec CreateGoodsSpecEntity(GoodsSpecInput input)
        {
            GoodsSpec goodsSpec = new GoodsSpec()
            {
                CreateTime = input.CreateTime,
                TenantId = input.TenantId,
                GoodsId = input.GoodsId,
                SpecSkuId = input.SpecSkuId,
                GoodsPrice = input.GoodsPrice,
                LinePrice = input.LinePrice,
                GoodsNo = input.GoodsNo,
                StockNum = input.StockNum,
                GoodsWeight = input.GoodsWeight
            };
            return goodsSpec;
        }
        /// <summary>
        /// 多规格构建实体数据
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public List<GoodsSpec> BuildGoodsSpecs(int goodsId)
        {
            var list = new List<GoodsSpec>();
            var specMany = JsonConvert.DeserializeObject<SpecManyInput>(SpecMany);
            foreach (var specList in specMany.SpecList)
            {
                specList.GoodsSpec.SpecSkuId = specList.SpecSkuId;
                specList.GoodsSpec.GoodsId = goodsId;
                specList.GoodsSpec.CreateTime = CreateTime;
                specList.GoodsSpec.TenantId = TenantId;
                var goodsSpec = CreateGoodsSpecEntity(specList.GoodsSpec);
                list.Add(goodsSpec);
            }
            return list;
        }
        /// <summary>
        /// 构建商品多规格关系实体
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public List<GoodsSpecRel> BuildGoodsSpecRels(int goodsId)
        {
            var list = new List<GoodsSpecRel>();
            var specMany = JsonConvert.DeserializeObject<SpecManyInput>(SpecMany);
            foreach (var specList in specMany.SpecList)
            {
                foreach (var goodsSpecRelDto in specList.GoodsSpecRels)
                {
                    goodsSpecRelDto.GoodsId = goodsId;
                    goodsSpecRelDto.CreateTime = CreateTime;
                    goodsSpecRelDto.TenantId = TenantId;
                    GoodsSpecRel model = new GoodsSpecRel()
                    {
                        GoodsId = goodsSpecRelDto.GoodsId,
                        CreateTime = goodsSpecRelDto.CreateTime,
                        TenantId = goodsSpecRelDto.TenantId,
                        SpecId = goodsSpecRelDto.SpecId,
                        SpecValueId = goodsSpecRelDto.SpecValueId
                    };
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
