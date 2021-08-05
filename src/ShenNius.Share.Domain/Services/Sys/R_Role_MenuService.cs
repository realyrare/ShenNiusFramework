using ShenNius.Share.Infrastructure.Extensions;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShenNius.Share.Models.Configs;

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface IR_Role_MenuService : IBaseServer<R_Role_Menu>
    {
        Task<ApiResult> SetMenuAsync(SetRoleMenuInput setRoleMenuInput);
        Task<ApiResult> SetBtnPermissionsAsync(RoleMenuBtnInput input);
    }
    public class R_Role_MenuService : BaseServer<R_Role_Menu>, IR_Role_MenuService
    {
        public async Task<ApiResult> SetMenuAsync(SetRoleMenuInput setRoleMenuInput)
        {
            var allUserMenus = await GetListAsync(d => d.IsPass);
            // allUserRoles.Where(d => d.UserId == setUserRoleInput.UserId && setUserRoleInput.RoleIds.Contains(d.RoleId));
            List<R_Role_Menu> list = new List<R_Role_Menu>();
            foreach (var item in setRoleMenuInput.MenuIds)
            {
                var model = allUserMenus.Where(d => d.RoleId == setRoleMenuInput.RoleId && d.MenuId == item);
                if (model == null)
                {
                    var r_User_Menu = new R_Role_Menu() { RoleId = setRoleMenuInput.RoleId, MenuId = item, IsPass = true, CreateTime = DateTime.Now };
                    list.Add(r_User_Menu);
                    //add                    
                }
            }
            await AddListAsync(list);
            return new ApiResult();
        }
        public async Task<ApiResult> SetBtnPermissionsAsync(RoleMenuBtnInput input)
        {
            //根据角色和菜单查询内容
       
            var model =await GetModelAsync(d => d.RoleId == input.RoleId && d.MenuId == input.MenuId);
            if (model.Id<=0)
            {
                throw new FriendlyException("您还没有授权当前菜单功能模块");
            }
            if (model.BtnCodeIds!=null)
            {
                //判断授权还是取消
                var list = model.BtnCodeIds.ToList();
                if (input.Status)
                {
                    //不包含则添加。包含放任不管
                    if (!list.Contains(input.BtnCodeId))
                    {                      
                        list.Add(input.BtnCodeId);
                    }
                }
                else
                {
                    //授权 包含则移除
                    if (list.Contains(input.BtnCodeId))
                    {
                        list.Remove(input.BtnCodeId);
                    }
                }
                model.BtnCodeIds =list.ToArray();
            }
            else
            {
                string [] arry= new string[]{ input.BtnCodeId};
                //增加
                model.BtnCodeIds = arry;
            }
          var sign=  await UpdateAsync(d => new R_Role_Menu() { BtnCodeIds = model.BtnCodeIds,ModifyTime=DateTime.Now }, d => d.MenuId == input.MenuId && d.RoleId == input.RoleId);
            return new ApiResult(sign);
          
        }
    }
}
