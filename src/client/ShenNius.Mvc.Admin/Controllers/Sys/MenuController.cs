using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Output.Sys;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Controllers.Sys
{
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IConfigService _configService;
        private readonly IR_Role_MenuService _r_Role_MenuService;
        private readonly ICurrentUserContext _currentUserContext;

        public MenuController(IMenuService menuService, IConfigService configService, IR_Role_MenuService r_Role_MenuService, ICurrentUserContext currentUserContext)
        {
            _menuService = menuService;
            _configService = configService;
            _r_Role_MenuService = r_Role_MenuService;
            this._currentUserContext = currentUserContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Modify(int id)
        {
            MenuDetailOutput model = null;
            string alreadyBtns = string.Empty;
            if (id > 0)
            {
                model.MenuOutput = await _menuService.GetModelAsync(d => d.Id == id);
                if (model.MenuOutput != null)
                {
                    if (model.MenuOutput.BtnCodeIds.Length > 0)
                    {
                        for (int i = 0; i < model.MenuOutput.BtnCodeIds.Length; i++)
                        {
                            alreadyBtns += model.MenuOutput.BtnCodeIds[i] + ",";
                        }
                        if (!string.IsNullOrEmpty(alreadyBtns))
                        {
                            alreadyBtns = alreadyBtns.TrimEnd(',');
                        }
                    }
                }
                ViewBag.AlreadyBtns = alreadyBtns;
            }
            else {
                model.MenuOutput = new Menu();
            }
            var configs = await _configService.GetListAsync(d => d.Type == nameof(Button));
            
            return View(model);
        }
    }
}
