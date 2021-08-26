using Microsoft.AspNetCore.Mvc;

namespace ShenNius.Mvc.Admin.Areas.Cms.Controllers
{
    [Area("cms")]
    public class MessageController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
