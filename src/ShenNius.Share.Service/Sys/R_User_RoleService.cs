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
    public interface IR_User_RoleService : IBaseServer<R_User_Role>
    {
        Task<ApiResult> SetRoleAsync(SetUserRoleInput setUserRoleInput);
    }
    public class R_User_RoleService : BaseServer<R_User_Role>, IR_User_RoleService
    {
        public async Task<ApiResult> SetRoleAsync(SetUserRoleInput setUserRoleInput)
        {
            var allUserRoles = await GetListAsync(d => d.IsEnable);
            List<R_User_Role> list = new List<R_User_Role>();
            foreach (var item in setUserRoleInput.RoleIds)
            {
                var model = allUserRoles?.FirstOrDefault(d => d.UserId == setUserRoleInput.UserId && d.RoleId == item);
                if (model == null)
                {
                    var r_User_Role = new R_User_Role() { UserId = setUserRoleInput.UserId, RoleId = item, IsEnable = true, CreateTime = DateTime.Now };
                    //add
                    list.Add(r_User_Role);
                }
            }
            await AddListAsync(list);
            return new ApiResult();
        }
    }
}
