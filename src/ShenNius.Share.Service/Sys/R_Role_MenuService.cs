using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Share.Service.Sys
{
    public interface IR_Role_MenuService : IBaseServer<R_Role_Menu>
    {
        Task<ApiResult> SetMenuAsync(SetRoleMenuInput setRoleMenuInput);
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
    }
}
