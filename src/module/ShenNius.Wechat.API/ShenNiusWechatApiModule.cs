using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Senparc.CO2NET;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.MessageHandlers.Middleware;
using Senparc.Weixin.RegisterServices;
using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;
using ShenNius.ModuleCore.Extensions;
using ShenNius.Share.BaseController;

/*************************************
* 类名：ShenNiusWechatApiModule
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/6/8 19:39:08
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Wechat.API
{
    [DependsOn(typeof(ShenNiusShareBaseControllerModule)
    )]
    public class ShenNiusWechatApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMemoryCache()//使用本地缓存必须添加
                    .AddSenparcWeixinServices(context.Configuration);//Senparc.Weixin 注册（必须）           
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = ServiceProviderServiceExtensions.GetRequiredService<IWebHostEnvironment>(context.ServiceProvider);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
          
            // IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting
            //注册 Senparc.Weixin 及基础库
            var senparcSetting = context.Configuration.GetValue<SenparcSetting>("SenparcSetting");
            var senparcWeixinSetting = context.Configuration.GetValue<SenparcWeixinSetting>("SenparcWeixinSetting");

            //注册 Senparc.Weixin 及基础库
            var registerService = app.UseSenparcGlobal(env, senparcSetting, _ => { }, true)
            .UseSenparcWeixin(senparcWeixinSetting, weixinRegister => weixinRegister.RegisterMpAccount(senparcWeixinSetting));
            app.UseRouting();
            //使用中间件注册 MessageHandler，指定 CustomMessageHandler 为自定义处理方法
            app.UseMessageHandlerForMp("/WeixinAsync",
            (stream, postModel, maxRecordCount, serviceProvider) => new CustomMessageHandler(stream, postModel, maxRecordCount, serviceProvider),
                options =>
                {
                    options.AccountSettingFunc = context => senparcWeixinSetting;
                });

        }
    }
}
