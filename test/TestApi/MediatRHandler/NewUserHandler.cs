using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TestApi.Model;

namespace TestApi.MediatRHandler
{
    public class NewUserHandler : IRequestHandler<NewUser, bool>
    {
        public Task<bool> Handle(NewUser request, CancellationToken cancellationToken)
        {
            // save to database  
            return Task.FromResult(true);
        }
    }
}
