using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Models.Entity.Cms;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Cms.Controllers
{
    [Area("cms")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            var model = id == 0 ? new Article() : await _articleService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
