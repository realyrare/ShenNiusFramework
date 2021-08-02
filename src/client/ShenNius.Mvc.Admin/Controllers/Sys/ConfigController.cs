using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Controllers.Sys
{
    public class ConfigController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Modify()
        {
            return View();
        }
    }
}
