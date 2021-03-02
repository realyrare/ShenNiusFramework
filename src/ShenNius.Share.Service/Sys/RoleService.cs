using ShenNius.Share.Infrastructure.ApiResponse;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Dtos.Output.Sys;
using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Service.Repository;
using ShenNius.Share.Service.Repository.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Share.Service.Sys
{
    public interface IRoleService : IBaseServer<Role>
    {
        Task<ApiResult> GetListPagesAsync(int page,int userId);
    }
    public class RoleService : BaseServer<Role>, IRoleService
    {
        public async Task<ApiResult> GetListPagesAsync(int page, int userId)
        {
          var query= await Db.Queryable<Role>().Select(d => new RoleListOutput() { 
            Id=d.Id,
            Name=d.Name,
            Description=d.Description,
            Status=false
            }).ToPageAsync(page,15);
            var userRoleList = await Db.Queryable<R_User_Role>().Where(d => d.UserId == userId&&d.IsEnable).ToListAsync();
            foreach (var item in query.Items)
            {
               var model= userRoleList.FirstOrDefault(d => d.RoleId == item.Id);
                if (model!=null)
                {
                    item.Status = true;
                }
            }
            return new ApiResult(data: new { count = query.TotalItems, items = query.Items });
        }
    }
}
