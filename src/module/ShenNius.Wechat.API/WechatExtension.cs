
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.MessageHandlers.Middleware;
using Senparc.Weixin.RegisterServices;
using System;
using System.Collections.Generic;
using System.Web;

/*************************************
* 类名：WechatExtension
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/10/19 19:17:14
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Wechat.API
{
    public static class WechatExtension
    {
        public static void AddWechatSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache()//使用本地缓存必须添加
                       .AddSenparcWeixinServices(configuration);//Senparc.Weixin 注册（必须）    
        }
        public static void UseWechat(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
        {     
            // IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting
            //注册 Senparc.Weixin 及基础库
            var senparcSetting = configuration.GetValue<SenparcSetting>("SenparcSetting");
            var senparcWeixinSetting = configuration.GetValue<SenparcWeixinSetting>("SenparcWeixinSetting");

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