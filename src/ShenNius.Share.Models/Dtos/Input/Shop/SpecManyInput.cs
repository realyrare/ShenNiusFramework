using Newtonsoft.Json;
using System;

/*************************************
* 类名：SpecMany
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/12 16:07:34
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Models.Dtos.Input.Shop
{
    public class SpecManyInput
    {
        [JsonProperty("spec_attr")]
        public SpecAttrInput[] SpecAttr { get; set; }

        [JsonProperty("spec_list")]
        public SpecListInput[] SpecList { get; set; }
    }

    public class SpecAttrInput
    {
        [JsonProperty("group_id")]
        public int SpecId { get; set; }

        [JsonProperty("group_name")]
        public string SpecName { get; set; }

        [JsonProperty("spec_items")]
        public SpecValueInput[] SpecItems { get; set; }
    }

    public class SpecListInput
    {
        [JsonProperty("spec_sku_id")]
        public string SpecSkuId { get; set; }

        [JsonProperty("rows")]
        public GoodsSpecRelInput[] GoodsSpecRels { get; set; }

        [JsonProperty("form")]
        public GoodsSpecInput GoodsSpec { get; set; }
    }


    public class SpecValueInput
    {
        [JsonProperty("spec_id")]
        public int SpecValueId { get; set; }

        [JsonProperty("spec_value")]
        public string SpecValueName { get; set; }

        [JsonIgnore]
        public int SpecId { get; set; }
        public DateTime ModifyTime { get; set; }
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 存储商品 规格 sku之间关系的id表
    /// </summary>
    public class GoodsSpecRelInput
    {
        public int Id { get; set; }

        public int GoodsId { get; set; }

        [JsonProperty("spec_id")]
        public int SpecId { get; set; }

        [JsonProperty("spec_value")]
        public string SpecValueName { get; set; }

        [JsonProperty("item_id")]
        public int SpecValueId { get; set; }

        public int TenantId { get; set; }
        public DateTime CreateTime { get; set; }
    }


    public class GoodsSpecInput
    {
        public int GoodsSpecId { get; set; }

        public int GoodsId { get; set; }

        [JsonProperty("goods_no")]
        public string GoodsNo { get; set; }

        [JsonProperty("goods_price")]
        public decimal GoodsPrice { get; set; }

        public int GoodsSales { get; set; }

        [JsonProperty("goods_weight")]
        public double GoodsWeight { get; set; }

        [JsonProperty("line_price")]
        public decimal LinePrice { get; set; }

        public string SpecSkuId { get; set; }

        [JsonProperty("stock_num")]
        public int StockNum { get; set; }
        public DateTime CreateTime { get; set; }
        public int TenantId { get; set; }
    }
}