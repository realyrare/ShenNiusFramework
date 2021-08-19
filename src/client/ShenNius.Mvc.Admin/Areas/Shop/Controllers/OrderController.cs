using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Areas.Shop.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
