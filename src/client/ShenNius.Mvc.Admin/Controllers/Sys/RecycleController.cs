using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Controllers.Sys
{
    public class RecycleController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
