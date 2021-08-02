using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("home.html")]
        public IActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                Redirect("/user/login");
            }
            
            return View();
        }

    }
}
