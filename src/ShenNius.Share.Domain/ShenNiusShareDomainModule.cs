using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.Share.Domain.Repository;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;
using ShenNius.Share.Infrastructure.Common;

namespace ShenNius.Share.Domain
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
            WebHelper.InjectAssembly(context.Services, "ShenNius.Share.Domain");
            context.Services.AddAutoMapper(typeof(AutomapperProfile));
            context.Services.AddHttpContextAccessor();
            //事务使用AOP 所以注入下。
            context.Services.AddScoped<DbContext>();

            //注入泛型BaseServer
            context.Services.AddScoped(typeof(IBaseServer<>), typeof(BaseServer<>));
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
           
        }

       
    }
}
