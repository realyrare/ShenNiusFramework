﻿using System;

namespace ShenNius.Layui.Admin.Model
{
    public class ConfigOutput
    {
        public int Id { get; set; }

        /// <summary>
        /// Desc:字典类型标识
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int ParentId { get; set; }

        /// <summary>
        /// Desc:字典值——名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:字典值——英文名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string EnName { get; set; }

        /// <summary>
        /// Desc:字典值——描述
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Summary { get; set; }

        public string Type { get; set; }
    }
}
