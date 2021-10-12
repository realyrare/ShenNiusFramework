using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.Share.Domain.Repository;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;

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
            InjectAssembly(context.Services, "ShenNius.Share.Domain");
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

        /// <summary>
        /// 自动注册服务——获取程序集中的实现类对应的多个接口
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblyName"></param>
        public void InjectAssembly(IServiceCollection services, string assemblyName)
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType).ToList();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    if (interfaceType.Length == 1)
                    {
                        services.AddTransient(interfaceType[0], item);
                    }
                    if (interfaceType.Length > 1)
                    {
                        services.AddTransient(interfaceType[1], item);
                    }
                }
            }
        }
    }
}
