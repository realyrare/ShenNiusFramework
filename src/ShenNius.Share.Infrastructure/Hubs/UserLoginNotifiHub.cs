using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/*************************************
* 类名：UserLoginNotifiHub
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/9/13 13:51:46
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.Hubs
{
    public class UserLoginNotifiHub : Hub
    {
        /*
       1.登陆成功前端调用SaveCurrentUserInfo，将userid，过期时间，signalr连接时间存入字典
       2.根据属性判断是否过期
       3.借助定时服务查询是否过期，过期后推送客户端，前端接收到后弹出登录到期提示
       4.存在的问题，服务端重启后主动推送信息会丢失
       解决方案：过期时间存入浏览器localstrage；轮询增加一个hub连接到客户端，客户端发送给服务端更新。暂不处理
        */
        public static Dictionary<int, CurrentUserHub> currentUsers = new Dictionary<int, CurrentUserHub>();
        public  Task SaveCurrentUserInfo(int  userId,bool isLogin)
        {
            if (currentUsers.ContainsKey(userId))
            {
                //如果是同一个用户且是不同的客户端登录，那么给客户端发送通知（下线）
                if (!currentUsers[userId].ConnectionId.Equals(Context.ConnectionId)&&isLogin==true)
                {
                    //向指定的用户发送
                    return Clients.Client(currentUsers[userId].ConnectionId).SendAsync("ReceiveMessage", userId, isLogin);
                }
                currentUsers[userId].UserId = userId;
                currentUsers[userId].IsLogin = isLogin;
                currentUsers[userId].ConnectionId = Context.ConnectionId;
            }
            else {
                currentUsers.Add(userId, new CurrentUserHub() { UserId = userId, IsLogin = isLogin,ConnectionId= Context.ConnectionId });
            }
            return Task.FromResult(0);
        }
    }
    public class CurrentUserHub
    {
        public int UserId { get; set; }
        public string ConnectionId { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool IsLogin { get; set; }
    }
}