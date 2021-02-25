using ShenNius.Share.Model.Entity.Sys;
using ShenNius.Share.Service.Repository;

namespace ShenNius.Share.Service.Sys
{
    public interface IConfigService : IBaseServer<Config>
    {

    }
    public class ConfigService : BaseServer<Config>, IConfigService
    {
    }
}
