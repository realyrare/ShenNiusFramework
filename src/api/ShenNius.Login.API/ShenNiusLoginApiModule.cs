using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuleCore.AppModule.Impl;
using ModuleCore.Context;
using ShenNius.Share.Infrastructure.JsonWebToken.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Login.API
{
  public  class ShenNiusLoginApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            //JwtSetting jwtSetting = new JwtSetting();
            //context.Configuration.Bind("JwtSetting", jwtSetting);
            context.Services.Configure<JwtSetting>(context.Configuration.GetSection("JwtSetting"));
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
