using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Model.Entity.Sys;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Sys.Controllers
{
  
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        //
        [HttpGet]
        public async Task<IActionResult> Modify(int id=0)
        {
            Role model = id==0?new Role() : await _roleService.GetModelAsync(d => d.Id == id);
            return View(model);
        }
        [HttpGet]
        public IActionResult SetMenu(int id)
        {
            ViewBag.RoleId = id;
            return View();
        }
    }
}
