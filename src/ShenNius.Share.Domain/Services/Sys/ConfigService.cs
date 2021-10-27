using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Model.Entity.Sys;

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface IConfigService : IBaseServer<Config>
    {

    }
    public class ConfigService : BaseServer<Config>, IConfigService
    {
    }
}
