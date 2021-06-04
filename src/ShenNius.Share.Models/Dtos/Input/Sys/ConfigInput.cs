using System;

namespace ShenNius.Share.Models.Dtos.Input.Sys
{
    public  class ConfigInput
    {
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
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
