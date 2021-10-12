using Microsoft.Extensions.DependencyInjection;
using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;

/*************************************
* 类名：ShenNiusShareRabbitMqModule
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/9/30 14:25:49
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.RabbitMq
{
    public class ShenNiusShareRabbitMqModule:AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {           
                context.Services.AddScoped<RabbitMQHelper>();
        }           
    }
}