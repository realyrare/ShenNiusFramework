using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ShenNius.ModuleCore.Context;

namespace ShenNius.ModuleCore.Extensions
{
    public static class ApplicationInitializationExtensions
    {
        public static IApplicationBuilder GetApplicationBuilder(this ApplicationInitializationContext  applicationInitializationContext)
        {
            return applicationInitializationContext.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
        }
 

    }
}
