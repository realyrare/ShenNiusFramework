using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Extension;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Service.Sys;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
            return new ApiResult(await _configService.GetListAsync(d=>d.Type=="按钮"));
        }

        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {
            foreach (var item in commonDeleteInput.Ids)
            {
                await _menuService.UpdateAsync(d=>new Menu() {Status=false },d=>d.Id==item);
            }
            //await _menuService.DeleteAsync(commonDeleteInput.Ids)
            return new ApiResult();
        }

        [HttpGet]
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
            if (model.Id>0)
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
            return await _menuService.AddToUpdateAsync(menuInput);
        }

        [HttpPut]
        public async Task<ApiResult> Modify([FromBody] MenuModifyInput menuModifyInput)
        {
            return await _menuService.ModifyAsync(menuModifyInput);
        }

        [HttpGet]
        public async Task<ApiResult> GetAllParentMenu()
        {
            //var data= await _menuService.GetListAsync(d => d.Status && (d.ParentIdList ==""|| d.ParentIdList==null));
            //  return new ApiResult(_mapper.Map<List<ParentMenuOutput>>(data));
            return await _menuService.GetAllParentMenuAsync();
        }
        /// <summary>
        /// 左侧树形菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> LoadLeftMenuTrees()
        {         
           var userId=Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(d => d.Type == JwtRegisteredClaimNames.Sid).Value);
               return  await _menuService.LoadLeftMenuTreesAsync(userId);
        }
    }
}
