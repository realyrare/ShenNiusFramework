using ModuleCore.Descriptor.Interface;
using System;
namespace ModuleCore.Descriptor.Impl
{
    /// <summary>
    /// 模块描述
    /// </summary>
    public class ModuleDescriptor : IModuleDescriptor
    {
        protected object _instance;

        public virtual Type ModuleType { get; protected set; }

        public virtual IModuleDescriptor[] Dependencies { get; protected set; }

        public virtual object Instance
        {
            get
            {
                if (this._instance == null)
                {
                    this._instance = Activator.CreateInstance(this.ModuleType);
                }
                return this._instance;
            }
        }

        public ModuleDescriptor(Type moduleType, params IModuleDescriptor[] dependencies)
        {
            this.ModuleType = moduleType;
            this.Dependencies = dependencies ?? new ModuleDescriptor[0];
        }
    }
}
