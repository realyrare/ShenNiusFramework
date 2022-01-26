using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Controllers
{

    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("error.html")]
        public IActionResult Error()
        {
            return View();
        }
        [HttpGet("no-control.html")]
        public IActionResult NoControl()
        {
            return View();
        }
    }
}
