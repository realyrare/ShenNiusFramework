using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuleCore.Context;
using ShenNius.ModuleCore.ObjectAccessor.Interface;
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
