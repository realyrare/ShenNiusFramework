using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ShenNius.ModuleCore.Context
{
    /// <summary>
    /// 应用初始化上下文
    /// </summary>
    public class ApplicationInitializationContext
    {
        public IServiceProvider ServiceProvider { get; }

        public IConfiguration Configuration { get; }
        public ApplicationInitializationContext([NotNull] IServiceProvider serviceProvider, [NotNull] IConfiguration configuration)
        {
            ServiceProvider = serviceProvider;
            Configuration = configuration;
        }
    }
}
