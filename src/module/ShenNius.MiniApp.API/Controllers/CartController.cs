using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Entity.Shop;
using System.Threading.Tasks;

namespace ShenNius.MiniApp.API.Controllers
{
    public  class CartController: MiniAppBaseController
    {
        private readonly ICartService _cartService;
        private readonly IGoodsService _goodsService;

        public CartController(ICartService cartService,IGoodsService goodsService)
        {
            _cartService = cartService;
            _goodsService = goodsService;
        }
        /// <summary>
        /// 购物车删除
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task< ApiResult> Delete([FromForm] int goodsId)
        {
            await _cartService.UpdateAsync(d=>new Cart() { Status=false}, d => d.GoodsId == goodsId && d.AppUserId==HttpWx.AppUserId);

            return new ApiResult();
        }
        [HttpPost("add")]
        public  Task<ApiResult> Add([FromForm] int goodsId, [FromForm] int goodsNum, [FromForm] string specSkuId)
        {         
           return _cartService.AddAsync(goodsId,goodsNum,HttpWx.AppUserId, specSkuId);            
        }
        /// <summary>
        /// 减掉商品数量
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpPost("sub")]
        public async Task<ApiResult> Sub([FromForm] int goodsId)
        {
            var isExistCartModel = await _cartService.GetModelAsync(l => l.AppUserId == HttpWx.AppUserId && l.GoodsId == goodsId);

            if (isExistCartModel?.Id != null)
            {
                if (isExistCartModel.GoodsNum < 1)
                {
                    return new ApiResult("该商品在购物车已经不存在了");
                }
                isExistCartModel.GoodsNum -= 1;
                await _cartService.UpdateAsync(d => new Cart() { GoodsNum = isExistCartModel.GoodsNum }, d => d.Id == isExistCartModel.Id&&d.AppUserId==HttpWx.AppUserId);
            }
            return new ApiResult(msg: "删减成功",200);
        }
        [HttpGet("lists")]
        public  Task<ApiResult> Lists(int tenantId)
        {
            return _cartService.GetListsAsync(HttpWx.AppUserId, tenantId);
        }
    }
}
