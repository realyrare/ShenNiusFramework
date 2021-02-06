using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Service.Repository;

namespace ShenNius.Share.Service.Sys
{
    public interface IRoleService : IBaseServer<Role>
    {

    }
    public class RoleService : BaseServer<Role>, IRoleService
    {
    }
}
