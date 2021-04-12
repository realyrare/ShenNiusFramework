/*************************************
* 类名：UserCommand
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/12 19:40:24
*┌───────────────────────────────────┐　    
*│　   版权所有：一起牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

using MediatR;

namespace ShenNius.Share.Infrastructure.CommandHandler.Model
{
    public class UserNotification : INotification
    {
        public int Id { get; set; }
        public string Name{ get; set; }
    }
}