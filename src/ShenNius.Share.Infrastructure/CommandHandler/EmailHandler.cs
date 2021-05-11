using MediatR;
using ShenNius.Share.Infrastructure.CommandHandler.Model;
using ShenNius.Share.Infrastructure.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

/*************************************
* 类名：EmailHandler
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/4/12 19:26:52
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.CommandHandler
{
    public class EmailHandler : INotificationHandler<UserNotification>
    {
        public Task Handle(UserNotification notification, CancellationToken cancellationToken)
        {
            string content = $"当前名为{notification.Name}的用户在{DateTime.Now}成功登录神牛系统";
            //Send email  
            EmailHelper.Send("神牛系统用户登录", content, notification.Name, notification.Email);
            return Task.FromResult(true);
        }
    }
}