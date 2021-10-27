using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Areas.Shop.Controllers
{
    [Area("shop")]
    public class AppUserController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
