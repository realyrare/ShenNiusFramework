using ModuleCore.Manage.Interface;
using System;
using Microsoft.Extensions.DependencyInjection;
namespace ModuleCore.Extensions
{
    public static class ModuleIApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用Module
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider UseModule(this IServiceProvider serviceProvider)
        {
            var moduleManager = serviceProvider.GetService<IModuleManager>();
            return moduleManager.ApplicationInitialization(serviceProvider);
        }
    }
}
