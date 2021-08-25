using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Models.Entity.Sys;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public class TenantController : Controller
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            this._tenantService = tenantService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        //
        [HttpGet]
        public async Task<IActionResult> Modify(int id = 0)
        {
            Tenant tenant = null;
            if (id == 0)
            {
                tenant = new Tenant();
            }
            else
            {
                tenant = await _tenantService.GetModelAsync(d => d.Id == id && d.IsDel == false);
            }
            return View(tenant);
        }
    }
}
