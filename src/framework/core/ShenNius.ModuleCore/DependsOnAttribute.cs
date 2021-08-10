using System;

namespace ShenNius.ModuleCore
{
    /// <summary>
    /// 模块依赖的模块 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute
    {
        /// <summary>
        /// 依赖的模块类型
        /// </summary>
        public Type[] DependModuleTypes { get; private set; }

        public DependsOnAttribute(params Type[] dependModuleTypes)
        {
            DependModuleTypes = dependModuleTypes ?? new Type[0];
        }
    }
}
