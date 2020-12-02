using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using ModuleCore.AppModule.Impl;
using ModuleCore.Context;
using ShenNius.ModuleCore.Extensions;
using System;
using System.IO;
using System.Reflection;

namespace ShenNius.Swagger
{
    public class ShenNiusSwaggerModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShenNius.API", Version = "v1" });
               
            });
            
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            try
            {
                var app = context.GetApplicationBuilder();
                app.UseSwagger();
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", "ShenNius API V1");
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
    }
}
