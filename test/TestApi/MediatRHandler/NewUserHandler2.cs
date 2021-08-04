using MediatR;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TestApi.Model;

namespace TestApi.MediatRHandler
{
    public class NewUserHandler2 : INotificationHandler<NewUser2>
    {
        public Task Handle(NewUser2 notification, CancellationToken cancellationToken)
        {
            //Save to log  
            Debug.WriteLine(" ****  Save user in database  *****");
            return Task.FromResult(true);
        }
    }
}
