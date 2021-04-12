using MediatR;

namespace TestApi.Model
{
    /*进程内通信两种模式  request notification*/
    public partial class NewUser : IRequest<bool>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public partial class NewUser2 : INotification
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
