using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ModuleCore.AppModule.Impl;
using ModuleCore.Context;
using ShenNius.Share.Infrastructure.Utils;
using ShenNius.Share.Service.Repository;
using System;

namespace ShenNius.Share.Service
{
    public class ShenNiusShareDomainModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
           string connectionStr= context.Configuration["ConnectionStrings:MySql"];
            if (string.IsNullOrEmpty(connectionStr))
            {
                throw new ArgumentException("data connectionStr is not fuond");
            }
            DbContext._connectionStr = connectionStr;
            InjectHelper.AddAssembly(context.Services, "ShenNius.Share.Domain");

            context.Services.AddAutoMapper(typeof(AutomapperProfile));
            context.Services.AddHttpContextAccessor();
            //事务使用AOP 所以注入下。
            context.Services.AddScoped<DbContext>();

            //注入泛型BaseServer
            context.Services.AddTransient(typeof(IBaseServer<>), typeof(BaseServer<>));
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
           
        }
    }
}
