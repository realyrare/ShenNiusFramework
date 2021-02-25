using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Dtos.Output.Sys;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Service.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShenNius.Sys.API.Controllers
{
    public class MenuController : ApiControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;
        private readonly IConfigService _configService;
        private readonly IR_Role_MenuService _r_Role_MenuService;

        public MenuController(IMenuService menuService,IMapper mapper,IConfigService configService, IR_Role_MenuService r_Role_MenuService)
        {
            _menuService = menuService;
            _mapper = mapper;
            _configService = configService;
            _r_Role_MenuService = r_Role_MenuService;
        }
        [HttpGet]
        public async Task<ApiResult> GetBtnCodeList()
        {
            return new ApiResult(await _configService.GetListAsync());
        }
        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {
            return new ApiResult(await _menuService.DeleteAsync(commonDeleteInput.Ids));
        }
        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            var res = await _menuService.GetPagesAsync(page, 15);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }

        /// <summary>
        /// 获取菜单按钮
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> BtnCode(int menuId=0,int roleId=0)
        {
            return await _menuService.BtnCodeByMenuIdAsync(menuId,roleId);
        }
        /// <summary>
        /// 树形菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> TreeByRole(int roleId)
        {
            return await _menuService.TreeRoleIdAsync(roleId);
        }
        /// <summary>
        /// 菜单按钮授权
        /// </summary>
        /// <param name="roleMenuInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> SetBtnPermissions([FromBody]RoleMenuBtnInput roleMenuInput)
        {
            if (roleMenuInput!=null)
            {
                throw new FriendlyException("test");
            }
           
            return await _r_Role_MenuService.SetBtnPermissionsAsync(roleMenuInput);
        }
        /// <summary>
        /// 授权菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> AddPermissions([FromBody]PermissionsInput input)
        {
            var model = await _r_Role_MenuService.GetModelAsync(d => d.RoleId == input.RoleId && d.MenuId == input.MenuId);
            if (model!=null)
            {
                return new ApiResult("已经存在该菜单权限了", 400);
            }
            R_Role_Menu addModel = new R_Role_Menu() { RoleId = input.RoleId, MenuId = input.MenuId };
            var sign= await _r_Role_MenuService.AddAsync(addModel);
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
        [HttpPost]
        public async Task<ApiResult> Add([FromBody] MenuInput menuInput)
        {
            var menu = _mapper.Map<Menu>(menuInput);
            return new ApiResult(await _menuService.AddAsync(menu));
        }

        [HttpPut]
        public async Task<ApiResult> Modify([FromBody] MenuModifyInput menuModifyInput)
        {
            return new ApiResult(await _menuService.UpdateAsync(d => new Menu()
            {
                Name = menuModifyInput.Name,
                Url = menuModifyInput.Url,
                ModifyTime = menuModifyInput.ModifyTime,
                HttpMethod=menuModifyInput.HttpMethod,
                Status=menuModifyInput.Status,
                ParentId=menuModifyInput.ParentId,
                Icon=menuModifyInput.Icon,
                Sort=menuModifyInput.Sort,
                BtnCodeIds= menuModifyInput.BtnCodeIds
            }, d => d.Id == menuModifyInput.Id));
        }
        [HttpGet]
        public async Task<ApiResult> GetAllParentMenu()
        {
          var data= await _menuService.GetListAsync(d => d.Status && d.ParentId == 0);
            return new ApiResult(_mapper.Map<List<ParentMenuOutput>>(data));
        }
    }
}
