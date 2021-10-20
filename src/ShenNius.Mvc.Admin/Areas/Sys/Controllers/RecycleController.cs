using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public class RecycleController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
