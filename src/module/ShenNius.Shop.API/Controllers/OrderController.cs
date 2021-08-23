using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.BaseController.Controllers;
using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Infrastructure.FileManager;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Common;
using ShenNius.Share.Models.Dtos.Input.Shop;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Dtos.Query.Shop;
using ShenNius.Share.Models.Entity.Shop;
using System.Threading.Tasks;

namespace ShenNius.Shop.API.Controllers
{
    /// <summary>
    /// 商品订单控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [MultiTenant]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _OrderService;
        public OrderController(IOrderService  OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpGet]
        public   Task<ApiResult> GetListPages(OrderKeyListTenantQuery query)
        {
            return  _OrderService.GetListPageAsync(query);
        }
        
    }
}
