﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ModuleCore.AppModule.Impl;
using ModuleCore.Context;
using ShenNius.Share.Infrastructure.Utils;
using ShenNius.Share.Service.Repository;
using ShenNius.Share.Service.Sys;
using System;

namespace ShenNius.Share.Service
{
    public class ShenNiusShareServiceModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
           string connectionStr= context.Configuration["ConnectionStrings:MySql"];
            if (string.IsNullOrEmpty(connectionStr))
            {
                throw new ArgumentException("data connectionStr is not fuond");
            }
            DbContext._connectionStr = connectionStr;
            InjectHelper.AddAssembly(context.Services, "ShenNius.Share.Service");

            context.Services.AddAutoMapper(typeof(AutomapperProfile));
            context.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //context.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
           
        }
    }
}
