﻿using ModuleCore.AppModule.Impl;
using ModuleCore.Attribute;
using ModuleCore.Context;
using ShenNius.Share.Service;

namespace ShenNius.Cms.API
{
    [DependsOn(typeof(ShenNiusShareServiceModule)
      )]
    public class ShenNiusCmsApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            
           

        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
