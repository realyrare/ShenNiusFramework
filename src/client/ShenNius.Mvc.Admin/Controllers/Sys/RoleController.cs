using Microsoft.AspNetCore.Mvc;
namespace ShenNius.Mvc.Admin.Controllers.Sys
{
    public class RoleController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        //
        [HttpGet]
        public IActionResult Modify()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SetMenu()
        {
            return View();
        }
    }
}
