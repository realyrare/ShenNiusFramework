using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Models.Configs;
using System.Threading.Tasks;

namespace ShenNius.MiniApp.API.Controllers
{
    public class OrderController:MiniAppBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderGoodsService _orderGoodsService;

        public OrderController(IOrderService orderService, IOrderGoodsService orderGoodsService)
        {
            _orderService = orderService;
            _orderGoodsService = orderGoodsService;
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpPost("cancel")]
        public  Task<ApiResult> Cancel([FromForm] int orderId, [FromForm] int goodsId)
        {
            return  _orderGoodsService.CancelOrderAsync(orderId, goodsId, HttpWx.TenantId);
        }
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost("receipt")]
        public  Task<ApiResult> Receipt([FromForm] int orderId)
        {
           return   _orderGoodsService.ReceiptAsync(orderId, HttpWx.AppUserId);

        }
    }
}
