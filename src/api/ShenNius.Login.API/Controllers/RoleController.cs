﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Infrastructure.Attributes;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Input;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Service.Sys;
using System;
using System.Threading.Tasks;

namespace ShenNius.Sys.API.Controllers
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
        [HttpDelete]
        public async Task<ApiResult> Deletes([FromBody] CommonDeleteInput commonDeleteInput)
        {
            return new ApiResult(await _roleService.DeleteAsync(commonDeleteInput.Ids));
        }
        [HttpGet]
        public async Task<ApiResult> GetListPages(int page, string key = null)
        {
            var res = await _roleService.GetPagesAsync(page, 15);
            return new ApiResult(data: new { count = res.TotalItems, items = res.Items });
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
        [HttpPost]
        public async Task<ApiResult> Add([FromBody] RoleInput roleInput)
        {
            var role = _mapper.Map<Role>(roleInput);
            return new ApiResult(await _roleService.AddAsync(role));
        }
        [HttpPost, Log("设置权限")]
        public async Task<ApiResult> SetMenu(SetRoleMenuInput setRoleMenuInput)
        {
            return await _r_Role_MenuService.SetMenuAsync(setRoleMenuInput);
        }
        [HttpPut]
        public async Task<ApiResult> Modify([FromBody] RoleModifyInput roleModifyInput)
        {
            //var role = _mapper.Map<Role>(roleModifyInput);
            return new ApiResult(await _roleService.UpdateAsync(d => new Role()
            {
                Name = roleModifyInput.Name,
                Description = roleModifyInput.Description,
                ModifyTime = roleModifyInput.ModifyTime
            }, d => d.Id == roleModifyInput.Id));
        }
    }
}
