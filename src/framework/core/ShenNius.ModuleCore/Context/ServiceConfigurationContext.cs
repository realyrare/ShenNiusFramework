using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ShenNius.ModuleCore.Context
{
    public class ServiceConfigurationContext
    {
        public IServiceCollection Services { get; protected set; }

        public IConfiguration Configuration { get; protected set; }


        public ServiceConfigurationContext(IServiceCollection services, IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
        }

    }
}
