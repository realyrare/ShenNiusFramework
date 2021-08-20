using ShenNius.Share.Models.Entity.Shop;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Share.Models.Dtos.Output.Shop
{
  public  class OrderDetailOutput:Order
    {
        public string AppUserName { get; set; }
        public AppUserAddress Address { get; set; } = new AppUserAddress();
        public List<OrderGoodsDetailOutput> GoodsDetailList { get; set; } = new List<OrderGoodsDetailOutput>();
    }
    public class OrderGoodsDetailOutput
    {
        public string GoodsImg { get; set; }
        public string GoodsName { get; set; }

        public int GoodsId { get; set; }
        public decimal GoodsPrice { get; set; }
        public double GoodsWeight { get; set; }
        public decimal TotalNum { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreateTime { get; set; }
        public string GoodsNo { get; set; }
    }
}
