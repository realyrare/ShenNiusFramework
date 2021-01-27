using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Service.Repository;

namespace ShenNius.Share.Service.Sys
{
    public interface IUserService : IBaseServer<User>
    {
        
    }
    public partial class UserService : BaseServer<User>, IUserService
    {
    }
}
