using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Output.Sys;
using System.Threading.Tasks;

namespace ShenNius.Mvc.Admin.Areas.Sys.Controllers
{
    [Area("sys")]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IConfigService _configService;

        private readonly ICurrentUserContext _currentUserContext;

        public MenuController(IMenuService menuService, IConfigService configService, ICurrentUserContext currentUserContext)
        {
            _menuService = menuService;
            _configService = configService;
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
            MenuDetailOutput model = new MenuDetailOutput();
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
            else
            {
                model.MenuOutput = new Menu();
            }
            var configs = await _configService.GetListAsync(d => d.Type == nameof(Button));
            model.ConfigOutputs = configs;
            return View(model);
        }
    }
}
