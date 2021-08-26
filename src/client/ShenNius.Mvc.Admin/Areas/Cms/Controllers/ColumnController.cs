using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Models.Entity.Cms;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Cms.Controllers
{
    [Area("cms")]
    public class ColumnController : Controller
    {
        private readonly IColumnService _columnService;

        public ColumnController(IColumnService columnService)
        {
            _columnService = columnService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            Column model = id == 0 ? new Column() : await _columnService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
