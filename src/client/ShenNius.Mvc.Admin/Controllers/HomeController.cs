using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Controllers
{
    public class HomeController : Controller
    {
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
