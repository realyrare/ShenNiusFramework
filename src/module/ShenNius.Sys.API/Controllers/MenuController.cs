using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Domain.Services.Sys;
using System.Threading.Tasks;
using ShenNius.Share.Infrastructure.Attributes;

namespace ShenNius.Sys.API.Controllers
{
    public class MenuController : ApiControllerBase
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
        public async Task<ApiResult> GetBtnCodeList()
        {
            return new ApiResult(await _configService.GetListAsync(d => d.Type == nameof(Button)));
        }

        [HttpDelete, Authority(Module = nameof(Menu), Method = nameof(Button.Delete))]
        public async Task<ApiResult> Deletes([FromBody] DeletesInput commonDeleteInput)
        {
            foreach (var item in commonDeleteInput.Ids)
            {
                await _menuService.UpdateAsync(d => new Menu() { Status = false }, d => d.Id == item);
            }
            return new ApiResult();
        }

        [HttpGet, Authority(Module = nameof(Menu))]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            return await _menuService.GetListPagesAsync(page, key);
        }
        /// <summary>
        /// 获取菜单按钮
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> BtnCode(int menuId = 0, int roleId = 0)
        {
            return await _menuService.BtnCodeByMenuIdAsync(menuId, roleId);
        }
        /// <summary>
        /// 树形菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> TreeByRole(int roleId)
        {
            return await _menuService.TreeRoleIdAsync(roleId);
        }
        /// <summary>
        /// 菜单按钮授权
        /// </summary>
        /// <param name="roleMenuInput"></param>
        /// <returns></returns>
        [HttpPost, Authority(Module = nameof(Menu), Method = nameof(Button.Auth))]
        public async Task<ApiResult> SetBtnPermissions([FromBody] RoleMenuBtnInput roleMenuInput)
        {
            return await _r_Role_MenuService.SetBtnPermissionsAsync(roleMenuInput);
        }
        /// <summary>
        /// 授权菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Authority(Module = nameof(Menu), Method = nameof(Button.Auth))]
        public async Task<ApiResult> AddPermissions([FromBody] PermissionsInput input)
        {
            var model = await _r_Role_MenuService.GetModelAsync(d => d.RoleId == input.RoleId && d.MenuId == input.MenuId);
            if (model.Id > 0)
            {
                return new ApiResult("已经存在该菜单权限了", 400);
            }
            R_Role_Menu addModel = new R_Role_Menu() { RoleId = input.RoleId, MenuId = input.MenuId };
            var sign = await _r_Role_MenuService.AddAsync(addModel);
            return new ApiResult(sign);
        }

        [HttpGet]
        public async Task<ApiResult> Detail(int id)
        {
            if (id == 0)
            {
                throw new FriendlyException(nameof(id));
            }
            var res = await _menuService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }
        [HttpPost, Authority(Module = nameof(Menu), Method = nameof(Button.Add))]
        public async Task<ApiResult> Add([FromBody] MenuInput menuInput)
        {
            return await _menuService.AddToUpdateAsync(menuInput);
        }

        [HttpPut, Authority(Module = nameof(Menu), Method = nameof(Button.Edit))]
        public async Task<ApiResult> Modify([FromBody] MenuModifyInput menuModifyInput)
        {
            return await _menuService.ModifyAsync(menuModifyInput);
        }

        [HttpGet]
        public async Task<ApiResult> GetAllParentMenu()
        {
            return await _menuService.GetAllParentMenuAsync();
        }
        /// <summary>
        /// 左侧树形菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> LoadLeftMenuTrees()
        {

            var userId = _currentUserContext.Id;
            return await _menuService.LoadLeftMenuTreesAsync(userId);
        }
    }
}
