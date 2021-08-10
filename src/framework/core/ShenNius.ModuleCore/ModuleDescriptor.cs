using System;
namespace ShenNius.ModuleCore
{
    /// <summary>
    /// 模块描述信息
    /// </summary>
    public interface IModuleDescriptor
    {
        /// <summary>
        /// 模块类型
        /// </summary>
        Type ModuleType { get; }
        /// <summary>
        /// 依赖项
        /// </summary>
        IModuleDescriptor[] Dependencies { get; }

        /// <summary>
        /// 实例,只创建一次
        /// </summary>
        object Instance { get; }
    }

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
