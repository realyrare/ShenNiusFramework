using ShenNius.Share.Models.Entity.Tenant;
using ShenNius.Share.Service.Repository;

namespace ShenNius.Share.Domain.Services.Cms
{
    public interface IMessageService : IBaseServer<Message>
    {

    }
    public class MessageService : BaseServer<Message>, IMessageService
    {
    }
}
