using System;

namespace ShenNius.Share.Models.Dtos.Input.Shop
{
    public class GoodsModifyInput : GoodsInput
    {
        public int Id { get; set; }

        public DateTime ModifyTime { get; set; } = DateTime.Now;

    }
}
