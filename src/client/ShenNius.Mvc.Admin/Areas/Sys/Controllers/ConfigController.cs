using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Model.Entity.Sys;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public class ConfigController : Controller
    {
        private readonly IConfigService _configService;

        public ConfigController(IConfigService configService)
        {
            _configService = configService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id)
        {
            var model = id == 0 ? new Config() : await _configService.GetModelAsync(d => d.Id == id && d.Status);
            return View(model);
        }
    }
}
