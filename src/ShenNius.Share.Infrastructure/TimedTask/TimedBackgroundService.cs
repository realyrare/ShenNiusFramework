using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShenNius.Share.Infrastructure.Hubs;
using System;
using System.Threading;
using System.Threading.Tasks;

/*************************************
* 类名：TimedBackgroundService
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/10/8 15:02:53
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.TimedTask
{
    public class TimedBackgroundService : BackgroundService
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IHubContext<UserLoginNotifiHub> _hubContext;
        public TimedBackgroundService(ILogger<TimedBackgroundService> logger, IHubContext<UserLoginNotifiHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {

            //给指定人推送消息	
            //  _hubContext.Clients.All.SendAsync("ReceiveMessage",  1);

            // _logger.LogInformation($"Hello World! - {DateTime.Now}");
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }
    }
}