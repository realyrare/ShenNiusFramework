using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Domain.Repository;

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface ILogService : IBaseServer<Log>
    {

    }
    public class LogService : BaseServer<Log>, ILogService
    {
    }
}
