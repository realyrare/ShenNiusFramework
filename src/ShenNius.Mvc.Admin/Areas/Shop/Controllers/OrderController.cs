using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Shop;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Shop.Controllers
{
    [Area("shop")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var model = await _orderService.GetOrderDetailAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model.Data);
        }
    }
}
