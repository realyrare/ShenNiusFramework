using System;
using System.Diagnostics.CodeAnalysis;

namespace ShenNius.ModuleCore.Context
{
    /// <summary>
    /// 应用卸载上下文
    /// </summary>
    public class ApplicationShutdownContext
    {
        public IServiceProvider ServiceProvider { get; }

        public ApplicationShutdownContext([NotNull] IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
