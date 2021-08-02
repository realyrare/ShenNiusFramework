using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Models.Configs;

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
        public IActionResult Modify(int id)
        {
            //if (id >0)
            //{
            //  var model=  await _menuService.GetModelAsync(d => d.Id == id);
            //    var result = await _httpHelper.GetAsync<ApiResult<MenuOutput>>($"menu/detail?id={id}", token);
            //    if (result != null && result.Success && result.StatusCode == 200)
            //    {
            //        if (result.Data != null)
            //        {
            //            MenuOutput = result.Data;
            //            if (result.Data.BtnCodeIds.Length > 0)
            //            {
            //                for (int i = 0; i < result.Data.BtnCodeIds.Length; i++)
            //                {
            //                    AlreadyBtns += result.Data.BtnCodeIds[i] + ",";
            //                }
            //                if (!string.IsNullOrEmpty(AlreadyBtns))
            //                {
            //                    AlreadyBtns = AlreadyBtns.TrimEnd(',');
            //                }
            //            }
            //        }
            //    }
            //}
            //if (!string.IsNullOrEmpty(token))
            //{
            //    var result2 = await _configService.GetListAsync(d => d.Type == nameof(Button));
            //    if (result2 != null && result2.Success && result2.StatusCode == 200)
            //    {
            //        ConfigOutputs = result2.Data;
            //    }
            //}
            return View();
        }
    }
}
