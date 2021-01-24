using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Service.Repository;

namespace ShenNius.Share.Service.Sys
{
    public interface IAdminService : IBaseServer<Admin>
    {
        
    }
    public class AdminService : BaseServer<Admin>, IAdminService
    {
    }
}
