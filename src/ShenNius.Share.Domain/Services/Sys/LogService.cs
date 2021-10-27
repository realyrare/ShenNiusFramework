using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Models.Entity.Sys;

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface ILogService : IBaseServer<Log>
    {

    }
    public class LogService : BaseServer<Log>, ILogService
    {
    }
}
