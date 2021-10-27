using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Domain.Services.Sys;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Input;
using ShenNius.Share.Models.Dtos.Input.Sys;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShenNius.Admin.API.Controllers.Sys
{
    public class RoleController : ApiControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly IR_Role_MenuService _r_Role_MenuService;

        public RoleController(IRoleService roleService, IMapper mapper, IR_Role_MenuService r_Role_MenuService)
        {
            _roleService = roleService;
            _mapper = mapper;
            _r_Role_MenuService = r_Role_MenuService;
        }
        [HttpDelete, Authority(Module = nameof(Role), Method = nameof(Button.Delete))]
        public async Task<ApiResult> Deletes([FromBody] DeletesInput commonDeleteInput)
        {
            return new ApiResult(await _roleService.DeleteAsync(commonDeleteInput.Ids));
        }
        [HttpGet, Authority(Module = nameof(Role))]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            Expression<Func<Role, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(key))
            {
                whereExpression = d => d.Name.Contains(key);
            }
            var res = await _roleService.GetPagesAsync(page, 15, whereExpression, d => d.Id, false);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
        }
        /// <summary>
        /// 根据角色获取当前已授权或未授权的所有角色
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetListPagesByUser(int page, int userId)
        {
            return await _roleService.GetListPagesAsync(page, userId);
        }
        [HttpGet]
        public async Task<ApiResult> Detail(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException(nameof(id));
            }
            var res = await _roleService.GetModelAsync(d => d.Id == id);
            return new ApiResult(data: res);
        }
        [HttpGet]
        public async Task<ApiResult> List()
        {
            var data = await _roleService.GetListAsync();
            return new ApiResult(data: data);
        }

        [HttpPost, Authority(Module = nameof(Role), Method = nameof(Button.Add))]
        public async Task<ApiResult> Add([FromBody] RoleInput roleInput)
        {
            var role = _mapper.Map<Role>(roleInput);
            return new ApiResult(await _roleService.AddAsync(role));
        }

        [HttpPost, Authority(Module = nameof(Role), Method = nameof(Button.Auth))]
        public async Task<ApiResult> SetMenu(SetRoleMenuInput setRoleMenuInput)
        {
            return await _r_Role_MenuService.SetMenuAsync(setRoleMenuInput);
        }

        [HttpPut, Authority(Module = nameof(Role), Method = nameof(Button.Edit))]
        public async Task<ApiResult> Modify([FromBody] RoleModifyInput roleModifyInput)
        {
            return new ApiResult(await _roleService.UpdateAsync(d => new Role()
            {
                Name = roleModifyInput.Name,
                Description = roleModifyInput.Description,
                ModifyTime = roleModifyInput.ModifyTime
            }, d => d.Id == roleModifyInput.Id));
        }
    }
}
