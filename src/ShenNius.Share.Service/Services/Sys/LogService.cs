using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Service.Repository;

namespace ShenNius.Share.Service.Sys
{
    public interface ILogService : IBaseServer<Log>
    {

    }
    public class LogService : BaseServer<Log>, ILogService
    {
    }
}
