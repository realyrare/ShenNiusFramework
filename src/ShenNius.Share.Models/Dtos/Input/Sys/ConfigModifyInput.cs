namespace ShenNius.Share.Models.Dtos.Input.Sys
{
    public class ConfigModifyInput
    {
      
        public int Id { get; set; }
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
