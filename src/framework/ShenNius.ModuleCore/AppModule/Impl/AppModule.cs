using ModuleCore.AppModule.Interface;
using ModuleCore.Context;
namespace ModuleCore.AppModule.Impl
{
    public abstract class AppModule : IAppModule
    {
        /// <summary>
        /// 配置服务前
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnPreConfigureServices(ServiceConfigurationContext context)
        {
        }
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnConfigureServices(ServiceConfigurationContext context)
        {

        }
        /// <summary>
        /// 配置服务后
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnPostConfigureServices(ServiceConfigurationContext context)
        {
        }
        /// <summary>
        /// 应用启动前
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
        }
        /// <summary>
        /// 应用启动
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
        /// <summary>
        /// 应用启动后
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
        }
        /// <summary>
        /// 应用停止
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnApplicationShutdown(ApplicationShutdownContext context)
        {

        }
    }
}
