using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestApi.Model;

namespace TestApi.MediatRHandler
{
    public class EmailHandler : INotificationHandler<NewUser2>
    {
        public Task Handle(NewUser2 notification, CancellationToken cancellationToken)
        {
            //Send email  
            Console.WriteLine(" ****  Email sent to user  *****");
            return Task.FromResult(true);
        }
    }
}
