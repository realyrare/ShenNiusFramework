using ShenNius.Share.Domain.Repository;
using ShenNius.Share.Models.Entity.Cms;

namespace ShenNius.Share.Domain.Services.Cms
{
    public interface IMessageService : IBaseServer<Message>
    {

    }
    public class MessageService : BaseServer<Message>, IMessageService
    {
    }
}
