using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuleCore.AppModule.Interface;
using ModuleCore.Descriptor.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModuleCore.Manage.Interface
{
    public interface IModuleManager : IDisposable
    {
        /// <summary>
        /// 启动模块
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        void StartModule<TModule>(IServiceCollection services)
            where TModule : IAppModule;
        /// <summary>
        /// 模块描述信息
        /// </summary>
        IReadOnlyList<IModuleDescriptor> ModuleDescriptors { get; }



        /// <summary>
        /// 模块排序
        /// </summary>
        /// <typeparam name="TModule">启动模块类型</typeparam>
        /// <returns>排序结果</returns>
        List<IModuleDescriptor> ModuleSort<TModule>()
            where TModule : IAppModule;


        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        IServiceCollection ConfigurationService(IServiceCollection services, IConfiguration configuration);


        /// <summary>
        /// 配置应用初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        IServiceProvider ApplicationInitialization(IServiceProvider serviceProvider);

        /// <summary>
        /// 应用程序停止
        /// </summary>
        void ApplicationShutdown();
    }
}
