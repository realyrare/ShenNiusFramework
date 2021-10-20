using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;

namespace ShenNius.API.Hosting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            //1.3.0 用法
            //.UseServiceProviderFactory(new AspectCoreServiceProviderFactory())
            //用AspectCore替换默认的IOC容器
            .UseServiceProviderFactory(new DynamicProxyServiceProviderFactory())
            .UseNLog();//加入nlog日志;
    }
}
