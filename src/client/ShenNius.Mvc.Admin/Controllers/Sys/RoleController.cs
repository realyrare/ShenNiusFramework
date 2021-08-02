using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Model.Entity.Sys;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Controllers.Sys
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IR_Role_MenuService _r_Role_MenuService;

        public RoleController(IRoleService roleService, IR_Role_MenuService r_Role_MenuService)
        {
            _roleService = roleService;

            _r_Role_MenuService = r_Role_MenuService;
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
        public IActionResult SetMenu()
        {
            return View();
        }
    }
}
