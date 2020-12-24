using ModuleCore.AppModule.Impl;
using ModuleCore.Context;
using ShenNius.Service.Configurations;
using System;

namespace ShenNius.Service
{
    public class ShenNiusShareServiceModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
           string connectionStr= context.Configuration["DbConnection:MySqlConnectionString"];
            if (string.IsNullOrEmpty(connectionStr))
            {
                throw new ArgumentException("data connectionStr is not fuond");
            }
            //AppSettings.Db.Connection => connectionStr;
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
           
        }
    }
}
