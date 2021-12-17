using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using ShenNius.API.Hosting;



Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Startup>();
})
//ÓÃAspectCoreÌæ»»Ä¬ÈÏµÄIOCÈÝÆ÷
.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory())
.UseNLog().Build().Run();
