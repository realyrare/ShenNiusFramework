using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Areas.Shop.Controllers
{
    public class AppUseraddressController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
