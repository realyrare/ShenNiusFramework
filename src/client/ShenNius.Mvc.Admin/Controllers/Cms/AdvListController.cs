using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Cms;
using ShenNius.Share.Models.Entity.Cms;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Controllers.Cms
{
    public class AdvListController : Controller
    {
        private readonly IAdvListService _advListService;

        public AdvListController(IAdvListService advListService)
        {
            _advListService = advListService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            AdvList model = id == 0 ? new AdvList() : await _advListService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
