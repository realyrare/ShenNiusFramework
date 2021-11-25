using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.Attributes;

namespace ShenNius.Mvc.Admin.Controllers
{
    [LogIgnore]
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
