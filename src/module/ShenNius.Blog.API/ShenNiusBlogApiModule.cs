using ModuleCore.AppModule.Impl;
using ModuleCore.Attribute;
using ShenNius.Share.BaseController;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShenNius.Blog.API
{
    [DependsOn(typeof(ShenNiusShareBaseControllerModule)
   )]
    public class ShenNiusBlogApiModule : AppModule
    {
    }
}
