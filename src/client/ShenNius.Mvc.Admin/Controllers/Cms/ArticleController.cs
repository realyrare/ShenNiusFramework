using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Controllers.Cms
{
    public class ArticleController : Controller
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
