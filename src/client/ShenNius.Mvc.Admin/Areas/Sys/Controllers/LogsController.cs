using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public class LogsController : Controller
    {
        private readonly ILogService _logService;
        public LogsController(ILogService logService)
        {
            _logService = logService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
          var model=await _logService.GetModelAsync(d => d.Id == id);
            return View(model);
        }
        [HttpGet]
        public IActionResult Echarts()
        {
            return View();
        }
    }
}
