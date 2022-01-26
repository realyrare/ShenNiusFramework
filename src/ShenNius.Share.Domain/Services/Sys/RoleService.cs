using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Domain.Repository.Extensions;
using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Models.Configs;
using ShenNius.Share.Models.Dtos.Input.Sys;
using ShenNius.Share.Models.Entity.Sys;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface IRoleService : IBaseServer<Role>
    {
        Task<ApiResult> GetListPagesAsync(int page, int userId);
    }
    public class RoleService : BaseServer<Role>, IRoleService
    {
        public async Task<ApiResult> GetListPagesAsync(int page, int userId)
        {
            var query = await Db.Queryable<Role>().Select(d => new RoleListOutput()
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Status = false
            }).ToPageAsync(page, 15);
            var userRoleList = await Db.Queryable<R_User_Role>().Where(d => d.UserId == userId && d.Status).ToListAsync();
            foreach (var item in query.Items)
            {
                var model = userRoleList.FirstOrDefault(d => d.RoleId == item.Id);
                if (model != null)
                {
                    item.Status = true;
                }
                else {
                    item.Status = false;
                }
            }
            return new ApiResult(data: new { count = query.TotalItems, items = query.Items });
        }
    }
}
