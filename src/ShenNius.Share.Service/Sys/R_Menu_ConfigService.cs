using ShenNius.Share.Models.Entity.Sys;
using ShenNius.Share.Service.Repository;

namespace ShenNius.Share.Service.Sys
{
    public interface IR_Menu_ConfigService : IBaseServer<R_Menu_Config>
    {

    }
    public class R_Menu_ConfigService : BaseServer<R_Menu_Config>, IR_Menu_ConfigService
    {
    }
}
