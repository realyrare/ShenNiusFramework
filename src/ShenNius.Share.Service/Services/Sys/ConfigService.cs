using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Domain.Repository;

namespace ShenNius.Share.Domain.Services.Sys
{
    public interface IConfigService : IBaseServer<Config>
    {

    }
    public class ConfigService : BaseServer<Config>, IConfigService
    {
    }
}
