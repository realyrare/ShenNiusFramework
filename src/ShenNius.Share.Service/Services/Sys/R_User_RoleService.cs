using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Domain.Repository;
using System;
using System.Threading.Tasks;

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface IR_User_RoleService : IBaseServer<R_User_Role>
    {
        Task<ApiResult> SetRoleAsync(SetUserRoleInput setUserRoleInput);

    }
    public class R_User_RoleService : BaseServer<R_User_Role>, IR_User_RoleService
    {
        public async Task<ApiResult> SetRoleAsync(SetUserRoleInput input)
        {           
            //分配角色
            if (input.Status)
            {
                var model = await GetModelAsync(d => d.UserId == input.UserId && d.RoleId == input.RoleId&&d.Status);
                if (model.Id>0)
                {
                    return new ApiResult("已经存在该角色了", 500);
                }
                R_User_Role addModel = new R_User_Role() {UserId=input.UserId,RoleId=input.RoleId,CreateTime=DateTime.Now,Status=true };
                await AddAsync(addModel);
            }
            else {
                await UpdateAsync(d => new R_User_Role() { Status = false }, d => d.UserId == input.UserId && d.RoleId == input.RoleId);
               // await DeleteAsync(d => d.UserId == input.UserId && d.RoleId == input.RoleId);
               //删除的话 要把授权的权限都要删除掉 风险比较高。
            }
            return new ApiResult();
        }
    }
}
