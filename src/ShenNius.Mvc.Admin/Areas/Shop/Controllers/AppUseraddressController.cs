using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Areas.Shop.Controllers
{
    [Area("shop")]
    public class AppUseraddressController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
