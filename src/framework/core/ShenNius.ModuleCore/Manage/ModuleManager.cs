using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.ModuleCore;
using ShenNius.ModuleCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ModuleCore.Manage
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

    public class ModuleManager : IModuleManager
    {
        /// <summary>
        /// 模块接口类型全名称
        /// </summary>
        public static string _moduleInterfaceTypeFullName = typeof(IAppModule).FullName;

        /// <summary>
        /// 模块明细和实例
        /// </summary>
        public virtual IReadOnlyList<IModuleDescriptor> ModuleDescriptors { get; protected set; }

        /// <summary>
        /// ioc容器
        /// </summary>
        public virtual IServiceProvider ServiceProvider { get; protected set; }

        /// <inheritdoc/>
        public virtual void StartModule<T>(IServiceCollection services)
           where T : IAppModule
        {
            var moduleDescriptors = new List<IModuleDescriptor>();
            var moduleDescriptorList = this.ModuleSort<T>();
            foreach (var item in moduleDescriptorList)
            {
                if (moduleDescriptors.Any(o => o.ModuleType.FullName == item.ModuleType.FullName))
                {
                    continue;
                }

                moduleDescriptors.Add(item);
                services.AddSingleton(item.ModuleType, item.Instance);
            }

            ModuleDescriptors = moduleDescriptors.AsReadOnly();
        }

        /// <inheritdoc/>
        public virtual IServiceCollection ConfigurationService(IServiceCollection services, IConfiguration configuration)
        {
            var context = new ServiceConfigurationContext(services, configuration);


            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnPreConfigureServices(context);
            }

            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnConfigureServices(context);
            }

            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnPostConfigureServices(context);
            }


            services.AddSingleton(context);

            return services;
        }

        /// <inheritdoc/>
        public virtual IServiceProvider ApplicationInitialization(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var context = new ApplicationInitializationContext(serviceProvider, configuration);


            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnPreApplicationInitialization(context);
            }

            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnApplicationInitialization(context);
            }

            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnPostApplicationInitialization(context);
            }


            this.ServiceProvider = serviceProvider;
            return serviceProvider;
        }


        /// <inheritdoc/>
        public virtual void ApplicationShutdown()
        {
            var context = new ApplicationShutdownContext(this.ServiceProvider);

            var modules = ModuleDescriptors.Reverse().ToList();

            foreach (var module in modules)
            {
                (module.Instance as IAppModule)?.OnApplicationShutdown(context);
            }
        }

        #region 模块排序

        /// <inheritdoc/>
        public virtual List<IModuleDescriptor> ModuleSort<TModule>()
        where TModule : IAppModule
        {
            var moduleDescriptors = VisitModule(typeof(TModule));

            return Topological.Sort(moduleDescriptors, o => o.Dependencies);
        }


        /// <summary>
        /// 遍历模块
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        protected virtual List<IModuleDescriptor> VisitModule(Type moduleType)
        {
            var moduleDescriptors = new List<IModuleDescriptor>();

            // 过滤抽象类、接口、泛型类、非类
            if (moduleType.IsAbstract
                || moduleType.IsInterface
                || moduleType.IsGenericType
                || !moduleType.IsClass)
            {
                return moduleDescriptors;
            }

            // 过滤没有实现IRModule接口的类
            var baseInterfaceType = moduleType.GetInterface(_moduleInterfaceTypeFullName, false);
            if (baseInterfaceType == null)
            {
                return moduleDescriptors;
            }

            // 
            var dependModulesAttribute = moduleType.GetCustomAttribute<DependsOnAttribute>(false);

            // 依赖属性为空
            if (dependModulesAttribute == null)
            {
                moduleDescriptors.Add(new ModuleDescriptor(moduleType));
            }
            else
            {
                // 依赖属性不为空,递归获取依赖
                var dependModuleDescriptors = new List<IModuleDescriptor>();
                foreach (var dependModuleType in dependModulesAttribute.DependModuleTypes)
                {
                    dependModuleDescriptors.AddRange(
                        VisitModule(dependModuleType)
                    );
                }

                // 创建模块描述信息,内容为模块类型和依赖类型
                moduleDescriptors.Add(new ModuleDescriptor(moduleType, dependModuleDescriptors.ToArray()));
            }

            return moduleDescriptors;
        }


        #endregion

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool state)
        {
            this.ApplicationShutdown();

        }

    }
}
