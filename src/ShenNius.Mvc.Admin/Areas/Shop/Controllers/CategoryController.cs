using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Shop;
using ShenNius.Share.Models.Entity.Shop;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Shop.Controllers
{
    [Area("shop")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            Category model = id == 0 ? new Category() : await _categoryService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
