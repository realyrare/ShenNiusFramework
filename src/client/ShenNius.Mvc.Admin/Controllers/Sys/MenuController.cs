using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Controllers.Sys
{
    public class MenuController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Modify()
        {
            return View();
        }
    }
}
