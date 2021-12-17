using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using ShenNius.Mvc.Admin;


Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
 {
     webBuilder.UseStartup<Startup>();
 })
//��AspectCore�滻Ĭ�ϵ�IOC����
.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory())
.UseNLog().Build().Run();