using System;
using System.Collections.Generic;
using System.Text;

namespace ModuleCore.Descriptor.Interface
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
}
