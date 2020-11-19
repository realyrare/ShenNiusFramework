using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModuleCore.AppModule.Interface;
using ModuleCore.Manage.Impl;
using ModuleCore.Manage.Interface;

namespace ModuleCore.Extensions
{
    public static class ModuleServiceCollectionExtensions
    {
        /// <summary>
        /// 添加模块服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddModule<T>(this IServiceCollection services, IConfiguration configuration)
            where T : IAppModule
        {
            ModuleManager moduleManager = new ModuleManager();
            moduleManager.StartModule<T>(services);
            moduleManager.ConfigurationService(services, configuration);

            services.TryAddSingleton<IModuleManager>(moduleManager);
            return services;
        }
    }
}
